/*
    Filename: TokenSettings.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Auth/Token

    File Purpose:
    This file defines the TokenSettings model class, 
    which holds configuration for TokenService.

    Class Purpose:
    This record serves as a model for data to configure the TokenService
    class. The properties correspond to token configuration values needed
    by the TokenService class.

    Written by: Michael Polhill
*/

namespace Plot.Data.Models.Auth.Token;

public record TokenSettings
{
    // VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES ------

    // Issuer of the token (Http://Plot.com)!!!!!!!!Change when info is available!!!!!!!!
    public string? Issuer { get; set; }

    // Audience of the token (Http://Plot.com)!!!!!!!!Change when info is available!!!!!!!!
    public string? Audience { get; set; }

    // Token expiration time (in hours)
    public double? ExpirationTime { get; set; }
    // Secret key
    public string? SecretKey { get; set; }
}