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
        _fixtureContext = fixtureContext ?? throw new ArgumentNullException(nameof(fixtureContext));
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
    public async Task<ActionResult<FixtureInstance>> GetFixtureInformation(int floorsetId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }



        return Ok(await _fixtureContext.GetFixtureInstances(floorsetId));
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
    public async Task<ActionResult> UpdateFixtureInformation(int floorsetId, [FromBody] Fixtures_State fixtures)
    {
        var old = await _fixtureContext.GetFixtureInstances(floorsetId) ?? new List<FixtureInstance>();
        var current = fixtures.CurrentFixtures ?? new List<FixtureInstance>();
        //Select_Floorset_Fixtures[] oldFixtures = query.Cast<Select_Floorset_Fixtures>().ToArray();
        //Select_Floorset_Fixtures[] newFixtures = fixtures.CurrentFixtures.Cast<Select_Floorset_Fixtures>().ToArray();

        IEnumerable<FixtureInstance> update = old.Intersect(current);
        IEnumerable<FixtureInstance> create = current.Except(old);
        IEnumerable<FixtureInstance> delete = old.Except(current);

        foreach (FixtureInstance instance in update)
        {
            await _fixtureContext.UpdateFixtureInstanceById(instance);
        }

        foreach (FixtureInstance instance in create)
        {
            await _fixtureContext.CreateFixtureInstance(instance);
        }

        foreach (FixtureInstance instance in delete)
        {
            int tuid = instance.TUID ?? -1;
            if (tuid != -1)
            {
                await _fixtureContext.DeleteFixtureInstanceById(tuid);
            }
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
    public async Task<ActionResult<FixtureModel>> CreateModel([FromBody] FixtureModel fixtureModel)
    {
        return Ok(await _fixtureContext.CreateFixtureModel(fixtureModel));
    }

    [Authorize(Policy = "Manager")]
    [HttpPatch("delete-model/{storeId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteModel(int modelId)
    {
        return Ok(await _fixtureContext.DeleteFixtureModelById(modelId));
    }

    [Authorize(Policy = "Manager")]
    [HttpPatch("update-model/{storeId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateFixtureModel([FromBody] FixtureModel update)
    {
        return Ok(await _fixtureContext.UpdateFixtureModelById(update));
    }
}