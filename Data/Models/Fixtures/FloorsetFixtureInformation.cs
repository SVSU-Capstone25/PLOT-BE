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

/// <summary>
/// This is the output format for the
/// information needed to fill in a floorset's
/// fixture/allocations data.
/// </summary>
public record FloorsetFixtureInformation
{
    public FixtureInstance[]? FixtureInstances { get; set; }
    public FixtureAllocations[]? Allocations { get; set; }
}

/// <summary>
/// This is the output format for the
/// fixture category allocations. 
/// </summary>
public record FixtureAllocations
{
    public string? Category { get; set; }
    public int? LFAllocated { get; set; }
    public int? LFTotal { get; set; }
}