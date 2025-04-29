/*
    Filename: BulkAddEmployeeAreaModel.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Fixtures

    File Purpose: This file contains the model
    for bulk adding employee areas.

    Class Purpose: This record is the input model
    for bulk adding employee areas. This is what the
    backend expects from the frontend for bulk adding
    an employee area.
    
    Written by: Clayton Cook
*/

namespace Plot.Data.Models.Fixtures;

public record BulkAddEmployeeAreaModel
{
    public required int FLOORSET_TUID { get; set; }
    public required int X1_POS { get; set; }
    public required int Y1_POS { get; set; }
    public required int X2_POS { get; set; }
    public required int Y2_POS { get; set; }
}