using Plot.Data.Models.Users;

namespace Plot.Data.Models.Stores;

public class Store
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public UserDTO[]? Employees { get; set; }
}