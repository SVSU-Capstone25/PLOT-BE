/*
    Filename: FixtureInstance.cs
    Part of Project: PLOT/PLOT-BE/Data/Models/Fixtures

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
    public FixtureModel? Fixture { get; set; }
    public int? FixtureInstanceId { get; set; }
    public int? FloorsetId { get; set; }
    public double? XPosition { get; set; }
    public double? YPosition { get; set; }
    public double? LFAllocated { get; set; }
    public string? Category { get; set; }
    public string? Note { get; set; }
}