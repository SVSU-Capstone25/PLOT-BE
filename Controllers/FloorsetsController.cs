/*
    Filename: FloorsetsController.cs
    Part of Project: PLOT/PLOT-BE/Controllers

    File Purpose:
    This file contains the floorsets controller endpoint mapping,
    which will transport floorset data from the frontend 
    to the database and vice versa. 
    
    Written by: Jordan Houlihan
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plot.Data.Models.Floorsets;
using Plot.DataAccess.Interfaces;

namespace Plot.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FloorsetsController : ControllerBase
{
    private readonly IFloorsetContext _floorsetContext;

    public FloorsetsController(IFloorsetContext floorsetContext)
    {
        _floorsetContext = floorsetContext;
    }

    /// <summary>
    /// This endpoint deals with getting a list of floorsets tied to a specified store.
    /// </summary>
    /// <param name="storeId">The id of the store as a route parameter in the url.</param>
    /// <returns>A list of floorset objects.</returns>
    [Authorize]
    [HttpGet("{storeId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public ActionResult<Floorset[]> GetFloorsetsByStore(int storeId)
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with creating a floorset.
    /// </summary>
    /// <param name="floorset">The new floorset information as a FloorsetDTO object in the request body.</param>
    /// <returns>The newly created floorset information as a Floorset object.</returns>
    [Authorize(Policy = "Manager")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public ActionResult<Floorset> Create(CreateFloorset floorset)
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with updating a floorset.
    /// </summary>
    /// <param name="floorsetId">The id of the floorset as a route parameter in the url.</param>
    /// <param name="floorset">The updated floorset information as a FloorsetDTO object in the request body.</param>
    /// <returns>The updated floorset information as a Floorset object.</returns>
    [Authorize(Policy = "Manager")]
    [HttpPatch("{floorsetId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Floorset> Update(int floorsetId, Floorset floorset)
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with deleting a floorset.
    /// </summary>
    /// <param name="floorsetId">The id of the floorset as a route parameter in the url.</param>
    /// <returns>This endpoint doesn't return a value.</returns>
    [Authorize(Policy = "Manager")]
    [HttpDelete("{floorsetId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult Delete(int floorsetId)
    {
        return NoContent();
    }
}