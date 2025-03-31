/*
    Filename: IStoreContext.cs
    Part of Project: PLOT/PLOT-BE/Plot/DataAccess/Interfaces

    File Purpose:
    This file contains the interface for database operations 
    that involve stores. 
    
    Class Purpose:
    This interface will be implemented by its respective DbContext
    class for production/testing.

    Written by: Jordan Houlihan
*/

using Plot.Data.Models.Stores;

namespace Plot.DataAccess.Interfaces;

public interface IStoreContext
{
    Task<IEnumerable<Store[]>?> GetStores();
    Task<Store?> GetStoreById(int storeId);
    Task<Store[]?> GetStoresByAccess(int userId);
    Task<Store?> UpdateStoreById(int storeId, Store store);
    Task<int> DeleteStoreById(int storeId);
    Task<Store?> CreateStore(CreateStore store);
}