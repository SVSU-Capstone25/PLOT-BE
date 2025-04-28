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
    Documented by Josh Rodack
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
    /// <summary>
    /// Public GetConnection value to create a connection to the database for
    /// the other contexts.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
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
    /// <summary>
    /// Generic implementation to pass stored procedures to the database
    /// and return the ienumerables to the front end.
    /// </summary>
    /// <typeparam name="T">Generic type for models</typeparam>
    /// <param name="storedProcedure">name of stored procedure.</param>
    /// <returns>IEnumerable of Ts </returns>
    public async Task<IEnumerable<T>?> GetStoredProcedureQuery<T>(string storedProcedure)
    {
        DynamicParameters parameters = new DynamicParameters();

        return await this.GetStoredProcedureQuery<T>(storedProcedure, parameters);
    }
    /// <summary>
    /// Generic implementation to pass stored procedures to the database
    /// and return the ienumerables to the front end.
    /// </summary>
    /// <typeparam name="T">Generic Model </typeparam>
    /// <param name="storedProcedure">Name of stored procedure</param>
    /// <param name="parameters"> Dapper model to pass stored procedure</param>
    /// <returns>IEnumerable of T</returns>
    public async Task<IEnumerable<T>?> GetStoredProcedureQuery<T>(string storedProcedure, DynamicParameters parameters)
    {
        try
        {
            var connection = GetConnection();

            var response = await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

            connection.Close();

            return response;
        }
        catch (SqlException exception)
        {
            Console.WriteLine($"Get query failed: {exception.Message} {exception.Procedure}");
            return default;
        }
    }
    /// <summary>
    /// Generic stored procedure for calls that only return a single value
    /// </summary>
    /// <typeparam name="T">Generic of value returned</typeparam>
    /// <param name="storedProcedure">name of stored procedure</param>
    /// <param name="parameters">parameters to be passed to sp</param>
    /// <returns>Generic model T</returns>
    public async Task<T?> GetFirstOrDefaultStoredProcedureQuery<T>(string storedProcedure, DynamicParameters parameters)
    {
        try
        {
            var connection = GetConnection();

            var response = await connection.QueryFirstOrDefaultAsync<T>(storedProcedure, parameters, commandType: System.Data.CommandType.StoredProcedure);

            connection.Close();

            return response;
        }
        catch (SqlException exception)
        {
            Console.WriteLine($"Get first or default query failed: {exception.Message}");
            return default;
        }
    }
    /// <summary>
    /// Generic implementation of a stored procedure for CRUD operations
    /// </summary>
    /// <param name="storedProcedure">Name of stored procedure</param>
    /// <param name="parameters">parameters for stored procedure.</param>
    /// <returns>int indicating success or failure.</returns>
    public async Task<int> CreateUpdateDeleteStoredProcedureQuery(string storedProcedure, DynamicParameters parameters)
    {
        try
        {
            var connection = GetConnection();

            var response = await connection.ExecuteAsync(storedProcedure, parameters, commandType: System.Data.CommandType.StoredProcedure);

            connection.Close();

            return response;
        }
        catch (SqlException exception)
        {
            Console.WriteLine($"Create/Update/Delete query failed: {exception.Message}");
            return 0;
        }
    }
}