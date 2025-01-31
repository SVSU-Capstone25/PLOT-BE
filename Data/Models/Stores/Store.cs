/*
    Filename: Store.cs
    Part of Project: PLOT/PLOT-BE/Data/Models/Stores
    File Purpose:
    This file contains the store object model for inputs/outputs used by 
    endpoints.
    Written by: Jordan Houlihan
*/

using Plot.Data.Models.Users;

namespace Plot.Data.Models.Stores;

public class Store
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public UserDTO[]? Employees { get; set; }
}