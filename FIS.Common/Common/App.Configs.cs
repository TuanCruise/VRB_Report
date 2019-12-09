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
                ConnectionString = ConfigurationSettings.AppSettings["ConnectionString"];
                ServiceUri = ConfigurationSettings.AppSettings["ServiceUri"];
                ServiceUriHttp = ConfigurationSettings.AppSettings["ServiceUriHttp"];
                UseProxy = ConfigurationSettings.AppSettings["UseProxy"];
                ProxyUrl = ConfigurationSettings.AppSettings["proxyUrl"];
                ProxyUser = ConfigurationSettings.AppSettings["proxyUser"];
                ProxyPass = ConfigurationSettings.AppSettings["proxyPass"];  
            }
        }
    }
}
