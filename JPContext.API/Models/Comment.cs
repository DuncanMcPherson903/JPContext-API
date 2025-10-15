namespace JPContext.API.Models;

public class Comment
{
    public int Id { get; set; }
    public required string Text { get; set; }
    public int UserProfileId { get; set; }
    public int VocabularyId { get; set; }
    public UserProfile UserProfile { get; set; }
    public Vocabulary Vocabulary { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
