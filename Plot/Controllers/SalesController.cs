/*
    Filename: SalesController.cs
    Part of Project: PLOT/PLOT-BE/Plot/Controllers

    File Purpose:
    This file contains the sales controller endpoint mapping,
    which will upload/process sales data from the frontend/database.
    
    Written by: Jordan Houlihan
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

    public SalesController(ISalesContext salesContext, ClaimParserService claimParserService)
    {
        _salesContext = salesContext;
        _claimParserService = claimParserService;
    }

    /// <summary>
    /// This endpoint deals with uploading the sales
    /// for an excel file.
    /// </summary>
    /// <param name="floorsetId">The floorset id</param>
    /// <param name="file">The excel file</param>
    /// <returns></returns>
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
    /// This endpoint deals with retreiving allocation 
    /// data for the allocation sidebar
    /// </summary>
    /// <param name="floorsetId">The id of the floorset</param>
    /// <returns>A list of allocation fulfillments.</returns>
    [HttpGet("allocation-fulfillments/{floorsetId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<AllocationFulfillments>>> GetAllocationFulfillments(int floorsetId)
    {
        var response = await _salesContext.GetAllocationFulfillments(floorsetId);

        return Ok(response);
    }

    /// <summary>
    /// This endpoint deals with inserting sales
    /// data given parameters such as when copying
    /// </summary>
    /// <param name="ExcelFileModel">The excel file</param>
    /// <returns></returns>
    ///    
    [HttpPost("insert-sales")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> InsertSales([FromBody] CreateExcelFileModel ExcelFileModels)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int rowsAffected = await _salesContext.InsertSales(ExcelFileModels);

        if (rowsAffected == 0)
        {
            return BadRequest();
        }

        return Ok();
    }

    /// <summary>
    /// Get sales data (from sales table) by floorset
    /// </summary>
    /// <param name="floorsetId">The floorset Id</param>
    /// <returns>UserDTO object</returns>
    [HttpGet("get-sales-by-floorset/{floorsetId}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<CreateExcelFileModel>>> GetSalesByFloorset(int floorsetId)
    {
        var ExcelFileModel = await _salesContext.GetSalesByFloorset(floorsetId);

        if (ExcelFileModel == null)
        {
            return NotFound();
        }

        return Ok(ExcelFileModel);
    }

}