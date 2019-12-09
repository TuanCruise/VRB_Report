using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraTab;
using FIS.AppClient.Controls;
using FIS.AppClient.Interface;
using FIS.Common;
using FIS.Controllers;
using FIS.Entities;
using FIS.Utils;
using DevExpress.XtraBars.Ribbon.ViewInfo;

namespace FIS.AppClient.Utils
{
    public class MainProcess
    {
        #region Private
        private List<Role> m_Roles;
        private readonly Form m_mainForm;
        #endregion

        #region Public Properties
        public static List<Role> Roles
        {
            get { return Instance.m_Roles; }
        }

        public static MainProcess Instance { get; set; }

//#if DEBUG
        internal bool IsAdvanceDeveloperMode { get; set; }
//#endif
        #endregion

        public MainProcess(Form mainForm)
        {
            m_mainForm = mainForm;
            Instance = this;
///#if DEBUG
            IsAdvanceDeveloperMode = false;
//#endif
        }

        public static void CloseAllModules()
        {
            var aliveModules = new ucModule[ucModule.AliveModuleIntances.Count];
            ucModule.AliveModuleIntances.CopyTo(aliveModules);

            foreach (var moduleInstance in aliveModules)
            {
                moduleInstance.CloseModule();
            }
        }

        public static Form GetMainForm()
        {
            if(Instance != null)
                return Instance.m_mainForm;
            return null;
        }

        public void InstallModule(string fileName)
        {
            try
            {

                var module = CreateModuleInstance(STATICMODULE.INSTALL_MODULE_PACKAGE, CODES.DEFMOD.SUBMOD.MODULE_MAIN);
                module["P01"] = fileName;

                module.ShowDialogModule(m_mainForm);
            }
            catch (FaultException ex)
            {
                frmInfo.ShowError(((IMain)m_mainForm).Language.ApplicationTitle, ex, m_mainForm);
            }
            catch (Exception ex)
            {
                frmInfo.ShowError(((IMain)m_mainForm).Language.ApplicationTitle, ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_UNKNOWN, ex.Message), m_mainForm);
            }
        }

        #region Create Module Instance
        //add by TrungTT - 28.11.2011 - Create Module Base
        public static ucModule CreateModuleBase(ModuleInfo moduleInfo)
        {
            ucModule ucModule = null;

            switch (moduleInfo.ModuleType)
            {
                case CODES.DEFMOD.MODTYPE.SEARCHMASTER:
                    ucModule = new ucSearchMaster();
                    break;
                case CODES.DEFMOD.MODTYPE.MAINTAIN:
                    ucModule = new ucMaintain();
                    break;
            }

            if (ucModule != null)
            {
                ucModule.InitializeModuleInfo(moduleInfo);
            }

            return ucModule;
        }
        //End TrungTT

