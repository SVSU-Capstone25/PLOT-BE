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
using Microsoft.AspNetCore.Identity;
using Plot.Data.Models.Auth.Login;
using Plot.Data.Models.Auth.Registration;
using Plot.Data.Models.Users;
using Plot.DataAccess.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;

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
            var connection = GetConnection();

            int oneTimePassword = RandomNumberGenerator.GetInt32(oneTimePasswordSizeLowerBound, oneTimePasswordSizeUpperBound);
            PasswordHasher<UserRegistration> hasher = new();

            string oneTimePasswordHash = hasher.HashPassword(user, oneTimePassword.ToString());


            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("FIRST_NAME", user.FIRST_NAME);
            parameters.Add("LAST_NAME", user.LAST_NAME);
            parameters.Add("EMAIL", user.EMAIL);
            parameters.Add("PASSWORD", oneTimePasswordHash);
            parameters.Add("ROLE_NAME", user.ROLE_NAME);

            return await connection.ExecuteAsync("Insert_Update_User", parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
        catch (SqlException exception)
        {
            Console.WriteLine("Database connection failed: ", exception.Message);
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
            Console.WriteLine("Database connection failed: ", exception.Message);
            return null;
        }
    }

    public async Task<int> UpdatePassword(LoginRequest user)
    {
        try
        {
            var connection = GetConnection();

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("EMAIL", user.EMAIL);
            parameters.Add("NewPassword", user.PASSWORD);

            return await connection.ExecuteAsync("Update_User_Password", parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
        catch (SqlException exception)
        {
            Console.WriteLine("Database connection failed: ", exception.Message);
            return 0;
        }
    }
    public async Task<int> DeleteUserById(int userId)
    {
        try
        {
            var connection = GetConnection();

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("USER_TUID", userId);
            

            return await connection.ExecuteAsync("Delete_User", parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception.Message));
            return 0;
        }
    }
}