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
    Task<IEnumerable<Select_Store>> GetStores();
    Task<IEnumerable<Store>> GetStoreById(int storeId);
    Task<int> UpdatePublicInfoStore(Select_Store updatestore);
    Task<int> UpdateSizeStore(Select_Store sizestore);
    Task<int> DeleteStoreById(int storeId);
    Task<IEnumerable<UserDTO>?> GetUsersAtStore(int storeid);
    Task<int> CreateStoreEntry(Select_Store createstore);
}