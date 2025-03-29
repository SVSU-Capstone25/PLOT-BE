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

namespace Plot.Data.Models.Stores;

public class Store
{
    public int? TUID { get; set; }
    public string? NAME { get; set; }
    public string? ADDRESS { get; set; }
    public string? CITY { get; set; }
    public string? STATE { get; set; }
    public string? ZIP { get; set; }
    public int? WIDTH { get; set; }
    public int? HEIGHT { get; set; }
    public IFormFile? BLUEPRINT_IMAGE { get; set; }
}