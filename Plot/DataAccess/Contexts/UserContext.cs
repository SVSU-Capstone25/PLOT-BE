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

using Dapper;
using Microsoft.Data.SqlClient;
using Plot.Data.Models.Users;
using Plot.DataAccess.Interfaces;

namespace Plot.DataAccess.Contexts;

public class UserContext : DbContext, IUserContext
{
    public async Task<IEnumerable<UserDTO>?> GetUsers()
    {
        try
        {
            using SqlConnection connection = GetConnection();

            var GetUsersSQL = "SELECT TUID As 'UserId', FIRST_NAME As 'FirstName', LAST_NAME As " +
                              "'LastName', EMAIL As 'Email', ACTIVE As 'Active', ROLE_TUID As 'Role' " +
                              "FROM Users " +
                              "WHERE ACTIVE = 1;";

            return await connection.QueryAsync<UserDTO>(GetUsersSQL);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return [];
        }
    }

    public Task<UserDTO?> GetUserById(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO?> UpdateUserPublicInfo(int userId, UpdatePublicInfoUser user)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteUserById(int userId)
    {
        throw new NotImplementedException();
    }
}