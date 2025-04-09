/*
    Filename: UploadFile.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Allocations

    File Purpose:
    This file contains the request input for sending
    an excel file over.
    
    Written by: Jordan Houlihan
*/

namespace Plot.Data.Models.Allocations;

public record UploadFile
{
    public IFormFile? EXCEL_FILE {get; set;}
}