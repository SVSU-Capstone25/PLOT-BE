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

namespace Plot.DataAccess.Contexts;

public class SalesContext : DbContext, ISalesContext
{
    public Task<IEnumerable<FixtureAllocations>> GetFixtureAllocations(int floorsetId)
    {
        throw new NotImplementedException();
    }

    public Task<int> UploadSales(int floorsetId, IFormFile excelFile)
    {
        //Need to come back to this with more eyes


        throw new NotImplementedException();
    }

    public async Task<IEnumerable<AllocationFulfillments>?> GetAllocationFulfillments(int floorsetId)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("FLOORSET_TUID", floorsetId);

        return await GetStoredProcedureQuery<AllocationFulfillments>("Select_Allocation_Fulfillments", parameters);
    }

    public async Task<int> InsertSales(CreateExcelFileModel ExcelFileModel)
    {
        DynamicParameters parameters = new DynamicParameters();

        Console.WriteLine("Inserting with params:");
        Console.WriteLine($"FILE_NAME: {ExcelFileModel.FILE_NAME}");
        Console.WriteLine($"CAPTURE_DATE: {ExcelFileModel.CAPTURE_DATE}");
        Console.WriteLine($"DATE_UPLOADED: {ExcelFileModel.DATE_UPLOADED}");
        Console.WriteLine($"FLOORSET_TUID: {ExcelFileModel.FLOORSET_TUID}");
        Console.WriteLine($"FILE_DATA length: {ExcelFileModel.FILE_DATA?.Length ?? 0}");

        parameters.Add("TUID", null);
        parameters.Add("FILE_NAME", ExcelFileModel.FILE_NAME);
        parameters.Add("FILE_DATA", ExcelFileModel.FILE_DATA);
        parameters.Add("CAPTURE_DATE", ExcelFileModel.CAPTURE_DATE);
        parameters.Add("DATE_UPLOADED", ExcelFileModel.DATE_UPLOADED);
        parameters.Add("FLOORSET_TUID", ExcelFileModel.FLOORSET_TUID);
        
        return await CreateUpdateDeleteStoredProcedureQueryForInsertSales("Insert_Update_Sales_File", parameters);
    }

    public async Task<IEnumerable<CreateExcelFileModel>> GetSalesByFloorset(int floorsetId)
    {
        Console.WriteLine("floorset id " + floorsetId);
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("FLOORSET_TUID", floorsetId);

        // Return to this after debugging DBContext
        //return await GetStoredProcedureQueryForGetSalesByFloorset<CreateExcelFileModel>("Select_Sales_By_Floorset", parameters);
        return await GetStoredProcedureQueryForGetSalesByFloorset("Select_Sales_By_Floorset", parameters);
    }
}