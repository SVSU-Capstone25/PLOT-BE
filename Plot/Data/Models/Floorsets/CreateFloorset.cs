/*
    Filename: CreateFloorset.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Floorsets

    File Purpose:
    This file contains the floorset object model tied
    to creating a floorset.
    
    Class Purpose:
    This record is used as a model for frontend requests
    that create a floorset.
    
    Written by: Jordan Houlihan
*/

using System.ComponentModel.DataAnnotations;

namespace Plot.Data.Models.Floorsets;

public record CreateFloorset
{
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