using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo_API.Models.Dtos;
using Todo_API.Models.Entities;
using Todo_API.Services.Abstracts;

namespace Todo_API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TodosController(ITodoService todoService): ControllerBase
{
    /// <summary>
    /// Get all todos
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<Todo>>> Index()
    {
        return await todoService.GetAll();
    }
    
    [HttpPost]
    public async Task<ActionResult<Todo>> CreateTodo(
        [FromBody] CreateTodoDto dto
        )
    {
        return await todoService.Create(dto);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Todo>> UpdateTodo(
        [FromRoute] Guid id,
        [FromBody] UpdateTodoDto dto
    )
    {
        return await todoService.Update(id, dto);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Boolean>> DeleteTodo(
        [FromRoute] Guid id
    )
    {
        return await todoService.Delete(id);
    }
}
