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
using Plot.Data.Models.Users;
namespace Plot.DataAccess.Interfaces;

public interface IStoreContext
{
    Task<IEnumerable<Store>> GetStores();
    Task<IEnumerable<Store>> GetStoreById(int storeId);
    Task<int> UpdatePublicInfoStore(int storeId, UpdatePublicInfoStore store);
    Task<int> UpdateSizeStore(int storeId, UpdateSizeStore store);
    Task<int> DeleteStoreById(int storeId);
    Task<IEnumerable<UserDTO>?> GetUsersAtStore(int storeid);
    Task<int> CreateStoreEntry(CreateStore store);
}