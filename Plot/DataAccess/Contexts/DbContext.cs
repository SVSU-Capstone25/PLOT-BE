/*
    Filename: DbContext.cs
    Part of Project: PLOT/PLOT-BE/DataAccess/Contexts

    File Purpose:
    This file contains the base database context class used
    to make a connection and execute sql queries to a database.
    
    Class Purpose:
    This class will be the base implementation for a standard database
    context class and all database context classes will inherit from it.

    Written by: Jordan Houlihan
*/

using Microsoft.Data.SqlClient;

namespace Plot.DataAccess.Contexts;

public class DbContext
{
    private readonly string _databaseConnection;

    public DbContext()
    {
        _databaseConnection = Environment.GetEnvironmentVariable("DB_CONNECTION")!;

        if (string.IsNullOrEmpty(_databaseConnection))
        {
            throw new ArgumentNullException("Database connection not found in environment file.");
        }
    }

    public SqlConnection GetConnection()
    {
        try
        {
            return new SqlConnection(_databaseConnection);
        }
        catch (Exception ex)
        {
            throw new ArgumentNullException("Database connection failed.", ex);
        }
    }
}