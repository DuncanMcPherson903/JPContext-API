namespace JPContext.API.Models;

public class Vocabulary
{
  public int Id { get; set; }
  public required string Term { get; set; }
  public required string Translation { get; set; }
  public required string Pronunciation { get; set; }
  public List<Example> Examples { get; set; } = new List<Example>();
  public List<Comment> Comments { get; set; } = new List<Comment>();
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
