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
          Username = "Admin",
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
          Username = "Admin",
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
    if (await context.Vocabulary.AnyAsync() || await context.Examples.AnyAsync())
    {
      return;
    }

    // Seed sample users and get their profiles
    var userProfiles = await SeedSampleUsers(context, userManager);
    var vocabulary = await SeedSampleVocabulary(context);
    var examples = await SeedSampleExamples(context);
    await SeedSampleComments(context);

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
          Username = "Sample",
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
          Username = "Jane",
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

  private static async Task<List<Vocabulary>> SeedSampleVocabulary(JPContextDbContext context)
  {
    var vocabulary = new List<Vocabulary>
      {
        new Vocabulary
        {
          Term = "大きい",
          Translation = "Big/Large",
          Pronunciation = "おおきい"
        },
        new Vocabulary
        {
          Term = "ふじ山",
          Translation = "Mt. Fuji",
          Pronunciation = "ふじさん"
        },
        new Vocabulary
        {
          Term = "正しい",
          Translation = "Correct",
          Pronunciation = "ただしい"
        },
        new Vocabulary
        {
          Term = "下げる",
          Translation = "To Lower Something",
          Pronunciation = "さげる"
        }
      };

    context.Vocabulary.AddRange(vocabulary);
    await context.SaveChangesAsync();

    return vocabulary;
  }

  private static async Task<List<Example>> SeedSampleExamples(JPContextDbContext context)
  {
    var examples = new List<Example>
      {
        new Example
        {
          Title = "Mt. Fuji News Report",
          Source = "News Report",
          VideoUrl = "video1@example.com",
          Subtitle = "私たちはふじ山にいます",
          EnglishSubtitle = "We are here at Mt. Fuji."
        },
        new Example
        {
          Title = "SampleTitle2",
          Source = "SampleSource2",
          VideoUrl = "video2@example.com",
          Subtitle = "テキストの例2",
          EnglishSubtitle = "ExampleText2"
        },
        new Example
        {
          Title = "SampleTitle3",
          Source = "SampleSource3",
          VideoUrl = "video3@example.com",
          Subtitle = "テキストの例3",
          EnglishSubtitle = "ExampleText3"
        },
        new Example
        {
          Title = "SampleTitle4",
          Source = "SampleSource4",
          VideoUrl = "video4@example.com",
          Subtitle = "テキストの例4",
          EnglishSubtitle = "ExampleText4"
        }
      };

    context.Examples.AddRange(examples);
    await context.SaveChangesAsync();

    return examples;
  }

  private static async Task<List<Comment>> SeedSampleComments(JPContextDbContext context)
  {
    var comments = new List<Comment>
      {
        new Comment
        {
          Text = "Test Text1",
          UserProfileId = 1,
          VocabularyId = 1
        },
        new Comment
        {
          Text = "Test Text2",
          UserProfileId = 2,
          VocabularyId = 2
        },
        new Comment
        {
          Text = "Test Text3",
          UserProfileId = 3,
          VocabularyId = 3
        },
        new Comment
        {
          Text = "Test Text4",
          UserProfileId = 4,
          VocabularyId = 4
        }
      };

    context.Comments.AddRange(comments);
    await context.SaveChangesAsync();

    return comments;
  }
}
