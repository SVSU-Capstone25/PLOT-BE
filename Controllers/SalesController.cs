/*
    Filename: SalesController.cs
    Part of Project: PLOT/PLOT-BE/Controllers

    File Purpose:
    This file contains the sales controller endpoint mapping,
    which will upload/process sales data from the frontend/database.
    
    Written by: Jordan Houlihan
*/

using Microsoft.AspNetCore.Mvc;

namespace Plot.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    public IActionResult UploadSales(int floorsetId, IFormFile excelFile)
    {
        return Ok();
    }
}