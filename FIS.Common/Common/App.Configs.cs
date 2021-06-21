using System.Configuration;

namespace FIS.Common
{
    public static partial class App
    {
        public static class Configs
        {
            public static string ConnectionString { get; set; }
            public static string ServiceUri { get; set; }
            public static string ServiceUriHttp { get; set; }
            public static string UpdatedToDateVersion { get; set; }
            public static string UseProxy { get; set; }
            public static string ProxyUrl { get; set; }
            public static string ProxyUser { get; set; }
            public static string ProxyPass { get; set; }

            static Configs()
            {
                ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];// ConfigurationSettings.AppSettings["ConnectionString"];
                ServiceUri = ConfigurationManager.AppSettings["ServiceUri"];// ConfigurationSettings.AppSettings["ServiceUri"];
                ServiceUriHttp = ConfigurationManager.AppSettings["ServiceUriHttp"];// ConfigurationSettings.AppSettings["ServiceUriHttp"];
                UseProxy = ConfigurationManager.AppSettings["UseProxy"];// ConfigurationSettings.AppSettings["UseProxy"];
                ProxyUrl = ConfigurationManager.AppSettings["proxyUrl"];// ConfigurationSettings.AppSettings["proxyUrl"];
                ProxyUser = ConfigurationManager.AppSettings["proxyUser"];// ConfigurationSettings.AppSettings["proxyUser"];
                ProxyPass = ConfigurationManager.AppSettings["proxyPass"];// ConfigurationSettings.AppSettings["proxyPass"];
            }
        }
    }
}
