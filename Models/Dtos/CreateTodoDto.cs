using System.ComponentModel.DataAnnotations;

namespace Todo_API.Models.Dtos;

public record CreateTodoDto(
    [Required]
    string Label, 
    string Description
    );
