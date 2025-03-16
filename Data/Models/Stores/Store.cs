/*
    Filename: Store.cs
    Part of Project: PLOT/PLOT-BE/Data/Models/Stores

    File Purpose:
    This file contains the store object model.
    
    Class Purpose:
    This record is used as the main model
    for stores in the application. This will
    look the same as the schema in the database.

    Written by: Jordan Houlihan
*/

using Plot.Data.Models.Users;

namespace Plot.Data.Models.Stores;

public class Store
{
    public int? StoreId { get; set; }
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