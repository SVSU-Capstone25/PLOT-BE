/*
    Filename: ErrorMessage.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Error
    
    Written by: Jordan Houlihan
*/

using System.ComponentModel.DataAnnotations;

namespace Plot.Data.Models.Error;

public record ErrorMessage
{
    [Required]
    public string? Message { get; set; }
}