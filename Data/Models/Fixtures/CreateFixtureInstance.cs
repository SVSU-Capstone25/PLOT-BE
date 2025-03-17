/*
    Filename: CreateFixtureInstance.cs
    Part of Project: PLOT/PLOT-BE/Data/Models/Fixtures

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
    public FixtureModel? Fixture { get; set; }
    public double? XPosition { get; set; }
    public double? YPosition { get; set; }
    public double? LFAllocated { get; set; }
    public string? Category { get; set; }
    public string? Note { get; set; }
}