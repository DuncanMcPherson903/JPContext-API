using Microsoft.AspNetCore.Identity;

namespace JPContext.API.Models;

public class UserProfile
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string IdentityUserId { get; set; }
    public IdentityUser IdentityUser { get; set; }
    public List<Comment> Comments { get; set; } = new List<Comment>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

}
