using System;
using System.Text.Json;
using System.Threading.Tasks;
using FuncWebhookDemo.Data;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace FuncWebjookDemo3;

public class ProcessGithubWebhook
{
    private readonly AppDbContext _db;

    public ProcessGithubWebhook(AppDbContext db) => _db = db;

    [FunctionName("ProcessGithubWebhook")]
    public async Task Run([QueueTrigger("github-webhooks", Connection = "AzureWebJobsStorage")] string message, ILogger log)
    {
        using var doc = JsonDocument.Parse(message);

        var deliveryId = doc.RootElement.GetProperty("deliveryId").GetString()!;
        var eventType = doc.RootElement.GetProperty("eventType").GetString()!;
        var payload = doc.RootElement.GetProperty("payload").GetString()!;

        // Asegurar BD creada
        await _db.Database.EnsureCreatedAsync();

        // Idempotencia: si ya existe, no duplicar
        var exists = await _db.WebhookEvents.AnyAsync(x => x.DeliveryId == deliveryId);
        if (exists)
        {
            log.LogWarning("Duplicate deliveryId ignored: {deliveryId}", deliveryId);
            return;
        }

        _db.WebhookEvents.Add(new WebhookEvent
        {
            DeliveryId = deliveryId,
            EventType = eventType,
            PayloadJson = payload,
            ReceivedUtc = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();

        log.LogInformation("Saved webhook event {eventType} delivery={deliveryId}", eventType, deliveryId);
    }
}
