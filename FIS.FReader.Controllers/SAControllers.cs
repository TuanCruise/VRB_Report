using System;
using System.Collections.Generic;
using System.ServiceModel;
using FIS.Base;
using FIS.Common;
using FIS.Utils;
using FIS.Entities;


namespace FIS.FReader.Controllers
{
    public class SAController : RemoteControllerBase, IDisposable
    {
        private readonly SAControllerClient m_Client;

        public SAController(Session MainSession)
        {
            try
            {
                m_Client = new SAControllerClient(m_Binding, m_EndpointAddress);
                m_Client.Open();

                if (MainSession != null)
                    m_Client.InitializeSessionID(MainSession.SessionKey);
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_CONNECT_TO_SERVER_FAIL, ex.Message);
            }
        }

        public void Dispose()
        {
            if(m_Client != null) m_Client.Close();
        }

        public void GetServerInfo(out ServerInfo serverInfo, out CachedHashInfo cachedInfo, string clientLanguageID)
        {
            serverInfo = m_Client.GetServerInfo(out cachedInfo, clientLanguageID);
        }

        public void GetCurrentSessionInfo(out Session session)
        {
            session = m_Client.GetCurrentSessionInfo();
        }

        public string GetSystemLog(bool clearLog)
        {
            return m_Client.GetSystemLog(clearLog);
        }

        public void TerminalCurrentSession()
        {
            try
            {
                m_Client.TerminalCurrentSession();
            }
            catch
            {
                
            }
            finally
            {
                App.Environment.ClientInfo.UserName = null;
                App.Environment.ClientInfo.SessionKey = null;
            }
        }

        public void ListCurrentRoles(out List<Role> roles)
        {
            roles = m_Client.ListCurrentRoles();
        }

        public void ListCodesInfo(out List<CodeInfo> codes)
        {
            codes = m_Client.ListCodesInfo();
        }

        public void ListErrorsInfo(out List<ErrorInfo> errors)
        {
            errors = m_Client.ListErrorsInfo();
        }

        public void ListValidatesInfo(out List<ValidateInfo> validates)
        {
            validates = m_Client.ListValidatesInfo();
        }

        public void ListLanguage(out List<LanguageInfo> langInfos, String langID)
        {
            langInfos = m_Client.ListLanguage(langID);
        }

        [OperationContract]
        public void ValidateFieldInfoSyntax(string moduleID, string subModule, string fieldID, List<string> values)
        {
            m_Client.ValidateFieldInfoSyntax(moduleID, subModule, fieldID, values);
        }

        public void ListMenuItems(out List<MenuItemInfo> menuItems)
        {
            menuItems = m_Client.ListMenuItems();
        }

        public void ListRibbonItems(out List<RibbonItemInfo> ribbonItems)
        {
            ribbonItems = m_Client.ListRibbonItems();
        }

        public void ListBatchInfo(out List<BatchInfo> moduleInfos, string moduleID)
        {
            moduleInfos = m_Client.ListBatchInfo(moduleID);
        }

        public void ListModuleInfo(out List<ModuleInfo> moduleInfos)
        {
            moduleInfos = m_Client.ListModuleInfo();
        }

        public void ListGroupSummaryInfo(out List<GroupSummaryInfo> groupSummaryInfos)
        {
            groupSummaryInfos = m_Client.ListGroupSummaryInfo();
        }

        public void ListExportHeaderInfo(out List<ExportHeader> ExportHeaderInfos)
        {
            ExportHeaderInfos = m_Client.ListExportHeaderInfo();
        }

        public void ListSysvarInfo(out List<SysvarInfo> SysvarInfos)
        {
            SysvarInfos = m_Client.ListSysvarInfo();
        }
        
        public void ListModuleField(out List<ModuleFieldInfo> moduleFields, out int endRow, int startRow)
        {
            moduleFields = m_Client.ListModuleField(out endRow, startRow);
        }

        public void GetSessionUserInfo(out User userInfo)
        {
            userInfo = m_Client.GetSessionUserInfo();
        }

        public void CheckRole(ModuleInfo moduleInfo)
        {
            m_Client.CheckRole(moduleInfo);
        }
        //add by TrungTT - 27.12.2012 - Userlog
        public void ExecuteUsersLog(string modID, string moduleName)
        {
            m_Client.ExecuteUsersLog(modID, moduleName);
        }
        //End TrungTT
        public void CreateUserSession(out Session session, String userName, String password)
        {
            session = m_Client.CreateUserSession(userName, password);
        }

