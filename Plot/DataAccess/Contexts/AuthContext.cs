/*
    Filename: AuthContext.cs
    Part of Project: PLOT/PLOT-BE/Plot/DataAccess/Contexts

    File Purpose:
    This file contains the database context for database operations 
    that involve authentication. 
    
    Class Purpose:
    This class will be sent to the endpoint controllers as a service through 
    dependency injection (In Program.cs) and will be used in the endpoints 
    to send data to the database server from the frontend and vice versa.

    Written by: Jordan Houlihan
*/

using System.Security.Cryptography;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Plot.Data.Models.Auth.Login;
using Plot.Data.Models.Auth.Registration;
using Plot.Data.Models.Users;
using Plot.DataAccess.Interfaces;

namespace Plot.DataAccess.Contexts;

public class AuthContext : DbContext, IAuthContext
{
    // CONSTANTS -- CONSTANTS -- CONSTANTS -- CONSTANTS -- CONSTANTS ------

    private readonly int oneTimePasswordSizeLowerBound = 32;
    private readonly int oneTimePasswordSizeUpperBound = 33;

    public async Task<int> CreateUser(UserRegistration user)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            int oneTimePassword = RandomNumberGenerator.GetInt32(oneTimePasswordSizeLowerBound, oneTimePasswordSizeUpperBound);
            PasswordHasher<UserRegistration> hasher = new();

            var CreateUserSQL = "INSERT INTO Users " +
                             "(FIRST_NAME, LAST_NAME, EMAIL, PASSWORD, ROLE_TUID, ACTIVE) " +
                             "VALUES " +
                             "(@FIRST_NAME, @LAST_NAME, @Email, @Password, @ROLE_TUID, @ACTIVE)";
            object CreateUserParameters = new
            {
                FIRST_NAME = user.FIRST_NAME,
                LAST_NAME = user.LAST_NAME,
                EMAIL = user.EMAIL,
                PASSWORD = hasher.HashPassword(user, oneTimePassword.ToString()),
                ROLE_TUID = user.ROLE,
                ACTIVE = true
            };

            return await connection.ExecuteAsync(CreateUserSQL, CreateUserParameters);
        }
        catch (SqlException exception)
        {
            Console.WriteLine("Database connection failed: ", exception);
            return 0;
        }
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            var GetUserByEmailSQL = "SELECT TUID, FIRST_NAME, LAST_NAME, EMAIL, " + 
                                    "PASSWORD, (SELECT NAME FROM Roles WHERE TUID = ROLE_TUID) AS 'ROLE', ACTIVE " +
                                    "FROM Users " +
                                    "WHERE EMAIL = @EMAIL";
            object GetUserByEmailParameters = new { EMAIL = email };

            return await connection.QuerySingleOrDefaultAsync<User>(GetUserByEmailSQL, GetUserByEmailParameters);
        }
        catch (SqlException exception)
        {
            Console.WriteLine("Database connection failed: ", exception);
            return null;
        }
    }

    public async Task<int> UpdatePassword(LoginRequest user)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            var UpdatePasswordSQL = "UPDATE Users " +
                                    "SET PASSWORD = @Password " +
                                    "WHERE EMAIL = @Email";
            object UpdatePasswordParameters = new { EMAIL = user.EMAIL, PASSWORD = user.PASSWORD };

            return await connection.ExecuteAsync(UpdatePasswordSQL, UpdatePasswordParameters);
        }
        catch (SqlException exception)
        {
            Console.WriteLine("Database connection failed: ", exception);
            return 0;
        }
    }
        public async Task<int> DeleteUserById(int userId)
    {
         try
        {
            using SqlConnection connection = GetConnection();

            var DeleteUserSQL = "UPDATE Users " + 
                                "SET ACTIVE = 0 "+
                                "WHERE TUID = " + userId;

            return await connection.ExecuteAsync(DeleteUserSQL);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return 0;
        }
    }
}