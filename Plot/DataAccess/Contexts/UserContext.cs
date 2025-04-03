/*
    Filename: UserContext.cs
    Part of Project: PLOT/PLOT-BE/Plot/DataAccess/Contexts

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
using Plot.Data.Models.Stores;
using Plot.DataAccess.Interfaces;
using System.Data;

namespace Plot.DataAccess.Contexts;

public class UserContext : DbContext, IUserContext
{
    public async Task<IEnumerable<Select_User>?> GetUsers()
    {
        try
        {
            using SqlConnection connection = GetConnection();

            var GetUsersSQL = "SELECT TUID, FIRST_NAME, LAST_NAME, " +
                              "EMAIL, ACTIVE, (SELECT NAME FROM Roles WHERE TUID = ROLE_TUID) AS 'ROLE' " +
                              "FROM Users " +
                              "WHERE ACTIVE = 1;";

            return await connection.QueryAsync<Select_User>(GetUsersSQL);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception.Message));
            return null;
        }
    }

    public async Task<Select_User?> GetUserById(int userId)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            var GetUserByIdSQL = "SELECT TUID, FIRST_NAME, LAST_NAME, " +
                              "EMAIL, ACTIVE, (SELECT NAME FROM Roles WHERE TUID = ROLE_TUID) AS 'ROLE' " +
                              "FROM Users " +
                              "WHERE TUID = " + userId + ";";

            return await connection.QueryFirstAsync<Select_User>(GetUserByIdSQL);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception.Message));
            return null;
        }
    }

    public async Task<int> UpdateUserPublicInfo(int userId, Select_User user)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            // Check if user exists
            var existingUser = await connection.QueryFirstOrDefaultAsync<Select_User>(
                "SELECT * FROM Users WHERE TUID = @UserId",
                new { UserId = userId }
            );

            if (existingUser == null)
                return -1; // User not found

            // Validate Role Name
            var roleExists = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM Roles WHERE NAME = @RoleName",
                new { RoleName = user.ROLE }
            );

            if (roleExists == 0)
                return -1; // Role not found

            // Update User Info

            var UpdateUserInfoSQL = "UPDATE Users " +
                                    "SET FIRST_NAME = @FirstName, " +
                                    "LAST_NAME = @LastName " +
                                    "ROLE_TUID = (SELECT TUID FROM Roles WHERE NAME = @RoleName) " +
                                    "WHERE TUID = @UserId;";
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("FirstName", user.FIRST_NAME);
            parameters.Add("LastName", user.LAST_NAME);
            parameters.Add("RoleName", user.ROLE);
            parameters.Add("UserId", userId);

            var rowsAffected = await connection.ExecuteAsync(UpdateUserInfoSQL, parameters);

            return rowsAffected;
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception.Message));
            return -1;
        }
    }

    public async Task<int> DeleteUserById(int userId)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            // Check if user exists first
            var existingUser = await connection.QueryFirstOrDefaultAsync<Select_User>(
                "Select_Users",
                new { UserID = userId },
                commandType: CommandType.StoredProcedure
            );

            if (existingUser == null)
                return 0; // User not found


            var rowsAffected = await connection.ExecuteAsync(
                "Delete_User",
                new { UserId = userId },
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected;
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception.Message));
            return -1;
        }
    }

    public async Task<int> AddUserToStore(int userid, int storeid)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            // Check if user exists first
            var existingUser = await connection.QueryFirstOrDefaultAsync<User>(
                "Select_Users",
                new { UserID = userid },
                commandType: CommandType.StoredProcedure
            );

            if (existingUser == null)
                return -1; // User not found


            // Check if the store exists
            var storeExists = await connection.QueryFirstOrDefaultAsync<int>(
                "SELECT COUNT(1) FROM Stores WHERE TUID = @StoreId",
                new { StoreId = storeid });

            if (storeExists == 0)
                return -1; // Store not found

            // Check if the user is already assigned to the store
            var accessExists = await connection.QueryFirstOrDefaultAsync<int>(
                "SELECT COUNT(1) FROM Access WHERE USER_TUID = @UserId AND STORE_TUID = @StoreId",
                new { UserId = userid, StoreId = storeid });

            if (accessExists > 0)
                return 0; // User is already assigned

            // Assign user to store
            var rowsAffected = await connection.ExecuteAsync(
                "INSERT INTO Access (USER_TUID, STORE_TUID) VALUES (@UserId, @StoreId);",
                new { UserId = userid, StoreId = storeid });

            return rowsAffected;
        }
        catch (SqlException exception)
        {
            Console.WriteLine("Database operation failed: " + exception.Message);
            return -1;
        }
    }

    public async Task<int> DeleteUserFromStore(int userid, int storeid)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            // Check if the user exists
            var userExists = await connection.QueryFirstOrDefaultAsync<int>(
                "SELECT COUNT(1) FROM Users WHERE TUID = @UserId",
                new { UserId = userid });

            if (userExists == 0)
                return -1; // User not found

            // Check if the store exists
            var storeExists = await connection.QueryFirstOrDefaultAsync<int>(
                "SELECT COUNT(1) FROM Stores WHERE TUID = @StoreId",
                new { StoreId = storeid });

            if (storeExists == 0)
                return -1; // Store not found

            // Check if the user is already removed
            var accessExists = await connection.QueryFirstOrDefaultAsync<int>(
                "SELECT COUNT(1) FROM Access WHERE USER_TUID = @UserId AND STORE_TUID = @StoreId",
                new { UserId = userid, StoreId = storeid });

            if (accessExists == 0)
                return -1; // User is already removed

            // Assign user to store
            var rowsAffected = await connection.ExecuteAsync(
                "DELETE FROM Access WHERE USER_TUID = @UserId AND STORE_TUID = @StoreId",
                new { StoreId = storeid, UserId = userid });

            return rowsAffected;
        }
        catch (SqlException exception)
        {
            Console.WriteLine("Database operation failed: " + exception.Message);
            return -1;
        }
    }

    public async Task<IEnumerable<Store>?> GetStoresForUser(int userid)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            // Check if user exists first
            var userExists = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM Users WHERE TUID = @UserId",
                new { UserId = userid });

            if (userExists == 0)
            {
                return null; // User does not exist
            }

            // Query stores for the given user
            var GetStoresSQL = "SELECT Stores.TUID, Stores.NAME, Stores.ADDRESS, " +
                               "Stores.CITY, Stores.STATE, Stores.ZIP, Stores.WIDTH, Stores.HEIGHT, Stores.BLUEPRINT_IMAGE " +
                               "FROM Stores INNER JOIN Access " +
                               "ON Stores.TUID = Access.STORE_TUID " +
                               "WHERE Access.USER_TUID = @UserId;";

            return await connection.QueryAsync<Store>(GetStoresSQL, new { UserId = userid });
        }
        catch (SqlException exception)
        {
            Console.WriteLine("Database connection failed: " + exception.Message);
            return null;
        }
    }


}