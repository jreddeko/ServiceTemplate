namespace ServiceTemplate.BackgroundWorkers
{
    // The outcome of the worker process. The class enumerate the below possible state.
    // This is also offering help on logging.
    internal enum HealthProbeOutcome
    {
        Unknown,
        Success,
        TransportFailure,
        HttpFailure,
        Timeout,
        Canceled,
    }
}
