namespace ServiceTemplate.Configuration
{
    public interface IRootConfiguration
    {
        IOpenIdConfiguration OpenIdConfiguration { get; }
        IResourceConfiguration ResourceConfiguration { get; }
        ITimerConfiguration TimerConfiguration { get; }
    }
}
