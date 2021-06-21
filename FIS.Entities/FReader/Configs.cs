using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace FIS.FRead
{
    public static class Configs
    {

        /// <summary>
        /// 
        /// </summary>
        public static string HOMEDirectory { get; private set; }
        static Configs()
        {
            HOMEDirectory = ConfigurationManager.AppSettings["proxyPass"]; // ConfigurationSettings.AppSettings["HOMEDirectory"];
        }
    }
}
