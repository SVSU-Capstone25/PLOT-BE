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

using System.Net;
using Plot.Data.Models.Fixtures;

namespace Plot.DataAccess.Interfaces;

public interface IFixtureContext
{
    Task<IEnumerable<FixtureInstance>?> GetFixtureInstances(int floorsetId);
    Task<IEnumerable<FixtureModel>?> GetFixtureModels(int StoreId);
    Task<int> CreateFixtureModel(int storeId, CreateFixtureModel fixtureModel);
    Task<int> CreateFixtureInstance(CreateFixtureInstance fixtureInstance);
    Task<int> UpdateFixtureModelById(FixtureModel fixtureModel);
    Task<int> UpdateFixtureInstanceById(UpdateFixtureInstance fixtureInstance);
    Task<int> DeleteFixtureModelById(int fixtureModelId);
    Task<int> DeleteFixtureInstanceById(int fixtureInstanceId);
    Task<int> AddEmployeeAreas(AddEmployeeAreaModel employeeAreas);
    Task<int> DeleteEmployeeAreas(DeleteEmployeeAreaModel employeeAreas);
    Task<IEnumerable<EmployeeAreaModel>?> GetEmployeeAreas(int floorsetId);
}