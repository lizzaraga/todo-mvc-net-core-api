using System.ComponentModel.DataAnnotations;

namespace Todo_API.Models.Dtos;

public record UpdateTodoDto (
    [Required]
    string Label, 
    string Description
    );
