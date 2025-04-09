/*
    Filename: FixturesController.cs
    Part of Project: PLOT/PLOT-BE/Plot/Controllers

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
using Plot.Data.Models.Allocations;

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
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<FixtureInstance>>> GetFixtureInstances(int floorsetId)
    {
        var fixtures = await _fixtureContext.GetFixtureInstances(floorsetId);

        if (fixtures == null)
        {
            return BadRequest();
        }

        return Ok(fixtures);
    }

    [Authorize]
    [HttpGet("get-fixture-models/{storeId:int}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<FixtureModel>>> GetFixtureModelsByStore(int storeId)
    {
        var fixtureModels = await _fixtureContext.GetFixtureModels(storeId);

        if (fixtureModels == null)
        {
            BadRequest();
        }

        return Ok(fixtureModels);
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
    public async Task<ActionResult> UpdateFixtureInformation(int floorsetId, [FromBody] FixturesState fixtures)
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

            Data.Models.Fixtures.UpdateFixtureInstance updateInstance = new Data.Models.Fixtures.UpdateFixtureInstance();

            updateInstance.X_POS = instance.X_POS;

            updateInstance.Y_POS = instance.Y_POS;

            //updateInstance.SUBCATEGORY = instance.SUBCATEGORY;

            //updateInstance.SUPERCATEGORY_TUID = instance.SUPERCATEGORY_NAME;

            await _fixtureContext.UpdateFixtureInstanceById(updateInstance);
        }

        foreach (FixtureInstance instance in create)
        {

            Data.Models.Fixtures.CreateFixtureInstance createInstance = new Data.Models.Fixtures.CreateFixtureInstance();

            createInstance.X_POS = instance.X_POS;

            createInstance.Y_POS = instance.Y_POS;

            await _fixtureContext.CreateFixtureInstance(createInstance);
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
    [HttpPost("create-fixture/{storeId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateModel([FromBody] CreateFixtureModel fixtureModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int rowsAffected = await _fixtureContext.CreateFixtureModel(fixtureModel);

        if (rowsAffected == 0)
        {
            return BadRequest();
        }

        return Ok();
    }

    [Authorize(Policy = "Manager")]
    [HttpDelete("delete-model/{modelId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteModel(int modelId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var rowsAffected = await _fixtureContext.DeleteFixtureModelById(modelId);

        if (rowsAffected == 0)
        {
            return BadRequest();
        }

        return Ok();
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