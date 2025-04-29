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
    Commented by: Joshua Rodack
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
    /// <summary>
    /// Creates a user in the database.
    /// </summary>
    /// <param name="user">User model to be passed to database</param>
    /// <returns>int indicating success or failure.</returns>
    public async Task<int> CreateUser(UserRegistration user)
    {
        int oneTimePassword = RandomNumberGenerator.GetInt32(oneTimePasswordSizeLowerBound, oneTimePasswordSizeUpperBound);
        PasswordHasher<UserRegistration> hasher = new();
        string oneTimePasswordHash = hasher.HashPassword(user, oneTimePassword.ToString());

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("FIRST_NAME", user.FIRST_NAME);
        parameters.Add("LAST_NAME", user.LAST_NAME);
        parameters.Add("EMAIL", user.EMAIL);
        parameters.Add("PASSWORD", oneTimePasswordHash);
        parameters.Add("ROLE_NAME", user.ROLE_NAME);

        return await CreateUpdateDeleteStoredProcedureQuery("Insert_Update_User", parameters);
    }
    /// <summary>
    /// Returns a user based on their email.
    /// </summary>
    /// <param name="email">Email string to check against database.</param>
    /// <returns>User model</returns>
    public async Task<User?> GetUserByEmail(string email)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("EMAIL", email);

        return await GetFirstOrDefaultStoredProcedureQuery<User>("Select_User_Login", parameters);
        // try
        // {
        //     using SqlConnection connection = GetConnection();

        //     var GetUserByEmailSQL = "SELECT TUID, FIRST_NAME, LAST_NAME, EMAIL, " +
        //                             "PASSWORD, (SELECT NAME FROM Roles WHERE TUID = ROLE_TUID) AS 'ROLE', ACTIVE " +
        //                             "FROM Users " +
        //                             "WHERE EMAIL = @EMAIL";
        //     object GetUserByEmailParameters = new { EMAIL = email };

        //     return await connection.QuerySingleOrDefaultAsync<User>(GetUserByEmailSQL, GetUserByEmailParameters);
        // }
        // catch (SqlException exception)
        // {
        //     Console.WriteLine("Database connection failed: ", exception.Message);
        //     return null;
        // }
    }
    /// <summary>
    /// updates the password of a user
    /// </summary>
    /// <param name="user">model holding the email of the user
    /// and the new password</param>
    /// <returns>int indicates success or failure.</returns>
    public async Task<int> UpdatePassword(LoginRequest user)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("EMAIL", user.EMAIL);
        parameters.Add("NewPassword", user.PASSWORD);

        return await CreateUpdateDeleteStoredProcedureQuery("Update_User_Password", parameters);
    }
    /// <summary>
    /// Deletes a user from the database by the user's id
    /// </summary>
    /// <param name="userId">User TUID</param>
    /// <returns>int indicating success or failure.</returns>
    public async Task<int> DeleteUserById(int userId)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("USER_TUID", userId);

        return await CreateUpdateDeleteStoredProcedureQuery("Delete_User", parameters);
    }
}