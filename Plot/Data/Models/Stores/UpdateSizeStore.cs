/*
    Filename: UpdateSizeStore.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Stores

    File Purpose:
    This file contains the store object model tied
    to updating the size of a store.
    
    Class Purpose:
    This record is used as a model for frontend requests
    that update the size of a store.
    
    Written by: Jordan Houlihan
*/

namespace Plot.Data.Models.Stores;

public record UpdateSizeStore
{
    public int? WIDTH { get; set; }
    public int? HEIGHT { get; set; }
}