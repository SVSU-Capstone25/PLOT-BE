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
using Plot.Data.Models.Users;

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
    [HttpGet("get-fixtures-instances/{floorsetId:int}")]
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
    /// updates the fixture instance in the database
    /// with the given updatefixtureinstance model
    /// </summary>
    /// <param name="updateFixture"></param>
    /// <returns></returns>
    [Authorize(Policy = "Manager")]
    [HttpPatch("update-fixture-instance")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateFixtureInstance([FromBody] UpdateFixtureInstance updateFixture)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        Console.WriteLine(updateFixture);

        int rowsAffected = await _fixtureContext.UpdateFixtureInstanceById(updateFixture);

        if (rowsAffected == 0)
        {
            return BadRequest();
        }

        return Ok();
    }

    /// <summary>
    /// call to the fixture context to create a new fixture instance
    /// </summary>
    /// <param name="newFixture"></param>
    /// <returns></returns>
    [Authorize(Policy = "Manager")]
    [HttpPost("create-fixture-instance")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateFixtureInstance([FromBody] CreateFixtureInstance newFixture)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        Console.WriteLine(newFixture);

        var response = await _fixtureContext.CreateFixtureInstance(newFixture);

        Console.WriteLine(response);

        return Ok(response);
    }

    /// <summary>
    /// deletes a fixture instance with the given id.
    /// </summary>
    /// <param name="fixtureInstanceId"></param>
    /// <returns></returns>
    [Authorize(Policy = "Manager")]
    [HttpDelete("delete-fixture-instance/{fixtureInstanceId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteFixtureInstance([FromBody] int fixtureInstanceId)
    {
        return Ok(await _fixtureContext.DeleteFixtureInstanceById(fixtureInstanceId));
    }

    /// <summary>
    /// Send a request to 
    /// </summary>
    /// <param name="fixtureModel"></param>
    /// <returns></returns>
    [Authorize(Policy = "Manager")]
    [HttpPost("create-fixture-model/{storeId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateModel(int storeId, [FromBody] CreateFixtureModel fixtureModel)
    {
        return Ok(await _fixtureContext.CreateFixtureModel(storeId, fixtureModel));
    }

    [Authorize(Policy = "Manager")]
    [HttpDelete("delete-fixture-model/{modelId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteModel(int modelId)
    {
        return Ok(await _fixtureContext.DeleteFixtureModelById(modelId));
    }

    [Authorize(Policy = "Manager")]
    [HttpPatch("update-fixture-model")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateFixtureModel([FromBody] FixtureModel update)
    {
        return Ok(await _fixtureContext.UpdateFixtureModelById(update));
    }

    [Authorize(Policy = "Manager")]
    [HttpPost("add-employee-areas")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AddEmployeeAreas([FromBody] IEnumerable<AddEmployeeAreaModel> employeeAreas)
    {
        Console.WriteLine(employeeAreas);
        if (!ModelState.IsValid) return BadRequest();

        foreach (var employeeArea in employeeAreas)
        {
            await _fixtureContext.AddEmployeeAreas(employeeArea);
        }

        return Ok();
    }

    [Authorize(Policy = "Manager")]
    [HttpPost("bulk-add-employee-areas")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> BulkAddEmployeeAreas([FromBody] BulkAddEmployeeAreaModel employeeAreas)
    {
        if (!ModelState.IsValid) return BadRequest();

        await _fixtureContext.BulkAddEmployeeAreas(employeeAreas);

        return Ok();
    }

    [Authorize(Policy = "Manager")]
    [HttpDelete("delete-employee-areas")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteEmployeeAreas([FromBody] IEnumerable<DeleteEmployeeAreaModel> employeeAreas)
    {
        if (!ModelState.IsValid) return BadRequest();

        foreach (var employeeArea in employeeAreas)
        {
            await _fixtureContext.DeleteEmployeeAreas(employeeArea);
        }

        return Ok();
    }

    [Authorize(Policy = "Manager")]
    [HttpDelete("bulk-delete-employee-areas")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> BulkDeleteEmployeeAreas([FromBody] BulkDeleteEmployeeAreaModel employeeAreas)
    {
        if (!ModelState.IsValid) return BadRequest();

        await _fixtureContext.BulkDeleteEmployeeAreas(employeeAreas);

        return Ok();
    }

    [Authorize(Policy = "Employee")]
    [HttpGet("get-employee-areas/{floorsetId:int}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<EmployeeAreaModel>>> GetAllEmployeeAreas(int floorsetId)
    {
        Console.WriteLine("Get employee areas");
        var employeeAreas = await _fixtureContext.GetEmployeeAreas(floorsetId);

        if (employeeAreas == null)
        {
            return BadRequest();
        }

        return Ok(employeeAreas);
    }
}