using Plot.Data.Models.Users;
using System.ComponentModel.DataAnnotations;
namespace Plot.Data.Models.Stores;
public class Select_Store
{
    public int? TUID { get; set; }

    [StringLength(100, ErrorMessage = "Store name cannot exceed 100 characters.")]
    public required string NAME { get; set; }

    [StringLength(100, ErrorMessage = "Address cannot exceed 100 characters.")]
    public required string ADDRESS { get; set; }

    [StringLength(100, ErrorMessage = "City cannot exceed 100 characters.")]
    public required string CITY { get; set; }

    [StringLength(25, ErrorMessage = "State cannot exceed 25 characters.")]
    public required string STATE { get; set; }

    [StringLength(10, ErrorMessage = "Zip code cannot exceed 10 characters.")]
    public required string ZIP { get; set; }

    [Range(int.MinValue, int.MaxValue, ErrorMessage = "Width must be an integer.")]
    public int WIDTH { get; set; }

    [Range(int.MinValue, int.MaxValue, ErrorMessage = "Length must be an integer.")]
    public int LENGTH { get; set; }
    public byte[]? BLUEPRINT_IMAGE { get; set; }
}