/*
    Filename: FixtureModel.cs
    Part of Project: PLOT/PLOT-BE/Data/Models/Fixtures

    File Purpose:
    This file contains the object model for the model
    of a fixture.
    
    Class Purpose:
    This record is used as the main model
    for the model of a fixture in the application. This will
    look the same as the schema in the database.

    Written by: Jordan Houlihan
*/

namespace Plot.Data.Models.Fixtures;

public class FixtureModel
{
    public int? FixtureId { get; set; }
    public string? Name { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public float? LFCapacity { get; set; }
    public int? HangerStack { get; set; }
    public float? LFTarget { get; set; }
    public int? StoreId { get; set; }
    public IFormFile? FixtureImage { get; set; }
}