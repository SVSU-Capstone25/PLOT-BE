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
    public required string? Email { get; set; }
    public required string? FirstName { get; set; }
    public required string? LastName { get; set; }
    public required string? Role { get; set; }
}