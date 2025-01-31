/*
    Filename: Floorset.cs
    Part of Project: PLOT/PLOT-BE/Data/Models/Floorset
    File Purpose:
    This file contains the floorset object model for inputs/outputs used by 
    endpoints.
    Written by: Jordan Houlihan
*/

namespace Plot.Data.Models.Floorsets;

public class Floorset
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public int? StoreId { get; set; }
    public DateTime? DateCreated { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? DateModified { get; set; }
    public int? ModifiedBy { get; set; }
}