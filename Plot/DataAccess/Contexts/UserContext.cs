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
    }

    public async Task<UserDTO?> GetUserById(int userId)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("USER_TUID", userId);

        return await GetFirstOrDefaultStoredProcedureQuery<UserDTO>("Select_Users", parameters);
    }

    public async Task<IEnumerable<Store>?> GetStoresForUser(int userid)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("USER_TUID", userid);

        return await GetStoredProcedureQuery<Store>("Select_Users_Store_Access", parameters);
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
    }

    public async Task<int> DeleteUserById(int userId)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("USER_TUID", userId);

        return await CreateUpdateDeleteStoredProcedureQuery("Delete_User", parameters);
    }

    
    public async Task<int> UpdateAccessList(UpdateAccessList updateAccessList)
    {

        int rowsAffected = 0;

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("USER_TUID", updateAccessList.USER_TUID);

        rowsAffected = await CreateUpdateDeleteStoredProcedureQuery("Delete_All_Access", parameters);

        if (rowsAffected != 0)
        {
            foreach (int store in updateAccessList.STORE_TUIDS)
            {
                parameters = new DynamicParameters();
                parameters.Add("USER_TUID", updateAccessList.USER_TUID);
                parameters.Add("STORE_TUID", store); 
                rowsAffected += await CreateUpdateDeleteStoredProcedureQuery("Insert_Access", parameters);
            }
        }

        return rowsAffected;
    }

    public async Task<int> AddUserToStore(AccessModel accessModel)
    {

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("USER_TUID", accessModel.USER_TUID);
        parameters.Add("STORE_TUID", accessModel.STORE_TUID); 
        int rowsAffected = await CreateUpdateDeleteStoredProcedureQuery("Insert_Access", parameters);

        return rowsAffected;
    }

    public async Task<int> DeleteUserFromStore(AccessModel accessModel)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("USER_TUID", accessModel.USER_TUID);
        parameters.Add("STORE_TUID", accessModel.STORE_TUID); 
        int rowsAffected = await CreateUpdateDeleteStoredProcedureQuery("Delete_Access", parameters);

        return rowsAffected;
    }
    
}