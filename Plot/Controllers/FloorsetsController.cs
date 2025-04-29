/*
    Filename: FloorsetsController.cs
    Part of Project: PLOT/PLOT-BE/Plot/Controllers

    File Purpose:
    This file contains the floorsets controller endpoint mapping,
    which will transport floorset data from the frontend 
    to the database and vice versa. 
    
    Written by: Jordan Houlihan, Michael Polhill, Clayton Cook, Joshua Rodack
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

    /// <summary>
    /// Constructor for the floorsets controller, initializes
    /// the services and context via dependency injection.
    /// </summary>
    /// <param name="floorsetContext">Database context for the floorsets</param>
    /// <param name="claimParserService">Service for reading the claims on the auth tokens</param>
    public FloorsetsController(IFloorsetContext floorsetContext, ClaimParserService claimParserService)
    {
        _floorsetContext = floorsetContext;
        _claimParserService = claimParserService;
    }

    /// <summary>
    /// Endpoint to get all floorsets for a store.
    /// </summary>
    /// <param name="storeId">Store id</param>
    /// <returns>List of floorsets for a store</returns>
    [Authorize]
    [HttpGet("get-floorsets/{storeId:int}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Floorset>>> GetFloorsetsByStore(int storeId)
    {
        return Ok(await _floorsetContext.GetFloorsetsByStoreId(storeId));
    }

    /// <summary>
    /// Endpoint to get a floorset by id. 
    /// </summary>
    /// <param name="floorsetId">Floorset id</param>
    /// <returns>Floorset</returns>
    [HttpGet("get-floorset/{floorsetId:int}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Floorset>> GetFloorsetByIdController(int floorsetId)
    {
        return Ok(await _floorsetContext.GetFloorsetById(floorsetId));
    }

    /// <summary>
    /// Endpoint to create a floorset.
    /// </summary>
    /// <param name="floorset">The information of the created floorset</param>
    /// <returns></returns>
    [Authorize(Policy = "Manager")]
    [HttpPost("create-floorset")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Create([FromBody] CreateFloorset floorset)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        int rowsAffected = await _floorsetContext.CreateFloorset(floorset);

        if (rowsAffected == 0)
        {
            return BadRequest();
        }

        return Ok();
    }

    /// <summary>
    /// Endpoint to update the public information of a floorset.
    /// </summary>
    /// <param name="floorsetId">Floorset id</param>
    /// <param name="floorset">The new information for the floorset</param>
    /// <returns>Empty response with the specified status code</returns>
    [Authorize(Policy = "Manager")]
    [HttpPatch("public-info/{floorsetId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdatePublicInfo(int floorsetId, [FromBody] UpdatePublicInfoFloorset floorset)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }


        int rowsAffected = await _floorsetContext.UpdateFloorsetById(floorsetId, floorset);

        if (rowsAffected == 0)
        {
            return BadRequest();
        }

        return Ok();
    }

    /// <summary>
    /// Endpoint to delete a floorset by id.
    /// </summary>
    /// <param name="floorsetId">Floorset id</param>
    /// <returns>Empty response with the specified status code</returns>
    [Authorize(Policy = "Manager")]
    [HttpDelete("{floorsetId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int floorsetId)
    {
        return Ok(await _floorsetContext.DeleteFloorsetById(floorsetId));
    }

    /// <summary>
    /// Endpoint to copy a floorset. This will make a new floorset with the values
    /// of a current floorset.
    /// </summary>
    /// <param name="FloorsetRef">Floorset id</param>
    /// <returns>Empty response with the specified status code</returns>
    [HttpPost("copy-floorset")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CopyFloorset([FromBody] FloorsetRef FloorsetRef)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        int rowsAffected = await _floorsetContext.CopyFloorset(FloorsetRef);

        if (rowsAffected == 0)
        {
            return BadRequest();
        }

        return Ok();
    }
}