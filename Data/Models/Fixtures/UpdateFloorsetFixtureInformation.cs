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

public record UpdateFloorsetFixtureInformation
{
    public int[]? DeletedFixtureInstances { get; set; }
    public int[]? DeletedFixtureModels { get; set; }
    public FixtureModel[]? FixtureModels { get; set; }
    public FixtureInstance[]? FixtureInstances { get; set; }
}