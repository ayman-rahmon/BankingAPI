namespace BankingAPI.WebAPI.DTOs;

public class UserRegisterDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } // Admin or User
}
