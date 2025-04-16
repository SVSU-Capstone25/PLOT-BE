/*
    Filename: ISalesContext.cs
    Part of Project: PLOT/PLOT-BE/Plot/DataAccess/Interfaces

    File Purpose:
    This file contains the interface for database operations 
    that involve sales/allocations.
    
    Class Purpose:
    This interface will be implemented by its respective DbContext
    class for production/testing.

    Written by: Jordan Houlihan
*/

using Plot.Data.Models.Allocations;

namespace Plot.DataAccess.Interfaces;

public interface ISalesContext
{
    Task<int> UploadSales(int floorsetId, IFormFile excelFile);
    Task<IEnumerable<FixtureAllocations>> GetFixtureAllocations(int floorsetId);

    Task<IEnumerable<AllocationFulfillments>?> GetAllocationFulfillments(int floorsetId);
}