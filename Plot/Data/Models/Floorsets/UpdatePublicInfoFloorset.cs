/*
    Filename: UpdatePublicInfoFloorset.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Floorsets

    File Purpose:
    This file contains the floorset object model tied
    to updating the public information of a floorset.
    
    Class Purpose:
    This record is used as a model for frontend requests
    that updates the public information of a floorset.
    
    Written by: Jordan Houlihan

    Changed which members have required tags, I left it on
    those that must be recorded for any update.
*/

using System.ComponentModel.DataAnnotations;

namespace Plot.Data.Models.Floorsets;

public record UpdatePublicInfoFloorset
{
    
    [StringLength(100, ErrorMessage = "Floorset name cannot exceed 100 characters.")]
    public string? NAME { get; set; }
    [Required]
    public int? STORE_TUID { get; set; }
    
    public DateTime? DATE_CREATED { get; set; }
    
    public int? CREATED_BY { get; set; }
    [Required]
    public DateTime? DATE_MODIFIED { get; set; }
    [Required]
    public int? MODIFIED_BY { get; set; }
    public byte[]? FLOORSET_IMAGE { get; set; }
}