using DevExpress.Skins;
using FIS.Common;
using FIS.Utils;
using System;
using System.Globalization;
using System.ServiceModel;
using System.Threading;
using System.Windows.Forms;

namespace FIS.AppClient
{
    public static class Program
    {
        public static string StrUserName = "";
        public static string StrPassWord = "";
        public static string StrMessageID = "";
        public static string strAppStartUpPath = "";
        public static bool blLogin = false;
        public static bool blCheckFile = false;
        public static string FileName = "";
        public static string strExecMod = "";
        public static string treeModuleID = "";
        public static bool blVerifyImport = false;
        public static bool blEnableImport = false;
        public static string txnum = "";
        public static string rptid = "";
        public static string rptlogID = "";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]

        static void Main(string[] args)
        {
            // TuanLM tam bo AutoUpdate
            //string strExeFile = "COMS.EXE";                      

            //string autoUpdateUri = ConfigurationSettings.AppSettings["AutoUpdateUri"];
            //string autoUpdateConfig = ConfigurationSettings.AppSettings["AutoUpdateConfig"];
            //string curentVersionApp = null;
            //string ManifestFile = "AutoUpdate.xml";
            //string strName = null;
            //string strVersion = null;
            //string strDate = null;
            //if (autoUpdateConfig == CONSTANTS.AutoConfigYes)
            //{
            //    try
            //    {
            //        XmlDocument xmld = new XmlDocument();                    
            //        var webClient = new WebClient();

            //        if (webClient.Proxy != null)
            //        {                                                
            //            MemoryStream ms = new MemoryStream(webClient.DownloadData(autoUpdateUri + ManifestFile));
            //            XmlTextReader rdr = new XmlTextReader(autoUpdateUri + ManifestFile);
            //            xmld.Load(rdr);
            //        }
            //        else
            //        {
            //            xmld.Load(autoUpdateUri + ManifestFile);
            //        }
            //        //TRUNGTT_20140804_End
            //        XmlNodeList xmlNodeList = xmld.SelectNodes("/update/Entry");
            //        foreach (XmlNode xmlNode in xmlNodeList)
            //        {
            //            strName = xmlNode.Attributes.GetNamedItem("filename").Value;
            //            strVersion = xmlNode.Attributes.GetNamedItem("version").Value;
            //            strDate = xmlNode.Attributes.GetNamedItem("date").Value;
            //            if (strName.ToUpper() == strExeFile.ToUpper())
            //                break;
            //        }

            //        if (File.Exists(Application.StartupPath + @"\" + strExeFile))
            //        {
            //            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(strExeFile);
            //            curentVersionApp = fileVersionInfo.FileMajorPart.ToString() + "." + fileVersionInfo.FileMinorPart.ToString() + "." + fileVersionInfo.FileBuildPart.ToString();
            //            //Application.ProductVersion
            //            //Version curVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;  

            //           // if (curentVersionApp != strVersion)
            //            if (curentVersionApp.CompareTo(strVersion) < 0)
            //            {
            //                if (File.Exists(Application.StartupPath + @"\" + "FIS.AutoUpdate.exe"))
            //                {
            //                    Process p = new Process();
            //                    p.StartInfo.FileName = "FIS.AutoUpdate.exe";
            //                    p.Start();
            //                    return;
            //                }
            //                else
            //                {
            //                    throw new FileNotFoundException("Không tìm thấy file FIS.AutoUpdate.exe trong thư mục cài đặt");
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        File.AppendAllText("LastErrors.log", string.Format("Check Auto Update Hệ Thống : {0} : {1} \r\n-------------\r\n", System.DateTime.Now.ToLongTimeString(), ex.ToString()));
            //        MessageBox.Show("Có lỗi trong quá trình Auto Update hệ thống", "Auto Update hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }

            //}

            //if (args.Length == 2)
            //{
            //    switch (args[0].ToUpper())
            //    {
            //        case "/I":
            //            foreach (var process in Process.GetProcesses())
            //            {
            //                Win32.COPYDATASTRUCT fileInfo;
            //                var bytes = Encoding.UTF8.GetBytes(args[1]);
            //                fileInfo.m_DwData = (IntPtr)0;
            //                fileInfo.m_LpData = args[1];
            //                fileInfo.m_CbData = bytes.Length + 1;

            //                Win32.SendMessage(process.MainWindowHandle, Win32.WM_COPYDATA, process.MainWindowHandle, ref fileInfo);
            //            }
            //            return;
            //    }
            //}

            if (args.Length == 0)
            {
                //MessageBox.Show("TrungTT");
            }
            else
            {
                StrUserName = args[0];
                StrPassWord = args[1];
            }

            var isInited = false;
            try
            {
                DevExpress.UserSkins.BonusSkins.Register();
                DevExpress.UserSkins.OfficeSkins.Register();
                strAppStartUpPath = Application.StartupPath;

                SkinManager.EnableFormSkins();
                frmSplash.ShowSplashScreen();

                for (var count = 1; count <= 3; count++)
                {
                    try
                    {
                        App.Environment = new ClientEnvironment
                        {
                            ClientInfo =
                                                      {
                                                          Culture = new CultureInfo("en-US")
                                                                        {
                                                                            DateTimeFormat =
                                                                                {
                                                                                    ShortDatePattern = "d/M/yyyy",
                                                                                    LongDatePattern = "dd MMMM yyyy"
                                                                                }
                                                                        }
                                                      }
                        };

                        ThreadUtils.SetClientCultureInfo();

                        isInited = true;
                        break;
                    }

                    catch (Exception e)
                    {
                        var ex = ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_UNKNOWN, e.Message);

                        for (var i = 5; i > 0; i--)
                        {
                            frmSplash.ChangeSplashStatus("Connect again after next " + i + "/" + count + " second(s)...");
                            Thread.Sleep(1000);
                        }
                    }
                }

                if (isInited)
                {
                    frmSplash.ChangeSplashStatus("Initializing application...");
                    frmSplash.CloseForm();


                    var frmMain = new frmMainRibbon();
                    Application.Run(frmMain);
                }
            }
            catch (FaultException ex)
            {
                frmInfo.ShowError("Main", ex);
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                frmInfo.ShowError("Main", ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_UNKNOWN, ex.Message));
                Environment.Exit(1);
            }
            finally
            {
                frmSplash.CloseForm();
                Environment.Exit(0);
            }
        }
    }
}