using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JPContext.API.Models;

namespace JPContext.API.Data;

public class JPContextDbContext : IdentityDbContext<IdentityUser>
{
  public JPContextDbContext(DbContextOptions<JPContextDbContext> options) : base(options) { }

  public DbSet<UserProfile> UserProfiles { get; set; }
  public DbSet<Vocabulary> Vocabulary { get; set; }
  public DbSet<Comment> Comments { get; set; }
  public DbSet<Example> Examples { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    // Configure all DateTime properties to use UTC time
    foreach (var entityType in modelBuilder.Model.GetEntityTypes())
    {
      foreach (var property in entityType.GetProperties())
      {
        if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
        {
          property.SetValueConverter(
            new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTime, DateTime>(
              v => v.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(v, DateTimeKind.Utc) : v.ToUniversalTime(),
              v => DateTime.SpecifyKind(v, DateTimeKind.Utc)));
        }
      }
    }

    // Configure foreign keys here

    modelBuilder.Entity<Comment>()
      .HasOne(c => c.Vocabulary)
      .WithMany(v => v.Comments)
      .HasForeignKey(c => c.VocabularyId);

    modelBuilder.Entity<Comment>()
      .HasOne(c => c.UserProfile)
      .WithMany(up => up.Comments)
      .HasForeignKey(c => c.UserProfileId);

  }
}
