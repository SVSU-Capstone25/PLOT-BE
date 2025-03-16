/*
    Filename: UsersController.cs
    Part of Project: PLOT/PLOT-BE/Controllers

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

namespace Plot.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserContext _userContext;

    public UsersController(IUserContext userContext)
    {
        _userContext = userContext;
    }

    /// <summary>
    /// This endpoint deals with returning all of the users
    /// </summary>
    /// <returns>Array of userDTO objects</returns>
    [Authorize]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public ActionResult<UserDTO[]> GetAll()
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with returning a specific user
    /// based on their id
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <returns>UserDTO object</returns>
    [Authorize]
    [HttpGet("{userId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public ActionResult<UserDTO> GetById(int userId)
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with updating a specific user
    /// based on their id
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <param name="user">The updated user</param>
    /// <returns>UserDTO object</returns>
    [Authorize(Policy = "Manager")]
    [HttpPatch("{userId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public ActionResult<UserDTO> Update(int userId, UpdateUser user)
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with deleting a specific user
    /// based on their id
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <returns>UserDTO object</returns>
    [Authorize(Policy = "Manager")]
    [HttpDelete("{userId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Delete(int userId)
    {
        return NoContent();
    }
}