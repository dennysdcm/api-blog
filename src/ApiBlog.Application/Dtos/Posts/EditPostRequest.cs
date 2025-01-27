namespace ApiBlog.Application.Dtos;

public record EditPostRequest(string Id, string Title, string Content);