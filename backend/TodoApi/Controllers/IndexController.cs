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

[Route("")]
[ApiController]
public class IndexController : ControllerBase
{
    [HttpGet(Name = "getSPA")]
    [SwaggerOperation(Description = "Gets the SPA html")]
    [ProducesResponseType(typeof(List<TodoItem>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TodoItemResponse>>> GetTodoItems()
    {
        return Ok();
    }
}