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
  public DbSet<ExampleVocabulary> ExampleVocabularies { get; set; }

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

    // Configure ExampleVocabulary as a join table
    modelBuilder.Entity<ExampleVocabulary>()
        .HasKey(ev => ev.Id);

    modelBuilder.Entity<ExampleVocabulary>()
        .HasOne(ev => ev.Vocabulary)
        .WithMany(v => v.Examples)
        .HasForeignKey(ev => ev.VocabularyId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<ExampleVocabulary>()
        .HasOne(ev => ev.Example)
        .WithMany(e => e.Vocabulary)
        .HasForeignKey(ev => ev.ExampleId)
        .OnDelete(DeleteBehavior.Cascade);

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
