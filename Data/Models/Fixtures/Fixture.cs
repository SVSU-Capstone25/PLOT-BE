/*
    Filename: Fixture.cs
    Part of Project: PLOT/PLOT-BE/Data/Models/Fixtures

    File Purpose:
    This file contains the fixture object model.
    
    Written by: Jordan Houlihan
*/

namespace Plot.Data.Models.Fixtures;

public class Fixture
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