/*
    Filename: Floorset.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Floorset

    File Purpose:
    This file contains the floorset object model.
    
    Class Purpose:
    This record is used as the main model
    for floorsets in the application. This will
    look the same as the schema in the database.

    Written by: Jordan Houlihan
*/

using System.ComponentModel.DataAnnotations;

namespace Plot.Data.Models.Floorsets;

public class Floorset
{
    [Required]
    public int? TUID { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "Floorset name cannot exceed 100 characters.")]
    public string? NAME { get; set; }
    [Required]
    public int? STORE_TUID { get; set; }
    [Required]
    public DateTime? DATE_CREATED { get; set; }
    [Required]
    public int? CREATED_BY { get; set; }
    [Required]
    public DateTime? DATE_MODIFIED { get; set; }
    [Required]
    public int? MODIFIED_BY { get; set; }
}