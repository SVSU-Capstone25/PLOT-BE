namespace Plot.Data.Models.Allocations;
using System.ComponentModel.DataAnnotations;
public class ExcelFileModel
{
    [Required]
    [StringLength(100, ErrorMessage = "File Name cannot exceed 100 characters.")]
    public required string NAME { get; set; }
    
}