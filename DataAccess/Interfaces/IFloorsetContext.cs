/*
    Filename: IFloorsetContext.cs
    Part of Project: PLOT/PLOT-BE/DataAccess/Interfaces

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
    Task<IEnumerable<Floorset[]>?> GetFloorsetsByStoreId(int storeId);
    Task<Floorset> CreateFloorset(CreateFloorset floorset);
    Task<Floorset?> UpdateFloorsetById(int floorsetId, Floorset floorset);
    Task<int> DeleteFloorsetById(int floorsetId);
}