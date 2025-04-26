/*
    Filename: FloorsetRef.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Floorset
    and PLOT/PLOT-FE/Plot/Data/Models/Floorset

    File Purpose:
    This file references the Floorset with just the TUID
    
    Class Purpose:
    This record is used to pass just the TUID to the database
    for cases where only the TUID is needed.

    Written by: Andrew Miller (4/24/2025)
*/

using System.ComponentModel.DataAnnotations;

namespace Plot.Data.Models.Floorsets;

public class FloorsetRef
{
    [Required]
    public int TUID { get; set; }
}