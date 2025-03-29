/*
    Filename: UserDTO.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Users

    File Purpose:
    This file contains the DTO (Data-Transfer Object)
    used to format the inputs and outputs expected
    from the backend for users.

    Class Purpose:
    This record is used as a way to transfer user
    data without transferring the sensitive data as
    well.
    
    Written by: Jordan Houlihan
*/

namespace Plot.Data.Models.Users;

public record UserDTO
{
    public int? TUID { get; set; }
    public string? EMAIL { get; set; }
    public string? FIRST_NAME { get; set; }
    public string? LAST_NAME { get; set; }
    public string? ROLE { get; set; }
    public bool? ACTIVE { get; set; }
}