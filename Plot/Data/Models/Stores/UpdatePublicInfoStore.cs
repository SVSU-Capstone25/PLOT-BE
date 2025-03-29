/*
    Filename: UpdatePublicInfoStore.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Stores

    File Purpose:
    This file contains the store object model tied
    to updating the public information of a store.
    
    Class Purpose:
    This record is used as a model for frontend requests
    that update the public information of a store.
    
    Written by: Jordan Houlihan
*/

using Plot.Data.Models.Users;

namespace Plot.Data.Models.Stores;

public record UpdatePublicInfoStore
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public IFormFile? BlueprintImage { get; set; }
}