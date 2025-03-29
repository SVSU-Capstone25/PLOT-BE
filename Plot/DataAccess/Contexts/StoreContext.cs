/*
    Filename: StoreContext.cs
    Part of Project: PLOT/PLOT-BE/Plot/DataAccess/Contexts

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

            var GetStoresSQL = "SELECT TUID As 'StoreId', NAME As 'Name', ADDRESS As " +
                              "'Address', CITY As 'City', STATE As 'State', ZIP As 'ZipCode', " +
                              "WIDTH As 'Width', HEIGHT As 'Height, BLUEPRINT_IMAGE As 'BlueprintImage'" +
                              "FROM Store " +
                              "WHERE ACTIVE = 1;";
            
            return await connection.QueryAsync<Store>(GetStoresSQL);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return [];
        }
    }

    public async Task<Store?> GetStoreById(int storeId)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            var GetStoresSQL = "SELECT TUID As 'StoreId', NAME As 'Name', ADDRESS As " +
                              "'ADDRESS', CITY As 'City', STATE As 'State', ZIP As 'ZipCode', " +
                              "WIDTH As 'Width', HEIGHT As 'Height, BLUEPRINT_IMAGE As 'BlueprintImage'" +
                              "FROM Store " +
                              "WHERE TUID = " + storeId + ";";
            
            return await connection.QueryAsync<Store>(GetStoresSQL);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return [];
        }
    }

    public async Task<Store?> UpdatePublicInfoStore(int storeId, UpdatePublicInfoStore store)
    {
         try
        {
            using SqlConnection connection = GetConnection();

            var UpdateStoreSQL = "UPDATE Store" +
                                "SET NAME = " + store.Name +
                                ", ADDRESS = " + store.Address + 
                                ", CITY = " + store.City + 
                                ", ZIP = " + store.ZipCode + 
                                ", BLUEPRINT_IMAGE = " + store.BlueprintImage + 
                                "WHERE TUID = " + storeId;

            return await connection.Execute(UpdateStoreSQL);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return [];
        }
    }
      public async Task<Store?> UpdateSizeStore(int storeId, UpdateSizeStore store)
    {
         try
        {
            using SqlConnection connection = GetConnection();

            var UpdateStoreSQL = "UPDATE Store" +
                                "SET WIDTH = " + store.Width +
                                ", HEIGHT = " + store.Height + 
                                "WHERE TUID = " + storeId;

            return await connection.Execute(UpdateStoreSQL);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return [];
        }
    }

    public async Task<int> DeleteStoreById(int storeId)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            var DeleteStoreSQL = "DELETE FROM Store" +
                                "WHERE TUID = " + storeId;

            return await connection.Execute(DeleteStoreSQL);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return [];
        }
    }
}