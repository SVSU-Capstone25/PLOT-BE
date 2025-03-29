/*
    Filename: UpdateFixtureModel.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Fixtures

    File Purpose:
    This file contains the fixture object model tied
    to updating the model of a fixture.

    Class Purpose:
    This record is used as a model for frontend requests
    that update the model of a fixture.
    
    Written by: Jordan Houlihan
*/

namespace Plot.Data.Models.Fixtures;

public record UpdateFixtureModel
{
    public int? FixtureId { get; set; }
    public string? Name { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public float? LFCapacity { get; set; }
    public IFormFile? FixtureImage { get; set; }
}