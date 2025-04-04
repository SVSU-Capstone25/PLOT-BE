/*
    Filename: IUserContext.cs
    Part of Project: PLOT/PLOT-BE/Plot/DataAccess/Interfaces

    File Purpose:
    This file contains the interface for database operations 
    that involve users. 
    
    Class Purpose:
    This interface will be implemented by its respective DbContext
    class for production/testing.

    Written by: Jordan Houlihan
*/

using Plot.Data.Models.Users;
using Plot.Data.Models.Stores;

namespace Plot.DataAccess.Interfaces;

public interface IUserContext
{
    Task<IEnumerable<UserDTO>?> GetUsers();
    Task<IEnumerable<UserDTO>?> GetUserById(int userId);
    Task<int> UpdateUserPublicInfo(int userId, UpdatePublicInfoUser user);
    Task<int?> DeleteUserById(int userId);
    Task<string?> AddUserToStore(int userid, int storeid);
    Task<int> DeleteUserFromStore(int userid, int storeid);
    Task<IEnumerable<Store>?> GetStoresForUser(int userid);

}