using Microsoft.Extensions.Configuration;

namespace MYRestApiServer.Core.Extensions
{
    public static class IConfigurationExtensions
    {
        public static T GetValue<T>(this IConfiguration configuration, string keyName)
        {
            return configuration.GetSection(keyName).Get<T>();
        }
    }
}
