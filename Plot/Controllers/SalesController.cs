/*
    Filename: SalesController.cs
    Part of Project: PLOT/PLOT-BE/Plot/Controllers

    File Purpose:
    This file contains the sales controller endpoint mapping,
    which will upload/process sales data from the frontend/database.
    
    Written by: Jordan Houlihan
*/
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plot.DataAccess.Interfaces;
using Plot.Services;

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
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UploadSales(int floorsetId, [FromBody] IFormFile excelFile)
    {
        return Ok(await _salesContext.UploadSales(floorsetId, excelFile));
    }
}