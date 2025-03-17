/*
    Filename: SalesController.cs
    Part of Project: PLOT/PLOT-BE/Controllers

    File Purpose:
    This file contains the sales controller endpoint mapping,
    which will upload/process sales data from the frontend/database.
    
    Written by: Jordan Houlihan
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Plot.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="floorsetId"></param>
    /// <param name="excelFile"></param>
    /// <returns></returns>
    [Authorize(Policy = "Manager")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UploadSales(int floorsetId, IFormFile excelFile)
    {
        return Ok();
    }
}