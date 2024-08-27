using Microsoft.Extensions.Logging;
using System.Security.Principal;

namespace SeriLogVsNLog;
internal static partial class Logs
{

    [LoggerMessage(LogLevel.Error, "Some log, bla bla {value}")]
    public static partial void Comment(this ILogger logger, int value, [LogProperties] IIdentity identity);
}
