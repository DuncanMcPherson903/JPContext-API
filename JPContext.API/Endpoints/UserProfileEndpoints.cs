using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JPContext.API.Data;
using JPContext.API.DTO;
using JPContext.API.Models;
using System.Security.Claims;

namespace JPContext.API.Endpoints;

public static class UserProfileEndpoints
{
  public static void MapUserProfileEndpoints(this WebApplication app)
  {
    // Update a User Profile
    app.MapPut("/users/{id}", async (
        int id,
        [FromBody] UserUpdateDto userUpdateDto,
        ClaimsPrincipal user,
        JPContextDbContext db,
        IMapper mapper) =>
    {
      var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
      if (userId == null)
      {
        return Results.Unauthorized();
      }

      var userProfile = await db.UserProfiles
        .FirstOrDefaultAsync(up => up.IdentityUserId == userId);

      if (userProfile == null)
      {
        return Results.NotFound("User profile not found.");
      }

      mapper.Map(userUpdateDto, userProfile);
      userProfile.UpdatedAt = DateTime.UtcNow;

      await db.SaveChangesAsync();

      return Results.Ok(mapper.Map<UserUpdateResponseDto>(userUpdateDto));
    }).RequireAuthorization();
  }
}
