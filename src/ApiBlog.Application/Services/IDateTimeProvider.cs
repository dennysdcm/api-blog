using System;

namespace ApiBlog.Application.Services;

public interface IDateTimeProvider
{
    DateTimeOffset UtcNow { get; }
}