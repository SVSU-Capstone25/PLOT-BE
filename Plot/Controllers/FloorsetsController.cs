/*
    Filename: FloorsetsController.cs
    Part of Project: PLOT/PLOT-BE/Plot/Controllers

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
using Plot.Services;

namespace Plot.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FloorsetsController : ControllerBase
{
    private readonly IFloorsetContext _floorsetContext;
    private readonly ClaimParserService _claimParserService;

    public FloorsetsController(IFloorsetContext floorsetContext, ClaimParserService claimParserService)
    {
        _floorsetContext = floorsetContext;
        _claimParserService = claimParserService;
    }

    /// <summary>
    /// This endpoint deals with getting a list of floorsets tied to a specified store.
    /// </summary>
    /// <param name="storeId">The id of the store as a route parameter in the url.</param>
    /// <returns>A list of floorset objects.</returns>
    // [Authorize]
    [HttpGet("{storeId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Floorset>>> GetFloorsetsByStore(int storeId)
    {
        return Ok(await _floorsetContext.GetFloorsetsByStoreId(storeId));
        // return NoContent();
    }

    /// <summary>
    /// This endpoint deals with creating a floorset.
    /// </summary>
    /// <param name="floorset">The new floorset information</param>
    /// <returns>The newly created floorset information</returns>
    [Authorize(Policy = "Manager")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Floorset> Create(CreateFloorset floorset)
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with updating the public information of a floorset.
    /// </summary>
    /// <param name="floorsetId">The id of the floorset</param>
    /// <param name="floorset">The updated public information</param>
    /// <returns>The updated floorset</returns>
    [Authorize(Policy = "Manager")]
    [HttpPatch("public-info/{floorsetId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Floorset> UpdatePublicInfo(int floorsetId, UpdatePublicInfoFloorset floorset)
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with deleting a floorset.
    /// </summary>
    /// <param name="floorsetId">The id of the floorset</param>
    /// <returns>This endpoint doesn't return a value.</returns>
    [Authorize(Policy = "Manager")]
    [HttpDelete("{floorsetId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Delete(int floorsetId)
    {
        return NoContent();
    }
}