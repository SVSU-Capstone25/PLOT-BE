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
    Edited by: Joshua Rodack
    Comments by: Josh Rodack
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
    /// <summary>
    /// Method to return all users from the database. It calls the 
    /// stored procedure to Select_Users and returns them
    /// in an IEnumberable of UserDTO models
    /// </summary>
    /// <returns>IENUMERABLE of UserDTO</returns>
    public async Task<IEnumerable<UserDTO>?> GetUsers()
    {
        return await GetStoredProcedureQuery<UserDTO>("Select_Users");
    }

    /// <summary>
    /// Passes in the Id of the user to return and 
    /// uses the associated stored procedure to grab the user.
    /// </summary>
    /// <param name="userId">the user's TUID</param>
    /// <returns>UserDTO</returns>
    public async Task<UserDTO?> GetUserById(int userId)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("USER_TUID", userId);

        return await GetFirstOrDefaultStoredProcedureQuery<UserDTO>("Select_Users", parameters);
    }
    /// <summary>
    /// Returns the user based on the associated email.
    /// </summary>
    /// <param name="userEmail">User's email</param>
    /// <returns>IENUMMERABLE of UserDTO</returns>
    public async Task<UserDTO?> GetUserByEmail(string userEmail)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("EMAIL", userEmail);

        return await GetFirstOrDefaultStoredProcedureQuery<UserDTO>("Select_User_Login", parameters);
    }
    /// <summary>
    /// Given a user's id, return the stores they work at.
    /// </summary>
    /// <param name="userid">TUID of the user</param>
    /// <returns>IEnummerable of Store models</returns>
    public async Task<IEnumerable<Store>?> GetStoresForUser(int userid)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("USER_TUID", userid);

        return await GetStoredProcedureQuery<Store>("Select_Users_Store_Access", parameters);
    }
    /// <summary>
    /// Return the stores a user doesn't work at.
    /// </summary>
    /// <param name="userid">TUID of user in question</param>
    /// <returns>Ienumerable of Store models</returns>
    public async Task<IEnumerable<Store>?> GetStoresNotForUser(int userid)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("USER_TUID", userid);

        return await GetStoredProcedureQuery<Store>("Select_Unassigned_User_Stores", parameters);
    }
    /// <summary>
    /// Updates the record of user.
    /// </summary>
    /// <param name="userId">TUID of user</param>
    /// <param name="user">New or current User information</param>
    /// <returns>An integer indicating success or failure.</returns>
    public async Task<int> UpdateUserPublicInfo(int userId, UpdatePublicInfoUser user)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("TUID", userId);
        parameters.Add("FIRST_NAME", user.FIRST_NAME);
        parameters.Add("LAST_NAME", user.LAST_NAME);
        parameters.Add("EMAIL", user.EMAIL);
        parameters.Add("ROLE_NAME", user.ROLE_NAME);

        return await CreateUpdateDeleteStoredProcedureQuery("Insert_Update_User", parameters);
    }
    /// <summary>
    /// Deletes a user
    /// </summary>
    /// <param name="userId">User TUID</param>
    /// <returns>Int indicating success or failure</returns>
    public async Task<int> DeleteUserById(int userId)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("USER_TUID", userId);

        return await CreateUpdateDeleteStoredProcedureQuery("Delete_User", parameters);
    }

    /// <summary>
    /// Updates the stores a user has access to by deleting all
    /// stores and then reinserting the records, including or lacking additions and 
    /// deletions
    /// </summary>
    /// <param name="updateAccessList">List of current records that
    /// the table should reflect</param>
    /// <returns>int indicating success</returns>
    public async Task<int> UpdateAccessList(UpdateAccessList updateAccessList)
    {

        int rowsAffected = 0;

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("USER_TUID", updateAccessList.USER_TUID);

        rowsAffected = await CreateUpdateDeleteStoredProcedureQuery("Delete_All_Access", parameters);

        foreach (int store in updateAccessList.STORE_TUIDS)
        {
            parameters = new DynamicParameters();
            parameters.Add("USER_TUID", updateAccessList.USER_TUID);
            parameters.Add("STORE_TUID", store);
            rowsAffected += await CreateUpdateDeleteStoredProcedureQuery("Insert_Access", parameters);
        }

        return rowsAffected;
    }
    /// <summary>
    /// Add a user to a store as an employee
    /// </summary>
    /// <param name="accessModel">the necessary field sfor updating
    /// employment</param>
    /// <returns>int indicating success</returns>
    public async Task<int> AddUserToStore(AccessModel accessModel)
    {

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("USER_TUID", accessModel.USER_TUID);
        parameters.Add("STORE_TUID", accessModel.STORE_TUID);
        int rowsAffected = await CreateUpdateDeleteStoredProcedureQuery("Insert_Access", parameters);

        return rowsAffected;
    }
    /// <summary>
    /// Remove a user from a store as an employee
    /// </summary>
    /// <param name="accessModel">the store and user pair</param>
    /// <returns>int indicating success or failures</returns>
    public async Task<int> DeleteUserFromStore(AccessModel accessModel)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("USER_TUID", accessModel.USER_TUID);
        parameters.Add("STORE_TUID", accessModel.STORE_TUID);
        int rowsAffected = await CreateUpdateDeleteStoredProcedureQuery("Delete_Access", parameters);

        return rowsAffected;
    }
    /// <summary>
    /// Get users by string stored procedure.
    /// </summary>
    /// <param name="usersByStringRequest">Paramters to be passed 
    /// to the stored procedure</param>
    /// <returns>IEnumerable of UserDTO models</returns>
    public async Task<IEnumerable<UserDTO>?> GetUsersByString(UsersByStringRequest usersByStringRequest)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("TUIDS", usersByStringRequest.TUIDS);

        return await GetStoredProcedureQuery<UserDTO>("Select_Users_By_String", parameters);
    }

}