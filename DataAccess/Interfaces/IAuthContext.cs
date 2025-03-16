/*
    Filename: IAuthContext.cs
    Part of Project: PLOT/PLOT-BE/DataAccess/Interfaces

    File Purpose:
    This file contains the interface for database operations 
    that involve authentication. 
    
    Class Purpose:
    This interface will be implemented by its respective DbContext
    class for production/testing.

    Written by: Jordan Houlihan
*/

using Plot.Data.Models.Auth.Login;
using Plot.Data.Models.Auth.Registration;
using Plot.Data.Models.Users;

namespace Plot.DataAccess.Interfaces;

public interface IAuthContext
{
    Task<int> CreateUser(UserRegistration user);
    Task<User?> GetUserByEmail(string email);
    Task<int> UpdatePassword(LoginRequest user);
}