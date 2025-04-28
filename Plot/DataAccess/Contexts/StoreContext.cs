/*
    Filename: StoreContext.cs
    Part of Project: PLOT/PLOT-BE/DataAccess/Contexts

    File Purpose:
    This file contains the database context for database operations 
    that involve stores. 
    
    Class Purpose:
    This class will be sent to the endpoint controllers as a service through 
    dependency injection (In Program.cs) and will be used in the endpoints 
    to send data to the database server from the frontend and vice versa.

    Written by: Jordan Houlihan
    Edited by: Joshua Rodack
*/

using Dapper;
using Microsoft.Data.SqlClient;
using Plot.Data.Models.Stores;
using Plot.Data.Models.Users;
using Plot.DataAccess.Interfaces;

namespace Plot.DataAccess.Contexts;

public class StoreContext : DbContext, IStoreContext
{
    /// <summary>
    /// Returns all stores
    /// </summary>
    /// <returns>IEnumerable of Store models</returns>
    public async Task<IEnumerable<Store>?> GetStores()
    {
        return await GetStoredProcedureQuery<Store>("Select_Stores");
    }

    /// <summary>
    /// Returns a specific store by id
    /// </summary>
    /// <param name="storeId">Store TUID</param>
    /// <returns>Returns a single store model</returns>
    public async Task<Store?> GetStoreById(int? storeId)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("STORE_TUID", storeId);

        return await GetFirstOrDefaultStoredProcedureQuery<Store>("Select_Stores", parameters);
        // try
        // {
        //     var connection = GetConnection();

        //     DynamicParameters parameters = new DynamicParameters();
        //     parameters.Add("STORE_TUID", storeId);
        //     return await connection.QueryFirstOrDefaultAsync<Store>("Select_Stores", parameters, commandType: System.Data.CommandType.StoredProcedure);

        // }
        // catch (SqlException exception)
        // {
        //     Console.WriteLine(("Database connection failed: ", exception.Message));
        //     return null;
        // }
    }

    /// <summary>
    /// Return the stores a user has access to based on role
    /// </summary>
    /// <param name="userId">User id in question</param>
    /// <returns>IEnumerable of Store models</returns>
    public async Task<IEnumerable<Store>?> GetByAccess(int? userId)
    {//grab stores a user works at

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("USER_TUID", userId);

        return await GetStoredProcedureQuery<Store>("Select_Users_Store_Access", parameters);
    }
    /// <summary>
    /// Update the record of a store in the database
    /// based on the id passed in
    /// </summary>
    /// <param name="storeId">Id of store to be updated.</param>
    /// <param name="updateStore">The model holding all the updated information</param>
    /// <returns>int indicating success or failure.</returns>
    public async Task<int> UpdatePublicInfoStore(int storeId, UpdatePublicInfoStore updateStore)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("TUID", storeId);
        parameters.Add("NAME", updateStore.NAME);
        parameters.Add("ADDRESS", updateStore.ADDRESS);
        parameters.Add("CITY", updateStore.CITY);
        parameters.Add("STATE", updateStore.STATE);
        parameters.Add("ZIP", updateStore.ZIP);
        parameters.Add("WIDTH", updateStore.WIDTH);
        parameters.Add("LENGTH", updateStore.LENGTH);
        parameters.Add("BLUEPRINT_IMAGE", updateStore.BLUEPRINT_IMAGE);
        parameters.Add("USER_TUIDS", updateStore.USER_TUIDS);

        return await CreateUpdateDeleteStoredProcedureQuery("Insert_Update_Store", parameters);
    }
    /// <summary>
    /// Update the size of the store
    /// </summary>
    /// <param name="storeId">id of store</param>
    /// <param name="updateStore">updated dimensions</param>
    /// <returns>int indicating success or failure</returns>
    public async Task<int> UpdateSizeStore(int storeId, UpdateSizeStore updateStore)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("TUID", storeId);
        parameters.Add("WIDTH", updateStore.WIDTH);
        parameters.Add("LENGTH", updateStore.LENGTH);

        return await CreateUpdateDeleteStoredProcedureQuery("Insert_Update_Store", parameters);
    }
    /// <summary>
    /// Delete store from database.
    /// </summary>
    /// <param name="storeId">TUID of store to be updated.</param>
    /// <returns>int indicating success or failure.</returns>
    public async Task<int> DeleteStoreById(int storeId)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("STORE_TUID", storeId);

        return await CreateUpdateDeleteStoredProcedureQuery("Delete_Store", parameters);
    }
    /// <summary>
    /// Returns the users that work at a store.
    /// </summary>
    /// <param name="storeId">Store ID</param>
    /// <returns>Ienumerable of UserDTO models</returns>
    public async Task<IEnumerable<UserDTO>?> GetUsersAtStore(int storeId)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("STORE_TUID", storeId);

        return await GetStoredProcedureQuery<UserDTO>("Select_Store_Users", parameters);
    }

    public async Task<int> CreateStoreEntry(CreateStore createstore)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("NAME", createstore.NAME);
        parameters.Add("ADDRESS", createstore.ADDRESS);
        parameters.Add("CITY", createstore.CITY);
        parameters.Add("STATE", createstore.STATE);
        parameters.Add("ZIP", createstore.ZIP);
        parameters.Add("WIDTH", createstore.WIDTH);
        parameters.Add("LENGTH", createstore.LENGTH);
        parameters.Add("BLUEPRINT_IMAGE", createstore.BLUEPRINT_IMAGE);
        parameters.Add("USER_TUIDS", createstore.USER_TUIDS);

        return await CreateUpdateDeleteStoredProcedureQuery("Insert_Update_Store", parameters);
    }
    /// <summary>
    /// Returns all users who don't work at a specific store.
    /// </summary>
    /// <param name="storeId">id of store in question</param>
    /// <returns>IEnumerable of UserDTO models</returns>
    public async Task<IEnumerable<UserDTO>?> GetUsersNotInStore(int storeId)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("STORE_TUID", storeId);

        return await GetStoredProcedureQuery<UserDTO>("Select_Users_Not_Assigned_To_Store", parameters);
    }


}