        public void ListSearchButton(out List<ButtonInfo> buttons)
        {
            buttons = m_Client.ListSearchButton();
        }

        public void ListSearchButtonParam(out List<ButtonParamInfo> @params)
        {
            @params = m_Client.ListSearchButtonParam();
        }

        public void ExecuteBatch(string moduleID, string batchName)
        {
            m_Client.ExecuteBatch(moduleID, batchName);
        }

        //add by trungtt - 1.6.2011 - batch dynamic market
        public void ExecuteBatchMarket(string moduleID, string batchName, string market)
        {
            m_Client.ExecuteBatchMarket(moduleID, batchName, market);
        }
        //public void ExecuteBatchMarket(string moduleID, string market)
        //{
        //    m_Client.ExecuteBatchMarket(moduleID, market);
        //}
        //end trungtt

        public void ExecuteSwitchModule(out string targetModule, string moduleID, string subModule, List<string> @params)
        {
            targetModule = m_Client.ExecuteSwitchModule(moduleID, subModule, @params);
        }

        public void ExecuteGenerateModulePackage(string moduleID, out string generatedPackage)
        {
            generatedPackage = m_Client.ExecuteGenerateModulePackage(moduleID);
        }

        public void ExecuteInstallModule(string modulePackageData)
        {
            m_Client.ExecuteInstallModule(modulePackageData);
        }

        public void ExecuteUninstallModule(string moduleID)
        {
            m_Client.ExecuteUninstallModule(moduleID);
        }

        public void ExecuteProcedure(string moduleID, string subModule, List<string> @params)
        {
            m_Client.ExecuteProcedure(moduleID, subModule, @params);
        }

        public void ExecuteSaveLanguage(string langID, string langName, string langValue)
        {
            m_Client.ExecuteSaveLanguage(langID, langName, langValue);
        }

        public void ExecuteMaintainQuery(out DataContainer executeResult, string moduleID, string subModule, List<string> @params)
        {
            executeResult = m_Client.ExecuteMaintainQuery(moduleID, subModule, @params);
        }
        public void ExecuteTransQuery(out DataContainer executeResult, string moduleID, string subModule, List<string> @params)
        {
            executeResult = m_Client.ExecuteTransQuery(moduleID, subModule, @params);
        }

        //TUDQ them
 
        public void GetTreeStore(out DataContainer executeResult, List<string> @params)
        {
            executeResult = m_Client.GetTreeViewStore(@params);
        }

        public void GetTreeViewLang(out DataContainer executeResult, List<string> @params)
        {
            executeResult = m_Client.GetTreeViewLang(@params);
        }
        //End

        public void ExecuteMaintain(out DataContainer container, string moduleID, string subModule, List<string> @params)
        {
            container = m_Client.ExecuteMaintain(moduleID, subModule, @params);
        }

        //add by trungtt - 23.8.2013
        public void ExecApprove(out DataContainer container, string moduleID, string subModule,string SecID, List<string> @params)
        {
            container = m_Client.ExecApprove(moduleID, subModule,SecID, @params);
        }
        //end trungtt

        //public void ExecuteApproved(out DataContainer container, string moduleID, string subModule, List<string> @params)
        //{
        //    container = m_Client.ExecuteApproved(moduleID, subModule, @params);
        //}
        

        //add by TrungTT 08.12.2011 - Edit Grid's column Layout
        public void GetExtraCurrentUserProfile(string extraProperty, out string extraValue)
        {
            extraValue = m_Client.GetExtraCurrentUserProfile(extraProperty);
        }
        public void SetExtraCurrentUserProfile(string extraProperty, string extraValue)
        {
            m_Client.SetExtraCurrentUserProfile(extraProperty, extraValue);
        }
        //end TrungTT
        
        //add by TrungTT- 28.11.2011 - Execute Procedure Fill Dataset
        public void ExecuteProcedureFillDataset(out DataContainer container, string storeData, List<string> @params)
        {
            container = m_Client.ExecuteProcedureFillDataset(storeData, @params);
        }
        //end TrungTT
        
        //add by TrungTT - 9.11.2011 - Batch put Msg
        public void ExecuteBatchPutMsg(out DataContainer container, string moduleID, string storeData, List<string> @params)
        {
            container = m_Client.ExecuteBatchPutMsg(moduleID, storeData, @params);
        }
        //End TrungTT

