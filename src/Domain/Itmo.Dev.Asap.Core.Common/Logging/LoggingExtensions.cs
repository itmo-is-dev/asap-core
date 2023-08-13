using Microsoft.Extensions.Logging;

namespace Itmo.Dev.Asap.Core.Common.Logging;

public static class LoggingExtensions
{
    public static ILogger WithPrefix(this ILogger logger, string prefix)
    {
        return new PrefixLoggerProxy(logger, prefix);
    }
}