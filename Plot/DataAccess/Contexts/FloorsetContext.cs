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
using System.Data;
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
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("StoreID", storeId);

            return await connection.QueryAsync<Floorset>("Select_Store_Floorsets", parameters, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return [];
        }

        // throw new NotImplementedException();
    }
    public async Task<int> CreateFloorset(CreateFloorset floorset)
    {
        try
        {
            using SqlConnection connection = GetConnection();
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("NAME", floorset.NAME);
            parameters.Add("STORE_TUID", floorset.STORE_TUID);
            parameters.Add("DATE_CREATED", floorset.DATE_CREATED);
            parameters.Add("CREATED_BY", floorset.CREATED_BY);
            parameters.Add("DATE_MODIFIED", floorset.DATE_MODIFIED);
            parameters.Add("MODIFIED_BY", floorset.MODIFIED_BY);
            return await connection.ExecuteAsync("Insert_Update_Floorset", parameters, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return 0;
        }
        //insert into Floorset Table
        //throw new NotImplementedException();
    }

    public async Task<int> UpdateFloorsetById(int floorsetId, UpdatePublicInfoFloorset updatefloorset)
    {
        try
        {
            using SqlConnection connection = GetConnection();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("TUID", floorsetId);
            parameters.Add("NAME", updatefloorset.NAME);
            parameters.Add("STORE_TUID", updatefloorset.STORE_TUID);
            parameters.Add("DATE_CREATED", updatefloorset.DATE_CREATED);
            parameters.Add("CREATED_BY", updatefloorset.CREATED_BY);
            parameters.Add("DATE_MODIFIED", updatefloorset.DATE_MODIFIED);
            parameters.Add("MODIFIED_BY", updatefloorset.MODIFIED_BY);
            return await connection.ExecuteAsync("Insert_Update_Floorset", parameters, commandType: CommandType.StoredProcedure);

        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return 0;
        }
        //Update Statement
        //throw new NotImplementedException();
    }
    public async Task<int> DeleteFloorsetById(int floorsetId)
    {

        try
        {
            using SqlConnection connection = GetConnection();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("FloorsetID", floorsetId);
            return await connection.ExecuteAsync("Delete_Floorset", parameters, commandType: CommandType.StoredProcedure);

        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return 0;
        }
        //Delete Statement
        //throw new NotImplementedException();
    }
}