/*
    Filename: SalesContext.cs
    Part of Project: PLOT/PLOT-BE/Plot/DataAccess/Contexts

    File Purpose:
    This file contains the database context for database operations 
    that involve sales and allocations.
    
    Class Purpose:
    This class will be sent to the endpoint controllers as a service through 
    dependency injection (In Program.cs) and will be used in the endpoints 
    to send data to the database server from the frontend and vice versa.

    Written by: Jordan Houlihan
*/
using Dapper;
using Microsoft.Data.SqlClient;
using Plot.Data.Models.Allocations;
using Plot.DataAccess.Interfaces;
using ClosedXML;
using System.Text.Json;

namespace Plot.DataAccess.Contexts;


public class SalesContext : DbContext, ISalesContext
{
    public Task<IEnumerable<FixtureAllocations>> GetFixtureAllocations(int floorsetId)
    {
        throw new NotImplementedException();
    }

    public async Task<int> UploadSales(int salesTuid, List<CreateFixtureAllocations> allocations)
    {
        
        var inputJson = JsonSerializer.Serialize(allocations.Select(a => new
        {
            category = a.SUPERCATEGORY,
            subCategory = a.SUBCATEGORY,
            units = a.TOTAL_SALES
        }));

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("SALES_TUID", salesTuid);
        parameters.Add("INPUT", inputJson);
        return await CreateUpdateDeleteStoredProcedureQuery("Insert_Sales_Allocations", parameters);
    }

    public async Task<IEnumerable<AllocationFulfillments>?> GetAllocationFulfillments(int floorsetId)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("FLOORSET_TUID", floorsetId);

        return await GetStoredProcedureQuery<AllocationFulfillments>("Select_Allocation_Fulfillments", parameters);
    }
}