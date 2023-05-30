using EasyMicroservices.Configuration.MicrosoftConfiguration.Providers;
using EasyMicroservices.Serialization.Newtonsoft.Json.Providers;
using Microsoft.Extensions.Configuration;

namespace EasyMicroservices.Configuration.Tests.Providers
{
    public class MicrosoftConfigurationProviderTest : BaseConfigurationProviderTest
    {
        public MicrosoftConfigurationProviderTest() : base(new MicrosoftConfigurationProvider(
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()))
        {

        }
    }
}
