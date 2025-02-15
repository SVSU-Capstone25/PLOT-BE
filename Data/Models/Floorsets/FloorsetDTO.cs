/*
    Filename: FloorsetDTO.cs
    Part of Project: PLOT/PLOT-BE/Data/Models/Floorsets
    File Purpose:
    This file contains the DTOs (Data-Transfer Objects)
    used to format the inputs and outputs expected
    from the backend for floorsets.
    Written by: Jordan Houlihan
*/

namespace Plot.Data.Models.Floorsets;

/// <summary>
/// This is the input format that the backend expects
/// to see when the frontend makes a request
/// to create and update a floorset.
/// </summary>
public record FloorsetDTO
{
    public string? Name { get; set; }
    public int? StoreId { get; set; }
    public DateTime? DateCreated { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? DateModified { get; set; }
    public int? ModifiedBy { get; set; }
}