        public static ucModule CreateModuleInstance(ModuleInfo moduleInfo)
        {
            ucModule ucModule = null;

            switch (moduleInfo.ModuleType)
            {
                case CODES.DEFMOD.MODTYPE.STATICMODULE:
                    switch (moduleInfo.ModuleID)
                    {
                        case STATICMODULE.GENERATE_MODULE_PACKAGE:
                            ucModule = new ucGeneratePackage();
                            break;
                        case STATICMODULE.INSTALL_MODULE_PACKAGE:
                            ucModule = new ucInstallPackage();
                            break;
                        case STATICMODULE.IEMODULE:
                            ucModule = new ucIEModule();
                            break;
                        case STATICMODULE.LOGIN_MODULE:
                            ucModule = new ucLogin();
                            break;
                        case STATICMODULE.EDITLANG:
                            ucModule = new ucEditLanguage();
                            break;
                        case STATICMODULE.USER_ROLE_MODULE:
                            ucModule = new ucUserRoleSetup();
                            break;
                        case STATICMODULE.GROUP_ROLE_MODULE:
                            ucModule = new ucGroupRoleSetup();
                            break;
                        case STATICMODULE.SYSTEM_LOG_VIEW:
                            ucModule = new ucSystemLog();
                            break;
                        case STATICMODULE.FIELD_MAKER:
                            ucModule = new ucModuleFieldMaker();
                            break;
                        case STATICMODULE.VIEW_DATA_FLOW:
                            ucModule = new ucShowDataFlow();
                            break;
                        case STATICMODULE.SQL_MODEL_DESIGNER:
                            ucModule = new ucSQLModel();
                            break;
                        case STATICMODULE.UPFILE_MODULE:
                            ucModule = new ucUploadFile();
                            break;
                        //TUDQ them
                        case STATICMODULE.COLUMNEXPORT:
                            ucModule = new ucColumnExport();
                            break;
                        //END
                    }
                    break;
                case CODES.DEFMOD.MODTYPE.SEARCHMASTER:
                    switch (moduleInfo.SubModule)
                    {
                        case CODES.DEFMOD.SUBMOD.MODULE_MAIN:
                            ucModule = new ucSearchMaster();
                            break;
                        case CODES.DEFMOD.SUBMOD.SEARCH_EXPORT:
                            ucModule = new ucSearchExport();
                            break;
                    }
                    break;
                case CODES.DEFMOD.MODTYPE.MODCHART:
                    //ucModule = new ucChartMaster();
                    ucModule = new ucChartManager();
                    break;
                case CODES.DEFMOD.MODTYPE.MAINTAIN:
                    ucModule = new ucMaintain();
                    break;
                //TUDQ them
                case CODES.DEFMOD.MODTYPE.TREEVIEW:
                    ucModule = new ucTreeView();
                    break;
                case CODES.DEFMOD.MODTYPE.EXPESSION:
                    ucModule= new ucExpression();
                    break;

                case CODES.DEFMOD.MODTYPE.TRANS:
                    ucModule = new ucApproveImport();
                    break;
                //End
                case CODES.DEFMOD.MODTYPE.STOREEXECUTE:
                    ucModule = new ucStoreExecute();
                    break;
                case CODES.DEFMOD.MODTYPE.SWITCHMODULE:
                    ucModule = new ucSwitchModule();
                    break;
                case CODES.DEFMOD.MODTYPE.IMPORTMASTER:
                    ucModule = new ucIMWizard();
                    break;
                case CODES.DEFMOD.MODTYPE.ALERTMASTER:
                    ucModule = new ucAlertMaster();
                    break;
                case CODES.DEFMOD.MODTYPE.BATCHPROCESS:
                    ucModule = new ucBatchProcess();
                    break;
                //edit by TrungTT - 3.2.2012 - Export Statistic
                case CODES.DEFMOD.MODTYPE.STATISTICSMASTER:
                    switch(moduleInfo.SubModule)
                    {                    
                        case CODES.DEFMOD.SUBMOD.MODULE_MAIN:
                            ucModule = new ucStatisticsMaster();
                            break;
                        case CODES.DEFMOD.SUBMOD.SEARCH_EXPORT:
                            ucModule = new ucSearchExport();
                            break;
                        case CODES.DEFMOD.SUBMOD.SEND_MAIL:
                            ucModule = new ucSendMail();
                            break;
                    }
                    break;
                //ADD BY TRUNGTT - 5.5.2011 - ADD MODULE REPORT
                case CODES.DEFMOD.MODTYPE.REPORTMASTER:
                    ucModule = new ucReportMaster();
                    break;
                //END TRUNGTT
            }
            if (ucModule != null)
            {
                ucModule.InitializeModuleInfo(moduleInfo);
            }

            return ucModule;
        }
        //add by TrungTT - 7.5.2012
        public static ucModule CreateModuleInstance(string moduleID, string subModule,string CallModule)
        {
            var moduleInfo = ModuleUtils.GetModuleInfo(moduleID, subModule);
            return CreateModuleInstance(moduleInfo);
        }
        //End trungtt

        public static ucModule CreateModuleInstance(string moduleID, string subModule)
        {
            //edit by trungtt - 7.5.2012
            //if (subModule == "EXP")
            //{
            //    subModule = "MMN";
            //    var moduleInfo = ModuleUtils.GetModuleInfo(moduleID, subModule);
            //    return CreateModuleInstance(moduleInfo);
            //}
            //else
            //{
            //    //end trungtt
            //    var moduleInfo = ModuleUtils.GetModuleInfo(moduleID, subModule);
            //    return CreateModuleInstance(moduleInfo);
            //}
            var moduleInfo = ModuleUtils.GetModuleInfo(moduleID, subModule);
            return CreateModuleInstance(moduleInfo);
        }

