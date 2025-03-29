/*
    Filename: SalesContext.cs
    Part of Project: PLOT/PLOT-BE/Plot/DataAccess/Contexts

    File Purpose:
    This file contains the database context for database operations 
    that involve sales and allocations.
    
    Class Purpose:
    This class will be sent to the endpoint controllers as a service through 
    dependency injection (In Program.cs) and will be used in the endpoints 
    to send data to the database server from the frontend and vice versa.

    Written by: Jordan Houlihan
*/
using Dapper;
using Microsoft.Data.SqlClient;
using Plot.Data.Models.Allocations;
using Plot.DataAccess.Interfaces;

namespace Plot.DataAccess.Contexts;

public class SalesContext : DbContext, ISalesContext
{
    public async Task<IEnumerable<FixtureAllocations>> GetFixtureAllocations(int floorsetId)
    {
         try
        {
            using SqlConnection connection = GetConnection();

            var GetFixtureInstacesSQL = "SELECT TOT_LF, ALLOCATED_LF ";
            
            return await connection.QueryAsync<FixtureAllocations>(GetFixtureInstacesSQL);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return [];
        }
    }

    public Task<int> UploadSales(int floorsetId, IFormFile excelFile)
    {
        //Need to come back to this with more eyes
        throw new NotImplementedException();
    }
}