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
    public async Task<int> CreateFixtureInstance(CreateFixtureInstance fixtureInstance)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("FLOORSET_TUID", fixtureInstance.FLOORSET_TUID);
        parameters.Add("FIXTURE_TUID", fixtureInstance.FIXTURE_TUID);
        parameters.Add("XPOS", fixtureInstance.X_POS);//It is XPOS and YPOS in the procedure, but in the db model and schema it is X_POS and Y_POS
        parameters.Add("YPOS", fixtureInstance.Y_POS);
        parameters.Add("HANGER_STACK", fixtureInstance.HANGER_STACK);
        parameters.Add("ALLOCATED_LF", fixtureInstance.ALLOCATED_LF);
        parameters.Add("EDITOR_ID", fixtureInstance.EDITOR_ID);
        parameters.Add("SUPERCATEGORY_TUID", fixtureInstance.SUPERCATEGORY_TUID);
        parameters.Add("SUBCATEGORY", fixtureInstance.SUBCATEGORY);
        parameters.Add("NOTE", fixtureInstance.NOTE);

        return await CreateUpdateDeleteStoredProcedureQuery("Insert_Floorset_Fixture", parameters);
    }

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

    public async Task<int> DeleteFixtureInstanceById(int fixtureInstanceId)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("FLOORSET_FIXTURE_TUID", fixtureInstanceId);

        return await CreateUpdateDeleteStoredProcedureQuery("Delete_Floorset_Fixture", parameters);
    }

    public async Task<int> DeleteFixtureModelById(int fixtureModelId)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("FIXTURE_TUID", fixtureModelId);

        return await CreateUpdateDeleteStoredProcedureQuery("Delete_Fixture", parameters);
    }

    public async Task<IEnumerable<FixtureInstance>?> GetFixtureInstances(int floorsetId)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("FLOORSET_TUID", floorsetId);

        return await GetStoredProcedureQuery<FixtureInstance>("Select_Floorset_Fixtures", parameters);
    }

    public async Task<IEnumerable<FixtureModel>?> GetFixtureModels(int StoreId)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("STORE_TUID", StoreId);

        return await GetStoredProcedureQuery<FixtureModel>("Select_Store_Fixtures", parameters);
    }

    public async Task<int> UpdateFixtureInstanceById(UpdateFixtureInstance fixtureInstance)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("TUID", fixtureInstance.TUID);
        parameters.Add("FLOORSET_TUID", fixtureInstance.FLOORSET_TUID);
        parameters.Add("FIXTURE_TUID", fixtureInstance.FIXTURE_TUID);
        parameters.Add("EDITOR_ID", fixtureInstance.EDITOR_ID);
        parameters.Add("XPOS", fixtureInstance.X_POS);//I swear to god if someone tries to change this to X_POS again I will scream
        parameters.Add("YPOS", fixtureInstance.Y_POS);// In the procedure it is XPOS and YPOS, but in the db model and schema it is X_POS and Y_POS
        parameters.Add("HANGER_STACK", fixtureInstance.HANGER_STACK);
        parameters.Add("ALLOCATED_LF", fixtureInstance.ALLOCATED_LF);
        parameters.Add("SUPERCATEGORY_TUID", fixtureInstance.SUPERCATEGORY_TUID);
        parameters.Add("SUBCATEGORY", fixtureInstance.SUBCATEGORY);
        parameters.Add("NOTE", fixtureInstance.NOTE);

        var response = await CreateUpdateDeleteStoredProcedureQuery("Insert_Update_Floorset_Fixture", parameters);

        Console.WriteLine(response);

        return response;
    }

    public async Task<int> UpdateFixtureModelById(FixtureModel fixtureModel)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("TUID", null);
        parameters.Add("NAME", fixtureModel.NAME);
        parameters.Add("WIDTH", fixtureModel.WIDTH);
        parameters.Add("LENGTH", fixtureModel.LENGTH);
        parameters.Add("LF_CAP", fixtureModel.LF_CAP);
        parameters.Add("ICON", fixtureModel.ICON);
        parameters.Add("STORE_TUID", fixtureModel.STORE_TUID);

        return await CreateUpdateDeleteStoredProcedureQuery("Insert_Update_Fixture", parameters);
    }
}