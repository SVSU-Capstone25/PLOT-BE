/*
    Filename: StoresController.cs
    Part of Project: PLOT/PLOT-BE/Controllers
    File Purpose:
    This file contains the store controller endpoint mapping,
    which will transport store data from the frontend
    to the database and vice versa.
    Written by: Jordan Houlihan
*/

using Microsoft.AspNetCore.Mvc;
using Plot.Data.Models.Stores;

namespace Plot.Controllers;

[ApiController]
[Route("[controller]")]
public class StoresController : ControllerBase
{

    /// <summary>
    /// This endpoint deals with getting a list of stores.
    /// </summary>
    /// <returns>A list of all stores as StoreDTO objects.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Store[]> GetAll()
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with getting a store with a specified id.
    /// </summary>
    /// <param name="storeId">The id of the store as a route parameter in the url.</param>
    /// <returns>The store tied to the storeId as a StoreDTO object.</returns>
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
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public ActionResult<Store> Create(StoreDTO store)
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with updating a store.
    /// </summary>
    /// <param name="storeId">The id of the store as a route parameter in the url.</param>
    /// <param name="store">The updated store information as a StoreDTO object in the request body.</param>
    /// <returns>The updated store information as a Store object.</returns>
    [HttpPut("{storeId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Store> Update(int storeId, StoreDTO store)
    {
        return NoContent();
    }

    /// <summary>
    /// This endpoint deals with deleting a store.
    /// </summary>
    /// <param name="storeId">The id of the store as a route parameter in the url.</param>
    /// <returns>This endpoint doesn't return a value.</returns>
    [HttpDelete("{storeId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult Delete(int storeId)
    {
        return NoContent();
    }
}