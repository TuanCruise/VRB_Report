using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FIS.Entities;
namespace FIS.Entities.FRead
{
    public static class Globals
    {
        public static Session GlobalSession { get; set; }

        public static void SetGlobalSession(Session s)
        {
            GlobalSession = s;
        }
    }
    /// <summary>
    /// Created by LongND5, 2010-June-23
    /// Trạng thái của các Process
    /// </summary>
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
