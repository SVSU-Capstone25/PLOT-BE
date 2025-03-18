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

using Plot.Data.Models.Stores;
using Plot.DataAccess.Interfaces;

namespace Plot.DataAccess.Contexts;

public class StoreContext : DbContext, IStoreContext
{
    public Task<Store[]?> GetStores()
    {
        throw new NotImplementedException();
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