/*
    Filename: CreateStore.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Stores

    File Purpose:
    This file contains the store object model tied
    to creating a store.
    
    Class Purpose:
    This record is used as a model for frontend requests
    that create a store.
    
    Written by: Jordan Houlihan
*/

using Plot.Data.Models.Users;

namespace Plot.Data.Models.Stores;

public record CreateStore
{
    public string? NAME { get; set; }
    public string? ADDRESS { get; set; }
    public string? CITY { get; set; }
    public string? STATE { get; set; }
    public string? ZIP { get; set; }
    public int? WIDTH { get; set; }
    public int? HEIGHT { get; set; }
    public IFormFile? BLUEPRINT_IMAGE { get; set; }
}