/*
    Filename: FloorsetFixtureInformation.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Fixtures

    File Purpose:
    This file contains the models used to format
    the fixture information for a floorset.

    Class Purpose:
    The records are used to format the response
    to the frontend for fixture information
    needed to populate a floorset.
    
    Written by: Jordan Houlihan
*/


using Plot.Data.Models.Allocations;

namespace Plot.Data.Models.Fixtures;

public record FloorsetFixtureInformation
{
    public IEnumerable<FixtureModel>? FixtureModels { get; set; }
    public IEnumerable<FixtureInstance>? FixtureInstances { get; set; }
    public IEnumerable<FixtureAllocations>? Allocations { get; set; }
}