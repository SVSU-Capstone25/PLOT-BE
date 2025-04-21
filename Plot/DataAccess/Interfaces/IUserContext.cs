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
    Task<UserDTO?> GetUserById(int userId);
    Task<UserDTO?> GetUserByEmail(string userEmail);
    Task<int> UpdateUserPublicInfo(int userId, UpdatePublicInfoUser user);
    Task<int> DeleteUserById(int userId);
    Task<int> DeleteUserFromStore(AccessModel accessModel);
    Task<IEnumerable<Store>?> GetStoresForUser(int userid);
    Task<IEnumerable<Store>?> GetStoresNotForUser(int userid);
    Task<int> AddUserToStore(AccessModel accessModel);
    Task<int> UpdateAccessList(UpdateAccessList updateAccessList);
}