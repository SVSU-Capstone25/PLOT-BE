/*
    Filename: Floorset.cs
    Part of Project: PLOT/PLOT-BE/Data/Models/Floorset

    File Purpose:
    This file contains the floorset object model.
    
    Class Purpose:
    This record is used as the main model
    for floorsets in the application. This will
    look the same as the schema in the database.

    Written by: Jordan Houlihan
*/

namespace Plot.Data.Models.Floorsets;

public class Floorset
{
    public int? FloorsetId { get; set; }
    public string? Name { get; set; }
    public int? StoreId { get; set; }
    public DateTime? DateCreated { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? DateModified { get; set; }
    public int? ModifiedBy { get; set; }
}