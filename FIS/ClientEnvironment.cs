using System.Collections.Generic;
using System.Diagnostics;
using DevExpress.XtraEditors.Repository;
using FIS.Common;
using FIS.Controllers;
using FIS.Entities;
using FIS.Extensions;
using FIS.Utils;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using FIS.AppClient.Utils;

namespace FIS.AppClient
{
    public class ClientEnvironment : AbstractEnvironment
    {
        private CachedHashInfo m_ServerCachedHashInfo;
        public CachedHashInfo ServerCachedHashInfo {
            get
            {
                return m_ServerCachedHashInfo;
            }
        }
        
        public override EnvironmentType EnvironmentType
        {
            get { return EnvironmentType.CLIENT_APPLICATION; }
        }

        public override sealed RegistryKey MaximusRegistry
        {
            get
            {
                return Registry.LocalMachine
                    .OpenSubKey("SOFTWARE", true)
                    .CreateSubKey("FIS-BANK")
                    .CreateSubKey("SMS")
                    .CreateSubKey("Client");
            }
        }

        public void RegisterExtension()
        {
            try
            {
                var directory = new FileInfo(Application.ExecutablePath).Directory.FullName;
                Registry.ClassesRoot
                    .CreateSubKey(".mpkg")
                    .SetValue(null, "MPKGFile", RegistryValueKind.String);
                Registry.ClassesRoot
                    .CreateSubKey("MPKGFile")
                    .SetValue(null, "Modules Package File", RegistryValueKind.String);
                Registry.ClassesRoot
                    .CreateSubKey("MPKGFile")
                    .CreateSubKey("DefaultIcon")
                    .SetValue(null, directory + "\\MPKG.ico", RegistryValueKind.String);
                Registry.ClassesRoot
                    .CreateSubKey("MPKGFile")
                    .CreateSubKey("Shell")
                    .CreateSubKey("Open")
                    .CreateSubKey("Command")
                    .SetValue(null, Application.ExecutablePath + " /I \"%1\"");
            }
            catch
            {                
            }
        }

        public ClientEnvironment()
        {
            ClientInfo = new ClientInfo
                             {
                                 //LanguageID =MaximusRegistry.GetValueOrCreate(CONSTANTS.REGNAME_LANGID, CONSTANTS.DEFAULT_LANGID)
                                 LanguageID = CONSTANTS.DEFAULT_LANGID
                             };

            GetServerInfo();
            InitializeEnvironment();
            RegisterExtension();

            RepositoryItem.EditValueChangedFiringDelay = 500;
        }

        public void GetServerInfo()
        {
            using (var ctrlSA = new SAController())
            {
                ctrlSA.GetServerInfo(out m_ServerInfo, out m_ServerCachedHashInfo, ClientInfo.LanguageID);
            }
        }

        protected override void OnInitializeStepChanged(string stepName)
        {
            frmSplash.ChangeSplashStatus(stepName);
        }

        public override void InitializeMenu()
        {
            MainProcess.InitializeMenu();
        }

        public override List<LanguageInfo> BuildLanguageCache()
        {
            var instanceHash = ClientInfo.LanguageID + "-" + ServerCachedHashInfo.LanguageHash;
            var colLanguageInfo = CachedUtils.GetCacheOf<LanguageInfo>(instanceHash);
            if(colLanguageInfo == null)
            {
                using (var ctrlSA = new SAController())
                {
                    ctrlSA.ListLanguage(out colLanguageInfo, ClientInfo.LanguageID);
                }

                CachedUtils.SetCacheOf(colLanguageInfo, instanceHash);
            }

            return colLanguageInfo;
        }

        public override List<ErrorInfo> BuildErrorsInfoCache()
        {
            var colErrorsInfo = CachedUtils.GetCacheOf<ErrorInfo>(ServerCachedHashInfo.ErrorsInfoHash);
            if(colErrorsInfo == null)
            {
                using (var ctrlSA = new SAController())
                {
                    ctrlSA.ListErrorsInfo(out colErrorsInfo);
                }

                CachedUtils.SetCacheOf(colErrorsInfo, ServerCachedHashInfo.ErrorsInfoHash);
            }

            return colErrorsInfo;
        }

        public override List<ModuleFieldInfo> BuildModuleFieldCache()
        {
            var modulesFieldsInfo = CachedUtils.GetCacheOf<ModuleFieldInfo>(ServerCachedHashInfo.ModuleFieldsInfoHash);
            if(modulesFieldsInfo == null)
            {
                using (var ctrlSA = new SAController())
                {
                    List<ModuleFieldInfo> tempModuleFields;
                    var startRow = 0;                                                                  
                    modulesFieldsInfo = new List<ModuleFieldInfo>();
                    do
                    {
                        ctrlSA.ListModuleField(out tempModuleFields, out startRow, startRow);
                        modulesFieldsInfo.AddRange(tempModuleFields.ToArray());
                    }
                    while (tempModuleFields.Count != 0);                                        
                }
                CachedUtils.SetCacheOf(modulesFieldsInfo, ServerCachedHashInfo.ModuleFieldsInfoHash);
            }


            return modulesFieldsInfo;
        }

        public override List<ModuleInfo> BuildModulesInfoCache()
        {
            var colModulesInfo = CachedUtils.GetCacheOf<ModuleInfo>(ServerCachedHashInfo.ModulesInfoHash);
            if(colModulesInfo == null)
            {
                using (var ctrlSA = new SAController())
                {
                    ctrlSA.ListModuleInfo(out colModulesInfo);
                }

                CachedUtils.SetCacheOf(colModulesInfo, ServerCachedHashInfo.ModulesInfoHash);
            }

            return colModulesInfo;
        }

