/*
    Filename: AllocationFulfillments.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Allocations

    TODO: File Purpose:

    TODO: Class Purpose:
    
    Written by: Clayton Cook
*/

namespace Plot.Data.Models.Allocations;

public record AllocationFulfillments
{
    public required string SUBCATEGORY { get; set; }
    public required string SUPERCATEGORY_NAME { get; set; }
    public required string SUPERCATEGORY_COLOR { get; set; }
    public required int TOTAL_SALES { get; set; }
    public required int SUPERCATEGORY_TUID { get; set; }
}