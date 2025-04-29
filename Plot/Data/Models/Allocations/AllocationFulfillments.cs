/*
    Filename: AllocationFulfillments.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Allocations

    File Purpose: This file contains the model
    for sale allocation outputs. This will be mainly used
    for the allocation sidebar.

    Class Purpose:
    This record is used as the output format for
    fixture category allocations.

    Written by: Clayton Cook
*/

namespace Plot.Data.Models.Allocations;

public record AllocationFulfillments
{
    public required int TUID { get; set; }
    public required string SUPERCATEGORY_NAME { get; set; }
    public required string SUBCATEGORY { get; set; }
    public required string SUPERCATEGORY_COLOR { get; set; }
    public required int CURRENT_LF { get; set; }
    public required int TOTAL_SALES { get; set; }
    public required int TOTAL_FLOORSET_LF { get; set; }
}