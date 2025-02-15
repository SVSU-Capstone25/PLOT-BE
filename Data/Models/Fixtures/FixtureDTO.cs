/*
    Filename: FixtureDTO.cs
    Part of Project: PLOT/PLOT-BE/Data/Models/Fixtures
    File Purpose:
    This file contains the DTOs (Data-Transfer Objects)
    used to format the inputs and outputs expected
    from the backend for fixtures.
    Written by: Jordan Houlihan
*/

namespace Plot.Data.Models.Fixtures;

/// <summary>
/// This is the input format for the
/// information needed to create and update a fixture model.
/// </summary>
public record FixtureDTO
{
    public string? Name { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public float? LFCapacity { get; set; }
    public int? HangerStack { get; set; }
    public float? LFTotal { get; set; }
    public int? StoreId { get; set; }
    public IFormFile? FixtureImage { get; set; }
}

/// <summary>
/// This is the input format for the
/// information needed to create and update a fixture
/// instance model.
/// </summary>
public record FixtureInstanceDTO
{
    public Fixture? Fixture { get; set; }
    public double? XPosition { get; set; }
    public double? YPosition { get; set; }
    public double? LFAllocated { get; set; }
    public string? Category { get; set; }
    public string? Note { get; set; }
}

/// <summary>
/// This is the output format for the
/// information needed to fill in a floorset's
/// fixture/allocations data.
/// </summary>
public record FloorsetFixtureInformationDTO
{
    public FixtureInstance[]? FixtureInstances { get; set; }
    public FixtureAllocationsDTO[]? Allocations { get; set; }
}

/// <summary>
/// This is the output format for the
/// fixture category allocations. 
/// </summary>
public record FixtureAllocationsDTO
{
    public string? Category { get; set; }
    public int? LFAllocated { get; set; }
    public int? LFTotal { get; set; }
}