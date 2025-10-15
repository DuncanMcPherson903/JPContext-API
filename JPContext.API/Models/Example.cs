namespace JPContext.API.Models;

public class Example
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Source { get; set; }
    public required string VideoUrl { get; set; }
    public required string Subtitle { get; set; }
    public required string EnglishSubtitle { get; set; }
    public List<Vocabulary> Vocabulary { get; set; } = new List<Vocabulary>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
