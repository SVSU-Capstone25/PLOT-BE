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
using Dapper;
using Microsoft.Data.SqlClient;
using Plot.Data.Models.Fixtures;
using Plot.DataAccess.Interfaces;

namespace Plot.DataAccess.Contexts;

public class FixtureContext : DbContext, IFixtureContext
{
    public async Task<int> CreateFixtureInstance(FixtureInstance fixtureInstance)
    {
        try
        {
            var connection = GetConnection();
            DynamicParameters parameters = new DynamicParameters();
            //parameters.Add("TUID", fixtureInstance.TUID);
            parameters.Add("FLOORSET_TUID", fixtureInstance.FLOORSET_TUID);
            parameters.Add("FIXTURE_TUID", fixtureInstance.FIXTURE_TUID);
            parameters.Add("XPOS", fixtureInstance.X_POS);
            parameters.Add("YPOS", fixtureInstance.Y_POS);
            parameters.Add("HANGER_STACK", fixtureInstance.HANGER_STACK);
            parameters.Add("TOT_LF", fixtureInstance.TOT_LF);
            parameters.Add("ALLOCATED_LF", fixtureInstance.ALLOCATED_LF);
            parameters.Add("EDITOR_ID", fixtureInstance.EDITOR_ID);
            parameters.Add("SUPERCATEGORY_TUID", fixtureInstance.SUPERCATEGORY_TUID);
            parameters.Add("SUBCATEGORY", fixtureInstance.SUBCATEGORY);
            parameters.Add("NOTE", fixtureInstance.NOTE);

            return await connection.ExecuteAsync("Insert_Update_Floorset_Fixture", parameters, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception.Message));
            return 0;
        }

        //throw new NotImplementedException();
    }

    public async Task<int> CreateFixtureModel(FixtureModel fixtureModel)
    {
        try
        {
            var connection = GetConnection();
            DynamicParameters parameters = new DynamicParameters();
            //parameters.Add("TUID", null);
            parameters.Add("NAME", fixtureModel.NAME);
            parameters.Add("WIDTH", fixtureModel.WIDTH);
            parameters.Add("LENGTH", fixtureModel.LENGTH);
            parameters.Add("LF_CAP", fixtureModel.LF_CAP);
            parameters.Add("ICON", fixtureModel.ICON);
            parameters.Add("STORE_TUID", fixtureModel.STORE_TUID);

            return await connection.ExecuteAsync("Insert_Update_Fixture", parameters, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception.Message));
            return 0;
        }
        //throw new NotImplementedException();
    }

    public async Task<int> DeleteFixtureInstanceById(int fixtureInstanceId)
    {
        try
        {
            var connection = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("FLOORSET_FIXTURE_TUID", fixtureInstanceId);

            return await connection.ExecuteAsync("Delete_Floorset_Fixture", parameters, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception.Message));
            return 0;
        }
        //throw new NotImplementedException();
    }

    public async Task<int> DeleteFixtureModelById(int fixtureModelId)
    {
        try
        {
            var connection = GetConnection();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("FIXTURE_TUID", fixtureModelId);

            return await connection.ExecuteAsync("Delete_Fixture", parameters, commandType: CommandType.StoredProcedure);

        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception.Message));
            return 0;
        }
        //throw new NotImplementedException();
    }

    public async Task<IEnumerable<FixtureInstance>?> GetFixtureInstances(int floorsetId)
    {
        try
        {
            var connection = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("FLOORSET_TUID", floorsetId);

            return await connection.QueryAsync<FixtureInstance>("Select_Floorset_Fixtures", parameters, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception.Message));
            return null;
        }
        //throw new NotImplementedException();
    }

    public async Task<IEnumerable<FixtureModel>?> GetFixtureModels(int StoreId)
    {
        try
        {
            var connection = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("STORE_TUID", StoreId);
            return await connection.QueryAsync<FixtureModel>("Select_Store_Fixtures", parameters, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception.Message));
            return null;
        }
        //throw new NotImplementedException();
    }

    public async Task<int> UpdateFixtureInstanceById(FixtureInstance fixtureInstance)
    {
        try
        {
            var connection = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("TUID", fixtureInstance.TUID);
            parameters.Add("FLOORSET_TUID", fixtureInstance.FLOORSET_TUID);
            parameters.Add("FIXTURE_TUID", fixtureInstance.FIXTURE_TUID);
            parameters.Add("XPOS", fixtureInstance.X_POS);
            parameters.Add("YPOS", fixtureInstance.Y_POS);
            parameters.Add("HANGER_STACK", fixtureInstance.HANGER_STACK);
            parameters.Add("TOT_LF", fixtureInstance.TOT_LF);
            parameters.Add("ALLOCATED_LF", fixtureInstance.ALLOCATED_LF);
            parameters.Add("EDITOR_ID", fixtureInstance.EDITOR_ID);
            parameters.Add("SUPERCATEGORY_TUID", fixtureInstance.SUPERCATEGORY_TUID);
            parameters.Add("SUBCATEGORY", fixtureInstance.SUBCATEGORY);
            parameters.Add("NOTE", fixtureInstance.NOTE);

            return await connection.ExecuteAsync("Insert_Update_Floorset_Fixture", parameters, commandType: CommandType.StoredProcedure);

        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception.Message));
            return 0;
        }
        //throw new NotImplementedException();
    }

    public async Task<int> UpdateFixtureModelById(FixtureModel fixtureModel)
    {
        try
        {
            var connection = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("TUID", null);
            parameters.Add("NAME", fixtureModel.NAME);
            parameters.Add("WIDTH", fixtureModel.WIDTH);
            parameters.Add("LENGTH", fixtureModel.LENGTH);
            parameters.Add("LF_CAP", fixtureModel.LF_CAP);
            parameters.Add("ICON", fixtureModel.ICON);
            parameters.Add("STORE_TUID", fixtureModel.STORE_TUID);
            return await connection.ExecuteAsync("Insert_Update_Fixture", parameters, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception.Message));
            return 0;
        }
        //throw new NotImplementedException();
    }
}