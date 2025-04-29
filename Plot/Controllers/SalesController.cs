/*
    Filename: SalesController.cs
    Part of Project: PLOT/PLOT-BE/Plot/Controllers

    File Purpose:
    This file contains the sales controller endpoint mapping,
    which will upload/process sales data from the frontend/database.
    
    Written by: Jordan Houlihan, Clayton Cook, Joshua Rodack
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plot.DataAccess.Interfaces;
using Plot.Services;
using Plot.Data.Models.Allocations;
using ClosedXML.Excel;
using System.Text.RegularExpressions;

namespace Plot.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISalesContext _salesContext;
    private readonly ClaimParserService _claimParserService;

    /// <summary>
    /// Constructor for the sales controller, initializes
    /// the services and context via dependency injection.
    /// </summary>
    /// <param name="salesContext">Database context for the sales allocations</param>
    /// <param name="claimParserService">Service for reading the claims on the auth tokens</param>
    public SalesController(ISalesContext salesContext, ClaimParserService claimParserService)
    {
        _salesContext = salesContext;
        _claimParserService = claimParserService;
    }

    /// <summary>
    /// Endpoint deals with uploading sales for a floorset.
    /// </summary>
    /// <param name="floorsetId">Floorset id</param>
    /// <param name="file">Excel file</param>
    /// <returns>Created sales allocations</returns>
    [Authorize(Policy = "Manager")]
    [HttpPost("upload-sales/{floorsetId:int}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<CreateFixtureAllocations>>> UploadSales(int floorsetId, [FromForm] IFormFile file)
    {
        if (file == null)
        {
            return BadRequest();
        }

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);

        var fileData = memoryStream.ToArray(); // capture before Excel use
        memoryStream.Position = 0;

        using var workbook = new XLWorkbook(memoryStream);
        var worksheet = workbook.Worksheet(1);

        //Save file shit------------------------------------------------------
        var captureDate = worksheet.Cell(1, 11).Value.ToString();

        var match = Regex.Match(captureDate, @"\b\d{1,2}/\d{1,2}/\d{4}\b");

        DateTime dateUploaded = DateTime.MinValue;

        if (match.Success)
        {
            string dateOnly = match.Value;
            dateUploaded = DateTime.Parse(dateOnly);
        }

        //Add datetime to the file name so that the 
        // file name is alwayse unique across all stores.
        // for some reason the DB fails if a file has the same name
        // across the entire. 
        var fileName = DateTime.Now + "-" + file.FileName;

        var saveExcelFile = new CreateExcelFileModel
        {
            FILE_NAME = fileName,
            FILE_DATA = fileData,
            CAPTURE_DATE = dateUploaded,
            DATE_UPLOADED = DateTime.Today,
            FLOORSET_TUID = floorsetId
        };

        var rows = worksheet.RowsUsed().Skip(6);

        List<CreateFixtureAllocations> allocations = [];

        foreach (var row in rows)
        {
            if (string.IsNullOrEmpty(row.Cell(1).Value.ToString()) && string.IsNullOrEmpty(row.Cell(2).Value.ToString()))
            {
                var categorySubCategoryNames = row.Cell(3).Value.ToString().Split(' ', 2);

                var allocation = new CreateFixtureAllocations
                {
                    SUPERCATEGORY = categorySubCategoryNames[0],
                    SUBCATEGORY = categorySubCategoryNames[1],
                    TOTAL_SALES = string.IsNullOrEmpty(row.Cell(5).Value.ToString()) ? 0 : double.Parse(row.Cell(5).Value.ToString())
                };

                Console.WriteLine(allocation);

                allocations.Add(allocation);
            }
        }

        var rowsAffected = await _salesContext.UploadSales(allocations, saveExcelFile);

        Console.WriteLine($"Insert sales: {rowsAffected}");

        if (rowsAffected == 0)
        {
            return BadRequest(rowsAffected);
        }

        return Ok(rowsAffected);
    }

    /// <summary>
    /// Endpoint to get the sales allocations and sales targets for a floorset.
    /// </summary>
    /// <param name="floorsetId">Floorset id</param>
    /// <returns>Empty response with the specified status code</returns>
    [HttpGet("allocation-fulfillments/{floorsetId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<AllocationFulfillments>>> GetAllocationFulfillments(int floorsetId)
    {
        var response = await _salesContext.GetAllocationFulfillments(floorsetId);

        return Ok(response);
    }
}