/*
    Filename: Store.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Stores

    File Purpose:
    This file contains the store object model.
    
    Class Purpose:
    This record is used as the main model
    for stores in the application. This will
    look the same as the schema in the database.

    Written by: Jordan Houlihan
*/

using Plot.Data.Models.Users;
using System.ComponentModel.DataAnnotations;
namespace Plot.Data.Models.Stores;

public class Store
{
    public int? StoreId { get; set; }

    [StringLength(100, ErrorMessage = "Store name cannot exceed 100 characters.")]
    public string Name { get; set; }

    [StringLength(100, ErrorMessage = "Address cannot exceed 100 characters.")]
    public string Address { get; set; }

    [StringLength(100, ErrorMessage = "City cannot exceed 100 characters.")]
    public string City { get; set; }

    [StringLength(25, ErrorMessage = "State cannot exceed 25 characters.")]
    public string State { get; set; }

    [StringLength(10, ErrorMessage = "Zip code cannot exceed 10 characters.")]
    public string ZipCode { get; set; }

    [Range(int.MinValue, int.MaxValue, ErrorMessage = "Width must be an integer.")]
    public int Width { get; set; }

    [Range(int.MinValue, int.MaxValue, ErrorMessage = "Height must be an integer.")]
    public int Height { get; set; }
    public UserDTO[]? Employees { get; set; }
    public IFormFile? BlueprintImage { get; set; }
}