/*
    Filename: IFixtureContext.cs
    Part of Project: PLOT/PLOT-BE/Plot/DataAccess/Interfaces

    File Purpose:
    This file contains the interface for database operations 
    that involve fixtures. 
    
    Class Purpose:
    This interface will be implemented by its respective DbContext
    class for production/testing.

    Written by: Jordan Houlihan
*/

using Plot.Data.Models.Fixtures;

namespace Plot.DataAccess.Interfaces;

public interface IFixtureContext
{
    Task<IEnumerable<Select_Floorset_Fixtures>>? GetFixtureInstances(int floorsetId);
    Task<IEnumerable<Select_Fixtures>>? GetFixtureModels(int StoreId);
    Task<int> CreateFixtureModel(Select_Fixtures fixtureModel);
    Task<int> CreateFixtureInstance(Select_Floorset_Fixtures fixtureInstance);
    Task<int> UpdateFixtureModelById(Select_Fixtures fixtureModel);
    Task<int> UpdateFixtureInstanceById(Select_Floorset_Fixtures fixtureInstance);
    Task<int> DeleteFixtureModelById(int fixtureModelId);
    Task<int> DeleteFixtureInstanceById(int fixtureInstanceId);
}