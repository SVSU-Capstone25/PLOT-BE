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
    /// <summary>
    /// Returns the allocations for a given floorset
    /// </summary>
    /// <param name="floorsetId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<IEnumerable<FixtureAllocations>> GetFixtureAllocations(int floorsetId)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Creates the sales allocations given a list of fixture allocations and
    /// the excel file
    /// </summary>
    /// <param name="allocations">Allocation models to receive values</param>
    /// <param name="excelFile">Excel file to be parsed</param>
    /// <returns>int indicating success or failure.</returns>
    public async Task<int> UploadSales(List<CreateFixtureAllocations> allocations, CreateExcelFileModel excelFile)
    {
        //Console.WriteLine("in Upload sales");

        var inputJson = JsonSerializer.Serialize(allocations.Select(a => new
        {
            category = a.SUPERCATEGORY,
            subCategory = a.SUBCATEGORY,
            units = a.TOTAL_SALES
        }));

        DynamicParameters parameters = new DynamicParameters();


        parameters.Add("FILE_NAME", excelFile.FILE_NAME);
        parameters.Add("FILE_DATA", excelFile.FILE_DATA);
        parameters.Add("CAPTURE_DATE", excelFile.CAPTURE_DATE);
        parameters.Add("DATE_UPLOADED", excelFile.DATE_UPLOADED);
        parameters.Add("FLOORSET_TUID", excelFile.FLOORSET_TUID);
        parameters.Add("INPUT", inputJson);

        return await CreateUpdateDeleteStoredProcedureQuery("Insert_Sales_With_Allocations", parameters);
    }

    /// <summary>
    /// given a floorset, return the fullfilments of allocations
    /// </summary>
    /// <param name="floorsetId">Floorset id</param>
    /// <returns>IEnumerable of fulfilled allocations</returns>
    public async Task<IEnumerable<AllocationFulfillments>?> GetAllocationFulfillments(int floorsetId)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("FLOORSET_TUID", floorsetId);

        return await GetStoredProcedureQuery<AllocationFulfillments>("Select_Allocation_Fulfillments", parameters);
    }

    // public async Task<int> SaveFileToDatabase(CreateExcelFileModel excelFile)
    // {
    //     Console.WriteLine(excelFile.ToString());

    //     DynamicParameters parameters = new DynamicParameters();
    //     parameters.Add("FILE_NAME", excelFile.FILE_NAME);
    //     parameters.Add("FILE_DATA", excelFile.FILE_DATA);
    //     parameters.Add("CAPTURE_DATE", excelFile.CAPTURE_DATE);
    //     parameters.Add("DATE_UPLOADED", excelFile.DATE_UPLOADED);
    //     parameters.Add("FLOORSET_TUID", excelFile.FLOORSET_TUID);

    //     return await CreateUpdateDeleteStoredProcedureQuery("Insert_Sales_File", parameters);
    // }
}