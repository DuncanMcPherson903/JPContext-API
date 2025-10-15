namespace JPContext.API.DTO;

public class CommentDto
{
  public int Id { get; set; }
  public required string Text { get; set; }
  public required string Username { get; set; }
  public int UserProfileId { get; set; }
  public int VocabularyId { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class CommentCreateDto
{
  public required string Text { get; set; }
  public int UserProfileId { get; set; }
  public int VocabularyId { get; set; }
}

public class CommentUpdateDto
{
    public required string Text { get; set; }
}
