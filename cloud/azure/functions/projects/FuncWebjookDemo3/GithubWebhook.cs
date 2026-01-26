using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace FuncWebjookDemo3;

public static class GithubWebhook
{
    [FunctionName("GithubWebhook")]
    public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "webhooks/github")] HttpRequest req,
        [Queue("github-webhooks", Connection = "AzureWebJobsStorage")] out string queueMessage,
        ILogger log)
    {
        queueMessage = null;

        // 1) Leer body
        string body;
        using (var reader = new StreamReader(req.Body))
            body = reader.ReadToEnd();

        // 2) Headers clave
        var eventType = req.Headers["X-GitHub-Event"].ToString();
        var deliveryId = req.Headers["X-GitHub-Delivery"].ToString();
        var signature256 = req.Headers["X-Hub-Signature-256"].ToString();

        if (string.IsNullOrWhiteSpace(eventType) || string.IsNullOrWhiteSpace(deliveryId))
            return new BadRequestObjectResult(new { error = "Missing GitHub headers." });

        // 3) Validar firma (seguridad real)
        var secret = Environment.GetEnvironmentVariable("GITHUB_WEBHOOK_SECRET");
        if (string.IsNullOrWhiteSpace(secret))
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);

        if (!VerifyGithubSignature(secret, body, signature256))
            return new UnauthorizedObjectResult(new { error = "Invalid signature." });

        // 4) Encolar para procesar async (respuesta rápida)
        var envelope = new
        {
            deliveryId,
            eventType,
            receivedUtc = DateTime.UtcNow,
            payload = body
        };

        queueMessage = System.Text.Json.JsonSerializer.Serialize(envelope);

        // GitHub espera respuesta rápida
        return new OkObjectResult(new { ok = true, queued = true, deliveryId, eventType });
    }

    private static bool VerifyGithubSignature(string secret, string body, string signatureHeader)
    {
        // GitHub manda: "sha256=<hex>"
        if (string.IsNullOrWhiteSpace(signatureHeader) || !signatureHeader.StartsWith("sha256="))
            return false;

        var expectedHashHex = signatureHeader["sha256=".Length..];

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(body));
        var computedHex = Convert.ToHexString(computed).ToLowerInvariant();

        // Comparación constante para evitar timing attacks
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(computedHex),
            Encoding.UTF8.GetBytes(expectedHashHex.ToLowerInvariant()));
    }
}
