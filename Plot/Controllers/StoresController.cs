/*
    Filename: StoresController.cs
    Part of Project: PLOT/PLOT-BE/Controllers

    File Purpose:
    This file contains the store controller endpoint mapping,
    which will transport store data from the frontend
    to the database and vice versa.
    
    Written by: Jordan Houlihan, Clayton Cook, Joshua Rodack
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

    /// <summary>
    /// Constructor for the stores controller, initializes
    /// the services and context via dependency injection.
    /// </summary>
    /// <param name="storeContext">Database context for the stores</param>
    /// <param name="claimParserService">Service for reading the claims on the auth tokens</param>
    public StoresController(IStoreContext storeContext, ClaimParserService claimParserService)
    {
        _storeContext = storeContext;
        _claimParserService = claimParserService;
    }

    /// <summary>
    /// Endpoint to get all of the stores.
    /// </summary>
    /// <returns>List of stores</returns>
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
    /// Endpoint to get a list of stores based on the logged in
    /// user's access.
    /// </summary>
    /// <param name="userId">User id</param>
    /// <returns>List of stores</returns>
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

    /// <summary>
    /// Endpoint to get a store by id.
    /// </summary>
    /// <param name="storeId">Store id</param>
    /// <returns>a store</returns>
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

    /// <summary>
    /// Endpoint to get the users working at a store.
    /// </summary>
    /// <param name="storeId">Store id</param>
    /// <returns>List of users</returns>
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

    /// TODO: Don't know why the return type was changed to only be one user.
    /// Should be IEnumerable<UserDTO> for a list of users.
    /// <summary>
    /// Endpoint to get users not working at a store.
    /// </summary>
    /// <param name="storeId">Store id</param>
    /// <returns>List of users</returns>
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
    /// Endpoint to create a store.
    /// </summary>
    /// <param name="store">The information of the new store</param>
    /// <returns>Empty response with the specified status code</returns>
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

    // TODO: Don't know why the return type is a store when there is no way
    // that this will return a store. The database stored procedure will
    // never return a store.
    /// <summary>
    /// Endpoint to update the public information of a store.
    /// </summary>
    /// <param name="storeId">Store id</param>
    /// <param name="store">The new information of the store</param>
    /// <returns></returns>
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
    /// Update the size of the store.
    /// </summary>
    /// <param name="storeId">Store id</param>
    /// <param name="store">The new size information of the store</param>
    /// <returns>Empty response with the specified status code</returns>
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
    /// Endpoint to delete a store.
    /// </summary>
    /// <param name="storeId">Store id</param>
    /// <returns>Empty response with the specified status code</returns>
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