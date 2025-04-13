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

            var response = await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

            //Console.WriteLine("Response is " + response);

            return response;

            //return await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
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
            var response = await connection.QueryFirstOrDefaultAsync<T>(storedProcedure, parameters, commandType: System.Data.CommandType.StoredProcedure);
            Console.WriteLine("In GetFirstOrDefaultStoredProcedureQuery and response is " + response);
            return response;
        }
        catch (SqlException exception)
        {
            Console.WriteLine($"Get first or default query failed: {exception.Message}");
            //Console.WriteLine("Connection string is: " + connection.ConnectionString);
            return default;
        }
    }

    public async Task<int> CreateUpdateDeleteStoredProcedureQuery(string storedProcedure, DynamicParameters parameters)
    {
        try
        {
            Console.WriteLine("stored procedure " + storedProcedure);
            Console.WriteLine("parameters" + parameters);
            foreach (var paramName in parameters.ParameterNames)
            {
                var value = parameters.Get<dynamic>(paramName);
                Console.WriteLine($"{paramName}: {value}");
            }

            var connection = GetConnection();
            var response = await connection.ExecuteAsync(storedProcedure, parameters, commandType: System.Data.CommandType.StoredProcedure);
            Console.WriteLine("response is " + response);
            return response;
        }
        catch (SqlException exception)
        {
            Console.WriteLine($"Create/Update/Delete query failed: {exception.Message}");
            return 0;
        }
    }
}