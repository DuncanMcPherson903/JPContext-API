namespace JPContext.API.Models;

public class Example
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Source { get; set; }
    public required string VideoUrl { get; set; }
    public required string Subtitle { get; set; }
    public required string EnglishSubtitle { get; set; }
    public required int UserProfileId { get; set; }
    public List<ExampleVocabulary> Vocabulary { get; set; } = new List<ExampleVocabulary>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class ExampleVocabulary
{
    public int Id { get; set; }
    public int ExampleId { get; set; }
    public Example Example { get; set; }
    public int VocabularyId { get; set; }
    public Vocabulary Vocabulary { get; set; }

}
