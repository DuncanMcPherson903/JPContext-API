using JPContext.API.Models;

namespace JPContext.API.DTO;

public class ExampleDto
{
  public int Id { get; set; }
  public string Title { get; set; }
  public string Source { get; set; }
  public string VideoUrl { get; set; }
  public string Subtitle { get; set; }
  public string EnglishSubtitle { get; set; }
  public int UserProfileId { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class ExampleCreateDto
{
  public required string Title { get; set; }
  public required string Source { get; set; }
  public required string VideoUrl { get; set; }
  public required string Subtitle { get; set; }
  public required string EnglishSubtitle { get; set; }
  public required int UserProfileId { get; set; }
  public required List<int> VocabularyId { get; set; }
}

public class ExampleUpdateDto
{
  public required string Title { get; set; }
  public required string Source { get; set; }
  public required string VideoUrl { get; set; }
  public required string Subtitle { get; set; }
  public required string EnglishSubtitle { get; set; }
  public required List<int> VocabularyId { get; set; }
}
