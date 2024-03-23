using System.ComponentModel.DataAnnotations;

namespace Todo_API.Models.Dtos;

public record RegisterDto(
    [Required]
    [EmailAddress]
    string Email,
    [DataType(DataType.Password)]
    string Password
    );
