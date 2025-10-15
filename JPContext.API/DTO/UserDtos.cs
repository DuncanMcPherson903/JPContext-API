using JPContext.API.Models;

namespace JPContext.API.DTO;

public class UserUpdateDto
{
  public required string Username { get; set; }
  public required string Email { get; set; }
}

public class UserUpdateResponseDto
{
  public bool Success { get; set; } = true;
  public string Message { get; set; } = "Profile updated successfully";
  public required UserUpdateDto User { get; set; }
  public DateTime UpdatedAt { get; set; }
}
