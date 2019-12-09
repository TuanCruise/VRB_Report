using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using FIS.Base;
using FIS.Common;

namespace FIS.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            GenerateDEFCODEScript();
            GenerateDEFERRORScript();
#endif
            try
            {
#if DEBUG
                App.Environment = new ServerEnvironment();
                new FISService().StartDebug();
#else
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
			    { 
				    new FISService() 
			    };
                ServiceBase.Run(ServicesToRun);
#endif
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("FIS.Service", ex.ToString(), EventLogEntryType.Error);
            }
        }
#if DEBUG
        private static void GenerateDEFERRORScript()
        {
            const string FILE_NAME = @"..\..\DEFERROR.SQL";
            File.Delete(FILE_NAME);
            foreach (var errorClass in Assembly.GetAssembly(typeof(ERR_SYSTEM)).GetTypes())
            {
                if (errorClass.Name.StartsWith("ERR_"))
                {
                    foreach (var error in errorClass.GetFields(BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.DeclaredOnly))
                    {
                        File.AppendAllText(FILE_NAME,
                            string.Format("DELETE FROM DEFERROR WHERE ERRCODE = '{0}';\r\n", (int)error.GetValue(null)));
                        File.AppendAllText(FILE_NAME,
                            string.Format("INSERT INTO DEFERROR(ERRCODE, ERRNAME) VALUES ({0}, '{1}');\r\n",
                                (int)error.GetValue(null), error.Name));
                    }

                    foreach (var attrClass in errorClass.GetCustomAttributes(typeof(SyncDBAttribute), false))
                    {
                        File.AppendAllText(FILE_NAME, string.Format("CREATE OR REPLACE\r\n"));
                        File.AppendAllText(FILE_NAME, string.Format("PACKAGE {0}\r\n", ((SyncDBAttribute)attrClass).SyncValue));
                        File.AppendAllText(FILE_NAME, string.Format("IS\r\n"));
                        foreach (var error in errorClass.GetFields(BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.DeclaredOnly))
                        {
                            foreach (var attrError in error.GetCustomAttributes(typeof(SyncDBAttribute), false))
                            {
                                File.AppendAllText(FILE_NAME,
                                    string.Format("    {0} CONSTANT VARCHAR2(3) := '{1}';\r\n", ((SyncDBAttribute)attrError).SyncValue, error.GetValue(null)));
                            }
                        }
                        File.AppendAllText(FILE_NAME, string.Format("END;\r\n"));
                    }
                }
            }
        }

        public static void GenerateDEFCODEScript()
        {
            const string FILE_NAME = @"..\..\DEFCODE.SQL";
            File.Delete(FILE_NAME);

            var deleteTexts = new List<string>();
            var insertTexts = new List<string>();
            var classesText = new List<string>();
            foreach(var assemblyName in Assembly.GetEntryAssembly().GetReferencedAssemblies())
            {
                if(assemblyName.FullName.StartsWith("FIS"))
                {
                    var assembly = Assembly.Load(assemblyName.FullName);
                    foreach (var type in assembly.GetTypes())
                    {
                        if (type.Namespace != null && type.Namespace.StartsWith("FIS.CODES"))
                        {
                            var splited = type.Namespace.Split(new[] {"."}, StringSplitOptions.None);
                            var strCDTYPE = splited[2];
                            var strCDNAME = type.Name;
                            
                            var deleteText = string.Format("DELETE FROM DEFCODE WHERE CDTYPE = '{0}' AND CDNAME = '{1}';\r\n", strCDTYPE, strCDNAME);
                            if (!deleteTexts.Contains(deleteText)) deleteTexts.Add(deleteText);

                            foreach (var cdvalue in type.GetFields(BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.DeclaredOnly))
                            {
                                var strCDVALUENAME = cdvalue.Name;
                                var insertText =
                                    string.Format(
                                        "INSERT INTO DEFCODE(CDTYPE, CDNAME, CDVALUENAME, CDVALUE) VALUES ('{0}', '{1}', '{2}', '{3}');\r\n",
                                        strCDTYPE, strCDNAME, strCDVALUENAME, (string) cdvalue.GetValue(null));
                                if (!insertTexts.Contains(insertText)) insertTexts.Add(insertText);
                            }

                            
                            foreach (var attr in type.GetCustomAttributes(typeof(SyncDBAttribute), false))
                            {
                                var classText = string.Format("CREATE OR REPLACE\r\n");
                                classText += string.Format("PACKAGE {0}\r\n", ((SyncDBAttribute)attr).SyncValue);
                                classText += string.Format("IS\r\n");
                                foreach (var cdvalue in type.GetFields(BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.DeclaredOnly))
                                {
                                    classText += string.Format("    {0} CONSTANT VARCHAR2(3) := '{1}';\r\n", cdvalue.Name, cdvalue.GetValue(null));
                                }
                                classText += string.Format("END;\r\n");

                                if (!classesText.Contains(classText)) classesText.Add(classText);
                            }
                        }
                    }
                }
            }

            File.AppendAllText(FILE_NAME,
                "-- CODES PACKAGES\r\n" +
                string.Join("/\r\n", classesText.ToArray()) +
                "-- CLEAN CODES\r\n" +
                string.Join("/\r\n", deleteTexts.ToArray()) +
                "-- INSERT CODES\r\n" +
                string.Join("/\r\n", insertTexts.ToArray()) +
                "-- COMMIT CHANGES\r\n" +
                "COMMIT;\r\n" +
                "-- COMPILE ALL INVALID OBJECTS\r\n" +
                "EXEC DBMS_UTILITY.COMPILE_SCHEMA(SCHEMA => USER, COMPILE_ALL => FALSE);\r\n");
        }
#endif
    }
}