        //add by TrungTT - 5.5.2011 - add module report
        public void ExecuteReport(out DataContainer container, string moduleID, string subModule, List<string> @params)
        {
            container = m_Client.ExecuteReport(moduleID, subModule, @params);
        }
        //end TrungTT

        //add by TrungTT - 10.11.2011 - maintain report
        public void ExecuteMaintainReport(out DataContainer container, string moduleID, string subModule, List<string> @params)
        {
            container = m_Client.ExecuteMaintainReport(moduleID, subModule, @params);
        }
        //end TrungTT
        
        //add by TrungTT - 9.11.2011 - Auto Report
        public void ExecuteAutoReport(out DataContainer container, string moduleID, string batchName, List<string> @params)
        {
            container = m_Client.ExecuteAutoReport(moduleID, batchName, @params);
        }
        //end TrungTT
        
        //add by TrungTT - 23.08.2011 - Menu Market
        public void ExcuteMarket(out DataContainer container)
        {
            container = m_Client.ExecuteMarket();
        }
        //end TrungTT
        
        //add by TrungTT - 23.09.2011 - Menu Skins
        public void ExecuteLoadSkins(out DataContainer container)
        {
            container = m_Client.ExecuteLoadSkins();
        }
        //end TrungTT

        //add by TrungTT - 27.09.2011 - Get System Date
        public void GetSysDate(out DataContainer container)
        {
            container = m_Client.GetSysDate();
        }
        //end TrungTT

        //add by TrungTT - 26.09.2011 - Load Current Skins
        public void ExecuteLoadCurrentSkins(out DataContainer container)
        {
            container = m_Client.ExecuteLoadCurrentSkins();
        }
        //End TrungTT

        //add by TrungTT - 25.08.2011 - Update Market
        public void UpdateMarket( List<string> @params)
        {
            m_Client.UpdateMarket(@params);
        }
        //End TrungTT

        //add by TrungTT - 25.09.2011 - Change Skins
        public void ExecuteChangeSkins(List<string> @params)
        {
            m_Client.ExecuteChangeSkins(@params);
        }
        //End TrungTT

        public void ExecuteDownLoadFile(out DataContainer container, string moduleID, string subModule, List<string> values)
        {
            container = m_Client.ExecuteDownloadFile(moduleID, subModule,values);
        }
        public void ExecuteChartMaster(out DataContainer executeResult, string moduleID, string subModule, List<string> values)
        {
            executeResult = m_Client.ExecuteChartMaster(moduleID, subModule, values);
        }


        public void CallbackQuery(out DataContainer executeResult, String moduleID, String callbackFieldID, List<String> @params)
        {
            executeResult = m_Client.CallbackQuery(moduleID, callbackFieldID, @params);
        }

        public void ListOracleParameter(out List<OracleParam> oracleParams)
        {
            oracleParams = m_Client.ListOracleParameter();
        }

        public void DisposeSearchResult(string moduleID, string subModule, string searchResultKey, DateTime searchTime)
        {
            m_Client.DisposeSearchResult(moduleID, subModule, searchResultKey, searchTime);
        }

        public void FetchAllSearchResult(out DataContainer searchResult, string moduleID, string subModule, string searchResultKey, DateTime searchTime, int fromRow)
        {
            searchResult = m_Client.FetchAllSearchResult(moduleID, subModule, searchResultKey, searchTime, fromRow);
        }

        public void FetchSearchResult(out DataContainer searchResult, out int bufferSize, out int minPage, out int maxPage, out int startRow, string moduleID, string subModule, string searchResultKey, DateTime searchTime, int startPage, int maxPageSize)
        {
            searchResult = m_Client.FetchSearchResult(out bufferSize, out minPage, out maxPage, out startRow, moduleID, subModule, searchResultKey, searchTime, startPage, maxPageSize);
        }

        public void ExecuteSearch(out string searchResultKey, out DateTime searchTime, string moduleID, string subModule, SearchConditionInstance conditionIntance, List<SearchConditionInstance> staticConditionInstances)
        {
            searchResultKey = m_Client.ExecuteSearch(out searchTime, moduleID, subModule, conditionIntance, staticConditionInstances);
        }

        public void ExecuteSearchEdit(string moduleID, string subModule, List<string> @params)
        {
            m_Client.ExecuteSearchEdit(moduleID, subModule, @params);
        }

