/*
    Filename: StoreDTO.cs
    Part of Project: PLOT/PLOT-BE/Data/Models/Stores
    File Purpose:
    This file contains the DTOs (Data-Transfer Objects)
    used to format the inputs and outputs expected
    from the backend for stores.
    Written by: Jordan Houlihan
*/

using Plot.Data.Models.Users;

namespace Plot.Data.Models.Stores;

/// <summary>
/// This is the input format that the backend expects
/// to see when the frontend makes a request
/// to create and update a store.
/// </summary>
public record StoreDTO
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public UserDTO[]? Employees { get; set; }
    public IFormFile? BlueprintImage { get; set; }
}