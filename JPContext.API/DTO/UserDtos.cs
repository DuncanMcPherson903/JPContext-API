using JPContext.API.Models;

namespace JPContext.API.DTOs;

public class UserUpdateDto
{
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string Email { get; set; }
}

public class UserUpdateResponseDto
{
  public bool Success { get; set; } = true;
  public string Message { get; set; } = "Profile updated successfully";
  public required UserUpdateDto User { get; set; }
  public DateTime UpdatedAt { get; set; }
}
