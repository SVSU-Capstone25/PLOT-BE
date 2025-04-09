/*
    Filename: FixtureAllocations.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Allocations

    File Purpose:
    This file contains the model for the fixture
    category allocations.

    Class Purpose:
    This record is used as the output format for
    fixture category allocations.
    
    Written by: Jordan Houlihan
*/

namespace Plot.Data.Models.Allocations;

public record FixtureAllocations
{
    public required int TUID {get; set;}
    public required int SUPERCATEGORY_TUID { get; set; }
    public required string SUBCATEGORY { get; set; }
    public required int TOTAL_SALES { get; set; }
    public required int SALES_TUID { get; set; }
}