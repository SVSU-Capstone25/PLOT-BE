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
using Dapper;
using Microsoft.Data.SqlClient;
using Plot.Data.Models.Fixtures;
using Plot.DataAccess.Interfaces;

namespace Plot.DataAccess.Contexts;

public class FixtureContext : DbContext, IFixtureContext
{
    public async Task<int> CreateFixtureInstance(CreateFixtureInstance fixtureInstance)
    {
         try
        {
            using SqlConnection connection = GetConnection();

            var CreateFixtureInstance = "INSERT INTO Floorsets_Fixtures (FLOORSET_TUID, " + 
                                    "FIXTURE_TUID, X_POS,Y_POS, HANGER_STACK, ALLOCATED_LF, TOT_LF,"+
                                    " CATEGORY, NOTE )" +
                                   "VALUES ('" + fixtureInstance.TUID +"','" + 
                                   fixtureInstance.FIXTURE_TUID + "','" +
                                   fixtureInstance.X_POS + "','"  + fixtureInstance.Y_POS + "','" +
                                   fixtureInstance.HANGER_STACK + "','" + fixtureInstance.ALLOCATED_LF +
                                   "','" + fixtureInstance.TOT_LF +
                                    "','" + fixtureInstance.CATEGORY + "','" + fixtureInstance.NOTE +"');";

            return await connection.ExecuteAsync(CreateFixtureInstance);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return 0;
        }
        
        //throw new NotImplementedException();
    }

    public async Task<int> CreateFixtureModel(CreateFixtureModel fixtureModel)
    {
         try
        {
            using SqlConnection connection = GetConnection();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("ID", null);
            parameters.Add("NAME", fixtureModel.NAME);
            parameters.Add("WIDTH", fixtureModel.WIDTH);
            parameters.Add("HEIGHT", fixtureModel.HEIGHT);
            parameters.Add("LF_CAP", fixtureModel.LF_CAP);
            parameters.Add("ICON", fixtureModel.ICON);
            parameters.Add("STORE_ID", fixtureModel.STORE_ID);
            return await connection.ExecuteAsync("Insert_Update_Fixtures",parameters, commandType: CommandType.StoredProcedure);
        
            return await connection.ExecuteAsync(CreateFixtureModel);
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

            var DeleteFixtureInstance = "DELETE FROM Floorsets_Fixtures "+
                                            "WHERE TUID =" + fixtureInstanceId +";";

            return await connection.ExecuteAsync(DeleteFixtureInstance);
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

            var DeleteFixtureInstance = "DELETE FROM Fixtures "+
                                            "WHERE TUID =" + fixtureModelId +";";

            return await connection.ExecuteAsync(DeleteFixtureInstance);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return 0;
        }
        //throw new NotImplementedException();
    }

    public async Task<IEnumerable<FixtureInstance>> GetFixtureInstances(int floorsetId)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            var GetFixtureInstancesbyId = "SELECT * " +
                                          "FROM Floorsets_Fixtures " +
                                          "WHERE FLOORSET_TUID = @floorsetId;";
            

            return await connection.QueryAsync<FixtureInstance>(GetFixtureInstancesbyId);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return [];
        }
        //throw new NotImplementedException();
    }

    public async Task<IEnumerable<FixtureModel>> GetFixtureModels(int StoreId)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            var GetFixtureModelsbyId = "SELECT * " +
                                       "FROM Fixtures " +
                                       "WHERE STORE_TUID = " + StoreId + ";";
            

            return await connection.QueryAsync<FixtureModel>(GetFixtureModelsbyId);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return null;
        }
        //throw new NotImplementedException();
    }

    public async Task<int> UpdateFixtureInstanceById(UpdateFixtureInstance fixtureInstance)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            var UpdateFloorset = "UPDATE Floorsets_Fixtures " + 
                                    "SET X_POS = " + fixtureInstance.X_POS +
                                    ", Y_POS = " + fixtureInstance.Y_POS +
                                    ", ALLOCATED_LF = " + fixtureInstance.ALLOCATED_LF +
                                    ", HANGER_STACK = " + fixtureInstance.HANGER_STACK + 
                                    ", TOT_LF = " + fixtureInstance.TOT_LF +
                                    ", CATEGORY = " + fixtureInstance.CATEGORY +
                                    ", NOTE = " + fixtureInstance.NOTE +
                                    " WHERE TUID = " + fixtureInstance.TUID;

            return await connection.ExecuteAsync(UpdateFloorset);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return 0;
        }
        //throw new NotImplementedException();
    }

    public async Task<int> UpdateFixtureModelById(UpdateFixtureModel fixtureModel)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("ID", fixtureModel.TUID);
            parameters.Add("NAME", fixtureModel.NAME);
            parameters.Add("WIDTH", fixtureModel.WIDTH);
            parameters.Add("HEIGHT", fixtureModel.HEIGHT);
            parameters.Add("LF_CAP", fixtureModel.LF_CAP);
            parameters.Add("ICON", fixtureModel.ICON);
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