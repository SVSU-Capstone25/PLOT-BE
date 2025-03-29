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
        throw new NotImplementedException();
    }

    public Task<FixtureModel> CreateFixtureModel(CreateFixtureModel fixtureModel)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteFixtureInstanceById(int fixtureInstanceId)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteFixtureModelById(int fixtureModelId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<FixtureInstance>> GetFixtureInstances(int floorsetId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<FixtureModel>> GetFixtureModels(int floorsetId)
    {
        throw new NotImplementedException();
    }

    public Task<FixtureInstance> UpdateFixtureInstanceById(UpdateFixtureInstance fixtureInstance)
    {
        throw new NotImplementedException();
    }

    public Task<FixtureModel> UpdateFixtureModelById(UpdateFixtureModel fixtureModel)
    {
        throw new NotImplementedException();
    }
}