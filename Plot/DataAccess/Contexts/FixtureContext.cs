/*
    Filename: FixtureContext.cs
    Part of Project: PLOT/PLOT-BE/Plot/DataAccess/Contexts

    File Purpose:
    This file contains the database context for database operations 
    that involve fixtures.
    
    Class Purpose:
    This class will be sent to the endpoint controllers as a service through 
    dependency injection (In Program.cs) and will be used in the endpoints 
    to send data to the database server from the frontend and vice versa.

    Written by: Jordan Houlihan
    Modified by: Joshua Rodack
    Comments by: Josh Rodack
*/
using System.Data;
using System.Net;
using Dapper;
using Microsoft.Data.SqlClient;
using Plot.Data.Models.Fixtures;
using Plot.DataAccess.Interfaces;

namespace Plot.DataAccess.Contexts;

public class FixtureContext : DbContext, IFixtureContext
{
    /// <summary>
    /// Create a record of a fixture on a floorset floor.
    /// </summary>
    /// <param name="fixtureInstance">Model of the values to be added to the database</param>
    /// <returns>int indicating success or failure.</returns>
    public async Task<int> CreateFixtureInstance(CreateFixtureInstance fixtureInstance)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("FLOORSET_TUID", fixtureInstance.FLOORSET_TUID);
        parameters.Add("FIXTURE_TUID", fixtureInstance.FIXTURE_TUID);
        parameters.Add("XPOS", fixtureInstance.X_POS);//It is XPOS and YPOS in the procedure, but in the db model and schema it is X_POS and Y_POS
        parameters.Add("YPOS", fixtureInstance.Y_POS);
        parameters.Add("HANGER_STACK", fixtureInstance.HANGER_STACK);
        parameters.Add("SUPERCATEGORY_TUID", fixtureInstance.SUPERCATEGORY_TUID);
        parameters.Add("SUBCATEGORY", fixtureInstance.SUBCATEGORY);
        parameters.Add("NOTE", fixtureInstance.NOTE);