        public static ucModule CreateModuleInstance(string moduleID)
        {
            ModuleInfo moduleInfo;
            if (moduleID.Contains("."))
            {
                var strs = moduleID.Split(new[] { "." }, StringSplitOptions.None);
                moduleInfo = ModuleUtils.GetModuleInfo(strs[0], strs[1]);
            }
            else
                moduleInfo = ModuleUtils.GetModuleInfo(moduleID);

            return CreateModuleInstance(moduleInfo);
        }
        #endregion

        #region Execute Module

//#if DEBUG
        public static void ForceLoad(string moduleID)
        {
            using (var ctrlSA = new SAController())
            {
                List<ModuleInfo> modulesInfo;
                List<ModuleFieldInfo> moduleFieldsInfo;
                List<ButtonInfo> buttonsInfo;
                List<ButtonParamInfo> buttonParamsInfo;
                List<LanguageInfo> languageInfo;
                List<OracleParam> oracleParamsInfo;
                ctrlSA.ForceLoadModule(
                    out modulesInfo,
                    out moduleFieldsInfo,
                    out buttonsInfo,
                    out buttonParamsInfo,
                    out languageInfo,
                    out oracleParamsInfo,
                    moduleID);
                ModuleUtils.ForceLoad(
                    moduleID,
                    modulesInfo,
                    moduleFieldsInfo,
                    buttonsInfo,
                    buttonParamsInfo,
                    languageInfo,
                    oracleParamsInfo);
                return;
            }
        }
//#endif
        private void _ExecuteModule(string moduleID)
        {
            try
            {
//#if DEBUG
                if (moduleID == "ADVANCE=ON")
                {
                    IsAdvanceDeveloperMode = true;
                    return;
                }
                if (moduleID == "ADVANCE=OFF")
                {
                    IsAdvanceDeveloperMode = false;
                    return;
                }
                if (moduleID.StartsWith("!"))
                {
                    moduleID = moduleID.Substring(1);
                    ForceLoad(moduleID);
                }

                if (moduleID.StartsWith("M?"))
                {
                    var module = CreateModuleInstance(STATICMODULE.MODULE_CONFIG, "MMN");
                    module.SetFieldValue("MODID", moduleID.Substring(2));
                    module.ShowModule(m_mainForm);
                    module.Execute();
                }
                else if (moduleID.StartsWith("CD?"))
                {
                    var module = CreateModuleInstance("03902", "MMN");
                    module.SetFieldValue("MODID", moduleID.Substring(3));
                    module.ShowModule(m_mainForm);
                    module.Execute();
                }
                else if (moduleID.StartsWith("SG?"))
                {
                    var module = CreateModuleInstance("03903", "MMN");
                    module.SetFieldValue("MODID", moduleID.Substring(3));
                    module.ShowModule(m_mainForm);
                    module.Execute();
                }
                else if (moduleID.StartsWith("CL?"))
                {
                    var module = CreateModuleInstance("03904", "MMN");
                    module.SetFieldValue("MODID", moduleID.Substring(3));
                    module.ShowModule(m_mainForm);
                    module.Execute();
                }
                else if (moduleID.StartsWith("CM?"))
                {
                    var module = CreateModuleInstance("03905", "MMN");
                    module.SetFieldValue("MODID", moduleID.Substring(3));
                    module.ShowModule(m_mainForm);
                    module.Execute();
                }
                else if (moduleID.StartsWith("PR?"))
                {
                    var module = CreateModuleInstance("03906", "MMN");
                    module.SetFieldValue("MODID", moduleID.Substring(3));
                    module.ShowModule(m_mainForm);
                    module.Execute();
                }
                else
                {
                    var module = CreateModuleInstance(moduleID);
                    module.ShowModule(m_mainForm);
                }
//#else
//                var module = CreateModuleInstance(moduleID);
//                module.ShowModule(m_mainForm);
//#endif
            }
            catch (FaultException ex)
            {
                frmInfo.ShowError(((IMain)m_mainForm).Language.ApplicationTitle, ex, m_mainForm);
            }
            catch (Exception ex)
            {
                frmInfo.ShowError(((IMain)m_mainForm).Language.ApplicationTitle, ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_UNKNOWN, ex.Message), m_mainForm);
            }
        }

        public static void ExecuteModule(string moduleID)
        {
            Instance._ExecuteModule(moduleID);
        }

        public static void ExecuteMenu(string menuName, MenuItemInfo menuInfo)
        {
            if (menuInfo != null)
            {
                Instance._ExecuteMenu(menuName, menuInfo.ModuleID, menuInfo.SubModule);
            }
            else
            {
                ExecuteMenu(menuName);
            }
        }

        public static void ExecuteMenu(string menuName, FIS.Entities.RibbonItemInfo ribbonInfo)
        {
            if (ribbonInfo != null)
            {
                Instance._ExecuteMenu(menuName, ribbonInfo.ModuleID, ribbonInfo.SubModule);
            }
            else
            {
                ExecuteMenu(menuName);
            }
        }

        public static void ExecuteMenu(string menuName)
        {
            Instance._ExecuteMenu(menuName, null, null);
        }

        private void _ExecuteMenu(string menuName, string moduleID, string subModule)
        {
            switch (menuName)
            {
                case "MNU_EXIT":
                    m_mainForm.Close();
                    return;
                case "MNU_LOGOUT":
                    if (frmConfirm.ShowConfirm(
                            ((IMain)m_mainForm).Language.LogoutTitle,
                            ((IMain)m_mainForm).Language.LogoutConfirm,
                            m_mainForm))
                    {
                        LogoutFromSystem(true);
                    }
                    break;
                case "MNU_HELP":                    
                    Help.ShowHelp(m_mainForm, "Help\\Help.chm");
                    break;
                case "MNU_ABOUT_US":
                    //var frm = new frmAboutUs();
                    //frm.Show();
                    break;
                case CONSTANTS.MENU_NAME_NDTNN:
                    var frmOwner = new frmAddIn();
                    frmOwner.Activate();                    
                    frmOwner.MdiParent = MainProcess.GetMainForm();
                    frmOwner.Show();
                    break;
                default:
                    if(moduleID != null && subModule != null)
                        ExecuteModule(moduleID, subModule);
                    break;
            }
        }

        public static void ExecuteModule(string moduleID, string subModule)
        {
            ExecuteModule(moduleID, subModule, false);
        }
        
        public static void ExecuteModule(string moduleID, string subModule, bool execute)
        {
            Instance._ExecuteModule(moduleID, subModule, execute);
        }

        private void _ExecuteModule(string moduleID, string subModule, bool execute)
        {
            try
            {
                var module = CreateModuleInstance(moduleID, subModule);
                module.ShowModule(m_mainForm);
                if (execute) module.Execute();
            }
            catch (FaultException ex)
            {
                frmInfo.ShowError(((IMain)m_mainForm).Language.ApplicationTitle, ex, m_mainForm);
            }
            catch (Exception ex)
            {
                frmInfo.ShowError(((IMain)m_mainForm).Language.ApplicationTitle, ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_UNKNOWN, ex.Message), m_mainForm);
            }
        }
        #endregion

        #region Login & Logout
        public delegate void ShowLoginInvoker();

        private void _ShowLogin()
        {
            if (m_mainForm.InvokeRequired)
            {
                new ShowLoginInvoker(ShowLogin).Invoke();
                return;
            }

            if (!m_mainForm.InvokeRequired)
            {
                var loginModule = CreateModuleInstance(STATICMODULE.LOGIN_MODULE, CODES.DEFMOD.SUBMOD.MODULE_MAIN);
                loginModule.ShowModule(m_mainForm);
            }
        }

        public static void ShowLogin()
        {
            Instance._ShowLogin();
        }

        public static void SaveLoginToRegistry(bool isSave, string userName, string password)
        {
            var clientEnvironment = (App.Environment as ClientEnvironment);
            if (clientEnvironment != null)
            {
                var appRegistry = clientEnvironment.MaximusRegistry;
                if (isSave)
                {
                    appRegistry.SetValue("UserName", userName);
                    appRegistry.SetValue("Password", password);
                    appRegistry.SetValue("SaveLogin", "Y");                    
                }
                else
                {
                    appRegistry.SetValue("UserName", string.Empty);
                    appRegistry.SetValue("Password", string.Empty);
                    appRegistry.SetValue("SaveLogin", "N");
                }
            }
        }

        private void _LogoutFromSystem(bool isShowLogin)
        {
            try
            {
                ((IMain)m_mainForm).OnLogout();
                CloseAllModules();
            }
            catch
            {
            }

            try
            {
                using (var ctrlSA = new SAController())
                {
                    ctrlSA.TerminalCurrentSession();
                    //add by trungtt - 14.5.2012
                    Program.blLogin = false;
                    //end trungtt
                }
            }
            catch
            {
            }

            if (isShowLogin)
            {
                try
                {
                    ShowLogin();
                }
                catch
                {
                }
            }
        }

        public static void LogoutFromSystem(bool isShowLogin)
        {
            Instance._LogoutFromSystem(isShowLogin);
        }

        private void _LoginToSystem(Session session)
        {
            
            
            
            App.Environment.ClientInfo.SessionKey = session.SessionKey;
            App.Environment.ClientInfo.UserName = session.Username;

            App.Environment.GetCurrentUserProfile();
            CachedUtils.SetCacheOf(new List<ClientInfo> { App.Environment.ClientInfo }, "LastSession");

#if DEBUG
            App.Environment.InitializeEnvironment();
#endif

            using (var ctrlSA = new SAController())
            {
                ctrlSA.ListCurrentRoles(out m_Roles);
            }

            ((IMain)m_mainForm).ApplyMenu();
            ((IMain)m_mainForm).StartupModules();

            Program.blLogin = false;
        }

        public void StartupModules()
        {
            if (App.Environment.ClientInfo.SessionKey != null)
            {
                ThemeUtils.ChangeSkin(App.Environment.ClientInfo.UserProfile.ApplicationSkinName);

                foreach (var module in AllCaches.ModulesInfo)
                {
                    if (module.StartMode == CODES.DEFMOD.STARTMODE.AUTOMATIC)
                    {
                        try
                        {
                            using (var ctrlSA = new SAController())
                            {
                                ctrlSA.CheckRole(module);
                                var startupModule = CreateModuleInstance(module.ModuleID, module.SubModule);
                                startupModule.ShowModule(m_mainForm);
                            }
                        }
                        catch (FaultException ex)
                        {
                            if (ex.Code.Name != ERR_SYSTEM.ERR_SYSTEM_MODULE_NOT_ALLOW_ACCESS.ToString())
                            {
                                frmInfo.ShowError(((IMain)m_mainForm).Language.ApplicationTitle, ex, m_mainForm);
                            }
                        }
                        catch (Exception ex)
                        {
                            frmInfo.ShowError(((IMain)m_mainForm).Language.ApplicationTitle, ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_UNKNOWN, ex.Message), m_mainForm);
                        }
                    }
                }
            }
        }

        public static void LoginToSystem(Session session)
        {
            Instance._LoginToSystem(session);
        }
        #endregion

        #region Interface Execute

        public static void InitializeMenu()
        {
            ((IMain)Instance.m_mainForm).InitializeMenu();
        }

        public static XtraTabPage AddTabModule(XtraTabPage tabPage)
        {
            return ((IMain)Instance.m_mainForm).AddTabModule(tabPage);
        }

        public static void RemoveTabModule(XtraTabPage tabPage)
        {
            ((IMain)Instance.m_mainForm).RemoveTabModule(tabPage);
        }

        public static void SelectTabPage(XtraTabPage tabPage)
        {
            ((IMain)Instance.m_mainForm).SelectTabPage(tabPage);
        }

        public static void AddModulePreview(ucModulePreview preview)
        {
            ((IMain)Instance.m_mainForm).AddModulePreview(preview);
        }

        public static void RegisterButton(BarButtonItem button)
        {
            ((IMain)Instance.m_mainForm).RegisterButton(button);
        }

        public static void CancelRegisterButton(BarButtonItem button)
        {
            ((IMain)Instance.m_mainForm).CancelRegisterButton(button);
        }

        public static void RemoveModulePreview(ucModulePreview preview)
        {
            ((IMain)Instance.m_mainForm).RemoveModulePreview(preview);
        }

        public static void Close()
        {
            Instance.m_mainForm.Close();
        }
        #endregion
    }
}
