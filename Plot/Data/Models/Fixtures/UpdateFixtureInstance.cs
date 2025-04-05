/*
    Filename: UpdateFixtureInstance.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Fixtures

    File Purpose:
    This file contains the fixture object model tied
    to updating the instance of a fixture.

    Class Purpose:
    This record is used as a model for frontend requests
    that update the instance of a fixture.
    
    Written by: Jordan Houlihan
*/

namespace Plot.Data.Models.Fixtures;

public record UpdateFixtureInstance
{
    public int? TUID { get; set; }
    public int? FIXTURE_TUID { get; set; }
    public int? FLOORSET_TUID { get; set; }
    public double? X_POS { get; set; }
    public double? Y_POS { get; set; }
    public double? ALLOCATED_LF { get; set; }
    public int? HANGER_STACK { get; set; }
    public float? TOT_LF { get; set; }
    public string? CATEGORY { get; set; }
    public string? NOTE { get; set; }
}