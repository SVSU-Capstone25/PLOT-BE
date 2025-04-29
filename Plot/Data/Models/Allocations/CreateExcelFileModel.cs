using System.ComponentModel.DataAnnotations;
/*
    Filename: CreateExcelFileModel.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Allocations

    File Purpose:
    This file contains the model for adding a file
    to the database. 
    
    Written by: Michael Polhill
*/

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