using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FIS.Entities;

namespace FIS.FReader.Entities
{
    public static class FReadEntities
    {
        public static Session MainSession { get; set; }
        public static void SetGlobalSession(Session s)
        {
            MainSession = s;
        }

        public static string HOMEFolderDirectory { get; set; }
        public static void SetDirectory(string s)
        {
            HOMEFolderDirectory = s.ToUpper();
        }
    }
    public enum ProcessState
    {
        Running = 0,
        Stopped = 1,
        Saving = 2,
        Error = 3,
        Sleep = 4,
        Buffer = 5
    }
}
