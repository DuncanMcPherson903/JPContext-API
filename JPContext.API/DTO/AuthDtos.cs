namespace JPContext.API.DTO;

public class RegistrationDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Username { get; set; }
}

public class LoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UserProfileDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
}
