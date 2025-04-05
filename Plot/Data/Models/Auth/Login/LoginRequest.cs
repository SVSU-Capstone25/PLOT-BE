/*
    Filename: LoginRequest.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Auth/Login
    
    File Purpose: 
    This file defines the model for
    login requests.

    Class Purpose:
    This record is used as a model for frontend 
    requests to login a user.

    Written by: Jordan Houlihan
*/

namespace Plot.Data.Models.Auth.Login;

public record LoginRequest
{
    public required string? EMAIL { get; set; }
    public required string? PASSWORD { get; set; }
}