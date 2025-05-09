/*
    Filename: FloorsetContext.cs
    Part of Project: PLOT/PLOT-BE/Plot/DataAccess/Contexts

    File Purpose:
    This file contains the database context for database operations 
    that involve floorsets. 
    
    Class Purpose:
    This class will be sent to the endpoint controllers as a service through 
    dependency injection (In Program.cs) and will be used in the endpoints 
    to send data to the database server from the frontend and vice versa.

    Written by: Jordan Houlihan
    Modified by: Josh Rodack
    Comments by: Josh Rodack
*/
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Plot.Data.Models.Floorsets;
using Plot.DataAccess.Interfaces;

namespace Plot.DataAccess.Contexts;

public class FloorsetContext : DbContext, IFloorsetContext
{
    /// <summary>
    /// Return all floorsets at a given store.
    /// </summary>
    /// <param name="storeId">Store TUID</param>
    /// <returns>IEnumerable of Floorset models</returns>
    public async Task<IEnumerable<Floorset>?> GetFloorsetsByStoreId(int storeId)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("STORE_TUID", storeId);

        return await GetStoredProcedureQuery<Floorset>("Select_Stores_Floorsets", parameters);
    }
    /// <summary>
    /// Get a single floorset by an id
    /// </summary>
    /// <param name="floorsetId">floorset id</param>
    /// <returns>a single floorset model</returns>
    public async Task<Floorset?> GetFloorsetById(int floorsetId)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("FLOORSET_TUID", floorsetId);
        return await GetFirstOrDefaultStoredProcedureQuery<Floorset>("Select_Floorsets", parameters);
    }
    /// <summary>
    /// Create floorset record in the database
    /// </summary>
    /// <param name="floorset">Floorset model to carry over field values</param>
    /// <returns>int indicating success or failure. </returns>
    public async Task<int> CreateFloorset(CreateFloorset floorset)
    {
        try
        {
            var connection = GetConnection();
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("NAME", floorset.NAME);
            parameters.Add("STORE_TUID", floorset.STORE_TUID);
            parameters.Add("DATE_CREATED", floorset.DATE_CREATED);
            parameters.Add("CREATED_BY", floorset.CREATED_BY);
            parameters.Add("DATE_MODIFIED", floorset.DATE_MODIFIED);
            parameters.Add("MODIFIED_BY", floorset.MODIFIED_BY);
            parameters.Add("FLOORSET_IMAGE", floorset.FLOORSET_IMAGE);

            return await connection.ExecuteAsync("INSERT INTO Floorsets " +
                                                 "(NAME, STORE_TUID, DATE_CREATED, CREATED_BY, DATE_MODIFIED, MODIFIED_BY, FLOORSET_IMAGE) " +
                                                 "VALUES " +
                                                 "(@NAME, @STORE_TUID, @DATE_CREATED, @CREATED_BY, @DATE_MODIFIED, @MODIFIED_BY, @FLOORSET_IMAGE);", parameters);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception.Message));
            return 0;
        }
    }
    /// <summary>
    /// Update a floorset given a floorset id
    /// </summary>
    /// <param name="floorsetId">floorset id</param>
    /// <param name="updateFloorset">values to be udpated</param>
    /// <returns>int indicating success or failure.</returns>
    public async Task<int> UpdateFloorsetById(int floorsetId, UpdatePublicInfoFloorset updateFloorset)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("TUID", floorsetId);
        parameters.Add("NAME", updateFloorset.NAME);
        parameters.Add("STORE_TUID", updateFloorset.STORE_TUID);
        parameters.Add("DATE_CREATED", updateFloorset.DATE_CREATED);
        parameters.Add("CREATED_BY", updateFloorset.CREATED_BY);
        parameters.Add("DATE_MODIFIED", updateFloorset.DATE_MODIFIED);
        parameters.Add("MODIFIED_BY", updateFloorset.MODIFIED_BY);
        parameters.Add("FLOORSET_IMAGE", updateFloorset.FLOORSET_IMAGE);

        return await CreateUpdateDeleteStoredProcedureQuery("Insert_Update_Floorset", parameters);
    }
    /// <summary>
    /// delete a floorset given an id
    /// </summary>
    /// <param name="floorsetId">floorset id</param>
    /// <returns>int indicating success or failure</returns>
    public async Task<int> DeleteFloorsetById(int floorsetId)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("FLOORSET_TUID", floorsetId);

        return await CreateUpdateDeleteStoredProcedureQuery("Delete_Floorset", parameters);
    }
    /// <summary>
    /// copy a floorset into a new record to be modified.
    /// </summary>
    /// <param name="FloorsetRef">model of floorset to be copied</param>
    /// <returns>int indicating success or failure.</returns>
    public async Task<int> CopyFloorset(FloorsetRef FloorsetRef)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("OLD_FLOORSET_TUID", FloorsetRef.TUID);
        Console.WriteLine("Old TUID " + FloorsetRef.TUID);

        return await CreateUpdateDeleteStoredProcedureQuery("Copy_Floorset", parameters);
    }
}