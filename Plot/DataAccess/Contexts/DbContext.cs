/*
    Filename: DbContext.cs
    Part of Project: PLOT/PLOT-BE/Plot/DataAccess/Contexts

    File Purpose:
    This file contains the base database context class used
    to make a connection and execute sql queries to a database.
    
    Class Purpose:
    This class will be the base implementation for a standard database
    context class and all database context classes will inherit from it.

    Written by: Jordan Houlihan
*/
using Dapper;
using Plot.Data.Models.Env;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Plot.DataAccess.Contexts;

public class DbContext
{
    private readonly EnvironmentSettings _envSettings;
    private readonly string _databaseConnection;

    public DbContext()
    {
        _envSettings = new();
        _databaseConnection = _envSettings.databaseConnection;
    }

    public SqlConnection GetConnection()
    {
        try
        {
            return new SqlConnection(_databaseConnection);
        }
        catch (Exception exception)
        {
            throw new ArgumentNullException("Database connection failed. ", exception.Message);
        }
    }

    public async Task<IEnumerable<T>?> GetStoredProcedureQuery<T>(string storedProcedure) 
    {
            DynamicParameters parameters = new DynamicParameters();

            return await this.GetStoredProcedureQuery<T>(storedProcedure, parameters);
    }

    public async Task<IEnumerable<T>?> GetStoredProcedureQuery<T>(string storedProcedure, DynamicParameters parameters) 
    {
        try 
        {
            var connection = GetConnection();
            
            return await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException exception)
        {
            Console.WriteLine($"Get query failed: {exception.Message} {exception.Procedure}");
            return default;
        }
    }

    public async Task<T?> GetFirstOrDefaultStoredProcedureQuery<T>(string storedProcedure, DynamicParameters parameters) 
    {
        try
        {
            var connection = GetConnection();
            return await connection.QueryFirstOrDefaultAsync<T>(storedProcedure, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
        catch (SqlException exception)
        {
            Console.WriteLine($"Get first or default query failed: {exception.Message}");
            return default;
        }
    }

    public async Task<int> CreateUpdateDeleteStoredProcedureQuery(string storedProcedure, DynamicParameters parameters)
    {
        try
        {
            var connection = GetConnection();
            return await connection.ExecuteAsync(storedProcedure, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
        catch (SqlException exception)
        {
            Console.WriteLine($"Create/Update/Delete query failed: {exception.Message}");
            return 0;
        }
    }
}