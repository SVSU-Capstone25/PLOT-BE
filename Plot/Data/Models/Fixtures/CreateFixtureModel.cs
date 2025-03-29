/*
    Filename: CreateFixtureModel.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Fixtures

    File Purpose:
    This file contains the fixture object model tied
    to creating the model of a fixture.

    Class Purpose:
    This record is used as a model for frontend requests
    that create the model of a fixture.
    
    Written by: Jordan Houlihan
*/

namespace Plot.Data.Models.Fixtures;

public record CreateFixtureModel
{
    public string? Name { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public float? LFCapacity { get; set; }
    public int? StoreId { get; set; }
    //TODO: define how we convert this file to 
    // be store in the database
    public IFormFile? FixtureImage { get; set; }
}
