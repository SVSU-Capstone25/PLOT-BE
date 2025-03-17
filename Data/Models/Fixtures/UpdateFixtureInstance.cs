/*
    Filename: UpdateFixtureInstance.cs
    Part of Project: PLOT/PLOT-BE/Data/Models/Fixtures

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
    public FixtureModel? Fixture { get; set; }
    public double? XPosition { get; set; }
    public double? YPosition { get; set; }
    public double? LFAllocated { get; set; }
    public string? Category { get; set; }
    public string? Note { get; set; }
}