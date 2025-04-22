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
    public int? TUID { get; set; }
    public string? NAME { get; set; }
    public int? WIDTH { get; set; }
    public int? LENGTH { get; set; }
    public int? LF_CAP { get; set; }
    public int? STORE_TUID { get; set; }
    public IFormFile? ICON { get; set; }
}