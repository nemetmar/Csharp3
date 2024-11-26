using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;

namespace ToDoList.WebApi;

[ApiController]
[Route("api/[controller]")]
public class ToDoItemsController : ControllerBase
{
    public static List<ToDoItem> items = [];

    private readonly IRepositoryAsync<ToDoItem> repository;


    public ToDoItemsController(IRepositoryAsync<ToDoItem> repository)
    {
        this.repository = repository;
    }





    [HttpPost]
    public async Task<IActionResult> CreateAsync(ToDoItemCreateRequestDto request)
    {
        var item = request.ToDomain();
        try
        {
            await repository.CreateAsync(item);
            var dto = ToDoItemGetResponseDto.FromDomain(item);
            return CreatedAtAction(nameof(ReadByIdAsync), new { toDoItemId = item.ToDoItemId }, dto);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ToDoItemGetResponseDto>>> ReadAsync()
    {
        try
        {
            var repositoryItems = await repository.ReadAsync();
            if (repositoryItems is null || !repositoryItems.Any())
                return NotFound();

            return Ok(repositoryItems.Select(ToDoItemGetResponseDto.FromDomain).ToList());
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{toDoItemId:int}")]
    [ActionName(nameof(ReadByIdAsync))] //toto tady musí být, aby fungovala srpávně CreatedAtAction u Create Async
    public async Task<IActionResult> ReadByIdAsync(int toDoItemId)
    {

        try
        {
            var item = await repository.ReadByIdAsync(toDoItemId);

            if (item == null)
                return NotFound();

            return Ok(ToDoItemGetResponseDto.FromDomain(item));
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{toDoItemId:int}")]
    public async Task<IActionResult> UpdateByIdAsync(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        try
        {
            var itemToUpdate = await repository.ReadByIdAsync(toDoItemId);
            if (itemToUpdate is null)
            {
                return NotFound(); //404
            }
            await repository.UpdateByIdAsync(toDoItemId, request.ToDomain());
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
        return NoContent();
    }

    [HttpDelete("{toDoItemId:int}")]
    public async Task<IActionResult> DeleteByIdAsync(int toDoItemId)
    {
        try
        {
            var itemToDelete = await repository.ReadByIdAsync(toDoItemId);
            if (itemToDelete is null)
            {
                return NotFound(); //404
            }
            await repository.DeleteByIdAsync(toDoItemId);

        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
        return NoContent();
    }
}
