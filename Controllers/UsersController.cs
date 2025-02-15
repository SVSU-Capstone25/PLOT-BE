/*
    Filename: UsersController.cs
    Part of Project: PLOT/PLOT-BE/Controllers
    File Purpose:
    This file contains the user controller endpoint mapping,
    which will transport the user data from the frontend 
    to the database and vice versa.
    Written by: Jordan Houlihan
*/

namespace Plot.Controllers;

using Microsoft.AspNetCore.Mvc;
using Plot.Data.Models.Users;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{

    /// <summary>
    /// This endpoint deals with returning a list of users.
    /// </summary>
    /// <returns>A list of all users as UserDTO objects.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public ActionResult<UserDTO[]> GetAll()
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with returning a user with a specified id.
    /// </summary>
    /// <param name="userId">The id of the user as a route parameter in the url.</param>
    /// <returns>The user tied to the userId as a UserDTO object.</returns>
    [HttpGet("{userId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public ActionResult<UserDTO> GetById(int userId)
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with updating a user.
    /// </summary>
    /// <param name="userId">The id of the user as a route parameter in the url.</param>
    /// <param name="userUpdateDTO">The updated user information from the request body as a UserUpdateDTO object.</param>
    /// <returns>The newly updated user information as a UserDTO object.</returns>
    [HttpPatch("{userId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public ActionResult<UserDTO> Update(int userId, UserUpdateDTO user)
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with deleting a user.
    /// </summary>
    /// <param name="userId">The id of the user as a route parameter in the url.</param>
    /// <returns>This endpoint doesn't return a value.</returns>
    [HttpDelete("{userId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult Delete(int userId)
    {
        return NoContent();
    }
}