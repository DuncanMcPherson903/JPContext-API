using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JPContext.API.Data;
using JPContext.API.DTO;
using JPContext.API.Models;
using System.Security.Claims;

namespace JPContext.API.Endpoints;

public static class ExampleEndpoints
{
  public static void MapExampleEndpoints(this WebApplication app)
  {

    // Get all examples
    app.MapGet("/examples", async (
      JPContextDbContext db,
      IMapper mapper,
      string? searchQuery) =>
    {
      var examples = await db.Examples
        .Where((e) => searchQuery == null ? (e.Id > 0) : e.Title.Contains(searchQuery))
        .ToListAsync();

      return Results.Ok(mapper.Map<List<ExampleDto>>(examples));
    });

    // Get example by id
    app.MapGet("/examples/{id}", async (
      int id,
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

      var example = await db.Examples
        .FirstOrDefaultAsync(e => e.Id == id);

      if (example == null)
      {
        return Results.NotFound("Example not found.");
      }

      return Results.Ok(mapper.Map<ExampleDto>(example));
    });

    // Create a new example
    app.MapPost("/examples", async (
      [FromBody] ExampleCreateDto exampleDto,
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

      // Create the example
      var example = mapper.Map<Example>(exampleDto);
      db.Examples.Add(example);
      await db.SaveChangesAsync();

      return Results.Created($"/examples/{example?.Id}", mapper.Map<ExampleDto>(example));
    }).RequireAuthorization();

    // Update an example
    app.MapPut("/examples/{id}", async (
        int id,
        [FromBody] ExampleUpdateDto exampleDto,
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

      var example = await db.Examples
        .FirstOrDefaultAsync(e => e.Id == id);

      if (example == null)
      {
        return Results.NotFound("Example not found.");
      }

      // Update the example
      mapper.Map(exampleDto, example);
      example.UpdatedAt = DateTime.UtcNow;

      await db.SaveChangesAsync();

      return Results.Ok(mapper.Map<ExampleDto>(example));
    }).RequireAuthorization();

    // Delete an example
    app.MapDelete("/examples/{id}", async (
        int id,
        ClaimsPrincipal user,
        JPContextDbContext db) =>
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

      var example = await db.Examples
        .FirstOrDefaultAsync(e => e.Id == id);

      if (example == null)
      {
        return Results.NotFound("Example not found.");
      }

      // Delete the vocabulary
      db.Examples.Remove(example);
      await db.SaveChangesAsync();

      return Results.NoContent();
    }).RequireAuthorization();
  }
}
