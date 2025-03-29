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
        try
        {
            using SqlConnection connection = GetConnection();

            var CreateFloorset = "INSERT INTO Floorsets (NAME, STORE_TUID, DATE_CREATED, CREATED_BY," +
                                    " DATE_MODIFIED, MODIFIED_BY)" +
                                   "VALUES ('" + floorset.Name +"','" + floorset.storeId + "','" +
                                   floorset.DateCreated + "','"  + floorset.CreatedBy + "','" +
                                   floorset.DateModified + "','" + floorset.ModifiedBy + "');";

            return await connection.Execute(CreateFloorset);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return [];
        }
        //insert into Floorset Table
        //throw new NotImplementedException();
    }

    public Task<Floorset?> UpdateFloorsetById(int floorsetId, UpdatePublicInfoFloorset floorset)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            var UpdateFloorset = "UPDATE Floorsets" + 
                                    "SET NAME = " + floorset.Name +
                                    ", DATE_MODIFIED = " + floorset.DateModified +
                                    ", MODIFIED_BY = " + floorset.ModifiedBy + 
                                    "WHERE TUID = " + floorsetId;

            return await connection.Execute(UpdateFloorset);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return [];
        }
        //Update Statement
        //throw new NotImplementedException();
    }
    public Task<int> DeleteFloorsetById(int floorsetId)
    {

        try
        {
            using SqlConnection connection = GetConnection();

            var DeleteFloorset = "DELETE FROM Floorsets" + 
                                    "WHERE TUID = " + floorsetId;

            return await connection.Execute(DeleteFloorset);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return [];
        }
        //Delete Statement
        //throw new NotImplementedException();
    }
}