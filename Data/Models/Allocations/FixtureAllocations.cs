/*
    Filename: FixtureAllocations.cs
    Part of Project: PLOT/PLOT-BE/Data/Models/Allocations

    File Purpose:
    This file contains the model for the fixture
    category allocations.

    Class Purpose:
    This record is used as the output format for
    fixture category allocations.
    
    Written by: Jordan Houlihan
*/

public record FixtureAllocations
{
    public string? Category { get; set; }
    public int? LFAllocated { get; set; }
    public int? LFTarget { get; set; }
}