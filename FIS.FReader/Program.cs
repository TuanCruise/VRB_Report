using System;
using System.Windows.Forms;
using System.Globalization;
using FIS.Common;
using FIS.FReader.Process;
using FIS.Base;
using FIS.Entities;


namespace FIS.FReader
{

    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}
