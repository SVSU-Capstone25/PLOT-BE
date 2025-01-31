/*
    Filename: FixturesController.cs
    Part of Project: PLOT/PLOT-BE/Controllers
    File Purpose:
    This file contains the floorset controller endpoint mapping.
    Written by: Jordan Houlihan
*/

using Microsoft.AspNetCore.Mvc;
using Plot.Data.Models.Floorsets;

namespace Plot.Controllers;

[ApiController]
[Route("[controller]")]
public class FloorsetsController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public ActionResult<Floorset[]> GetAll()
    {
        return Ok();
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public ActionResult<Floorset> GetById(int id)
    {
        return Ok();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public ActionResult<Floorset> Create(Floorset floorset)
    {
        return CreatedAtAction(nameof(Create), new { id = 1 });
    }

    [HttpPatch("/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Floorset> Update(int id, Floorset floorset)
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