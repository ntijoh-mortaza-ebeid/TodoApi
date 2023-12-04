using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Contracts.TodoItem;
using Swashbuckle.AspNetCore.Annotations;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TodoItemsController : ControllerBase
{
    private readonly TodoContext _context;

    public TodoItemsController(TodoContext context)
    {
        _context = context;
    }

    [HttpGet(Name = "getAllTodoItems")]
    [SwaggerOperation(Description = "Gets all todo items")]
    [ProducesResponseType(typeof(List<TodoItem>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TodoItemResponse>>> GetTodoItems()
    {
        return Ok(await _context.TodoItems.ToListAsync());
    }

    [HttpGet(template: "{id:long}", Name = "getTodoItemById")]
    [SwaggerOperation(Description = "Returns a todo item given an id")]
    [ProducesResponseType(typeof(TodoItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTodoItem([FromRoute]long id)
    {
        TodoItem? todoItem = await _context.TodoItems.FindAsync(id);

        if (todoItem == null)
        {
            return NotFound();
        }

        var response = new TodoItemResponse(
            todoItem.Id,
            todoItem.Name,
            todoItem.IsComplete
        );

        return Ok(response);
    }

    [HttpPatch(template: "{id:long}", Name = "patchTodoItemById")]
    [SwaggerOperation(Description = "Patches a todo item given an id and requested changes")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> PatchTodoItem([FromRoute]long id, [FromBody]UpdateTodoItemRequest request)
    {
        var currentTodoItem = await _context.TodoItems.FindAsync(id);
        if (currentTodoItem == null)
        {
            return NotFound();
        }

        var newTodoItem = new TodoItem
        {
            Id = currentTodoItem.Id,
            Name = request.Name ?? currentTodoItem.Name,
            IsComplete = request.IsComplete ?? currentTodoItem.IsComplete
        };

        _context.Entry(currentTodoItem).CurrentValues.SetValues(newTodoItem);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TodoItemExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpPost(Name = "postTodoItem")]
    [SwaggerOperation(Description = "Creates a todo item")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<TodoItem>> PostTodoItem([FromBody]CreateTodoItemRequest request)
    {
        var todoItem = new TodoItem
        {
            Name = request.Name,
            IsComplete = request.IsComplete ?? false
        };

        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            actionName: "GetTodoItem",
            routeValues: new { id = todoItem.Id },
            value: todoItem);
    }

    [HttpDelete(template: "{id:long}", Name = "deleteTodoItemById")]
    [SwaggerOperation(Description = "Deletes a todo item given an id")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteTodoItem([FromRoute]long id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TodoItemExists(long id)
    {
        return _context.TodoItems.Any(e => e.Id == id);
    }
}
