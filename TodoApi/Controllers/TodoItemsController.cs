using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Contracts.TodoItem;
using TodoApi.Contracts;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoItemsController : ControllerBase
{
    private readonly TodoContext _context;

    public TodoItemsController(TodoContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemResponse>>> GetTodoItems()
    {
        return Ok(await _context.TodoItems.ToListAsync());
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetTodoItem(long id)
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

    [HttpPatch("{id:long}")]
    public async Task<IActionResult> PatchTodoItem(long id, UpdateTodoItemRequest request)
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

    [HttpPost]
    public async Task<ActionResult<TodoItem>> PostTodoItem(CreateTodoItemRequest request)
    {
        var todoItem = new TodoItem
        {
            Name = request.Name,
            IsComplete = request.IsComplete
        };

        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            actionName: "GetTodoItem",
            routeValues: new { id = todoItem.Id },
            value: todoItem
        );
    }

    // DELETE: api/TodoItems/5
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteTodoItem(long id)
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
