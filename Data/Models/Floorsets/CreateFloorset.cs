/*
    Filename: CreateFloorset.cs
    Part of Project: PLOT/PLOT-BE/Data/Models/Floorsets

    File Purpose:
    This file contains the floorset object model tied
    to creating a floorset.
    
    Class Purpose:
    This record is used as a model for frontend requests
    that create a floorset.
    
    Written by: Jordan Houlihan
*/

namespace Plot.Data.Models.Floorsets;

public record CreateFloorset
{
    public string? Name { get; set; }
    public int? StoreId { get; set; }
    public DateTime? DateCreated { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? DateModified { get; set; }
    public int? ModifiedBy { get; set; }
}