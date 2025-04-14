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
    /// <param name="excelFile">The excel file</param>
    /// <returns></returns>
    [Authorize(Policy = "Manager")]
    [HttpPost("upload-sales/{floorsetId:int}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<CreateFixtureAllocations>>> UploadSales(int floorsetId, [FromBody] UploadFile excelFile)
    {
        if (excelFile.EXCEL_FILE == null)
        {
            return BadRequest();
        }

        using var memoryStream = new MemoryStream();
        await excelFile.EXCEL_FILE.CopyToAsync(memoryStream);

        using var workbook = new XLWorkbook(memoryStream);
        var worksheet = workbook.Worksheet(1);
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
                    UNITS = string.IsNullOrEmpty(row.Cell(6).Value.ToString()) ? 0 : int.Parse(row.Cell(6).Value.ToString())
                };

                allocations.Add(allocation);
            }
        }

        return Ok();
    }

    /// <summary>
    /// This endpoint deals with retreiving allocation 
    /// data for the allocation sidebar
    /// </summary>
    /// <param name="floorsetId">The id of the floorset</param>
    /// <returns>A list of allocation fufillments.</returns>
    [HttpGet("allocation-fufillments/{floorsetId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<AllocationFufillments>>> GetAllocationFufillments(int floorsetId)
    {
        return Ok(await _salesContext.GetAllocationFufillments(floorsetId));
    }
}