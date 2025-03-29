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

            var GetFloorsetsById = "SELECT * " +
                                   "FROM Floorsets " +
                                   "WHERE STORE_TUID = @StoreId;";
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
    public async Task<int> CreateFloorset(CreateFloorset floorset)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            var CreateFloorsetSQL = "INSERT INTO Floorsets (NAME, STORE_TUID, DATE_CREATED, CREATED_BY," +
                                    " DATE_MODIFIED, MODIFIED_BY)" +
                                   "VALUES ('" + floorset.NAME +"','" + floorset.STORE_TUID + "','" +
                                   floorset.CREATED_BY + "','"  + floorset.CREATED_BY + "','" +
                                   floorset.DATE_MODIFIED + "','" + floorset.MODIFIED_BY + "');";

            return await connection.ExecuteAsync(CreateFloorsetSQL);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return 0;
        }
        //insert into Floorset Table
        //throw new NotImplementedException();
    }

    public async Task<int> UpdateFloorsetById(int floorsetId, UpdatePublicInfoFloorset floorset)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            var UpdateFloorset = "UPDATE Floorsets" + 
                                    "SET NAME = " + floorset.NAME +
                                    ", DATE_MODIFIED = " + floorset.DATE_MODIFIED +
                                    ", MODIFIED_BY = " + floorset.MODIFIED_BY + 
                                    "WHERE TUID = " + floorsetId;

            return await connection.ExecuteAsync(UpdateFloorset);
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

            var DeleteFloorset = "DELETE FROM Floorsets" + 
                                    "WHERE TUID = " + floorsetId;

            return await connection.ExecuteAsync(DeleteFloorset);
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