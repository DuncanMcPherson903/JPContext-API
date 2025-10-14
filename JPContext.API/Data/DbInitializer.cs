using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using JPContext.API.Models;

namespace JPContext.API.Data;

public static class DbInitializer
{
  public static async Task Initialize(IServiceProvider serviceProvider, ILogger logger)
  {
    try
    {
      using var scope = serviceProvider.CreateScope();
      var context = scope.ServiceProvider.GetRequiredService<JPContextDbContext>();
      var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
      var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

      // Apply migrations if they are not applied
      context.Database.Migrate();

      // Seed roles
      await SeedRoles(roleManager);

      // Seed admin user
      await SeedAdminUser(userManager, context);

      // Seed sample data
      await SeedSampleData(context, userManager);

      logger.LogInformation("Database initialized successfully.");
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "An error occurred while initializing the database.");
      throw;
    }
  }

  private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
  {
    string[] roles = { "Admin", "User" };

    foreach (var role in roles)
    {
      if (!await roleManager.RoleExistsAsync(role))
      {
        await roleManager.CreateAsync(new IdentityRole(role));
      }
    }
  }

  private static async Task SeedAdminUser(UserManager<IdentityUser> userManager, JPContextDbContext context)
  {
    // Check if admin user exists
    var adminEmail = "admin@jpcontext.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
      // Create admin user
      adminUser = new IdentityUser
      {
        UserName = adminEmail,
        Email = adminEmail,
        EmailConfirmed = true
      };

      var result = await userManager.CreateAsync(adminUser, "Admin123!");

      if (result.Succeeded)
      {
        // Add admin role
        await userManager.AddToRoleAsync(adminUser, "Admin");

        // Create admin user profile
        var adminProfile = new UserProfile
        {
          FirstName = "Admin",
          LastName = "User",
          Email = adminEmail,
          IdentityUserId = adminUser.Id,
          CreatedAt = DateTime.UtcNow,
          UpdatedAt = DateTime.UtcNow
        };

        context.UserProfiles.Add(adminProfile);
        await context.SaveChangesAsync();

        // Verify the admin profile was created
        var verifyProfile = await context.UserProfiles.FirstOrDefaultAsync(up => up.IdentityUserId == adminUser.Id);
        if (verifyProfile == null)
        {
          // If profile creation failed, log it and try again
          Console.WriteLine("WARNING: Admin profile creation failed on first attempt. Trying again...");

          // Try creating the profile again
          context.UserProfiles.Add(adminProfile);
          await context.SaveChangesAsync();
        }
      }
    }
    else
    {
      // Verify admin user has a profile
      var adminProfile = await context.UserProfiles.FirstOrDefaultAsync(up => up.IdentityUserId == adminUser.Id);
      if (adminProfile == null)
      {
        // Create admin user profile if it doesn't exist
        adminProfile = new UserProfile
        {
          FirstName = "Admin",
          LastName = "User",
          Email = adminEmail,
          IdentityUserId = adminUser.Id,
          CreatedAt = DateTime.UtcNow,
          UpdatedAt = DateTime.UtcNow
        };

        context.UserProfiles.Add(adminProfile);
        await context.SaveChangesAsync();
        Console.WriteLine("Created missing admin profile for existing admin user.");
      }
    }
  }

  private static async Task SeedSampleData(JPContextDbContext context, UserManager<IdentityUser> userManager)
  {
    // Only seed sample data if the database is empty
    if (await context.Users.AnyAsync())
    {
        return;
    }

    // Seed sample users and get their profiles
    var userProfiles = await SeedSampleUsers(context, userManager);

  }

  private static async Task<List<UserProfile>> SeedSampleUsers(JPContextDbContext context, UserManager<IdentityUser> userManager)
  {
    var userProfiles = new List<UserProfile>();

    // Sample user 1
    var userEmail1 = "user@jpcontext.com";
    var sampleUser1 = await userManager.FindByEmailAsync(userEmail1);

    if (sampleUser1 == null)
    {
      sampleUser1 = new IdentityUser
      {
        UserName = userEmail1,
        Email = userEmail1,
        EmailConfirmed = true
      };

      var result = await userManager.CreateAsync(sampleUser1, "User123!");

      if (result.Succeeded)
      {
        await userManager.AddToRoleAsync(sampleUser1, "User");

        var userProfile1 = new UserProfile
        {
          FirstName = "Sample",
          LastName = "User",
          Email = userEmail1,
          IdentityUserId = sampleUser1.Id,
          CreatedAt = DateTime.UtcNow,
          UpdatedAt = DateTime.UtcNow
        };

        context.UserProfiles.Add(userProfile1);
        await context.SaveChangesAsync();
        userProfiles.Add(userProfile1);
      }
    }
    else
    {
      var existingProfile = await context.UserProfiles.FirstOrDefaultAsync(up => up.IdentityUserId == sampleUser1.Id);
      if (existingProfile != null)
      {
        userProfiles.Add(existingProfile);
      }
    }

    // Sample user 2
    var userEmail2 = "jane@jpcontext.com";
    var sampleUser2 = await userManager.FindByEmailAsync(userEmail2);

    if (sampleUser2 == null)
    {
      sampleUser2 = new IdentityUser
      {
        UserName = userEmail2,
        Email = userEmail2,
        EmailConfirmed = true
      };

      var result = await userManager.CreateAsync(sampleUser2, "User123!");

      if (result.Succeeded)
      {
        await userManager.AddToRoleAsync(sampleUser2, "User");

        var userProfile2 = new UserProfile
        {
          FirstName = "Jane",
          LastName = "Doe",
          Email = userEmail2,
          IdentityUserId = sampleUser2.Id,
          CreatedAt = DateTime.UtcNow,
          UpdatedAt = DateTime.UtcNow
        };

        context.UserProfiles.Add(userProfile2);
        await context.SaveChangesAsync();
        userProfiles.Add(userProfile2);
      }
    }
    else
    {
      var existingProfile = await context.UserProfiles.FirstOrDefaultAsync(up => up.IdentityUserId == sampleUser2.Id);
      if (existingProfile != null)
      {
        userProfiles.Add(existingProfile);
      }
    }

    return userProfiles;
  }
}
