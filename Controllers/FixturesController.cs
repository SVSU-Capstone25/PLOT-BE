/*
    Filename: FixturesController.cs
    Part of Project: PLOT/PLOT-BE/Controllers
    File Purpose:
    This file contains the fixture controller endpoint mapping,
    which will transport the base fixture model data and fixture instance data 
    from the frontend to the database and vice versa.
    Written by: Jordan Houlihan
*/

using Microsoft.AspNetCore.Mvc;
using Plot.Data.Models.Fixtures;

namespace Plot.Controllers;

[ApiController]
[Route("[controller]")]
public class FixturesController : ControllerBase
{
    /// <summary>
    /// This endpoint deals with returning a floorset's fixture information
    /// to fill out the floorset's grid and category allocations menu.
    /// </summary>
    /// <param name="floorsetId">The id of the floorset as a route parameter in the url.</param>
    /// <returns>A floorset's fixture information as a FloorsetFixtureInformationDTO object.</returns>
    [HttpGet("{floorsetId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<FloorsetFixtureInformationDTO> GetFixtureInformation(int floorsetId)
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with creating a fixture model.
    /// </summary>
    /// <param name="storeId">The id of the store as a route parameter in the url.</param>
    /// <param name="fixture">The new fixture information as a FixtureDTO object in the request body.</param>
    /// <returns>The newly created fixture information as a Fixture object.</returns>
    [HttpPost("models/{storeId:int}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Fixture> CreateFixtureModel(int storeId, FixtureDTO fixture)
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with creating a fixture instance model.
    /// </summary>
    /// <param name="storeId">The id of the store as a route parameter in the url.</param>
    /// <param name="floorsetId">The id of the floorset as a route parameter in the url.</param>
    /// <param name="fixture">The new fixture instance information as a FixtureDTO object in the request body.</param>
    /// <returns>The newly created fixture instance information as a FixtureInstance object.</returns>
    [HttpPost("instances/{storeId:int}/{floorsetId:int}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<FixtureInstance> CreateFixtureInstance(int storeId, int floorsetId, FixtureInstanceDTO fixture)
    {
        return NoContent();
    }


    /// <summary>
    /// This endpoint deals with updating a fixture model.
    /// </summary>
    /// <param name="fixtureId">The id of the fixture model as a route parameter in the url.</param>
    /// <param name="fixture">The updated fixture model information as a FixtureDTO object in the request body.</param>
    /// <returns>The updated fixture information as a Fixture object.</returns>
    [HttpPut("models/{fixtureId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Fixture> UpdateFixtureModel(int fixtureId, FixtureDTO fixture)
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with updating a fixture instance.
    /// </summary>
    /// <param name="fixtureInstanceId">The id of the fixture instance model as a route parameter in the url.</param>
    /// <param name="fixture">The updated fixture instance information as a FixtureInstanceDTO object in the request body.</param>
    /// <returns>The updated fixture instance information as a FixtureInstance object.</returns>
    [HttpPut("instances/{fixtureInstanceId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<FixtureInstance> UpdateFixtureInstance(int fixtureInstanceId, FixtureInstanceDTO fixture)
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with deleting a fixture model.
    /// </summary>
    /// <param name="fixtureId">The id of the fixture model as a route parameter in the url.</param>
    /// <returns>This endpoint doesn't return a value.</returns>
    [HttpDelete("models/{fixtureId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult DeleteFixtureModel(int fixtureId)
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with deleting a fixture instance.
    /// </summary>
    /// <param name="fixtureInstanceId">The id of the fixture instance model as a route parameter in the url.</param>
    /// <returns>This endpoint doesn't return a value.</returns>
    [HttpDelete("instances/{fixtureInstanceId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult DeleteFixtureInstance(int fixtureInstanceId)
    {
        return NoContent();
    }
}