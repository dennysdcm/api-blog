using System;
using ApiBlog.Application.Services;

namespace ApiBlog.Infrastructure;

internal class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}