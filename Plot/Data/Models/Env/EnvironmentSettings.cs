/*
    Filename: EnvironmentSettings.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Environment

    File Purpose:
    This file defines the EnvironmentSettings record,
    defining the settings for the environment that will
    be used to configure the settings of the environment.

    Written by: Jordan Houlihan
*/

namespace Plot.Data.Models.Env;

public class EnvironmentSettings
{
    public readonly string audience;
    public readonly string issuer;
    public readonly string databaseConnection;
    public readonly string email_pass;
    public readonly string secret_key;
    public readonly string expiration_time;

    public EnvironmentSettings()
    {
        audience = Environment.GetEnvironmentVariable("AUDIENCE")!;
        issuer = Environment.GetEnvironmentVariable("ISSUER")!;
        databaseConnection = Environment.GetEnvironmentVariable("DB_CONNECTION")!;
        email_pass = Environment.GetEnvironmentVariable("EMAIL_PASS")!;
        secret_key = Environment.GetEnvironmentVariable("SECRET_KEY")!;
        expiration_time = Environment.GetEnvironmentVariable("EXPIRATION_TIME")!;
    }
}