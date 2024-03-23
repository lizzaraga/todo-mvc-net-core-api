using Todo_API.Models.Dtos;
using Todo_API.Models.Entities;

namespace Todo_API.Services.Abstracts;

public interface ITodoService
{
    Task<Todo> Create(CreateTodoDto dto);
    Task<List<Todo>> GetAll();
    Task<Todo> Update(Guid id, UpdateTodoDto dto);
    Task<bool> Delete(Guid id);
    Task<Todo> Archive(Guid id);

}
