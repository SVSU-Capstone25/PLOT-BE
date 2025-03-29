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
    public int? UserId { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? Role { get; set; }
    public bool? Active { get; set; }
}