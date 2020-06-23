using Microsoft.Extensions.Options;

namespace ServiceTemplate.Configuration
{
    public class RootConfiguration : IRootConfiguration
    {
        public IOpenIdConfiguration OpenIdConfiguration { get; set; }
        public IResourceConfiguration ResourceConfiguration { get; set; }
        public ITimerConfiguration TimerConfiguration { get; set; }

        public RootConfiguration(IOptions<OpenIdConfiguration> openIdConfiguration,
            IOptions<ResourceConfiguration> resourceConfiguration,
            IOptions<TimerConfiguration> timerConfiguration)
        {
            OpenIdConfiguration = openIdConfiguration.Value;
            ResourceConfiguration = resourceConfiguration.Value;
            TimerConfiguration = timerConfiguration.Value;
        }
    }
}
