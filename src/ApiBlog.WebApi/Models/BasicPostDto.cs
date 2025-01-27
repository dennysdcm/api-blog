using System.ComponentModel.DataAnnotations;

namespace ApiBlog.WebApi.Models;

public record BasicPostDto(
    [Required] string Title,
    [Required] string Content);