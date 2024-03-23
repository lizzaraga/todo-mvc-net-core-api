using Todo_API.Data;
using Todo_API.Models.Dtos;
using Todo_API.Models.Entities;
using Todo_API.Services.Abstracts;

namespace Todo_API.Services;

public class TodoService(ILogger<TodoService> logger, TodoDbContext context): ITodoService
{
   
    
    public async Task<Todo> Create(CreateTodoDto dto)
    {
        var todo = new Todo()
        {
            Label = dto.Label,
            Description = dto.Description,
        };
        var entry = context.Todos.Add(todo);
        await context.SaveChangesAsync();
        return entry.Entity;
    }

    public Task<List<Todo>> GetAll()
    {
        return Task.FromResult(context.Todos.ToList());
    }

    public async Task<Todo> Update(Guid id, UpdateTodoDto dto)
    {
        var todo = context.Todos.FirstOrDefault(x => x.Id == id);
        if (todo is not null)
        {
            todo.Label = dto.Label;
            todo.Description = dto.Description;
            await context.SaveChangesAsync();

        }

        return todo;
    }

    public async Task<bool> Delete(Guid id)
    {
        var isRemove = false;
        var todo = context.Todos.FirstOrDefault(x => x.Id == id);
        if (todo is not null)
        {
            context.Todos.Remove(todo);
            isRemove = (await context.SaveChangesAsync()) > 0;
        }
           
        return isRemove;
    }

    public Task<Todo> Archive(Guid id)
    {
        throw new NotImplementedException();
    }
}
