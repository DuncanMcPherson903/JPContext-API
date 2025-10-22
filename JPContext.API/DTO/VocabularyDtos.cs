namespace JPContext.API.DTO;

public class VocabularyDto
{
  public int Id { get; set; }
  public required string Term { get; set; }
  public required string Translation { get; set; }
  public required string Pronunciation { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}

public class VocabularyCreateDto
{
  public required string Term { get; set; }
  public required string Translation { get; set; }
  public required string Pronunciation { get; set; }
}

public class VocabularyUpdateDto
{
  public  string Term { get; set; }
  public  string Translation { get; set; }
  public  string Pronunciation { get; set; }
}
