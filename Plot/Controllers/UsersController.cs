/*
    Filename: UsersController.cs
    Part of Project: PLOT/PLOT-BE/Plot/Controllers

    File Purpose:
    This file contains the user controller endpoint mapping,
    which will transport the user data from the frontend 
    to the database and vice versa.

    Written by: Jordan Houlihan
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plot.Data.Models.Users;
using Plot.Data.Models.Stores;
using Plot.DataAccess.Interfaces;
using Plot.Services;

namespace Plot.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserContext _userContext;
    private readonly ClaimParserService _claimParserService;

    public UsersController(IUserContext userContext, IAuthContext authContext, ClaimParserService claimParserService)
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
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
    {
        var users = await _userContext.GetUsers();

        if (users == null) 
        {
            BadRequest();
        }

        return Ok(users);
    }

    /// <summary>
    /// This endpoint deals with returning a specific user
    /// based on their id.
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <returns>UserDTO object</returns>
    [Authorize]
    [HttpGet("get-users-by-id/{userId:int}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDTO>> GetById(int userId)
    {
        var user = await _userContext.GetUserById(userId);

        if (user == null)
        {
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
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDTO>> UpdatePublicInfo(int userId, [FromBody] UpdatePublicInfoUser user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int rowsAffected = await _userContext.UpdateUserPublicInfo(userId, user);

        if (rowsAffected == 0) 
        {
            return NotFound();
        }

        var updatedUser = await _userContext.GetUserById(userId);

        if (updatedUser == null)
        {
            return NotFound();
        }

        return Ok(updatedUser);
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
    [HttpDelete("delete-user/{userId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int userId)
    {
        int rowsAffected = await _userContext.DeleteUserById(userId);

        if (rowsAffected == 0)
        {
            return NotFound();
        }

        return Ok();
    }

    /// <summary>
    /// Adds a user as an employee to a store 
    /// </summary>
    /// <param name="userid">id of user being assigned</param>
    /// <param name="storeid">id of store being registered at</param>
    /// <returns></returns>
    /// TODO: Turn to put
    // [Authorize (Policy = "Manager")]
    // [HttpPost("store-registration")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // public async Task<ActionResult> AddUserToStore([FromBody] DeleteUserFromStoreRequest dufsr)
    // {
    //     return Ok(await _userContext.AddUserToStore(DeleteUserFromStoreRequest dufsr));
    // }

    [Authorize(Policy = "Manager")]
    [HttpPost("update-access-list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateUserAccessList([FromBody] UpdateAccessList updateAccessList)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int rowsAffected = 0;

        await _userContext.UpdateAccessList(updateAccessList);

        if (rowsAffected != updateAccessList.STORE_TUIDS.Count<int>())
        {
            return NotFound();
        }

        return Ok();
    }

    [Authorize(Policy = "Manager")]
    [HttpPost("add-user-to-store")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AddUserToStore([FromBody] AccessModel accessModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int rowsAffected = await _userContext.AddUserToStore(accessModel);

        if (rowsAffected == 0)
        {
            return BadRequest();
        }

        return Ok();
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
    public async Task<ActionResult> DeleteUserFromStore([FromBody] AccessModel accessModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        int rowsAffected = await _userContext.DeleteUserFromStore(accessModel);

        if (rowsAffected == 0)
        {
            return BadRequest();
        }

        return Ok();
    }

    /// <summary>
    /// returns all the stores a user works at
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <returns>UserDTO object</returns>
    [Authorize]
    [HttpGet("stores/{userId:int}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Store>>> GetStoreOfUserById(int userId)
    {
        var stores = await _userContext.GetStoresForUser(userId);

        if (stores == null)
        {
            NotFound();
        }

        return Ok();
    }
}