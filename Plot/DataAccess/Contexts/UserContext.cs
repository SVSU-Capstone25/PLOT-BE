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
    public async Task<IEnumerable<UserDTO>?> GetUsers()
    {
        return await GetStoredProcedureQuery<UserDTO>("Select_Users");
        
        // try
        // {
        //     var connection = GetConnection();
        //     DynamicParameters parameters = new DynamicParameters();
        //     return await connection.QueryAsync<UserDTO>("Select_Users", parameters, commandType: CommandType.StoredProcedure);
        // }
        // catch (SqlException exception)
        // {
        //     Console.WriteLine(("Database connection failed: ", exception.Message));
        //     return null;
        // }
    }

    public async Task<UserDTO?> GetUserById(int userId)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("USER_TUID", userId);

        return await GetFirstOrDefaultStoredProcedureQuery<UserDTO>("Select_Users", parameters);

        // try
        // {
        //     var connection = GetConnection();
        //     DynamicParameters parameters = new DynamicParameters();
        //     parameters.Add("USER_TUID", user.TUID);
        //     return await connection.QueryFirstOrDefaultAsync<UserDTO>("Select_Users", parameters, commandType: CommandType.StoredProcedure);
        // }
        // catch (SqlException exception)
        // {
        //     Console.WriteLine(("Database connection failed: ", exception.Message));
        //     return null;
        // }
    }

    public async Task<IEnumerable<Store>?> GetStoresForUser(int userid)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("USER_TUID", userid);

        return await GetStoredProcedureQuery<Store>("Select_Users_Store_Access", parameters);
        // try
        // {
        //     var connection = GetConnection();
        //     DynamicParameters parameters = new DynamicParameters();
        //     parameters.Add("USER_TUID", userid);
        //     return await connection.QueryAsync<Store>("Select_Users_Store_Access", parameters, commandType: CommandType.StoredProcedure);
        // }
        // catch (SqlException exception)
        // {
        //     Console.WriteLine(("Database connection failed: ", exception.Message));
        //     return [];
        // }
    }

    public async Task<int> AddUserToStore(AccessModel addUser)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("USER_TUID", addUser.USER_TUID);
        parameters.Add("STORE_TUID", addUser.STORE_TUID);

        return await CreateUpdateDeleteStoredProcedureQuery("Insert_Access", parameters);

        // try
        // {
        //     var connection = GetConnection();
        //     DynamicParameters parameters = new DynamicParameters();
        //     parameters.Add("USER_TUID", addUser.USER_TUID);
        //     parameters.Add("STORE_TUID", addUser.STORE_TUID);

        //     return await connection.ExecuteAsync("Insert_Access", parameters, commandType: CommandType.StoredProcedure);
        // }
        // catch (SqlException exception)
        // {
        //     Console.WriteLine(("Database connection failed: ", exception.Message));
        //     return 0;
        // }
    }

    public async Task<int> UpdateUserPublicInfo(int userId, UpdatePublicInfoUser user)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("TUID", userId);
        parameters.Add("FIRST_NAME", user.FIRST_NAME);
        parameters.Add("LAST_NAME", user.LAST_NAME);
        parameters.Add("EMAIL", user.EMAIL);
        parameters.Add("ROLE_NAME", user.ROLE_NAME);   
     
        return await CreateUpdateDeleteStoredProcedureQuery("Insert_Update_User", parameters);

        // try
        // {
        //     var connection = GetConnection();
        //     DynamicParameters parameters = new DynamicParameters();
        //     parameters.Add("FIRST_NAME", user.FIRST_NAME);
        //     parameters.Add("LAST_NAME", user.LAST_NAME);
        //     return await connection.ExecuteAsync("Insert_Update_User", parameters, commandType: CommandType.StoredProcedure);
        // }
        // catch (SqlException exception)
        // {
        //     Console.WriteLine(("Database connection failed: ", exception.Message));
        //     return 0;
        // }
    }

    public async Task<int> DeleteUserById(int userId)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("USER_TUID", userId);

        return await CreateUpdateDeleteStoredProcedureQuery("Delete_User", parameters);

        // try
        // {
        //     var connection = GetConnection();
        //     DynamicParameters parameters = new DynamicParameters();
        //     parameters.Add("USER_TUID", userId);
        //     return await connection.ExecuteAsync("Delete_User", parameters, commandType: CommandType.StoredProcedure);
        // }
        // catch (SqlException exception)
        // {
        //     Console.WriteLine(("Database connection failed: ", exception.Message));
        //     return 0;
        // }
    }

    public async Task<int> DeleteUserFromStore(int userid, int storeid)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("USER_TUID", userid);
        parameters.Add("STORE_TUID", storeid);

        return await CreateUpdateDeleteStoredProcedureQuery("Delete_Access", parameters);
        
        // try
        // {
        //     var connection = GetConnection();
        //     DynamicParameters parameters = new DynamicParameters();
        //     parameters.Add("USER_TUID", userid);
        //     parameters.Add("STORE_TUID", storeid);
        //     // var DeleteRelation = "DELETE FROM Access " +
        //     //                        "WHERE USER_TUID = " +userid + " AND STORE_TUID = " + storeid;

        //     return await connection.ExecuteAsync("Delete_Access", parameters, commandType: CommandType.StoredProcedure);
        // }
        // catch (SqlException exception)
        // {
        //     Console.WriteLine(("Database connection failed: ", exception.Message));
        //     return 0;
        // }
    }
}