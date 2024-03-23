using System.ComponentModel.DataAnnotations;

namespace Todo_API.Controllers;

public record LoginDto(
    [Required] [EmailAddress] string Email,
    [DataType(DataType.Password)] string Password);
