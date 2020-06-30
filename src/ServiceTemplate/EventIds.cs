using Microsoft.Extensions.Logging;

namespace ServiceTemplate
{
    internal static class EventIds
    {
        public static readonly EventId WorkerStopping = new EventId(1, "WorkerStopping");
        public static readonly EventId WorkerGracefulShutdown = new EventId(2, "WorkerGracefulShutdown");
        public static readonly EventId ServiceStopped = new EventId(3, "ServiceStopped");
    }
}
