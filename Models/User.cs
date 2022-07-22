using System.ComponentModel.DataAnnotations;

namespace razor.Models;

public class User
{
    public int Id { get; set; }
    public UserRole Role { get; set; }

    [Required]
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
public enum UserRole { SuperAdmin, Admin, User }