using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence;

namespace ToDoList.WebApi;

[ApiController]
[Route("api/[controller]")]
public class ToDoItemsController : ControllerBase
{
    public static List<ToDoItem> items = [];

    private readonly ToDoItemsContext context;

    public ToDoItemsController(ToDoItemsContext context)
    {
        this.context = context;
    }



    [HttpPost]
    public IActionResult Create(ToDoItemCreateRequestDto request)
    {
        var item = request.ToDomain();
        try
        {
            context.ToDoItems.Add(item);
            context.SaveChanges();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
        return CreatedAtAction("Create", ToDoItemGetResponseDto.FromDomain(item));
    }

    [HttpGet]
    public ActionResult<IEnumerable<ToDoItemGetResponseDto>> Read()
    {
        try
        {

            if (!context.ToDoItems.Any()) return NotFound();
            
            var dtoItems = new List<ToDoItemGetResponseDto>();
            foreach (var obj in context.ToDoItems)
            {
                dtoItems.Add(ToDoItemGetResponseDto.FromDomain(obj));
            }

            return Ok(dtoItems);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{toDoItemId:int}")]
    public IActionResult ReadById(int toDoItemId)
    {

        try
        {
            var item = context.ToDoItems.Find(toDoItemId);

            if (item == null) return NotFound();

            return Ok(ToDoItemGetResponseDto.FromDomain(item));
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{toDoItemId:int}")]
    public IActionResult UpdateById(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        try
        {
            var itemToUpdate = context.ToDoItems.Find(toDoItemId);
            if(itemToUpdate is null) return NotFound();

            itemToUpdate.Name = request.Name;
            itemToUpdate.Description = request.Description;
            itemToUpdate.IsCompleted = request.IsCompleted;

            context.SaveChanges();

        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
        return NoContent();
    }

    [HttpDelete("{toDoItemId:int}")]
    public IActionResult DeleteById(int toDoItemId)
    {
        try
        {
            var obj = context.ToDoItems.Find(toDoItemId);

            if(obj is null) return NotFound();

            context.ToDoItems.Remove(obj);
            context.SaveChanges();

        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
        return NoContent();
    }
}