        public override List<CodeInfo> BuildCodesInfoCache()
        {
            var colCodesInfo = CachedUtils.GetCacheOf<CodeInfo>(ServerCachedHashInfo.CodesInfoHash);
            if(colCodesInfo == null)
            {
                using (var ctrlSA = new SAController())
                {
                    ctrlSA.ListCodesInfo(out colCodesInfo);
                }

                CachedUtils.SetCacheOf(colCodesInfo, ServerCachedHashInfo.CodesInfoHash);
            }

            return colCodesInfo;
        }

        public override List<ButtonInfo> BuildSearchButtonsCache()
        {
            var colSearchButtonsInfo = CachedUtils.GetCacheOf<ButtonInfo>(ServerCachedHashInfo.SearchButtonsInfoHash);
            if (colSearchButtonsInfo == null)
            {
                using (var ctrlSA = new SAController())
                {
                    ctrlSA.ListSearchButton(out colSearchButtonsInfo);
                }

                CachedUtils.SetCacheOf(colSearchButtonsInfo, ServerCachedHashInfo.SearchButtonsInfoHash);
            }
            return colSearchButtonsInfo;
        }

        public override List<ButtonParamInfo> BuildSearchButtonParamsCache()
        {
            var colSearchButtonParamsInfo = CachedUtils.GetCacheOf<ButtonParamInfo>(ServerCachedHashInfo.SearchButtonParamsInfoHash);
            if(colSearchButtonParamsInfo == null)
            {
                using (var ctrlSA = new SAController())
                {
                    ctrlSA.ListSearchButtonParam(out colSearchButtonParamsInfo);
                }

                CachedUtils.SetCacheOf(colSearchButtonParamsInfo, ServerCachedHashInfo.SearchButtonParamsInfoHash);
            }
            return colSearchButtonParamsInfo;
        }

        public override List<OracleParam> BuildOracleParamsCache()
        {
            var oracleParamsInfo = CachedUtils.GetCacheOf<OracleParam>(ServerCachedHashInfo.OracleParamsInfoHash);

            if(oracleParamsInfo == null)
            {
                using (var ctrlSA = new SAController())
                {
                    ctrlSA.ListOracleParameter(out oracleParamsInfo);
                }
                
                CachedUtils.SetCacheOf(oracleParamsInfo ,ServerCachedHashInfo.OracleParamsInfoHash);
            }

            return oracleParamsInfo;
        }

        public override List<ValidateInfo> BuildValidatesInfoCache()
        {
            var colValidatesInfo = CachedUtils.GetCacheOf<ValidateInfo>(ServerCachedHashInfo.ValidatesInfoHash);
            if(colValidatesInfo == null)
            {
                using (var ctrlSA = new SAController())
                {
                    ctrlSA.ListValidatesInfo(out colValidatesInfo);
                }

                CachedUtils.SetCacheOf(colValidatesInfo, ServerCachedHashInfo.ValidatesInfoHash);
            }

            return colValidatesInfo;
        }

        public override List<NameValueItem> GetSourceList(ModuleInfo moduleInfo, ModuleFieldInfo fieldInfo, List<string> values)
        {
            using (var ctrlSA = new SAController())
            {
                List<NameValueItem> listSource;
                ctrlSA.GetListSource(out listSource, moduleInfo.ModuleID, moduleInfo.SubModule, fieldInfo.FieldID, values);
                return listSource;
            }
        }

        public override void GetCurrentUserProfile()
        {
            UserProfile userProfile;

            using (var ctrlSA = new SAController())
            {
                ctrlSA.GetCurrentUserProfile(out userProfile);
            }

            App.Environment.ClientInfo.UserProfile = userProfile;
        }

        public override List<GroupSummaryInfo> BuildGroupSummaryCache()
        {
            var colGroupSummaryInfo = CachedUtils.GetCacheOf<GroupSummaryInfo>(ServerCachedHashInfo.GroupSummaryInfoHash);
            if (colGroupSummaryInfo == null)
            {
                using (var ctrlSA = new SAController())
                {
                    ctrlSA.ListGroupSummaryInfo(out colGroupSummaryInfo);
                }

                CachedUtils.SetCacheOf(colGroupSummaryInfo, ServerCachedHashInfo.GroupSummaryInfoHash);
            }
            return colGroupSummaryInfo;
        }

        public override List<ExportHeader> BuildExportHeaderCache()
        {
            var colExportHeaderInfo = CachedUtils.GetCacheOf<ExportHeader>(ServerCachedHashInfo.ExportHeaderInfoHash);
            if (colExportHeaderInfo == null)
            {
                using (var ctrlSA = new SAController())
                {
                    ctrlSA.ListExportHeaderInfo(out colExportHeaderInfo);
                }

                CachedUtils.SetCacheOf(colExportHeaderInfo, ServerCachedHashInfo.ExportHeaderInfoHash);
            }
            return colExportHeaderInfo;
        }

        public override List<SysvarInfo> BuildSysvarInfoCache()
        {
            var colSysvarInfo = CachedUtils.GetCacheOf<SysvarInfo>(ServerCachedHashInfo.SysvarInfoHash);
            if (colSysvarInfo == null)
            {
                using (var ctrlSA = new SAController())
                {
                    ctrlSA.ListSysvarInfo(out colSysvarInfo);
                }

                CachedUtils.SetCacheOf(colSysvarInfo, ServerCachedHashInfo.SysvarInfoHash);
            }
            return colSysvarInfo;
        }
    }
}
