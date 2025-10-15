using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JPContext.API.Data;
using JPContext.API.DTO;
using JPContext.API.Models;
using System.Security.Claims;

namespace JPContext.API.Endpoints;

public static class CommentEndpoints
{
  public static void MapCommentEndpoints(this WebApplication app)
  {

    // Get comment by id
    app.MapGet("/comment/{id}", async (
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

      var comment = await db.Comments
        .FirstOrDefaultAsync(v => v.Id == id);

      if (comment == null)
      {
        return Results.NotFound("Comment not found.");
      }

      return Results.Ok(mapper.Map<CommentDto>(comment));
    });

    // Create a new comment
    app.MapPost("/comment", async (
      [FromBody] CommentCreateDto commentDto,
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

      // Create the comment
      var comment = mapper.Map<Comment>(commentDto);
      db.Comments.Add(comment);
      await db.SaveChangesAsync();

      return Results.Created($"/comment/{comment?.Id}", mapper.Map<CommentDto>(comment));
    }).RequireAuthorization();

    // Update a comment
    app.MapPut("/comment/{id}", async (
        int id,
        [FromBody] CommentUpdateDto commentDto,
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

      var comment = await db.Comments
        .FirstOrDefaultAsync(v => v.Id == id);

      if (comment == null)
      {
        return Results.NotFound("Comment not found.");
      }

      // Update the comment
      mapper.Map(commentDto, comment);
      comment.UpdatedAt = DateTime.UtcNow;

      await db.SaveChangesAsync();

      return Results.Ok(mapper.Map<CommentDto>(comment));
    }).RequireAuthorization();

    // Delete a comment
    app.MapDelete("/comment/{id}", async (
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

      var comment = await db.Comments
        .FirstOrDefaultAsync(v => v.Id == id);

      if (comment == null)
      {
        return Results.NotFound("Comment not found.");
      }

      // Delete the comment
      db.Comments.Remove(comment);
      await db.SaveChangesAsync();

      return Results.NoContent();
    }).RequireAuthorization();
  }
}
