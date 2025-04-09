/*
    Filename: IFloorsetContext.cs
    Part of Project: PLOT/PLOT-BE/Plot/DataAccess/Interfaces

    File Purpose:
    This file contains the interface for database operations 
    that involve floorsets. 
    
    Class Purpose:
    This interface will be implemented by its respective DbContext
    class for production/testing.

    Written by: Jordan Houlihan
*/

using Plot.Data.Models.Floorsets;

namespace Plot.DataAccess.Interfaces;

public interface IFloorsetContext
{
    Task<IEnumerable<Floorset>?> GetFloorsetsByStoreId(int storeId);
    Task<int> CreateFloorset(CreateFloorset floorset);
    Task<int> UpdateFloorsetById(int floorsetId, UpdatePublicInfoFloorset updatefloorset);
    Task<int> DeleteFloorsetById(int floorsetId);
}