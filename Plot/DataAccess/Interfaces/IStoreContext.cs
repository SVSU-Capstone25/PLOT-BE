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
using Plot.Data.Models.Users;
using Plot.Data.Models.Stores;

namespace Plot.DataAccess.Interfaces;

public interface IStoreContext
{
    Task<IEnumerable<Store>?> GetStores();
    Task<Store?> GetStoreById(int? storeId);
    Task<IEnumerable<Store>?> GetByAccess(int? userId);
    Task<int> UpdatePublicInfoStore(int storeId, UpdatePublicInfoStore updatestore);
    Task<int> UpdateSizeStore(int storeId, UpdateSizeStore sizestore);
    Task<int> DeleteStoreById(int storeId);
    Task<IEnumerable<UserDTO>?> GetUsersAtStore(int storeId);
    Task<IEnumerable<UserDTO>?> GetUsersNotInStore(int storeId);
    Task<int> CreateStoreEntry(CreateStore createstore);
}