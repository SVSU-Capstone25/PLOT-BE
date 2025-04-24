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
using Plot.Data.Models.Users;
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
    [Authorize(Policy = "Manager")]
    [HttpGet("get-all")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Store>>> GetAll()
    {
        var stores = await _storeContext.GetStores();

        if (stores == null)
        {
            return BadRequest();
        }

        return Ok(stores);
    }

    /// <summary>
    /// This endpoint deals with getting stores based on a user's access.
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <returns>Array of stores</returns>
    [Authorize]
    [HttpGet("access/{userId:int}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Store>>> GetByAccess(int userId)
    {
        var stores = await _storeContext.GetByAccess(userId);

        if (stores == null)
        {
            return BadRequest();
        }

        return Ok(stores);
    }

    [Authorize]
    [HttpGet("get-store/{storeId:int}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Store>> GetById(int storeId)
    {
        var store = await _storeContext.GetStoreById(storeId);

        if (store == null)
        {
            return BadRequest();
        }

        return Ok(store);
    }

    [Authorize]
    [HttpGet("get-users-by-store/{storeId:int}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsersAtStore(int storeId)
    {
        var users = await _storeContext.GetUsersAtStore(storeId);

        if (users == null)
        {
            return BadRequest();
        }

        return Ok(users);
    }

    [Authorize]
    [HttpGet("get-users-not-in-store/{storeId:int}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDTO>> GetUsersNotInStore(int storeId)
    {
        var users = await _storeContext.GetUsersNotInStore(storeId);

        if (users == null)
        {
            return BadRequest();
        }

        return Ok(users);
    }
    /// <summary>
    /// This endpoint deals with creating a store.
    /// </summary>
    /// <param name="store">The new store</param>
    /// <returns>The newly created store</returns>
    [Authorize(Policy = "Owner")]
    [HttpPost("create-store")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateStore([FromBody] CreateStore store)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int rowsAffected = await _storeContext.CreateStoreEntry(store);

        if (rowsAffected == 0)
        {
            return BadRequest();
        }

        //controller logic could get added from other branches
        return Ok();
    }

    /// <summary>
    /// This endpoint deals with updating the public information of a store. This includes
    /// the name, location, image, and employees. 
    /// </summary>
    /// <param name="store">The updated public information for the store</param>
    /// <returns>The updated store</returns>
    [Authorize(Policy = "Manager")]
    [HttpPatch("public-info/{storeId:int}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Store>> UpdatePublicInfo(int storeId, [FromBody] UpdatePublicInfoStore store)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingStore = await _storeContext.GetStoreById(storeId);

        Console.WriteLine(existingStore);

        //return 404 error if no store was found
        if (existingStore == null)
        {
            return NotFound();
        }

        //controller logic could get added from other branches
        return Ok(await _storeContext.UpdatePublicInfoStore(storeId, store));
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
    public async Task<ActionResult> UpdateSizeStore(int storeId, [FromBody] UpdateSizeStore store)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int rowsAffected = await _storeContext.UpdateSizeStore(storeId, store);

        if (rowsAffected == 0)
        {
            return NotFound();
        }

        return Ok();
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
    public async Task<ActionResult> Delete(int storeId)
    {
        var existingStore = await _storeContext.GetStoreById(storeId);

        //return 404 error if no store was found
        if (existingStore == null)
        {
            return NotFound();
        }

        //controller logic should get overridden by other branches
        return Ok(await _storeContext.DeleteStoreById(storeId));
    }
}