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

using Plot.Data.Models.Fixtures;
using Plot.DataAccess.Interfaces;

namespace Plot.DataAccess.Contexts;

public class FixtureContext : DbContext, IFixtureContext
{
    public Task<FixtureInstance> CreateFixtureInstance(CreateFixtureInstance fixtureInstance)
    {
         try
        {
            using SqlConnection connection = GetConnection();

            var CreateFixtureInstance = "INSERT INTO Floorsets_Fixtures (FLOORSET_TUID, " + 
                                    "FIXTURE_TUID, X_POS,Y_POS, HANGER_STACK, ALLOCATED_LF,"+
                                    " CATEGORY, NOTE )" +
                                   "VALUES ('" + fixtureInstance.FloorsetId +"','" + 
                                   fixtureInstance.FixtureId + "','" +
                                   fixtureInstance.XPosition + "','"  + fixtureInstance.YPosition + "','" +
                                   fixtureInstance.HangerStack + "','" + fixtureInstance.LFAllocated +
                                    "','" + fixtureInstance.Category + "','" + fixtureInstance.Note +"');";

            return await connection.Execute(CreateFixtureInstance);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return [];
        }
        
        //throw new NotImplementedException();
    }

    public Task<FixtureModel> CreateFixtureModel(CreateFixtureModel fixtureModel)
    {
         try
        {
            using SqlConnection connection = GetConnection();

            var CreateFixtureModel = "INSERT INTO Fixtures (NAME, " + 
                                    "WIDTH, HEIGHT, LF_CAP, ICON, STORE_TUID)"+
                                   "VALUES ('" + fixtureModel.Name +"','" + 
                                   fixtureModel.Width + "','" +
                                   fixtureModel.Height + "','"  + fixtureModel.LFCapacity + "','" +
                                   fixtureModel.FixtureImage + "','" + fixtureModel.StoreId +
                                    "');";

            return await connection.Execute(CreateFixtureModel);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return [];
        }
        //throw new NotImplementedException();
    }

    public Task<int> DeleteFixtureInstanceById(int fixtureInstanceId)
    {
         try
        {
            using SqlConnection connection = GetConnection();

            var DeleteFixtureInstance = "DELETE FROM Floorsets_Fixtures"+
                                            "WHERE TUID =" + fixtureInstanceId +";";

            return await connection.Execute(DeleteFixtureInstance);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return [];
        }
        //throw new NotImplementedException();
    }

    public Task<int> DeleteFixtureModelById(int fixtureModelId)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            var DeleteFixtureInstance = "DELETE FROM Fixtures"+
                                            "WHERE TUID =" + fixtureModelId +";";

            return await connection.Execute(DeleteFixtureInstance);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return [];
        }
        //throw new NotImplementedException();
    }

    public Task<IEnumerable<FixtureInstance>> GetFixtureInstances(int floorsetId)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            var GetFixtureInstancesbyId = "SELECT TUID As 'FixtureInstanceId', FLOORSET_TUID As 'FloorsetId'," +
                                    "FIXTURE_TUID As 'FixtureModelId', X_POS As 'XPosition'"+
                                    ", Y_POS As 'YPosition', ALLOCATED_LF As 'LFAllocated',"+
                                    "TOT_LF As 'LFTarget', HANGER_STACK As 'HangerStack',"+
                                    "CATEGORY As 'Category', NOTE As 'Note'"+
                                   "FROM Floorsets_Fixtures WHERE FLOORSET_TUID = @floorsetId;";
            

            return await connection.QueryAsync<FixtureInstance>(GetFixtureInstancesbyId);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return [];
        }
        //throw new NotImplementedException();
    }

    public Task<IEnumerable<FixtureModel>> GetFixtureModels(int StoreId)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            var GetFixtureModelsbyId = "SELECT TUID As 'FixtureId', NAME As 'Name',"+
                                    " WIDTH As 'Width', HEIGHT As 'Height', LF_CAP As 'LFCapacity" +
                                    ", ICON As 'FixtureImage', STORE_TUID As 'StoreId'"+
                                   "FROM Fixtures WHERE STORE_TUID = @StoreId;";
            

            return await connection.QueryAsync<FixtureModels>(GetFixtureModelsbyId);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return [];
        }
        //throw new NotImplementedException();
    }

    public Task<FixtureInstance> UpdateFixtureInstanceById(UpdateFixtureInstance fixtureInstance)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            var UpdateFloorset = "UPDATE Floorsets_Fixtures" + 
                                    "SET X_POS = " + fixtureInstance.XPosition +
                                    ",Y_POS = " + fixtureInstance.YPosition +
                                    ",ALLOCATED_LF = " + fixtureInstance.LFAllocated +
                                    ",HANGER_STACK = " + fixtureInstance.HangerStack + 
                                    ",TOT_LF = " + fixtureInstance.LFTarget +
                                    ",CATEGORY = " + fixtureInstance.Category+
                                    ",NOTE = " + fixtureInstance.Note +
                                    "WHERE TUID = " + fixtureInstance.FixtureInstanceId;

            return await connection.Execute(UpdateFloorset);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return [];
        }
        //throw new NotImplementedException();
    }

    public Task<FixtureModel> UpdateFixtureModelById(UpdateFixtureModel fixtureModel)
    {
        try
        {
            using SqlConnection connection = GetConnection();

            var UpdateFloorset = "UPDATE Fixtures" + 
                                    "SET NAME = " + fixtureModel.Name +
                                    ", WIDTH = " + fixtureModel.Width +
                                    ", HEIGHT = " + fixtureModel.Height +
                                    "LF_CAP = " + fixtureModel.LFCapacity +
                                    "ICON = " + fixtureModel.FixtureImage +
                                    "WHERE TUID = " + fixtureModel.FixtureId;

            return await connection.Execute(UpdateFloorset);
        }
        catch (SqlException exception)
        {
            Console.WriteLine(("Database connection failed: ", exception));
            return [];
        }
        //throw new NotImplementedException();
    }
}