using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

namespace ToDoList.WebApi;

[ApiController]
[Route("api/[controller]")]
public class ToDoItemsController : ControllerBase
{
    private static List<ToDoItem> items = [];

    [HttpPost]
    public IActionResult Create(ToDoItemCreateRequestDto request)
    {
        var item = request.ToDomain();
        try
        {
            item.ToDoItemId = items.Count == 0 ? 1 : items.Max(i => i.ToDoItemId) + 1;
            items.Add(item);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
        return CreatedAtAction("Create", ToDoItemGetResponseDto.FromDomain(item));
    }

    [HttpGet]
    public IActionResult Read()
    {
        try
        {
            if (items.Count == 0) throw new Exception("Nic tu není.");
            var dtoItems = new List<ToDoItemGetResponseDto>();
            foreach(ToDoItem obj in items)
            {
                dtoItems.Add(ToDoItemGetResponseDto.FromDomain(obj));
            }
            return Ok(dtoItems);
        }
        catch(Exception ex)
        {
            if (items.Count == 0)  return Problem(ex.Message, null, StatusCodes.Status404NotFound);
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{toDoItemId:int}")]
    public IActionResult ReadById(int toDoItemId)
    {
        bool itemNotFound = false;
        try
        {
            var item = items.FirstOrDefault(i => i.ToDoItemId == toDoItemId);
            if (item == null)
            {
                itemNotFound = true;
                throw new Exception("Položka neexistuje.");
            }
            return Ok(ToDoItemGetResponseDto.FromDomain(item));
        }
        catch(Exception ex)
        {
            if (itemNotFound)  return Problem(ex.Message, null, StatusCodes.Status404NotFound);
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{toDoItemId:int}")]
    public IActionResult UpdateById(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        bool itemNotFound = false;
        var item = request.ToDomain();
        item.ToDoItemId = toDoItemId;
        try
        {
            var obj = items.FirstOrDefault(i => i.ToDoItemId == toDoItemId);
            if (obj != null)
            {
                obj.Name = item.Name;
                obj.Description = item.Description;
                obj.IsCompleted = item.IsCompleted;
            }
            else
            {
                itemNotFound = true;
                throw new Exception("Položka nenalezena.");
            }
        }
        catch(Exception ex)
        {
            if (itemNotFound)  return Problem(ex.Message, null, StatusCodes.Status404NotFound);
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
        return CreatedAtAction("Create", ToDoItemGetResponseDto.FromDomain(item));
    }

    [HttpDelete("{toDoItemId:int}")]
    public IActionResult DeleteById(int toDoItemId)
    {
        bool itemNotFound = false;
        try
        {
            var obj = items.FirstOrDefault(i => i.ToDoItemId == toDoItemId);
            if (obj != null)
            {
                items.Remove(obj);
            }
            else
            {
                itemNotFound = true;
                throw new Exception("Položka nenalezena.");
            }
        }
        catch(Exception ex)
        {
            if (itemNotFound)  return Problem(ex.Message, null, StatusCodes.Status404NotFound);
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
        return Ok();
    }
}
