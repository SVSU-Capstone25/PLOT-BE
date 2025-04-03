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
using System.Reflection.Metadata.Ecma335;
using Dapper;
using Microsoft.Data.SqlClient;
using Plot.Data.Models.Fixtures;
using Plot.DataAccess.Interfaces;

namespace Plot.DataAccess.Contexts;

public class FixtureContext : DbContext, IFixtureContext
{
    public async Task<int> CreateFixtureInstance(Select_Floorset_Fixtures fixtureInstance)
    {
         try
        {
            using SqlConnection connection = GetConnection();
            DynamicParameters parameters = new DynamicParameters();
            //parameters.Add("TUID", fixtureInstance.TUID);
            parameters.Add("FLOORSET_TUID", fixtureInstance.FLOORSET_TUID);
            parameters.Add("FIXTURE_TUID", fixtureInstance.FIXTURE_TUID);
            parameters.Add("X_POS", fixtureInstance.X_POS);
            parameters.Add("Y_POS", fixtureInstance.Y_POS);
            parameters.Add("HANGER_STACK", fixtureInstance.HANGER_STACK);
            parameters.Add("TOT_LF", fixtureInstance.TOT_LF);
            parameters.Add("ALLOCATED_LF", fixtureInstance.ALLOCATED_LF);
            parameters.Add("CATEGORY", fixtureInstance.CATEGORY);
            parameters.Add("NOTE", fixtureInstance.NOTE);

            return await connection.ExecuteAsync("Insert_Update_Floorset_Fixtures", parameters, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return 0;
        }
        
        //throw new NotImplementedException();
    }

    public async Task<int> CreateFixtureModel(Select_Fixtures fixtureModel)
    {
         try
        {
            using SqlConnection connection = GetConnection();
            DynamicParameters parameters = new DynamicParameters();
            //parameters.Add("TUID", null);
            parameters.Add("NAME", fixtureModel.NAME);
            parameters.Add("WIDTH", fixtureModel.WIDTH);
            parameters.Add("HEIGHT", fixtureModel.HEIGHT);
            parameters.Add("LF_CAP", fixtureModel.LF_CAP);
            parameters.Add("ICON", fixtureModel.ICON);
            parameters.Add("STORE_TUID", fixtureModel.STORE_TUID);
            return await connection.ExecuteAsync("Insert_Update_Fixtures",parameters, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return 0;
        }
        //throw new NotImplementedException();
    }

    public async Task<int> DeleteFixtureInstanceById(int fixtureInstanceId)
    {
         try
        {
            using SqlConnection connection = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("FloorsetFixtureID", fixtureInstanceId);
            return await connection.ExecuteAsync("Delete_Floorset_Fixture",parameters, commandType: CommandType.StoredProcedure);
        
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return 0;
        }
        //throw new NotImplementedException();
    }

    public async Task<int> DeleteFixtureModelById(int fixtureModelId)
    {
        try
        {
            using SqlConnection connection = GetConnection();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("FixtureID", fixtureModelId);
            
            return await connection.ExecuteAsync("Delete_Fixture",parameters, commandType: CommandType.StoredProcedure);
        
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return 0;
        }
        //throw new NotImplementedException();
    }

    public async Task<IEnumerable<Select_Floorset_Fixtures>>? GetFixtureInstances(int floorsetId)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("FloorsetID", floorsetId);
            return await connection.QueryAsync<Select_Floorset_Fixtures>("Select_Floorset_Fixtures", parameters, commandType:CommandType.StoredProcedure);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return null;
        }
        //throw new NotImplementedException();
    }

    public async Task<IEnumerable<Select_Fixtures>>? GetFixtureModels(int StoreId)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("STORE_TUID", StoreId);
            return await connection.QueryAsync<Select_Fixtures>("Select_Fixtures", parameters, commandType:CommandType.StoredProcedure);
       }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return null;
        }
        //throw new NotImplementedException();
    }

    public async Task<int> UpdateFixtureInstanceById(Select_Floorset_Fixtures fixtureInstance)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("TUID", fixtureInstance.TUID);
            parameters.Add("FLOORSET_TUID", fixtureInstance.FLOORSET_TUID);
            parameters.Add("FIXTURE_TUID", fixtureInstance.FIXTURE_TUID);
            parameters.Add("X_POS", fixtureInstance.X_POS);
            parameters.Add("Y_POS", fixtureInstance.Y_POS);
            parameters.Add("HANGER_STACK", fixtureInstance.HANGER_STACK);
            parameters.Add("TOT_LF", fixtureInstance.TOT_LF);
            parameters.Add("ALLOCATED_LF", fixtureInstance.ALLOCATED_LF);
            parameters.Add("CATEGORY", fixtureInstance.CATEGORY);
            parameters.Add("NOTE", fixtureInstance.NOTE);

            return await connection.ExecuteAsync("Insert_Update_Floorset_Fixtures", parameters, commandType: CommandType.StoredProcedure);
        
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return 0;
        }
        //throw new NotImplementedException();
    }

    public async Task<int> UpdateFixtureModelById(Select_Fixtures fixtureModel)
    {
        try
        {
            using SqlConnection connection = GetConnection();

             DynamicParameters parameters = new DynamicParameters();
            parameters.Add("TUID", null);
            parameters.Add("NAME", fixtureModel.NAME);
            parameters.Add("WIDTH", fixtureModel.WIDTH);
            parameters.Add("HEIGHT", fixtureModel.HEIGHT);
            parameters.Add("LF_CAP", fixtureModel.LF_CAP);
            parameters.Add("ICON", fixtureModel.ICON);
            parameters.Add("STORE_TUID", fixtureModel.STORE_TUID);
            return await connection.ExecuteAsync("Insert_Update_Fixtures",parameters, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return 0;
        }
        //throw new NotImplementedException();
    }
}