/*
    Filename: CreateFixtureAllocations.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Allocations

    File Purpose:
    This file contains the model for adding fixture
    allocations to the database.
    
    Written by: Jordan Houlihan
*/

namespace Plot.Data.Models.Allocations;

public record CreateFixtureAllocations
{
    public string? SUPERCATEGORY { get; set; }
    public string? SUBCATEGORY {get; set;}
    public int? UNITS {get; set;}
}