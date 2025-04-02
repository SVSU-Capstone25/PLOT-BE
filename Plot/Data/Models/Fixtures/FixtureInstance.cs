/*
    Filename: FixtureInstance.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Fixtures

    File Purpose:
    This file contains the object model for the instance
    of a fixture.
    
    Class Purpose:
    This record is used as the main model
    for the instance of a fixture in the application. This will
    look the same as the schema in the database.

    Written by: Jordan Houlihan
*/

namespace Plot.Data.Models.Fixtures;

public class FixtureInstance
{
    public int? TUID { get; set; }
    public int? FIXTURE_TUID { get; set; }
    public int? FLOORSET_TUID { get; set; }
    public double? X_POS { get; set; }
    public double? Y_POS { get; set; }
    public double? ALLOCATED_LF { get; set; }
    public int? HANGER_STACK { get; set; }
    public string? CATEGORY { get; set; }
    public string? NOTE { get; set; }
}