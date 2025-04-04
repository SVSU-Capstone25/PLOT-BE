/*
    Filename: FloorsetContext.cs
    Part of Project: PLOT/PLOT-BE/Plot/DataAccess/Contexts

    File Purpose:
    This file contains the database context for database operations 
    that involve floorsets. 
    
    Class Purpose:
    This class will be sent to the endpoint controllers as a service through 
    dependency injection (In Program.cs) and will be used in the endpoints 
    to send data to the database server from the frontend and vice versa.

    Written by: Jordan Houlihan
*/
using Dapper;
using Microsoft.Data.SqlClient;
using Plot.Data.Models.Floorsets;
using Plot.DataAccess.Interfaces;

namespace Plot.DataAccess.Contexts;

public class FloorsetContext : DbContext, IFloorsetContext
{
    public async Task<IEnumerable<Floorset>> GetFloorsetsByStoreId(int storeId)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            var GetFloorsetsById = "SELECT TUID As 'FloorsetId', NAME As 'Name', " +
                                   "STORE_TUID As 'StoreId', DATE_CREATED As 'DateCreated', " +
                                   "CREATED_BY As 'CreatedBy', DATE_MODIFIED As 'DateModified', MODIFIED_BY As 'ModifiedBy' " +
                                   "FROM Floorsets WHERE STORE_TUID = @StoreId;";
            object UpdatePasswordParameters = new { StoreId = storeId };

            return await connection.QueryAsync<Floorset>(GetFloorsetsById, UpdatePasswordParameters);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return [];
        }

        // throw new NotImplementedException();
    }
    public Task<Floorset> CreateFloorset(CreateFloorset floorset)
    {
        throw new NotImplementedException();
    }

    public Task<Floorset?> UpdateFloorsetById(int floorsetId, Floorset floorset)
    {
        throw new NotImplementedException();
    }
    public Task<int> DeleteFloorsetById(int floorsetId)
    {
        throw new NotImplementedException();
    }
}