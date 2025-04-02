/*
    Filename: FixturesController.cs
    Part of Project: PLOT/PLOT-BE/Plot/Controllers

    File Purpose:
    This file contains the fixture controller endpoint mapping,
    which will transport the base fixture model data and fixture instance data 
    from the frontend to the database and vice versa.

    Written by: Jordan Houlihan
*/
using Dapper;
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
    [HttpGet("get-fixtures/{floorsetId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<FloorsetFixtureInformation>> GetFixtureInformation(int floorsetId, int storeId)
    {
        FloorsetFixtureInformation ffi = new FloorsetFixtureInformation();
        var ilist = await _fixtureContext.GetFixtureModels(storeId);

        ffi.FixtureModels = ilist.ToList();
        if (ffi.FixtureModels == null) 
        {
            return InternalServerError();
        }
        ilist = await _fixtureContext.GetFixtureInstances(floorsetId);
        ffi.FixtureInstances = ilist.ToList();

        if (ffi.FixtureInstances == null)
        {
            return InternalServerError();
        }
        ilist = await _salesContext.GetSalesAllocations(floorsetId);

        ffi.Allocations = ilist.ToList();
        if (ffi.Allocations == null)
        {
            return InternalServerError();
        }
        
        return Ok(ffi);
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
    [HttpPatch("update-fixture/{floorsetId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateFixtureInformation(int floorsetId, int storeId, UpdateFloorsetFixtureInformation uffi)
    {
        var n;
        for(int i = 0; i<uffi.FixtureInstances.size; i++)
        {
            n = await _fixtureContext.UpdateFixtureInstanceById(uffi.FixtureInstances[i]);
        }
        for(int i = 0; i<uffi.DeletedFixtureInstances.size; i++)
        {
            n = await _fixtureContext.DeleteFixtureInstanceById(uffi.DeletedFixtureInstances[i]);
        }
        
      
        return Ok();
    }

    /// <summary>
    /// Send a request to 
    /// </summary>
    /// <param name="fixtureModel"></param>
    /// <returns></returns>
    [Authorize(Policy = "Manager")]
    [HttpPatch("create-fixture/{storeId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FixtureModel>> CreateModel(CreateFixtureModel fixtureModel)
    {
        return Ok(await _fixtureContext.CreateFixtureModel(fixtureModel));
    }

    [Authorize(Policy = "Manager")]
    [HttpPatch("delete-store/{storeId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteModel(int modelId)
    {
        return Ok(await _fixtureContext.DeleteFixtureModelById(modelId));
    }
    

}