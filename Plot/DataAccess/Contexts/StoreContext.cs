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
using Plot.DataAccess.Interfaces;

namespace Plot.DataAccess.Contexts;

public class StoreContext : DbContext, IStoreContext
{
    public async Task<IEnumerable<Store[]>?> GetStores()
    {
        try{
            using SqlConnection connection = GetConnection();

            var GetStoresSQL = "SELECT TUID As 'StoreId', NAME As 'Name', ADDRESS As " +
                          "'Address', CITY As 'City', STATE As 'State', ZIP As 'ZipCode', " +
                          " WIDTH As 'Width', HEIGHT As 'Height', BLUEPRINT_IMAGE As 'BlueprintImage'" + 
                          "FROM Stores";

            return await connection.QueryAsync<Store[]>(GetStoresSQL);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return [];
        }
        
    }

    public Task<Store?> GetStoreById(int storeId)
    {
        throw new NotImplementedException();
    }

    public Task<Store?> UpdateStoreById(int storeId, Store store)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteStoreById(int storeId)
    {
        throw new NotImplementedException();
    }
}