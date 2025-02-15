/*
    Filename: FloorsetsController.cs
    Part of Project: PLOT/PLOT-BE/Controllers
    File Purpose:
    This file contains the floorset controller endpoint mapping,
    which will transport floorset data from the frontend 
    to the database and vice versa.
    Written by: Jordan Houlihan
*/

using Microsoft.AspNetCore.Mvc;
using Plot.Data.Models.Floorsets;

namespace Plot.Controllers;

[ApiController]
[Route("[controller]")]
public class FloorsetsController : ControllerBase
{

    /// <summary>
    /// This endpoint deals with getting a list of floorsets tied to a specified store.
    /// </summary>
    /// <param name="storeId">The id of the store as a route parameter in the url.</param>
    /// <returns>A list of floorset objects.</returns>
    [HttpGet("{storeId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public ActionResult<Floorset[]> GetByStore(int storeId)
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with creating a floorset.
    /// </summary>
    /// <param name="floorset">The new floorset information as a FloorsetDTO object in the request body.</param>
    /// <returns>The newly created floorset information as a Floorset object.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public ActionResult<Floorset> Create(FloorsetDTO floorset)
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with updating a floorset.
    /// </summary>
    /// <param name="floorsetId">The id of the floorset as a route parameter in the url.</param>
    /// <param name="floorset">The updated floorset information as a FloorsetDTO object in the request body.</param>
    /// <returns>The updated floorset information as a Floorset object.</returns>
    [HttpPatch("{floorsetId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Floorset> Update(int floorsetId, FloorsetDTO floorset)
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with deleting a floorset.
    /// </summary>
    /// <param name="floorsetId">The id of the floorset as a route parameter in the url.</param>
    /// <returns>This endpoint doesn't return a value.</returns>
    [HttpDelete("{floorsetId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult Delete(int floorsetId)
    {
        return NoContent();
    }
}