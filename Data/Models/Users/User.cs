namespace Plot.Data.Models.Users;

public class User
{
    public int? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public int? Role { get; set; }
}