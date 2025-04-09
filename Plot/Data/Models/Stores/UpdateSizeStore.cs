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
using System.ComponentModel.DataAnnotations;
namespace Plot.Data.Models.Stores;

public record UpdateSizeStore
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Width must be an integer.")]
    public int WIDTH { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Length must be an integer.")]
    public int LENGTH { get; set; }
}