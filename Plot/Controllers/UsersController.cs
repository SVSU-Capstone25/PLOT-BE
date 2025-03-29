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
using Plot.DataAccess.Interfaces;
using Plot.Services;

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
    [HttpGet]
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
    [HttpGet("{userId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<UserDTO> GetById(int userId)
    {
        return Ok();
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
    public ActionResult<UserDTO> UpdatePublicInfo(int userId, UpdatePublicInfoUser user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with updating the role of a specific user
    /// based on their id.
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <param name="role">The new role for the user</param>
    /// <returns>The updated user</returns>
    [Authorize(Policy = "Owner")]
    [HttpPatch("role/{userId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<UserDTO> UpdateRole(int userId, [FromBody] int role)
    {
        return Ok();
    }

    /// <summary>
    /// This endpoint deals with deleting a specific user
    /// based on their id.
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <returns>UserDTO object</returns>
    [Authorize(Policy = "Manager")]
    [HttpDelete("{userId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Delete(int userId)
    {
        return NoContent();
    }
}