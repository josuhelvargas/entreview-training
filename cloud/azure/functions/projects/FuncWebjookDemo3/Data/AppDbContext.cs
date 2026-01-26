using Microsoft.EntityFrameworkCore;

namespace FuncWebhookDemo.Data;

public class AppDbContext : DbContext
{
  public DbSet<WebhookEvent> WebhookEvents => Set<WebhookEvent>();

  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<WebhookEvent>()
        .HasIndex(x => x.DeliveryId)
        .IsUnique(); // evita duplicados
  }
}
