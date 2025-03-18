/*
    Filename: IUserContext.cs
    Part of Project: PLOT/PLOT-BE/DataAccess/Interfaces

    File Purpose:
    This file contains the interface for database operations 
    that involve users. 
    
    Class Purpose:
    This interface will be implemented by its respective DbContext
    class for production/testing.

    Written by: Jordan Houlihan
*/

using Plot.Data.Models.Users;

namespace Plot.DataAccess.Interfaces;

public interface IUserContext
{
    Task<UserDTO[]?> GetUsers();
    Task<UserDTO?> GetUserById(int userId);
    Task<UserDTO?> UpdateUserPublicInfo(int userId, UpdatePublicInfoUser user);
    Task<int> DeleteUserById(int userId);
}