        public void GetSearchStatistic(out DataContainer searchStatistic, string moduleID, string subModule, SearchConditionInstance conditionIntance, List<SearchConditionInstance> staticConditionInstances)
        {
            searchStatistic = m_Client.GetSearchStatistic(moduleID, subModule, conditionIntance, staticConditionInstances);
        }

        public void ExecuteAlert(out DataContainer alertResult, string moduleID, string subModule)
        {
            alertResult = m_Client.ExecuteAlert(moduleID, subModule);
        }

        public void ExecuteAlertClick(string moduleID, string subModule)
        {
            m_Client.ExecuteAlertClick(moduleID, subModule);
        }

        public void ExecuteImport(string moduleID, string subModule, List<string> @params)
        {
            m_Client.ExecuteImport(moduleID, subModule, @params);
        }

        public void NewsInfo(out List<NewsInfo> listNewsInfo)
        {
            listNewsInfo = m_Client.NewsInfo();
        }

        public void GetModImport(out System.Data.DataSet executeResult, string rptID)
        {
            executeResult = m_Client.GetModImport(rptID);
        }

        public void GetCurrentUserProfile(out UserProfile userProfile)
        {
            userProfile = m_Client.GetCurrentUserProfile();
        }

        public void ExecuteStatistics(out string searchResultKey, out DateTime searchTime, string moduleID, string subModule, List<string> @params)
        {
            searchResultKey = m_Client.ExecuteStatistics(out searchTime, moduleID, subModule, @params);
        }

        public void GetListSource(out List<NameValueItem> listSource, String moduleID, String subModule, String fieldID, List<String> values)
        {
            listSource = m_Client.GetListSource(moduleID, subModule, fieldID, values);
        }

        public void ListGroupRoles(out List<Role> roles, int groupID)
        {
            roles = m_Client.ListGroupRoles(groupID);
        }

        public void ListUserRoles(out List<Role> roles, int userID)
        {
            roles = m_Client.ListUserRoles(userID);
        }

        public void SaveGroupRoles(List<Role> roles, int groupID)
        {
            m_Client.SaveGroupRoles(roles, groupID);
        }

        public void SaveUserRoles(List<Role> roles, int userID)
        {
            m_Client.SaveUserRoles(roles, userID);
        }

        public void SaveLayout(string moduleID, string subModule, string languageID, string layout)
        {
            m_Client.SaveLayout(moduleID, subModule, languageID, layout);
        }

        //public void SaveFile(string filename, Byte[] file)
        //{
        //    m_Client.SaveFile(filename,file);
        //}

        public void SaveFile(FileUpload file)
        {
            m_Client.SaveFile(file);
        }

        //trungtt - 20.12.2013 - sign file
        public void SignFile(string filename,string certificateinfo,string worker, Byte[] file)
        {
            m_Client.SignFile(filename,certificateinfo,worker, file);
        }
        //end trungtt

        public void ListUsersInGroup(out List<User> users, int groupID)
        {
            users = m_Client.ListUsersInGroup(groupID);
        }

        public void ExecuteSQL(out DataContainer container, string sqlQuery)
        {
            container = m_Client.ExecuteSQL(sqlQuery);
        }

        public void ExecuteStoreProcedure(string storeProcedure, List<string> values)
        {
            m_Client.ExecuteStoreProcedure(storeProcedure, values);
        }

        public void ForceLoadModule(
            out List<ModuleInfo> modulesInfo,
            out List<ModuleFieldInfo> moduleFieldsInfo,
            out List<ButtonInfo> buttonsInfo,
            out List<ButtonParamInfo> buttonParamsInfo,
            out List<LanguageInfo> languageInfo,
            out List<OracleParam> oracleParamsInfo,
            string moduleID)
        {
            modulesInfo = m_Client.ForceLoadModule(
                out moduleFieldsInfo,
                out buttonsInfo,
                out buttonParamsInfo,
                out languageInfo,
                out oracleParamsInfo,
                moduleID);
        }
       public void ResetCache()
        {
            m_Client.ResetCache();
        }
       //TuDQ sua
       public void ExecuteChartQuery(out System.Data.DataSet executeResult, string moduleID, string subModule, List<string> @params)
       {
           executeResult = m_Client.ExecuteChartQuery(moduleID, subModule, @params);
       }
       public void GetChartInf(out System.Data.DataSet executeResult, List<string> @params)
       {
           executeResult = m_Client.GetChartInf(@params);
       }
       
        //End
    }
}