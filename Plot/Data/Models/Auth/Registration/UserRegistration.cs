/*
    Filename: UserRegistration.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Auth/Registration
    
    File Purpose: 
    This file defines the model for
    registering a user.

    Class Purpose:
    This record is used as a model for frontend 
    requests to register a new user.

    Written by: Jordan Houlihan
*/

namespace Plot.Data.Models.Auth.Registration;

public record UserRegistration
{
    public required string? EMAIL { get; set; }
    public required string? FIRST_NAME { get; set; }
    public required string? LAST_NAME { get; set; }
    public required string? ROLE { get; set; }
}