/*
    Filename: UserContext.cs
    Part of Project: PLOT/PLOT-BE/DataAccess/Contexts

    File Purpose:
    This file contains the database context for database operations 
    that involve users.
    
    Class Purpose:
    This class will be sent to the endpoint controllers as a service through 
    dependency injection (In Program.cs) and will be used in the endpoints 
    to send data to the database server from the frontend and vice versa.

    Written by: Jordan Houlihan
*/

using Plot.Data.Models.Users;
using Plot.DataAccess.Interfaces;

namespace Plot.DataAccess.Contexts;

public class UserContext : DbContext, IUserContext
{
    public Task<UserDTO[]> GetUsers()
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO?> GetUserById(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO?> UpdateUserPublicInfo(UpdateUser user)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteUserById(int userId)
    {
        throw new NotImplementedException();
    }
}