/*
    Filename: AddEmployeeAreaModel.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Fixtures

    File Purpose: This file contains the model
    for adding an employee area.

    Class Purpose: This record is the input model
    for adding an employee area. This is what the
    backend expects from the frontend for adding
    an employee area.
    
    Written by: Clayton Cook
*/

namespace Plot.Data.Models.Fixtures;

public record AddEmployeeAreaModel
{
    public required int FLOORSET_TUID { get; set; }
    public required int X_POS { get; set; }
    public required int Y_POS { get; set; }
}