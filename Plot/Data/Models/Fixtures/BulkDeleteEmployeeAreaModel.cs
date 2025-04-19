/*
    Filename: AddEmployeeAreaModel.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Fixtures

    TODO: File Purpose: 

    TODO: Class Purpose:
    
    Written by: Clayton Cook
*/

namespace Plot.Data.Models.Fixtures;

public record BulkDeleteEmployeeAreaModel
{
    public required int FLOORSET_TUID { get; set; }
    public required int X1_POS { get; set; }
    public required int Y1_POS { get; set; }
    public required int X2_POS { get; set; }
    public required int Y2_POS { get; set; }
}