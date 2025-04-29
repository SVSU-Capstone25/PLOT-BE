/*
    Filename: FixturesController.cs
    Part of Project: PLOT/PLOT-BE/Plot/Controllers

    File Purpose:
    This file contains the fixture controller endpoint mapping,
    which will transport the base fixture model data and fixture instance data 
    from the frontend to the database and vice versa.

    Written by: Jordan Houlihan, Clayton Cook, Joshua Rodack
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

    /// <summary>
    /// Constructor for the fixtures controller, initializes
    /// the services and context via dependency injection.
    /// </summary>
    /// <param name="fixtureContext">Database context for the fixtures</param>
    /// <param name="salesContext">Database context for the sales allocations</param>
    /// <param name="claimParserService">Service for reading the claims on the auth tokens</param>
    public FixturesController(IFixtureContext fixtureContext, ISalesContext salesContext, ClaimParserService claimParserService)
    {
        _fixtureContext = fixtureContext;
        _salesContext = salesContext;
        _claimParserService = claimParserService;

    }

    /// <summary>
    /// Endpoint to get all of the fixture instances for a floorset.
    /// This will mainly be used to populate the floorset grid in the fixture editor
    /// page.
    /// </summary>
    /// <param name="floorsetId">Floorset id</param>
    /// <returns>List of fixture instances for a floorset</returns>
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

    /// <summary>
    /// Endpoint to get all of the fixture models for a store.
    /// This will mainly be used to populate the fixture sidebar in the fixture
    /// editor page.
    /// </summary>
    /// <param name="storeId">Store id</param>
    /// <returns>List of fixture models for a store</returns>
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
    /// Endpoint to update a fixture instance. 
    /// </summary>
    /// <param name="updateFixture">The new information for the fixture instance</param>
    /// <returns>Empty response with the specified status code</returns>
    [Authorize(Policy = "Manager")]
    [HttpPatch("update-fixture-instance")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateFixtureInstance([FromBody] UpdateFixtureInstance updateFixture)
    {

        Console.WriteLine(updateFixture.TUID);
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        Console.WriteLine(updateFixture);

        int rowsAffected = await _fixtureContext.UpdateFixtureInstanceById(updateFixture);

        Console.WriteLine(rowsAffected);

        if (rowsAffected == 0)
        {
            return BadRequest();
        }

        return Ok();
    }

    /// <summary>
    /// Endpoint to create a fixture instance. 
    /// </summary>
    /// <param name="newFixture">The information of the created fixture instance</param>
    /// <returns>id of the created fixture instance</returns>
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

        return Ok(await _fixtureContext.CreateFixtureInstance(newFixture));
    }

    /// <summary>
    /// Endpoint to delete a fixture instance by id.
    /// </summary>
    /// <param name="fixtureInstanceId">Fixture instance id</param>
    /// <returns>Empty response with the specified status code</returns>
    [Authorize(Policy = "Manager")]
    [HttpDelete("delete-fixture-instance/{fixtureInstanceId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteFixtureInstance(int fixtureInstanceId)
    {
        return Ok(await _fixtureContext.DeleteFixtureInstanceById(fixtureInstanceId));
    }

    /// <summary>
    /// Endpoint to create a fixture model for a store.
    /// </summary>
    /// <param name="storeId">Store id</param>
    /// <param name="fixtureModel">The information of the created fixture model</param>
    /// <returns>Empty response with the specified status code</returns>
    [Authorize(Policy = "Manager")]
    [HttpPost("create-fixture-model/{storeId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateModel(int storeId, [FromBody] CreateFixtureModel fixtureModel)
    {
        return Ok(await _fixtureContext.CreateFixtureModel(storeId, fixtureModel));
    }

    /// <summary>
    /// Endpoint to delete a fixture model.
    /// </summary>
    /// <param name="modelId">Model id</param>
    /// <returns>Empty response with the specified status code</returns>
    [Authorize(Policy = "Manager")]
    [HttpDelete("delete-fixture-model/{modelId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteModel(int modelId)
    {
        return Ok(await _fixtureContext.DeleteFixtureModelById(modelId));
    }

    /// <summary>
    /// Endpoint to update a fixture model.
    /// </summary>
    /// <param name="fixtureId">Fixture id</param>
    /// <param name="update">The new information for the fixture model</param>
    /// <returns>Empty response with the specified status code</returns>
    [Authorize(Policy = "Manager")]
    [HttpPatch("update-fixture-model/{fixtureId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateFixtureModel(int fixtureId, [FromBody] CreateFixtureModel update)
    {
        return Ok(await _fixtureContext.UpdateFixtureModelById(fixtureId, update));
    }

    /// <summary>
    /// Endpoint to add employee areas to a floorset.
    /// </summary>
    /// <param name="employeeAreas">Employee areas</param>
    /// <returns>Empty response with the specified status code</returns>
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

    /// <summary>
    /// Endpoint to bulk add employee areas to a floorset.
    /// </summary>
    /// <param name="employeeAreas">Employee areas</param>
    /// <returns>Empty response with the specified status code</returns>
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

    /// <summary>
    /// Endpoint to delete employee areas from a floorset.
    /// </summary>
    /// <param name="employeeAreas">Employee areas</param>
    /// <returns>Empty response with the specified status code</returns>
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

    /// <summary>
    /// Endpoint to bulk delete employee areas from a floorset.
    /// </summary>
    /// <param name="employeeAreas">Employee areas</param>
    /// <returns>Empty response with the specified status code</returns>
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

    /// <summary>
    /// Endpoint to get all employee areas for a floorset.
    /// </summary>
    /// <param name="floorsetId">Floorset id</param>
    /// <returns>List of employee areas for a floorset</returns>
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