using Microsoft.Extensions.Configuration;

namespace BackendChallenge.EndToEnd
{
    static class ConfigurationFactory
    {
        private static IConfigurationRoot configuration;

         public static IConfigurationRoot GetConfiguration()
         {
            if (configuration == null) {
                configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            }

            return configuration;
         }
    }
}
