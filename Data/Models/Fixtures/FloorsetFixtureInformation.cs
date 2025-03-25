/*
    Filename: FloorsetFixtureInformation.cs
    Part of Project: PLOT/PLOT-BE/Data/Models/Fixtures

    File Purpose:
    This file contains the models used to format
    the fixture information for a floorset.

    Class Purpose:
    The records are used to format the response
    to the frontend for fixture information
    needed to populate a floorset.
    
    Written by: Jordan Houlihan
*/


namespace Plot.Data.Models.Fixtures;

public record FloorsetFixtureInformation
{
    public FixtureModel[]? FixtureModels { get; set; }
    public FixtureInstance[]? FixtureInstances { get; set; }
    public FixtureAllocations[]? Allocations { get; set; }
}