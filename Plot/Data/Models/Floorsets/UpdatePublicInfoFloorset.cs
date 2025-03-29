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
*/

namespace Plot.Data.Models.Floorsets;

public record UpdatePublicInfoFloorset
{
    public string? NAME { get; set; }
    public DateTime? DATE_CREATED { get; set; }
    public int? MODIFIED_BY { get; set; }
}