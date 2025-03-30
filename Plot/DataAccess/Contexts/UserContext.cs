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

namespace Plot.DataAccess.Contexts;

public class UserContext : DbContext, IUserContext
{
    public async Task<IEnumerable<UserDTO>?> GetUsers()
    {
        try
        {
            using SqlConnection connection = GetConnection();

            var GetUsersSQL = "SELECT TUID, FIRST_NAME, LAST_NAME, " +
                              "EMAIL, ACTIVE, ROLE_TUID " +
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

    public async Task<IEnumerable<UserDTO>?> GetUserById(int userId)
    {
         try
        {
            using SqlConnection connection = GetConnection();

            var GetUserByIdSQL = "SELECT TUID, FIRST_NAME, LAST_NAME, " +
                              "EMAIL, ACTIVE, ROLE_TUID " +
                              "FROM Users " +
                              "WHERE TUID = " + userId + ";";

            return await connection.QueryAsync<UserDTO>(GetUserByIdSQL);
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

            var UpdateUserSQL = "UPDATE Users" +
                                "SET FIRST_NAME = " + user.FIRST_NAME +
                                ", LAST_NAME = " + user.LAST_NAME + 
                                ", ROLE_TUID = " + user.ROLE + 
                                "WHERE TUID = " + userId + ";";

            return await connection.ExecuteAsync(UpdateUserSQL);
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
        }
    }

   
    public async Task<int> AddUserToStore(int userid, int storeid)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            var CreateHiring = "INSERT INTO Access (USER_TUID, STORE_TUID)" +
                                   "VALUES ('" + userid+"', '" + storeid + "');";

            return await connection.ExecuteAsync(CreateHiring);
        }
        catch (SqlException exception)
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