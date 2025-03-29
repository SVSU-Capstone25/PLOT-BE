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

namespace Plot.Data.Models.Floorsets;

public class Floorset
{
    public int? TUID { get; set; }
    public string? NAME { get; set; }
    public int? STORE_TUID { get; set; }
    public DateTime? DATE_CREATED { get; set; }
    public int? CREATED_BY { get; set; }
    public DateTime? DATE_MODIFIED { get; set; }
    public int? MODIFIED_BY { get; set; }
}