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

    public async Task<IEnumerable<UserDTO>?> GetUserById(int userId)
    {
         try
        {
            using SqlConnection connection = GetConnection();

            var GetUserByIdSQL = "SELECT TUID As 'UserId', FIRST_NAME As 'FirstName', LAST_NAME As " +
                              "'LastName', EMAIL As 'Email', ACTIVE As 'Active', ROLE_TUID As 'Role' " +
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
                                "SET FIRST_NAME = " + user.FirstName +
                                ", LAST_NAME = " + user.LastName + 
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

            var DeleteUserSQL = "DELETE FROM Users" + 
                                "WHERE TUID = " + userId;

            return await connection.ExecuteAsync(DeleteUserSQL);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return 0;
        }
    }



    public async Task<int> CreateUser(User user)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            var CreateUser = "INSERT INTO Users (FIRST_NAME, LAST_NAME, EMAIL, PASSWORD, ROLE_TUID," +
                                    " ACTIVE)" +
                                   "VALUES ('" + user.FirstName +"','" + user.LastName + "','" +
                                   user.Email + "','"  + user.Password + "','" +
                                   user.Role + "','" + user.Active + "');";

            return await connection.ExecuteAsync(CreateUser);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return 0;
        }
    }
    public async Task<int> AddUserToStore(int userid, int storeid)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            var CreateHiring = "INSERT INTO Access (USER_TUID, STORE_TUID)" +
                                   "VALUES ('" + userid+"','" + storeid + "');";

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

            var DeleteRelation = "DELETE FROM Access" +
                                   "WHERE USER_TUID = " +userid + " AND STORE_TUID = " + storeid;

            return await connection.ExecuteAsync(DeleteRelation);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return 0;
        }
    }
    public async Task<IEnumerable<UserDTO>?> GetUsersAtStore(int storeid)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            var GetUserByIdSQL = "SELECT Users.TUID As 'UserId', Users.FIRST_NAME As 'FirstName', Users.LAST_NAME As " +
                              "'LastName', Users.EMAIL As 'Email', Users.ACTIVE As 'Active', Users.ROLE_TUID As 'Role' " +
                              "FROM Users " +
                              "INNER JOIN Access " +
                              "ON Users.TUID = Access.USER_TUID " +
                              "WHERE Access.STORE_TUID = " + storeid + ";";

            return await connection.QueryAsync<UserDTO>(GetUserByIdSQL);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return [];
        }
    }
    public async Task<IEnumerable<Store>?> GetStoresForUser(int userid)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            var GetStoresSQL = "SELECT Store.TUID As 'StoreId', Store.NAME As 'Name', Store.ADDRESS As " +
                              "'Address', Store.CITY As 'City', Store.STATE As 'State', Store.ZIP As 'ZipCode', " +
                              "Store.WIDTH As 'Width', Store.HEIGHT As 'Height, Store.BLUEPRINT_IMAGE As 'BlueprintImage'" +
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