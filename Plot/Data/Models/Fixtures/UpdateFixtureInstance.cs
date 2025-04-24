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
    public required int TUID { get; set; }
    public required int FLOORSET_TUID { get; set; }
    public int? FIXTURE_TUID { get; set; }
    public int? X_POS { get; set; }
    public int? Y_POS { get; set; }
    public int? HANGER_STACK { get; set; }
    public string? SUBCATEGORY { get; set; }
    public string? NOTE { get; set; }
    public int? SUPERCATEGORY_TUID { get; set; }
    public string? FIXTURE_IDENTIFIER { get; set; }
}