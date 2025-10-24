using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JPContext.API.Data;
using JPContext.API.DTO;
using JPContext.API.Models;
using System.Security.Claims;

namespace JPContext.API.Endpoints;

public static class VocabularyEndpoints
{
  public static void MapVocabularyEndpoints(this WebApplication app)
  {

    // Get all vocab
    app.MapGet("/vocabulary", async (
      JPContextDbContext db,
      IMapper mapper,
      string? searchQuery) =>
    {
      var vocabulary = await db.Vocabulary
        .Where((v) => searchQuery == null ? (v.Id > 0) : v.Term.Contains(searchQuery))
        .ToListAsync();

      return Results.Ok(mapper.Map<List<VocabularyDto>>(vocabulary));
    });

    // Get vocab by id
    app.MapGet("/vocabulary/{id}", async (
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

      var vocabulary = await db.Vocabulary
        .FirstOrDefaultAsync(v => v.Id == id);

      if (vocabulary == null)
      {
        return Results.NotFound("Vocabulary term not found.");
      }

      return Results.Ok(mapper.Map<VocabularyDto>(vocabulary));
    });

    // Create a new vocabulary
    app.MapPost("/vocabulary", async (
      [FromBody] VocabularyCreateDto vocabDto,
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

      // Create the vocab
      var vocab = mapper.Map<Vocabulary>(vocabDto);
      db.Vocabulary.Add(vocab);
      await db.SaveChangesAsync();

      return Results.Created($"/vocabulary/{vocab?.Id}", mapper.Map<VocabularyDto>(vocab));
    }).RequireAuthorization("AdminOnly");

    // Update a vocabulary
    app.MapPut("/vocabulary/{id}", async (
        int id,
        [FromBody] VocabularyUpdateDto vocabDto,
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

      var vocab = await db.Vocabulary
        .FirstOrDefaultAsync(v => v.Id == id);

      if (vocab == null)
      {
        return Results.NotFound("Vocabulary not found.");
      }

      // Update the vocab
      mapper.Map(vocabDto, vocab);
      vocab.UpdatedAt = DateTime.UtcNow;

      await db.SaveChangesAsync();

      return Results.Ok(mapper.Map<VocabularyDto>(vocab));
    }).RequireAuthorization("AdminOnly");

    // Delete a vocabulary
    app.MapDelete("/vocabulary/{id}", async (
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

      var vocabulary = await db.Vocabulary
        .FirstOrDefaultAsync(v => v.Id == id);

      if (vocabulary == null)
      {
        return Results.NotFound("Vocabulary not found.");
      }

      // Delete the vocabulary
      db.Vocabulary.Remove(vocabulary);
      await db.SaveChangesAsync();

      return Results.NoContent();
    }).RequireAuthorization("AdminOnly");

    // Get Vocabulary by Example id
    app.MapGet("/examples/{id}/vocabulary", async (
      int id,
      ClaimsPrincipal user,
      JPContextDbContext db,
      IMapper mapper) =>
    {

      var exampleVocabularies = await db.ExampleVocabularies
        .Include(ev => ev.Example)
        .ThenInclude(e => e.Vocabulary.Where(ev => ev.VocabularyId == ev.Vocabulary.Id))
        .ThenInclude(ev => ev.Vocabulary)
        .Where(ev => ev.ExampleId == id)
        .ToListAsync();

      var vocabulary = exampleVocabularies.Select(ev => ev.Vocabulary).ToList();

      if (vocabulary == null)
      {
        return Results.NotFound("Vocabulary not found.");
      }

      return Results.Ok(mapper.Map<List<VocabularyDto>>(vocabulary));
    });

  }
}
