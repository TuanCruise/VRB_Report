using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using FIS.Entities;

namespace FIS.FReader
{
    public static class Configs
    {
        public static string HOMEDirectory { get; private set; }

        static Configs()
        {
            HOMEDirectory = ConfigurationManager.AppSettings["HOMEDirectory"];
        }
    }
}
