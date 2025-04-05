/*
    Filename: StoreContext.cs
    Part of Project: PLOT/PLOT-BE/DataAccess/Contexts

    File Purpose:
    This file contains the database context for database operations 
    that involve stores. 
    
    Class Purpose:
    This class will be sent to the endpoint controllers as a service through 
    dependency injection (In Program.cs) and will be used in the endpoints 
    to send data to the database server from the frontend and vice versa.

    Written by: Jordan Houlihan
*/

using Dapper;
using Microsoft.Data.SqlClient;
using Plot.Data.Models.Stores;
using Plot.Data.Models.Users;
using Plot.DataAccess.Interfaces;

namespace Plot.DataAccess.Contexts;

public class StoreContext : DbContext, IStoreContext
{
    public async Task<IEnumerable<Store>> GetStores()
    {
        try
        {
            using SqlConnection connection = GetConnection();

            return await connection.QueryAsync<Store>("Select_Stores", commandType: System.Data.CommandType.StoredProcedure);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception.Message));
            return [];
        }
    }

    public async Task<IEnumerable<Store>> GetStoreById(int? storeId)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("StoreID", storeId);
            return await connection.QueryAsync<Store>("Select_Stores", parameters, commandType: System.Data.CommandType.StoredProcedure);

        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception.Message));
            return [];
        }
    }

    public async Task<int> UpdatePublicInfoStore(int storeId, UpdatePublicInfoStore updatestore)
    {
        try
        {
            using SqlConnection connection = GetConnection();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("TUID", storeId);
            parameters.Add("NAME", updatestore.NAME);
            parameters.Add("ADDRESS", updatestore.ADDRESS);
            parameters.Add("CITY", updatestore.CITY);
            parameters.Add("STATE", updatestore.STATE);
            parameters.Add("ZIP", updatestore.ZIP);
            parameters.Add("BLUEPRINT_IMAGE", updatestore.BLUEPRINT_IMAGE);
            return await connection.ExecuteAsync("Insert_Update_Stores", parameters, commandType: System.Data.CommandType.StoredProcedure);

        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception.Message));
            return 0;
        }
    }
    public async Task<int> UpdateSizeStore(int storeId, UpdateSizeStore updatestore)
    {
        try
        {
            using SqlConnection connection = GetConnection();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("TUID", storeId);
            parameters.Add("WIDTH", updatestore.WIDTH);
            parameters.Add("HEIGHT", updatestore.LENGTH);

            return await connection.ExecuteAsync("Insert_Update_Stores", parameters, commandType: System.Data.CommandType.StoredProcedure);

        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception.Message));
            return 0;
        }
    }

    public async Task<int> DeleteStoreById(int storeId)
    {
        try
        {
            using SqlConnection connection = GetConnection();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("StoreID", storeId);
            return await connection.ExecuteAsync("Delete_Store", parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception.Message));
            return 0;
        }
    }
    public async Task<IEnumerable<UserDTO>?> GetUsersAtStore(int storeid)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            var GetUsersByStore = "SELECT TUID, FIRST_NAME, LAST_NAME, EMAIL, ROLE, ACTIVE " +
                                  "FROM Users " +
                                  $"WHERE TUID IN (SELECT USER_TUID FROM Access WHERE STORE_TUID = {storeid});";

            return await connection.QueryAsync<UserDTO>(GetUsersByStore);
        }
        catch (SqlException exception)
        {
            Console.WriteLine("Database connection failed: ", exception.Message);
            return [];
        }
    }

    public async Task<int> CreateStoreEntry(CreateStore store)
    {
        try
        {
            using SqlConnection connection = GetConnection();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("NAME", store.NAME);
            parameters.Add("ADDRESS", store.ADDRESS);
            parameters.Add("CITY", store.CITY);
            parameters.Add("STATE", store.STATE);
            parameters.Add("ZIP", store.ZIP);
            parameters.Add("WIDTH", store.WIDTH);
            parameters.Add("LENGTH", store.LENGTH);
            parameters.Add("BLUEPRINT_IMAGE", store.BLUEPRINT_IMAGE);
            return await connection.ExecuteAsync("Insert_Update_Stores", parameters, commandType: System.Data.CommandType.StoredProcedure);

        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception.Message));
            return 0;
        }
    }
}