/*
    Filename: FixturesController.cs
    Part of Project: PLOT/PLOT-BE/Controllers

    File Purpose:
    This file contains the fixture controller endpoint mapping,
    which will transport the base fixture model data and fixture instance data 
    from the frontend to the database and vice versa.

    Written by: Jordan Houlihan
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plot.Data.Models.Fixtures;
using Plot.DataAccess.Interfaces;
using Plot.Services;

namespace Plot.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FixturesController : ControllerBase
{
    private readonly IFixtureContext _fixtureContext;
    private readonly ISalesContext _salesContext;
    private readonly ClaimParserService _claimParserService;

    public FixturesController(IFixtureContext fixtureContext, ISalesContext salesContext, ClaimParserService claimParserService)
    {
        _fixtureContext = fixtureContext;
        _salesContext = salesContext;
        _claimParserService = claimParserService;
    }

    /// <summary>
    /// This endpoint deals with returning a floorset's fixture information
    /// to fill out the floorset's grid and category allocations menu.
    /// </summary>
    /// <param name="floorsetId">The id of the floorset</param>
    /// <returns>A floorset's fixture information</returns>
    [Authorize]
    [HttpGet("{floorsetId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<FloorsetFixtureInformation> GetFixtureInformation(int floorsetId)
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with updating a floorset's fixture information
    /// by going through the fixtures sent from the frontend designer and
    /// processing if each fixture model or instance should be inserted,
    /// updating, or deleted from the database.
    /// </summary>
    /// <param name="floorsetId">The id of the floorset</param>
    /// <param name="floorsetFixtureInformation">A floorset's fixture information</param>
    /// <returns></returns>
    [Authorize(Policy = "Manager")]
    [HttpPatch("{floorsetId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<FloorsetFixtureInformation> UpdateFixtureInformation(int floorsetId, UpdateFloorsetFixtureInformation floorsetFixtureInformation)
    {
        return Ok();
    }
}