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

            var GetStoresSQL = "SELECT TUID, NAME, ADDRESS, CITY, STATE, ZIP, WIDTH, HEIGHT " +
                               "FROM Stores;";
            
            return await connection.QueryAsync<Store>(GetStoresSQL);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception.Message));
            return [];
        }
    }

    public async Task<IEnumerable<Store>> GetStoreById(int storeId)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            var GetStoresSQL = "SELECT * " +
                               "FROM Stores " +
                               "WHERE TUID = " + storeId + ";";
            
            return await connection.QueryAsync<Store>(GetStoresSQL);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception.Message));
            return [];
        }
    }

    public async Task<int> UpdatePublicInfoStore(int storeId, UpdatePublicInfoStore store)
    {
        // try
        // {
        //     using SqlConnection connection = GetConnection();
        //     DynamicParameters parameters = new DynamicParameters();
        //     parameters.Add("ID", floorset.TUID);
        //     parameters.Add("NAME", floorset.NAME);
        //     parameters.Add("STORE_TUID", floorset.STORE_TUID);
        //     parameters.Add("DATE_CREATED", floorset.DATE_CREATED);
        //     parameters.Add("CREATED_BY", floorset.CREATED_BY);
        //     parameters.Add("DATE_MODIFIED", floorset.DATE_MODIFIED);
        //     parameters.Add("MODIFIED_BY", floorset.MODIFIED_BY);
        //     return await connection.ExecuteAsync("Insert_Update_Floorset",parameters, commandType: CommandType.StoredProcedure);
        
        // }
        // catch (SqlException exception)
        // {
        //     Console.WriteLine(("Database connection failed: ", exception));
        //     return 0;
        // }
        throw new NotImplementedException();
    }
    public async Task<int> UpdateSizeStore(int storeId, UpdateSizeStore store)
    {
         try
        {
            using SqlConnection connection = GetConnection();

            var UpdateStoreSQL = "UPDATE Stores" +
                                "SET WIDTH = " + store.WIDTH +
                                ", HEIGHT = " + store.HEIGHT + 
                                "WHERE TUID = " + storeId;

            return await connection.ExecuteAsync(UpdateStoreSQL);
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

            var DeleteStoreSQL = "DELETE FROM Store" +
                                "WHERE TUID = " + storeId;

            return await connection.ExecuteAsync(DeleteStoreSQL);
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
        } catch (SqlException exception)
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

            var CreateStoreSQL = "INSERT INTO Stores (NAME, ADDRESS, CITY, STATE, ZIP, WIDTH, HEIGHT, BLUEPRINT_IMAGE)" +
                                "VALUES ('" + store.NAME + "','" + store.ADDRESS + "','" + 
                                store.CITY + "','" + store.STATE + "','" + store.ZIP + "','" +
                                store.WIDTH + "','" + store.HEIGHT + "','" + store.BLUEPRINT_IMAGE + "');";

            return await connection.ExecuteAsync(CreateStoreSQL);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception.Message));
            return 0;
        }
    }
}