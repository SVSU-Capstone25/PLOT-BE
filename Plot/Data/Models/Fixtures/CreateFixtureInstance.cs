/*
    Filename: CreateFixtureInstance.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Fixtures

    File Purpose:
    This file contains the fixture object model tied
    to creating the instance of a fixture.

    Class Purpose:
    This record is used as a model for frontend requests
    that create the instance of a fixture.
    
    Written by: Jordan Houlihan
*/

namespace Plot.Data.Models.Fixtures;

public record CreateFixtureInstance
{
    public int? TUID { get; set; }
    public int? FIXTURE_TUID { get; set; }
    public int? FLOORSET_TUID { get; set; }
    public int? X_POS { get; set; }
    public int? Y_POS { get; set; }
    public required int ALLOCATED_LF { get; set; }
    public required int HANGER_STACK { get; set; }
    public int? SUPERCATEGORY_TUID { get; set; }
    public string? SUBCATEGORY { get; set; }
    public int? EDITOR_ID { get; set; }
    public string? NOTE { get; set; }

}