using System.ComponentModel.DataAnnotations;

namespace Plot.Data.Models.Allocations;

public class CreateExcelFileModel
{
    [Required]
    [StringLength(100, ErrorMessage = "File Name cannot exceed 100 characters.")]
    public required string FILE_NAME { get; set; }

    public required byte[] FILE_DATA { get; set; }
    public required DateTime CAPTURE_DATE { get; set; }
    public required DateTime DATE_UPLOADED { get; set; }
    public required int FLOORSET_TUID { get; set; }


}