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
    public static List<ToDoItem> items = [];

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
        /*
        Idealne pouzitie CreatedAtAction je taketo, ale je pravda, ze to ja som vam ukazala zlu verziu v breakout room, kedze som bola zle naucena z prace, ako sa to pouziva.
        CreatedAtAction vie vracat rovno odkaz na vytvorenu instanciu a tato verzia metody berie tri parametre:
        1. meno metody, na ktorej odkaze najdeme novovytvorenu item (v nasom pripade ReadById)
        2. anonymny typ routeValues, ak nejake su, nasa ReadById berie {toDoItemId:int}, takze do tohto parametru vlozime nove ID
        - anonymny typ musi obsahovat rovnake nazvy route values, inak nam to nebude fungovat (napr. nefungovalo by new { id = ...}, lebo id nase ReadById nema v route)
        3. vysledny response
        */
        return CreatedAtAction(nameof(ReadById), new { toDoItemId = item.ToDoItemId }, ToDoItemGetResponseDto.FromDomain(item));
        //return CreatedAtAction("Create", ToDoItemGetResponseDto.FromDomain(item));
    }

    [HttpGet]
    public ActionResult<IEnumerable<ToDoItemGetResponseDto>> Read()
    {
        try
        {
            // tu nemusis vyhadzovat vynimku, ale rovno vrat NotFound - z catch tym padom moze ist ten check na 0 prec
            if (items.Count == 0)
                throw new Exception("Nic tu není.");
            var dtoItems = new List<ToDoItemGetResponseDto>();
            foreach (ToDoItem obj in items)
            {
                dtoItems.Add(ToDoItemGetResponseDto.FromDomain(obj));
            }
            return Ok(dtoItems);
        }
        catch (Exception ex)
        {
            if (items.Count == 0)
                return NotFound();
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{toDoItemId:int}")]
    public IActionResult ReadById(int toDoItemId)
    {
        bool itemNotFound = false;
        try
        {
            var item = items.Find(i => i.ToDoItemId == toDoItemId);
            if (item == null)
            {
                // nie je nutne mat itemNotFound vobec, staci v tomto rovno vratit NotFound(), z catch to moze ist taktiez prec
                itemNotFound = true;
                throw new Exception("Položka neexistuje.");
            }
            return Ok(ToDoItemGetResponseDto.FromDomain(item));
        }
        catch (Exception ex)
        {
            if (itemNotFound)
                return NotFound();
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{toDoItemId:int}")]
    public IActionResult UpdateById(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        // rovnako ako pri ReadById, nie je nutna itemNotFound
        bool itemNotFound = false;
        var item = request.ToDomain();
        item.ToDoItemId = toDoItemId;
        try
        {
            var index = items.FindIndex(i => i.ToDoItemId == toDoItemId);
            if (index != -1)
            {
                items[index] = item;
            }
            else
            {
                itemNotFound = true;
                throw new Exception("Položka nenalezena.");
            }
        }
        catch (Exception ex)
        {
            if (itemNotFound)
                return NotFound();
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
        // tu je treba podla zadania vratit 204 NoContent
        return CreatedAtAction("Create", ToDoItemGetResponseDto.FromDomain(item));
    }

    [HttpDelete("{toDoItemId:int}")]
    public IActionResult DeleteById(int toDoItemId)
    {
        // znova, tovno vratit NotFound ak sa nenajde, itemNotFound nemusi byt
        bool itemNotFound = false;
        try
        {
            var obj = items.Find(i => i.ToDoItemId == toDoItemId);
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
        catch (Exception ex)
        {
            if (itemNotFound)
                return NotFound();
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
        // podla zadania vratit 204 NoContent
        return Ok();
    }
}
