namespace JPContext.API.DTO;

public class ExampleDto
{
  public int Id { get; set; }
  public required string Title { get; set; }
  public required string Source { get; set; }
  public required string VideoUrl { get; set; }
  public required string Subtitle { get; set; }
  public required string EnglishSubtitle { get; set; }
  public required List<VocabularyDto> Vocabulary { get; set; }
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
  public required List<VocabularyDto> Vocabulary { get; set; }
}

public class ExampleUpdateDto
{
  public required string Title { get; set; }
  public required string Source { get; set; }
  public required string VideoUrl { get; set; }
  public required string Subtitle { get; set; }
  public required string EnglishSubtitle { get; set; }
  public required List<VocabularyDto> Vocabulary { get; set; }
}
