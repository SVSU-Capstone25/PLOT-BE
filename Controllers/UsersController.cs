/*
    Filename: UsersController.cs
    Part of Project: PLOT/PLOT-BE/Controllers
    File Purpose:
    This file contains the user controller endpoint mapping.
    Written by: Jordan Houlihan
*/

namespace Plot.Controllers;

using Microsoft.AspNetCore.Mvc;
using Plot.Data.Models.Users;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public ActionResult<UserDTO[]> GetAll()
    {
        return Ok();
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public ActionResult<UserDTO> GetById(int id)
    {
        return Ok();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public ActionResult<UserDTO> Create(User user)
    {
        return CreatedAtAction(nameof(Create), new { id = 1 });
    }

    [HttpPatch("/UpdateName")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public ActionResult<UserDTO> UpdateName(UserDTO user)
    {
        return Ok();
    }

    [HttpPatch("/UpdateRole")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public ActionResult<UserDTO> UpdateRole(UserDTO user)
    {
        return Ok();
    }

    [HttpPatch("/UpdateEmail")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public ActionResult<UserDTO> UpdateEmail(UserDTO user)
    {
        return Ok();
    }

    [HttpPatch("/UpdatePassword")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public ActionResult<UserDTO> UpdatePassword(User user)
    {
        return Ok();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult Delete(int id)
    {
        return NoContent();
    }
}