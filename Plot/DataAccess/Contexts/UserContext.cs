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
        try
        {
            using SqlConnection connection = GetConnection();

          
            DynamicParameters parameters = new DynamicParameters();
            return await connection.QueryAsync<UserDTO>("Select_Users",parameters, commandType: CommandType.StoredProcedure);

           
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return [];
        }
    }

    public async Task<IEnumerable<UserDTO>?> GetUserById(int userId)
    {
         try
        {
            using SqlConnection connection = GetConnection();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserID", userId);
            return await connection.QueryAsync<UserDTO>("Select_Users",parameters, commandType: CommandType.StoredProcedure);

        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return [];
        }
    }

    public async Task<int> UpdateUserPublicInfo(int userId, UpdatePublicInfoUser user)
    {
         try
        {
            using SqlConnection connection = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("FIRSTNAME", user.FIRST_NAME);
            parameters.Add("LASTNAME",user.LAST_NAME);
            parameters.Add("ROLENAME", user.ROLE);
            return await connection.ExecuteAsync("Insert_Update_User",parameters, commandType: CommandType.StoredProcedure);

        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return 0;
        }
    }

    public async Task<int> DeleteUserById(int userId)
    {
        try
        {
            using SqlConnection connection = GetConnection();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId",userId);
            return await connection.ExecuteAsync("Delete_User",parameters, commandType: CommandType.StoredProcedure);
        } catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return 0;
        }
    }

    public async Task<int> DeleteUserFromStore(int userid, int storeid)
    {
        try
        {
            using SqlConnection connection = GetConnection();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("User_tuid",userid);
            parameters.Add("Store_tuid",storeid);
            // var DeleteRelation = "DELETE FROM Access " +
            //                        "WHERE USER_TUID = " +userid + " AND STORE_TUID = " + storeid;

            return await connection.ExecuteAsync("Delete_Access",parameters, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return 0;
        }
    }

     public async Task<IEnumerable<Store>?> GetStoresForUser(int userid)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            var GetStoresSQL = "SELECT Store.TUID, Store.NAME, Store.ADDRESS, " +
                              "Store.CITY, Store.STATE, Store.ZIP, " +
                              "Store.WIDTH, Store.HEIGHT, Store.BLUEPRINT_IMAGE, " +
                              "FROM Store " +
                              "INNER JOIN Access " +
                              "ON Store.TUID = Access.STORE_TUID " +
                              "WHERE Access.USER_TUID = "+ userid +";";
            
            return await connection.QueryAsync<Store>(GetStoresSQL);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return [];
        }
    }
}