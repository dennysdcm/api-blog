using System;
using ApiBlog.Domain.Enums;

namespace ApiBlog.Domain.Exceptions;

public class PostInvalidOperationException(PostStatus currentStatus)
    : Exception($"Post invalid operation since post is {currentStatus} state");