        return await GetFirstOrDefaultStoredProcedureQuery<int>("Insert_Floorset_Fixture", parameters);
    }
    /// <summary>
    /// Create a model for a type of fixture that can be placed on the floorset floor.
    /// </summary>
    /// <param name="storeId">id fixture is added to</param>
    /// <param name="fixtureModel">values of the fixture</param>
    /// <returns>int indicating success or failure.</returns>
    public async Task<int> CreateFixtureModel(int storeId, CreateFixtureModel fixtureModel)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("NAME", fixtureModel.NAME);
        parameters.Add("WIDTH", fixtureModel.WIDTH);
        parameters.Add("LENGTH", fixtureModel.LENGTH);
        parameters.Add("ICON", fixtureModel.ICON);
        parameters.Add("LF_CAP", fixtureModel.LF_CAP);
        parameters.Add("STORE_TUID", storeId);

        return await CreateUpdateDeleteStoredProcedureQuery("Insert_Fixture", parameters);
    }
    /// <summary>
    /// Delete a fixture placed on a floorset by its id.
    /// </summary>
    /// <param name="fixtureInstanceId">TUID of fixture.</param>
    /// <returns>int indicating success or failure.</returns>
    public async Task<int> DeleteFixtureInstanceById(int fixtureInstanceId)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("FLOORSET_FIXTURE_TUID", fixtureInstanceId);

        var response = await CreateUpdateDeleteStoredProcedureQuery("Delete_Floorset_Fixture", parameters);

        return response;
    }
    /// <summary>
    /// remove a fixture model from a store based on id
    /// </summary>
    /// <param name="fixtureModelId">id of fixture model</param>
    /// <returns>int indicating success or failure.</returns>
    public async Task<int> DeleteFixtureModelById(int fixtureModelId)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("FIXTURE_TUID", fixtureModelId);

        return await CreateUpdateDeleteStoredProcedureQuery("Delete_Fixture", parameters);
    }
    /// <summary>
    /// Return all fixture instances on a given floorset.
    /// </summary>
    /// <param name="floorsetId">id of the floorset</param>
    /// <returns>IEnumerable of fixture instance models</returns>
    public async Task<IEnumerable<FixtureInstance>?> GetFixtureInstances(int floorsetId)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("FLOORSET_TUID", floorsetId);

        return await GetStoredProcedureQuery<FixtureInstance>("Select_Floorset_Fixtures", parameters);
    }

    /// <summary>
    /// Return all models at a store
    /// </summary>
    /// <param name="StoreId">Store TUID</param>
    /// <returns>IEnumerable of Fixture models</returns>
    public async Task<IEnumerable<FixtureModel>?> GetFixtureModels(int StoreId)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("STORE_TUID", StoreId);

        return await GetStoredProcedureQuery<FixtureModel>("Select_Store_Fixtures", parameters);
    }
    /// <summary>
    /// Update the instance of a fixture on a floorset.
    /// </summary>
    /// <param name="fixtureInstance">Values of the fixture to be updated</param>
    /// <returns>int indicating success or failure.</returns>
    public async Task<int> UpdateFixtureInstanceById(UpdateFixtureInstance fixtureInstance)
    {
        Console.WriteLine(fixtureInstance);

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("TUID", fixtureInstance.TUID);
        parameters.Add("FLOORSET_TUID", fixtureInstance.FLOORSET_TUID);
        parameters.Add("XPOS", fixtureInstance.X_POS);
        parameters.Add("YPOS", fixtureInstance.Y_POS);
        parameters.Add("HANGER_STACK", fixtureInstance.HANGER_STACK);
        parameters.Add("SUPERCATEGORY_TUID", fixtureInstance.SUPERCATEGORY_TUID);
        parameters.Add("SUBCATEGORY", fixtureInstance.SUBCATEGORY);
        parameters.Add("NOTE", fixtureInstance.NOTE);
        parameters.Add("FIXTURE_IDENTIFIER", fixtureInstance.FIXTURE_IDENTIFIER);

        var response = await CreateUpdateDeleteStoredProcedureQuery("Update_Floorset_Fixture", parameters);

        Console.WriteLine(response);

        return response;
    }
    /// <summary>
    /// Update the values of a fixture model at a store.
    /// </summary>
    /// <param name="fixtureId">id of fixture to be updated</param>
    /// <param name="fixtureModel">values to be updated</param>
    /// <returns>int indicating success or failure.</returns>
    public async Task<int> UpdateFixtureModelById(int fixtureId, CreateFixtureModel fixtureModel)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("TUID", fixtureId);
        parameters.Add("NAME", fixtureModel.NAME);
        parameters.Add("WIDTH", fixtureModel.WIDTH);
        parameters.Add("LENGTH", fixtureModel.LENGTH);
        parameters.Add("LF_CAP", fixtureModel.LF_CAP);
        parameters.Add("ICON", fixtureModel.ICON);
        parameters.Add("STORE_TUID", fixtureModel.STORE_TUID);

        return await CreateUpdateDeleteStoredProcedureQuery("Insert_Update_Fixture", parameters);
    }
    /// <summary>
    /// Add employee areas to a floorset.
    /// </summary>
    /// <param name="employeeAreaModel">Values of the employee area</param>
    /// <returns>Int indicating success or failure.</returns>
    public async Task<int> AddEmployeeAreas(AddEmployeeAreaModel employeeAreaModel)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("FLOORSET_TUID", employeeAreaModel.FLOORSET_TUID);
        parameters.Add("X_POS", employeeAreaModel.X_POS);
        parameters.Add("Y_POS", employeeAreaModel.Y_POS);

        return await CreateUpdateDeleteStoredProcedureQuery("Insert_Employee_Area", parameters);
    }
    /// <summary>
    /// Add employee areas in bulk
    /// </summary>
    /// <param name="employeeAreaModel"> Values for the table</param>
    /// <returns>int indicating success or failure. </returns>
    public async Task<int> BulkAddEmployeeAreas(BulkAddEmployeeAreaModel employeeAreaModel)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("FLOORSET_TUID", employeeAreaModel.FLOORSET_TUID);

        parameters.Add("X1_POS", employeeAreaModel.X1_POS);
        parameters.Add("Y1_POS", employeeAreaModel.Y1_POS);
        parameters.Add("X2_POS", employeeAreaModel.X2_POS);
        parameters.Add("Y2_POS", employeeAreaModel.Y2_POS);

        return await CreateUpdateDeleteStoredProcedureQuery("Bulk_Insert_Employee_Area", parameters);
    }
    /// <summary>
    /// Remove employee areas in bulk.
    /// </summary>
    /// <param name="employeeAreaModel">Values for the table.</param>
    /// <returns>int indicating success and failure</returns>
    public async Task<int> BulkDeleteEmployeeAreas(BulkDeleteEmployeeAreaModel employeeAreaModel)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("FLOORSET_TUID", employeeAreaModel.FLOORSET_TUID);

        parameters.Add("X1_POS", employeeAreaModel.X1_POS);
        parameters.Add("Y1_POS", employeeAreaModel.Y1_POS);
        parameters.Add("X2_POS", employeeAreaModel.X2_POS);
        parameters.Add("Y2_POS", employeeAreaModel.Y2_POS);

        return await CreateUpdateDeleteStoredProcedureQuery("Bulk_Delete_Employee_Area", parameters);
    }
    /// <summary>
    /// Delete a single employee area from the floorset.
    /// </summary>
    /// <param name="employeeAreaModel">Model with values for the table</param>
    /// <returns>int indicating success or failure.</returns>
    public async Task<int> DeleteEmployeeAreas(DeleteEmployeeAreaModel employeeAreaModel)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("FLOORSET_TUID", employeeAreaModel.FLOORSET_TUID);
        parameters.Add("X_POS", employeeAreaModel.X_POS);
        parameters.Add("Y_POS", employeeAreaModel.Y_POS);

        return await CreateUpdateDeleteStoredProcedureQuery("Delete_Employee_Area", parameters);
    }
    /// <summary>
    /// Return all employee areas for a given floorset.
    /// </summary>
    /// <param name="floorsetId">Floorset TUID</param>
    /// <returns>IEnumerable of Employee area models</returns>
    public async Task<IEnumerable<EmployeeAreaModel>?> GetEmployeeAreas(int floorsetId)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("FLOORSET_TUID", floorsetId);

        return await GetStoredProcedureQuery<EmployeeAreaModel>("Select_Employee_Area", parameters);
    }
}