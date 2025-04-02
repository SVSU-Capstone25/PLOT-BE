/*
    Filename: UsersController.cs
    Part of Project: PLOT/PLOT-BE/Plot/Controllers

    File Purpose:
    This file contains the user controller endpoint mapping,
    which will transport the user data from the frontend 
    to the database and vice versa.

    Written by: Jordan Houlihan
*/
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plot.Data.Models.Users;
using Plot.Data.Models.Stores;
using Plot.DataAccess.Interfaces;
using Plot.Services;
using Microsoft.Extensions.FileProviders;

namespace Plot.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserContext _userContext;
    private readonly ClaimParserService _claimParserService;

    public UsersController(IUserContext userContext, ClaimParserService claimParserService)
    {
        _userContext = userContext;
        _claimParserService = claimParserService;
    }

    /// <summary>
    /// This endpoint deals with returning all of the users.
    /// </summary>
    /// <returns>Array of userDTO objects</returns>
    [Authorize]
    [HttpGet("get-all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
    {
        return Ok(await _userContext.GetUsers());
    }

    /// <summary>
    /// This endpoint deals with returning a specific user
    /// based on their id.
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <returns>UserDTO object</returns>
    [Authorize]
    [HttpGet("get-users-by-id/{userId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDTO>> GetById(int userId)
    {
        var user = await _userContext.GetUserById(userId);
        if(user == null || !user.Any()){
            return NotFound();
        }
        return Ok(user);
    }

    /// <summary>
    /// This endpoint deals with updating the public information of a 
    /// specific user based on their id.
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <param name="user">The updated public information</param>
    /// <returns>The updated user</returns>
    [Authorize]
    [HttpPatch("public-info/{userId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<int>> UpdatePublicInfo(int userId, [FromBody] UpdatePublicInfoUser user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var update = await _userContext.UpdateUserPublicInfo(userId, user);
    
        if(update == -1){
            return NotFound();
        }
       return Ok(update);
    }

    /// <summary>
    /// This endpoint deals with updating the role of a specific user
    /// based on their id.
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <param name="role">The new role for the user</param>
    /// <returns>The updated user</returns>
    // [Authorize(Policy = "Owner")]
    // [HttpPatch("role/{userId:int}")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // public ActionResult<UserDTO> UpdateRole(int userId, [FromBody] int role)
    // {
    //     return Ok();
    // }

    /// <summary>
    /// This endpoint deals with deleting a specific user
    /// based on their id.
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <returns>UserDTO object</returns>
    [Authorize(Policy = "Manager")]
    [HttpPatch("delete-user/{userId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<int>> Delete(int userId)
    {
        var deleted = await _userContext.DeleteUserById(userId);
        if(deleted == 0){
            return NotFound();
        }
        return NoContent();
    }

    /// <summary>
    /// Adds a user as an employee to a store 
    /// </summary>
    /// <param name="userid">id of user being assigned</param>
    /// <param name="storeid">id of store being registered at</param>
    /// <returns></returns>
    [Authorize (Policy = "Manager")]
    [HttpPost("store-registration")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AddUserToStore([FromBody] UserStoreRequest dufsr)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var added = await _userContext.AddUserToStore(dufsr.userid, dufsr.storeid);
        if(added == -1){
            return NotFound();
        }
        return Ok(added);
    }

    /// <summary>
    /// remove association between an employee and astore
    /// </summary>
    /// <param name="userId">id of user being assigned</param>
    /// <param name="storeId">id of store being registered at</param>
    /// <returns></returns>
    [Authorize(Policy = "Manager")]
    [HttpPost("delete-user-from-store")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteFromStore([FromBody] UserStoreRequest dufsr)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var removed = await _userContext.DeleteUserFromStore(dufsr.userid, dufsr.storeid);
        if(removed == -1){
            return NotFound();
        }
        return NoContent();
    }

    /// <summary>
    /// returns all the stores a user works at
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <returns>UserDTO object</returns>
    [Authorize]
    [HttpGet("stores/{userId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Store>?> GetStoresForUser(int userId)
    {
        var stores = await _userContext.GetStoresForUser(userId);
        if(stores == null || !stores.Any()){
            return NotFound();
        }

        return Ok(stores);
    }
}