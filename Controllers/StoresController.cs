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

namespace Plot.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StoresController : ControllerBase
{
    private readonly IStoreContext _storeContext;

    public StoresController(IStoreContext storeContext)
    {
        _storeContext = storeContext;
    }

    /// <summary>
    /// This endpoint deals with getting all of the stores
    /// </summary>
    /// <returns>Array of Store objects</returns>
    [Authorize]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Store[]> GetAll()
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with getting a store with a specified id.
    /// </summary>
    /// <param name="storeId">The id of the store as a route parameter in the url.</param>
    /// <returns>The store tied to the storeId as a StoreDTO object.</returns>
    [Authorize]
    [HttpGet("{storeId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Store> GetById(int storeId)
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with creating a store.
    /// </summary>
    /// <param name="store">The new store information as a StoreDTO object in the request body.</param>
    /// <returns>The newly created store information as a Store object.</returns>
    [Authorize(Policy = "Owner")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public ActionResult<Store> Create(CreateStore store)
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with updating a store.
    /// </summary>
    /// <param name="store">The updated store information as a Store object in the request body.</param>
    /// <returns>The updated store information as a Store object.</returns>
    [Authorize(Policy = "Manager")]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Store> Update(Store store)
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with deleting a store.
    /// </summary>
    /// <param name="storeId">The id of the store as a route parameter in the url.</param>
    /// <returns>This endpoint doesn't return a value.</returns>
    [Authorize(Policy = "Owner")]
    [HttpDelete("{storeId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult Delete(int storeId)
    {
        return NoContent();
    }
}