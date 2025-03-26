/*
    Filename: StoresController.cs
    Part of Project: PLOT/PLOT-BE/Controllers

    File Purpose:
    This file contains the store controller endpoint mapping,
    which will transport store data from the frontend
    to the database and vice versa.
    
    Written by: Jordan Houlihan
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plot.Data.Models.Stores;
using Plot.DataAccess.Interfaces;
using Plot.Services;

namespace Plot.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StoresController : ControllerBase
{
    private readonly IStoreContext _storeContext;
    private readonly ClaimParserService _claimParserService;

    public StoresController(IStoreContext storeContext, ClaimParserService claimParserService)
    {
        _storeContext = storeContext;
        _claimParserService = claimParserService;
    }

    /// <summary>
    /// This endpoint deals with getting all of the stores.
    /// </summary>
    /// <returns>Array of stores</returns>
    [Authorize(Policy = "Owner")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<Store[]> GetAll()
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with getting stores based on a user's access.
    /// </summary>
    /// <param name="storeId">The id of the user</param>
    /// <returns>Array of stores</returns>
    [Authorize]
    [HttpGet("{userId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Store[]> GetByAccess(int userId)
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with creating a store.
    /// </summary>
    /// <param name="store">The new store</param>
    /// <returns>The newly created store</returns>
    [Authorize(Policy = "Owner")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Store> Create(CreateStore store)
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with updating the public information of a store. This includes
    /// the name, location, image, and employees. 
    /// </summary>
    /// <param name="store">The updated public information for the store</param>
    /// <returns>The updated store</returns>
    [Authorize(Policy = "Manager")]
    [HttpPatch("public-info/{storeId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Store> UpdatePublicInfo(int storeId, UpdatePublicInfoStore store)
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with updating the size of a store.
    /// </summary>
    /// <param name="store">The updated size of the store</param>
    /// <returns>The updated store</returns>
    [Authorize(Policy = "Manager")]
    [HttpPatch("size/{storeId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Store> UpdateSize(int storeId, UpdateSizeStore store)
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with deleting a store.
    /// </summary>
    /// <param name="storeId">The id of the store</param>
    /// <returns>This endpoint doesn't return a value</returns>
    [Authorize(Policy = "Owner")]
    [HttpDelete("{storeId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Delete(int storeId)
    {
        return NoContent();
    }
}