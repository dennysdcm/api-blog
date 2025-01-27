using System;

namespace ApiBlog.Application.Exceptions;

public class PostNotFoundException(string postId) : Exception($"Post with Id {postId} was not found");