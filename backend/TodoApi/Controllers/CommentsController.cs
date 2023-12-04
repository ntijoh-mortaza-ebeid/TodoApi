using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using TodoApi.Contracts.Comment;
using TodoApi.Contracts.TodoItem;
using TodoApi.Models;

namespace TodoApi.Controllers;

[Route("api/TodoItems/{todoItemId}/[controller]")]
[ApiController]
[Produces("application/json")]
public class CommentsController : ControllerBase
{
    private readonly TodoContext _context;

    public CommentsController(TodoContext context)
    {
        _context = context;
    }

    [HttpGet(Name = "getAllCommentsForTodo")]
    [SwaggerOperation(Description = "Gets all comments for a todo item")]
    [ProducesResponseType(typeof(List<Comment>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetComments(long todoItemId)
    {
        var todoItem = await _context.TodoItems.FindAsync(todoItemId);
        if (todoItem == null)
        {
            return NotFound();
        }

        return Ok(todoItem.Comments);
    }

    [HttpGet(template: "{id:long}", Name = "GetCommentById")]
    [SwaggerOperation(Description = "Returns a comment given an id")]
    [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetComment(long todoItemId, long commentId)
    {
        var todoItem = await _context.TodoItems.FindAsync(todoItemId);

        if (todoItem == null)
        {
            return NotFound();
        }

        var comment = todoItem.Comments.Find(comment => comment.Id == commentId);

        if (comment == null)
        {
            return NotFound(); // TODO find out what happens when you feed comment here
        }

        var response = new CommentResponse(
            comment.Id,
            comment.Contents
        );

        return Ok(response);
    }

    [HttpPatch(template: "{id:long}", Name = "UpdateCommentById")]
    [SwaggerOperation(Description = "Patches a comment given an id and requested changes")]
    [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PatchComment(long todoItemId, long commentId, UpdateCommentRequest request)
    {
        var currentTodoItem = await _context.TodoItems.FindAsync(todoItemId);

        if (currentTodoItem == null)
        {
            return NotFound();
        }

        var currentComment = currentTodoItem.Comments.Find(comment => comment.Id == commentId);

        if (currentComment == null)
        {
            return NotFound(); // TODO find out what happens when you feed comment here
        }

        var newComments = currentTodoItem.Comments.Select(comment => {
            if (comment.Id == commentId)
            {
                return new Comment
                {
                    Id = currentComment.Id,
                    Contents = request.Contents ?? currentComment.Contents
                };
            }
            return comment;
        }).ToList();

        var newTodoItem = new TodoItem
        {
            Id = currentTodoItem.Id,
            Name = currentTodoItem.Name,
            IsComplete = currentTodoItem.IsComplete,
            Comments = newComments
        };

        _context.Entry(currentTodoItem).CurrentValues.SetValues(newTodoItem);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CommentExists(todoItemId, commentId))
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

    [HttpPost(Name = "postComment")]
    [SwaggerOperation(Description = "Creates a comment")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Comment>> PostComment(long todoItemId, CreateCommentRequest request)
    {
        var todoItem = await _context.TodoItems.FindAsync(todoItemId);
        
        if (todoItem == null)
        {
            return NotFound();
        }

        var newComment = new Comment
        {
            Contents = request.Contents
        };
        
        todoItem.Comments.Add(newComment);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            bool todoItemExists = _context.TodoItems.Any(todoItem => todoItem.Id == todoItemId);
            if (!todoItemExists)
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction(
            actionName: "GetComment",
            routeValues: new { todoItemId, commentId = newComment.Id },
            value: newComment);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(long id)
    {
        var comment = await _context.Comment.FindAsync(id);
        if (comment == null)
        {
            return NotFound();
        }

        _context.Comment.Remove(comment);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CommentExists(long todoItemId, long commentId)
    {
        var todoItem = _context.TodoItems.Find(todoItemId);
        if (todoItem == null) {  return false; }
        return todoItem.Comments.Any(comment => comment.Id == commentId);
    }

    //private Comment? FetchComment(long todoItemId, long commentId)
    //{ TODO Should this be implemented? check if anything useful can be sent in NotFound.
    //} If not then this should be implemented, thinks the present me
}
