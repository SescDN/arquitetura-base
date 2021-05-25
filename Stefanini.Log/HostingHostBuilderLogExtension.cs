using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Stefanini.Log
{
    public static class HostingHostBuilderLogExtension
    {
        public static IHostBuilder UseLog(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureLogging(logging =>
              {
                  logging.ClearProviders();
                  logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
              }).UseNLog();
        }
    }
}
