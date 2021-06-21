using FIS.Common;
using FIS.Entities;
using System.Collections.Generic;
using System.Linq;

namespace FIS.Utils
{
    public static class SysvarUtils
    {
        public static List<SysvarInfo> GetValues(string grName)
        {
            return (from value in AllCaches.SysvarsInfo
                    where value.GrName == grName
                    select value).ToList();
        }

        public static string GetVarValue(string grName, string varName)
        {
            return (from value in AllCaches.SysvarsInfo
                    where value.GrName == grName && value.VarName == varName
                    select value).First().VarValue;
        }
    }
}
