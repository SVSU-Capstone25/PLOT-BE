/*
    Filename: UpdateFloorsetFixtureInformation.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Fixtures

    File Purpose:
    This file contains the models used to update
    the fixture information for a floorset.

    Class Purpose:
    The records are used to format the request
    from the frontend for updating fixture information.
    
    Written by: Jordan Houlihan
*/


namespace Plot.Data.Models.Fixtures;

public record UpdateFloorsetFixtureInformation
{
    public int[]? DeletedFixtureInstances { get; set; }
    //public int[]? DeletedFixtureModels { get; set; }
    //public UpdateFixtureModel[]? FixtureModels { get; set; }
    public UpdateFixtureInstance[]? FixtureInstances { get; set; }
}