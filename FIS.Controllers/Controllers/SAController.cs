using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text.RegularExpressions;
using FIS.Base;
using FIS.Common;
using FIS.Entities;
using FIS.Utils;
using Oracle.DataAccess.Client;
using ClientWS.Stuffs;
using System.Security.Cryptography.X509Certificates;
using System.Net.Mail;
using System.Text;
using System.DirectoryServices.Protocols;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Wizards;
using DevExpress.XtraPrinting;
using System.Drawing;


namespace FIS.Controllers
{
    [ServiceContract(Name = "ISAController", Namespace = "http://fis.vn/")]
    public class SAController : ControllerBase
    {
        private static readonly List<SearchResult> CachedSearchResult = new List<SearchResult>();
        public Session Session { get; set; }
        SmtpClient smtp;
        static Regex ValidEmailRegex = CreateValidEmailRegex();

#if DEBUG
        public static Dictionary<string, string> ModulesLog { get; set; }
        static SAController()
        {
            ModulesLog = new Dictionary<string, string>();
        }

        [OperationContract]
        public string GetSystemLog(bool clearLog)
        {
            if (clearLog) ModulesLog.Clear();

            var html = ModulesLog.Aggregate(@"<head>
	<style>
		body {background: black; color: white}
		table{border-collapse:collapse;}
		th, td { font-family: Tahoma; font-size: 8pt; border: 1px solid #333333;padding: 2px 5px;border-collapse: collapse}
        th {width: 200px; text-align: left;}
		th.mh {width: auto; text-align: center;}
        .g{color: lightgreen;}
        .y{color: yellow;}
	</style>
</head>
<body>", (current, pair) => current + string.Format(@"<table cellspacing=0 cellpadding=0 width=100%>
		<thead>
			<tr>
				<th class=mh colspan=2>{0}</th>
			</tr>
		</thead>
		<tbody>{1}
		</tbody>
	</table><br/>", pair.Key, pair.Value));
            html += @"</body>";
            return html;
        }

        public void WriteLog(string logKey, string name, string value)
        {
            WriteLog(logKey, name, value, "normal");
        }

        public void WriteLog(string logKey, string name, string value, string specialClass)
        {
            try
            {
                ModulesLog[logKey] += string.Format("<tr><th class=" + specialClass + ">{0}</th><td class=" + specialClass + ">{1}</td></tr>", name, value);
            }
            catch
            {
            }
        }
#endif
        [OperationContract]
        public void GetServerInfo(out ServerInfo serverInfo, out CachedHashInfo cachedInfoHash, string clientLanguageID)
        {
#if DEBUG
            App.Environment.InitializeEnvironment();
#endif
            serverInfo = App.Environment.ServerInfo;
            cachedInfoHash = App.Environment.CachedHashInfo;
        }

        [OperationContract]
        public void GetCurrentSessionInfo(out Session session)
        {
            session = Session;
        }

        [OperationContract]
        public void InitializeSessionID(string sessionID)
        {
            if (sessionID != null)
            {
                var sessions = OracleHelper.ExecuteStoreProcedure<Session>(ConnectionString, null, SYSTEM_STORE_PROCEDURES.GET_SESSION_INFO, sessionID);

                if (sessions.Count == 1)
                {
                    Session = sessions[0];
                    if (Session.SessionStatus == CODES.SESSIONS.SESSIONSTATUS.SESSION_TERMINATED)
                    {
                        if (Session.Username != Session.TerminatedUsername)
                        {
                            throw ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_SESSION_TERMINATED_BY_ADMIN,
                                Session.TerminatedUsername + ": " + Session.Description);
                        }
                        throw ErrorUtils.CreateError(ERR_SYSTEM.ERR_SYSTEM_SESSION_TERMINATED_BY_SELF);
                    }

                    if (Session.SessionStatus == CODES.SESSIONS.SESSIONSTATUS.SESSION_TIMEOUT)
                    {
                        throw ErrorUtils.CreateError(ERR_SYSTEM.ERR_SYSTEM_SESSION_TIMEOUT);
                    }
                }
                else
                {
                    throw ErrorUtils.CreateError(ERR_SYSTEM.ERR_SYSTEM_SESSION_NOT_EXISTS_OR_DUPLICATE);
                }
            }
        }

        [OperationContract]
        public void GetSessionUserInfo(out User userInfo)
        {
            try
            {
                userInfo = OracleHelper.ExecuteStoreProcedure<User>(ConnectionString, null, SYSTEM_STORE_PROCEDURES.GET_SESSION_USER_INFO, Session.Username)[0];
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void CheckRole(ModuleInfo moduleInfo)
        {
            if (!string.IsNullOrEmpty(moduleInfo.RoleID))
            {
                if (Session == null) throw ErrorUtils.CreateError(ERR_SYSTEM.ERR_SYSTEM_MODULE_NOT_ALLOW_ACCESS);
                if (Session.Type == CONSTANTS.USER_TYPE_UBCKNN)
                {
                    OracleHelper.ExecuteStoreProcedure(ConnectionString, null, SYSTEM_STORE_PROCEDURES.CHECK_USER_ROLE, Session.UserID, moduleInfo.RoleID);
                }                
            }
        }

        [OperationContract]
        public void ListCurrentRoles(out List<Role> roles)
        {
            _ListUserRoles(out roles, Session.UserID);
        }

        public List<CodeInfo> BuildCodesInfo()
        {
            try
            {
                return OracleHelper.ExecuteStoreProcedure<CodeInfo>(ConnectionString, null, SYSTEM_STORE_PROCEDURES.LIST_DEFCODE);
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void ListCodesInfo(out List<CodeInfo> codesInfo)
        {
            codesInfo = AllCaches.CodesInfo;
        }

        public List<ErrorInfo> BuildErrorsInfo()
        {
            try
            {
                return OracleHelper.ExecuteStoreProcedure<ErrorInfo>(ConnectionString, null, SYSTEM_STORE_PROCEDURES.LIST_DEFERROR);
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void ListErrorsInfo(out List<ErrorInfo> errorsInfo)
        {
            errorsInfo = AllCaches.BaseErrorsInfo;
        }

        public List<ValidateInfo> BuildValidatesInfo()
        {
            try
            {
                return OracleHelper.ExecuteStoreProcedure<ValidateInfo>(ConnectionString, null, SYSTEM_STORE_PROCEDURES.LIST_DEFVALIDATE);
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void ListValidatesInfo(out List<ValidateInfo> validatesInfo)
        {
            validatesInfo = AllCaches.BaseValidatesInfo;
        }

        public List<LanguageInfo> BuildLanguageInfo()
        {
            try
            {
                return OracleHelper.ExecuteStoreProcedure<LanguageInfo>(ConnectionString, null, SYSTEM_STORE_PROCEDURES.LIST_DEFLANG);
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void ListLanguage(out List<LanguageInfo> languageInfos, string languageID)
        {
            //languageInfos = (from language in AllCaches.BaseLanguageInfo
            //                 where language.LanguageID == languageID && language.AppType == apptype || language.BrType == apptype
            //                 select language).ToList();
            languageInfos = (from language in AllCaches.BaseLanguageInfo
                             where language.LanguageID == languageID 
                             select language).ToList();
        }

        [OperationContract]
        public void ListMenuItems(out List<MenuItemInfo> menuItems)
        {
            try
            {
#if DEBUG
                menuItems = OracleHelper.ExecuteStoreProcedure<MenuItemInfo>(ConnectionString, null, SYSTEM_STORE_PROCEDURES.LIST_DEFMENU, "Y");
#else
                menuItems = OracleHelper.ExecuteStoreProcedure<MenuItemInfo>(ConnectionString, null, SYSTEM_STORE_PROCEDURES.LIST_DEFMENU, "N");
#endif
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void ListRibbonItems(out List<RibbonItemInfo> ribbonItems)
        {
            try
            {
#if DEBUG
                ribbonItems = OracleHelper.ExecuteStoreProcedure<RibbonItemInfo>(ConnectionString, null, SYSTEM_STORE_PROCEDURES.LIST_DEFRIBBON, Session.Type);                
#else
                ribbonItems = OracleHelper.ExecuteStoreProcedure<RibbonItemInfo>(ConnectionString, null, SYSTEM_STORE_PROCEDURES.LIST_DEFRIBBON,Session.Type);
#endif
                var checkedRibbonItems = new List<RibbonItemInfo>();
                // LongND5: First add all item no child
                foreach (var ribbonParent in ribbonItems)
                {
                    var count = 0;
                    foreach (var ribbonChild in ribbonItems)
                    {
                        if (ribbonParent.RibbonID == ribbonChild.RibbonOwnerID)
                        {
                            count++;
                        }
                    }

                    if (count == 0)
                    {
                        try
                        {
                            if (ribbonParent.ModuleID != null && Session.Type ==1 )
                            {
                                CheckRole(ModuleUtils.GetModuleInfo(ribbonParent.ModuleID, ribbonParent.SubModule));
                            }
                            checkedRibbonItems.Add(ribbonParent);
                        }
                        catch
                        {
                        }
                    }
                }

                // Now add parent
                var stop = false;
                while (!stop)
                {
                    stop = true;
                    var tmp = new RibbonItemInfo[checkedRibbonItems.Count];
                    checkedRibbonItems.CopyTo(tmp);
                    foreach (var ribbonChild in tmp)
                    {
                        foreach (var ribbonParent in ribbonItems)
                        {
                            if (!checkedRibbonItems.Contains(ribbonParent) && ribbonParent.RibbonID == ribbonChild.RibbonOwnerID)
                            {
                                checkedRibbonItems.Add(ribbonParent);
                                stop = false;
                            }
                        }
                    }
                }

                ribbonItems = checkedRibbonItems;
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        public List<ModuleInfo> BuildModulesInfo()
        {
            try
            {
                var moduleInfos = new List<ModuleInfo>();
                moduleInfos.AddRange(OracleHelper.ExecuteStoreProcedure<ModuleInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.LIST_STATIC_MODULE).ToArray());
                moduleInfos.AddRange(OracleHelper.ExecuteStoreProcedure<ModuleInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.LIST_BATCH_MODULE).ToArray());
                moduleInfos.AddRange(OracleHelper.ExecuteStoreProcedure<StatisticsModuleInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.LIST_STATISTICS_MODULE).ToArray());
                moduleInfos.AddRange(OracleHelper.ExecuteStoreProcedure<MaintainModuleInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.LIST_MAINTAIN_MODULE).ToArray());
                moduleInfos.AddRange(OracleHelper.ExecuteStoreProcedure<ChartModuleInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.LIST_CHART_MODULE).ToArray());
                moduleInfos.AddRange(OracleHelper.ExecuteStoreProcedure<SearchModuleInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.LIST_SEARCHMASTER_MODULE).ToArray());
                moduleInfos.AddRange(OracleHelper.ExecuteStoreProcedure<SwitchModuleInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.LIST_SWITCH_MODULE).ToArray());
                moduleInfos.AddRange(OracleHelper.ExecuteStoreProcedure<ImportModuleInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.LIST_IMPORT_MODULE).ToArray());
                moduleInfos.AddRange(OracleHelper.ExecuteStoreProcedure<ExecProcModuleInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.LIST_EXECUTEPROC_MODULE).ToArray());
                moduleInfos.AddRange(OracleHelper.ExecuteStoreProcedure<AlertModuleInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.LIST_ALERT_MODULE).ToArray());
                moduleInfos.AddRange(OracleHelper.ExecuteStoreProcedure<ReportModuleInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.LIST_REPORT_MODULE).ToArray());
                //TUDQ
                moduleInfos.AddRange(OracleHelper.ExecuteStoreProcedure<ModuleInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.LIST_TREE_MODULE).ToArray());
                moduleInfos.AddRange(OracleHelper.ExecuteStoreProcedure<ModuleInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.LIST_EXP_MODULE).ToArray());
                //END
                return moduleInfos;
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void ListModuleInfo(out List<ModuleInfo> moduleInfos)
        {
            moduleInfos = AllCaches.ModulesInfo;
        }

        [OperationContract]
        public void ListBatchInfo(out List<BatchInfo> moduleInfos, string moduleID)
        {
            try
            {
                moduleInfos = new List<BatchInfo>();
                moduleInfos.AddRange(OracleHelper.ExecuteStoreProcedure<BatchInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.LIST_BATCH_INFO, moduleID).ToArray());
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        public List<ModuleFieldInfo> BuildModuleFieldsInfo()
        {
            try
            {
                return OracleHelper.ExecuteStoreProcedure<ModuleFieldInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.LIST_FIELD_INFO);
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void ListModuleField(out List<ModuleFieldInfo> moduleFields, out int endRow, int startRow,string apptype)
        {
            const int MAX_FIELD_TO_TRANSFER = 1000;
            moduleFields = AllCaches.ModuleFieldsInfo.Skip(startRow).Take(MAX_FIELD_TO_TRANSFER).ToList();
            endRow = startRow + moduleFields.Count;            
        }

        [OperationContract]
        public void GetDataSource(out List<NameValueItem> sourceData, string sourceName, List<string> values)
        {
            try
            {
                sourceData = OracleHelper.ExecuteStoreProcedure<NameValueItem>(ConnectionString, Session, sourceName, values.ToArray());
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void TerminalCurrentSession()
        {
            if (Session != null)
                OracleHelper.ExecuteStoreProcedure(ConnectionString, null, SYSTEM_STORE_PROCEDURES.TERMINAL_SESSION_INFO, Session.SessionKey, Session.Username, CONSTANTS.SESSION_LOGOUT_DESCRIPTION);
        }

        public void CreateUserSession(out Session session, string userName, string password, string clientIP, string dnsName)
        {
            try
            {
                session = OracleHelper.ExecuteStoreProcedure<Session>(ConnectionString, null, SYSTEM_STORE_PROCEDURES.CREATE_NEW_SESSION, userName, password,clientIP)[0];

                session.SessionKey = CommonUtils.MD5Standard(session.SessionID.ToString());
                session.ClientIP = clientIP;
                session.DNSName = dnsName;

                OracleHelper.ExecuteStoreProcedure(ConnectionString, null, SYSTEM_STORE_PROCEDURES.UPDATE_SESSION_INFO,
                    session.SessionID,
                    session.SessionKey,
                    session.ClientIP,
                    session.DNSName);
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void CreateUserSession(out Session session, string userName, string password)
        {
            try
            {
                var endpoint = (RemoteEndpointMessageProperty)
                    OperationContext.Current
                    .IncomingMessageProperties[RemoteEndpointMessageProperty.Name];

                //IPHostEntry entry = null;                
                string dnsName;
                //try
                //{                                        
                //    entry = Dns.GetHostByAddress(endpoint.Address);
                //    dnsName = Dns.GetHostEntry(endpoint.Address).Aliases[0];                    
                //}
                //catch
                //{
                //    if (entry == null)
                //        dnsName = "Not Resolved";
                //    else
                //        dnsName = entry.HostName;
                //}
                dnsName = "Not Resolved";
                if (GetVarValue(SYSVAR.GRNAME_SYS, SYSVAR.VARNAME_DOMAINLOGIN) == CONSTANTS.Yes)
                {
                    if (ValidateUser(userName, password))
                    {
                        CreateUserSession(out session, userName, password, endpoint.Address, dnsName);
                    }
                    else
                    {
                        throw ErrorUtils.CreateError(1);
                    }
                }
                else
                {
                    CreateUserSession(out session, userName, password, endpoint.Address, dnsName);
                }                                
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        private bool ValidateUser(string username,string password)
        {
            bool blValidation;
            try
            {
                string AddSrv = GetVarValue(SYSVAR.GRNAME_SYS, SYSVAR.VARNAME_DOMAINSRV);
                string DomainName = GetVarValue(SYSVAR.GRNAME_SYS, SYSVAR.VARNAME_DOMAINNAME);
                if (username.IndexOf("@") ==-1)
                {
                    username = username + "@" + DomainName;
                }

                LdapDirectoryIdentifier ldapDir = new LdapDirectoryIdentifier(AddSrv);
                LdapConnection lcon = new LdapConnection(ldapDir);
                NetworkCredential nc = new NetworkCredential(username,password);
                lcon.Credential = nc;
                lcon.AuthType = AuthType.Basic;                
                lcon.Bind(nc);
                blValidation = true;
            }
            catch (LdapException err)
            {
                blValidation = false;
            }
            return blValidation;
        }

        private  string GetVarValue(string grname, string varname)
        {
            string result = null;
            try
            {
                List<string> values = new List<string>();
                values.Add(grname);
                values.Add(varname);
                DataTable dt = new DataTable();
                OracleHelper.FillDataTable(ConnectionString, "sp_sysvar_sel_bygrame", out dt, values.ToArray());                                
                if (dt.Rows.Count > 0)
                {
                    result = dt.Rows[0]["VARVALUE"].ToString();                                                
                }                
            }
            catch(Exception ex)
                {                  
            }
            return result;
        }

        public List<ButtonInfo> BuildSearchButtonsInfo()
        {
            try
            {
                //return OracleHelper.ExecuteStoreProcedure<ButtonInfo>(ConnectionString, null, SYSTEM_STORE_PROCEDURES.LIST_BUTTON);
                return OracleHelper.ExecuteStoreProcedure<ButtonInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.LIST_BUTTON);
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void ListSearchButton(out List<ButtonInfo> buttons)
        {
            buttons = AllCaches.SearchButtonsInfo;
        }

        public List<ButtonParamInfo> BuildSearchButtonParamsInfo()
        {
            try
            {
                return OracleHelper.ExecuteStoreProcedure<ButtonParamInfo>(ConnectionString, null, SYSTEM_STORE_PROCEDURES.LIST_BUTTON_PARAM);
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void ListSearchButtonParam(out List<ButtonParamInfo> searchButtonParamsInfo)
        {
            searchButtonParamsInfo = AllCaches.SearchButtonParamsInfo;
        }

        [OperationContract]
        public void ExecuteSwitchModule(out string targetModule, string moduleID, string subModule, List<string> values)
        {
            try
            {
                var switchInfo = (SwitchModuleInfo)ModuleUtils.GetModuleInfo(moduleID, subModule);
                CheckRole(switchInfo);
                targetModule = OracleHelper.ExecuteStoreProcedureGeneric<string>(ConnectionString, Session, switchInfo.SwitchStore, values.ToArray())[0];
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void ExecuteMaintainQuery(out DataContainer executeResult, string moduleID, string subModule, List<string> values)
        {
            try
            {
                var maintainInfo = (MaintainModuleInfo)ModuleUtils.GetModuleInfo(moduleID, subModule);
                CheckRole(maintainInfo);
                DataTable table = null;
                switch (subModule)
                {
                    case CODES.DEFMOD.SUBMOD.MAINTAIN_ADD:
                        OracleHelper.FillDataTable(ConnectionString, Session, maintainInfo.AddSelectStore, out table, values.ToArray());
                        break;
                    case CODES.DEFMOD.SUBMOD.MAINTAIN_EDIT:
                        OracleHelper.FillDataTable(ConnectionString, Session, maintainInfo.EditSelectStore, out table, values.ToArray());
                        break;
                    case CODES.DEFMOD.SUBMOD.MAINTAIN_VIEW:                                               
                        OracleHelper.FillDataTable(ConnectionString, Session, maintainInfo.ViewSelectStore, out table, values.ToArray());                       
                        break;
                }
                executeResult = new DataContainer { DataTable = table };
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void ExecuteTransQuery(out DataContainer executeResult, string moduleID, string subModule, List<string> values)
        {
            try
            {
                var maintainInfo = (MaintainModuleInfo)ModuleUtils.GetModuleInfo(moduleID, subModule);
                CheckRole(maintainInfo);
                DataTable table = null;               
                OracleHelper.FillDataTable(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.TRANS_STOREPROC, out table, values.ToArray());                       
                executeResult = new DataContainer { DataTable = table };
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        //add by TrungTT - 5.5.2011 - add module report
        [OperationContract]
        public void ExecuteReport(out DataContainer container, string moduleID, string subModule, List<string> values)
        {
            try
            {
                var reportInfo = (ReportModuleInfo)ModuleUtils.GetModuleInfo(moduleID, subModule);
                CheckRole(reportInfo);
                DataSet ds;
                OracleHelper.FillDataSet(ConnectionString, Session, reportInfo.StoreName, out ds, values.ToArray());
                container = new DataContainer() { DataSet = ds };
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("SMS", ex.ToString(), EventLogEntryType.Error);
                throw ErrorUtils.CreateError(ex);
            }
        }
        //end TrungTT

        //add by TrungTT - 10.11.2011 - maintain report
        [OperationContract]
        public void ExecuteMaintainReport(out DataContainer container, string moduleID, string subModule, List<string> values)
        {
            try
            {
                var ReportInfo = (ReportModuleInfo)ModuleUtils.GetModuleInfo(moduleID, subModule);
                //CheckRole(ReportInfo);
                DataSet ds;
                DataTable dt;
                                                    
                // Send Email
                if (ReportInfo.SendEmail == CONSTANTS.Yes)
                {
                    //01. Lay thong tin Email
                    // Get content
                    string content = null; string subject = null; string contractno = null; string client = null; string email = null; string payment_date = null; string statement_date = null;
                    List<String> tempValue = new List<string>();
                    tempValue.Add(moduleID);
                    OracleHelper.FillDataTable(ConnectionString, Session, "sp_mailrouter_sel", out dt, tempValue.ToArray());                    
                    if (dt.Rows.Count != 0)
                    {
                        content = dt.Rows[0]["content"].ToString();
                        subject = dt.Rows[0]["subject"].ToString();
                    }                    
                    
                    OracleHelper.FillDataTable(ConnectionString, Session, "sp_list_mail_visa", out dt, values[1]);                    

                    string[] arr = values[0].Split(',');
                    
                    foreach (string arrItem in arr)
                    {
                        List<string> ValueArray = new List<string>();
                        ValueArray.Add(arrItem);
                        ValueArray.Add(values[1]);
                        OracleHelper.FillDataSet(ConnectionString, Session, ReportInfo.StoreName, out ds, ValueArray.ToArray());

                        for (int i = 0; i < dt.Rows.Count;i++ )
                        {
                            if (dt.Rows[i]["contractno"].ToString() == ValueArray[0].ToString())
                            {
                                contractno = dt.Rows[i]["contractno"].ToString();
                                client = dt.Rows[i]["client"].ToString();
                                email = dt.Rows[i]["email"].ToString();
                                payment_date = dt.Rows[i]["payment_date"].ToString();
                                statement_date = dt.Rows[i]["statement_date"].ToString();

                                Directory.CreateDirectory("Reports");
                                ds.WriteXml("Reports\\" + ReportInfo.ReportName + ".xml", XmlWriteMode.WriteSchema);
                                var report = XtraReport.FromFile("Reports\\" + ReportInfo.ReportName + ".repx", true);                                                                
                                report.XmlDataPath = "Reports\\" + ReportInfo.ReportName + ".xml";

                                string strProcedureName = ReportInfo.StoreName.ToString();
                                string strReportName = ReportInfo.ReportName.ToString();
                                if (strProcedureName.Length > 9)
                                {
                                    CreateReportMultiDetail(strProcedureName, strReportName, report, ds);
                                }
                                SendEmailSaoke(report, subject, content, client, email, payment_date, statement_date, contractno);
                            }

                        }                        
                    }
                   
                    container = null;
                }
                else
                {
                    OracleHelper.FillDataSet(ConnectionString, Session, ReportInfo.StoreName, out ds, values.ToArray());
                    container = new DataContainer() { DataSet = ds };      
                }                                
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }
        private void CreateReportMultiDetail(string strProcedureName, string strReportName, XtraReport reportMaster, DataSet ds)
        {
            XRSubreport subReport;
            XtraReport reportDetail;
            int n = Int32.Parse(strProcedureName.Substring(9, 2).ToString());

            if (!strProcedureName.Contains('C'))
            {
                for (int i = 1; i <= n; i++)
                {
                    reportDetail = XtraReport.FromFile("Reports\\" + strReportName + "_" + i + ".repx", true);
                    reportDetail.XmlDataPath = "Reports\\" + strReportName + ".xml";
                    int j = i - 1;
                    if (j == 0)
                        reportDetail.DataMember = "Table";
                    else
                        reportDetail.DataMember = "Table" + j;
                    subReport = ((XRSubreport)reportMaster.FindControl("subreport" + i, true));

                    subReport.ReportSource = reportDetail;
                }
            }            
        }        

        //add by TrungTT - 8.11.2011 - Auto Report
        [OperationContract]
        public void ExecuteAutoReport(out DataContainer container, string moduleID, string batchName, List<string> values)
        {
            try
            {
                var moduleInfo = ModuleUtils.GetModuleInfo(moduleID, CODES.DEFMOD.SUBMOD.MODULE_MAIN);
                CheckRole(moduleInfo);
                DataSet ds;
                var batchInfo = OracleHelper.ExecuteStoreProcedure<BatchInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.LIST_BATCHINFO_BY_NAME, moduleID, batchName)[0];
                OracleHelper.FillDataSet(ConnectionString, Session, batchInfo.BatchStore, out ds, values.ToArray());
                container = new DataContainer() { DataSet = ds };
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }
        //End TrungTT

        //add by trungtt - 24.08.2011 - Menu Market
        //[OperationContract]
        //public void ExecuteMarket(out DataContainer container)
        //{
        //    try
        //    {
        //        DataSet ds;
        //        OracleHelper.FillDataSet(ConnectionString, Session, "SP_EXECUTE_MARKET_CURRENT", out ds, null);
        //        container = new DataContainer() { DataSet = ds };
        //    }
        //    catch (FaultException)
        //    {
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ErrorUtils.CreateError(ex);
        //    }
        //}
        //end trungtt

        //add by TrungTT - 23.09.2011 - Menu Skins
        [OperationContract]
        public void ExecuteLoadSkins(out DataContainer container)
        {
            try
            {
                DataSet ds;
                OracleHelper.FillDataSet(ConnectionString, Session, "SP_ExecuteLoadSkins", out ds, null);
                container = new DataContainer() { DataSet = ds };
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }
        //end TrungTT

        //add by TrungTT - 27.09.2011 - Get System Date
        [OperationContract]
        public void GetSysDate(out DataContainer container)
        {
            try
            {
                DataSet ds;
                OracleHelper.FillDataSet(ConnectionString, Session, "SP_GetSysDate", out ds, null);
                container = new DataContainer() { DataSet = ds };
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }
        //end TrungTT

        //add by TrungTT - 26.09.2011 - Load Current Skins
        [OperationContract]
        public void ExecuteLoadCurrentSkins(out DataContainer container)
        {
            try
            {
                DataSet ds;
                OracleHelper.FillDataSet(ConnectionString, Session, "SP_EXECUTELOADCURRENTSKINS", out ds, null);
                container = new DataContainer() { DataSet = ds };
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }
        //end TrungTT

        //ADD BY TRUNGTT - 08.12.2011 - EDIT LAYOUT GRID'S COLUMN - SOURCE BY LONGND5
        [OperationContract]
        public void GetExtraCurrentUserProfile(string extraProperty, out string extraValue)
        {
            try
            {
                extraValue = null;
                if (Session != null)
                {
                    extraValue = OracleHelper.ExecuteStoreProcedureGeneric<string>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.PROFILES_SEL_EXTRA, Session.Username, extraProperty)[0];
                }
            }
            catch
            {
                extraValue = null;
            }
        }
        [OperationContract]
        public void SetExtraCurrentUserProfile(string extraProperty, string extraValue)
        {
            try
            {
                if (Session != null)
                {
                    OracleHelper.ExecuteStoreProcedure(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.PROFILES_UDP_EXTRA, Session.Username, extraProperty, extraValue);
                }
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }
        //END TRUNGTT

        //add by trungtt - 25.08.2011 - update market
        [OperationContract]
        public void UpdateMarket(List<string> values)
        {
            try
            {
                OracleHelper.ExecuteStoreProcedure(ConnectionString, Session, "SP_MARKET_CURRENT", values.ToArray());
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }
        //end trungtt

        //add by trungtt - 26.09.2011 - Change Skins
        [OperationContract]
        public void ExecuteChangeSkins(List<string> values)
        {
            try
            {
                OracleHelper.ExecuteStoreProcedure(ConnectionString, Session, "SP_EXECUTECHANGESKINS", values.ToArray());
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }
        //end trungtt

        //CuongNH7 : 18-05-2011
        [OperationContract]
        public void ExecuteDownloadFile(out DataContainer container, string moduleID, string subModule, List<string> values)
        {
            try
            {
                var maintainInfo = (MaintainModuleInfo)ModuleUtils.GetModuleInfo(moduleID, subModule);
                //CheckRole(reportInfo); 
                container = null;
                DataSet ds;
                OracleHelper.FillDataSet(ConnectionString, Session, maintainInfo.ReportStore, out ds, values.ToArray());
                if (ds.Tables.Count == 1)
                    container = new DataContainer() { DataTable = ds.Tables[0] };

            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }
        //End CuongNH7
        [OperationContract]
        public void ExecuteImport(string moduleID, string subModule, List<string> values)
        {
            try
            {
            var importInfo = (ImportModuleInfo)ModuleUtils.GetModuleInfo(moduleID, subModule);
                CheckRole(importInfo);
                OracleHelper.ExecuteStoreProcedure(ConnectionString, Session, importInfo.ImportStore, values.ToArray());
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void ExecuteMaintain(out DataContainer container, string moduleID, string subModule, List<string> values)
        {            
            try
            {
                var maintainInfo = (MaintainModuleInfo)ModuleUtils.GetModuleInfo(moduleID, subModule);
                CheckRole(maintainInfo);
                string txnum = null;
                container = null;
                DataSet ds = new DataSet();
                DataSet dsLog = new DataSet();
                List<string> values1 = new List<string>();
               
                switch (subModule)
                {
                    case CODES.DEFMOD.SUBMOD.MAINTAIN_ADD:

                        if (maintainInfo.Approve == CODES.MODMAINTAIN.APROVE.YES)
                        {
                            try
                            {
                                // Checking data
                                if (!string.IsNullOrEmpty(maintainInfo.ADDINSERTCHECK))
                                {
                                    OracleHelper.ExecuteStoreProcedure(ConnectionString, Session, maintainInfo.ADDINSERTCHECK, values.ToArray());
                                }

                                // Gettxnum                                
                                OracleHelper.FillDataSet(ConnectionString, Session, "sp_get_txnum", out dsLog, values1.ToArray());
                                if (dsLog.Tables.Count == 1)
                                {
                                    txnum = dsLog.Tables[0].Rows[0][0].ToString();
                                }
                                //Cap nhat bang tllog
                                dsLog.Dispose();
                                values1.Clear();
                                values1.Add(txnum);
                                values1.Add(moduleID);
                                values1.Add(subModule);
                                OracleHelper.ExecuteStoreProcedure(ConnectionString, Session, "sp_tllog_ins", values1.ToArray());
                                // Cap nhat bang tllogfld 
                                dsLog.Dispose();
                                values1.Clear();
                                values1.Add(maintainInfo.ModuleID);
                                values1.Add(maintainInfo.AddInsertStore);
                                OracleHelper.FillDataSet(ConnectionString, Session, "SP_GET_FLDCD", out dsLog, values1.ToArray());
                                if (dsLog.Tables.Count == 1)
                                {
                                    List<string> valuesFLDCD = new List<string>();

                                    for (int i = 0; i < dsLog.Tables[0].Rows.Count; i++)
                                    {
                                        if (values[i] != null && values[i] != "")
                                        {
                                            valuesFLDCD.Clear();
                                            valuesFLDCD.Add(txnum);
                                            valuesFLDCD.Add(dsLog.Tables[0].Rows[i]["FLDNAME"].ToString());
                                            //HUYVQ_19/06/2014
                                            if (dsLog.Tables[0].Rows[i]["DATA_TYPE"].ToString() == "DATE")
                                            {
                                                DateTime d = Convert.ToDateTime(values[i], App.Environment.ServerInfo.Culture);
                                                valuesFLDCD.Add(String.Format("{0:dd/MM/yyyy}",d));
                                            }
                                            else
                                            {
                                                valuesFLDCD.Add(values[i].ToString());
                                            }      
                                            //valuesFLDCD.Add(values[i].ToString());
                                            valuesFLDCD.Add(dsLog.Tables[0].Rows[i]["DATA_TYPE"].ToString());
                                            OracleHelper.ExecuteStoreProcedure(ConnectionString, Session, "sp_tllogfld_ins", valuesFLDCD.ToArray());
                                        }
                                    }
                                }
                                //SendMail(moduleID, maintainInfo.SubModule, null, null,null);
                            }
                            catch (Exception ex)
                            {                               
                                OracleHelper.ExecuteStoreProcedure(ConnectionString, Session, "sp_tllog_udp", txnum);
                                throw ex;
                            }                                                        
                        }
                        //add by trungtt - 10.19.2014
                        else if (maintainInfo.Report == "SML")
                        {
                            SendMailByModuleMaintaince(values);
                        }
                        //end trungtt - 10.9.2014
                        else
                        {
                            OracleHelper.FillDataSet(ConnectionString, Session, maintainInfo.AddInsertStore, out ds, values.ToArray());
                            if (ds.Tables.Count == 1)
                                container = new DataContainer() { DataTable = ds.Tables[0] };
                            if (maintainInfo.SendEmail == CODES.DEFMOD.SENDMAIL.YES)
                            {
                                //DataTable resultTable;
                                //string RptName = string.Empty;
                                //OracleHelper.FillDataTable(ConnectionString, Session, "SP_RPTMASTER_SEL_BY_RPTID", out resultTable, values.ToArray());
                                //if (resultTable.Rows.Count > 0)
                                //{
                                //    RptName = resultTable.Rows[0][0].ToString();
                                //}
                                //SendMail(moduleID, maintainInfo.SubModule, null, RptName, null);  
                                SendMail(moduleID, maintainInfo.SubModule);
                            }                            
                        }                            
                        break;
                    case CODES.DEFMOD.SUBMOD.MAINTAIN_EDIT:                       
                        // Bo sung truong hop doi voi log
                        if (maintainInfo.Approve == CODES.MODMAINTAIN.APROVE.YES)
                        {
                            try
                            {
                                // Checking data
                                if (!string.IsNullOrEmpty(maintainInfo.EDITSTORECHECK))
                                {
                                    OracleHelper.ExecuteStoreProcedure(ConnectionString, Session, maintainInfo.EDITSTORECHECK, values.ToArray());
                                }

                                // Gettxnum                            
                                OracleHelper.FillDataSet(ConnectionString, Session, "sp_get_txnum", out dsLog, values1.ToArray());
                                if (dsLog.Tables.Count == 1)
                                {
                                    txnum = dsLog.Tables[0].Rows[0][0].ToString();
                                }
                                //Cap nhat bang tllog
                                dsLog.Dispose();
                                values1.Clear();
                                values1.Add(txnum);
                                values1.Add(moduleID);
                                values1.Add(subModule);
                                OracleHelper.ExecuteStoreProcedure(ConnectionString, Session, "sp_tllog_ins", values1.ToArray());
                                // Cap nhat bang tllogfld 
                                dsLog.Dispose();
                                values1.Clear();
                                values1.Add(maintainInfo.ModuleID);
                                values1.Add(maintainInfo.EditUpdateStore);
                                OracleHelper.FillDataSet(ConnectionString, Session, "SP_GET_FLDCD", out dsLog, values1.ToArray());
                                if (dsLog.Tables.Count == 1)
                                {
                                    List<string> valuesFLDCD = new List<string>();

                                    for (int i = 0; i < dsLog.Tables[0].Rows.Count; i++)
                                    {
                                        if (values[i] != null && values[i] != "")
                                        {
                                            valuesFLDCD.Clear();
                                            valuesFLDCD.Add(txnum);
                                            valuesFLDCD.Add(dsLog.Tables[0].Rows[i]["FLDNAME"].ToString());
                                            //HUYVQ_19/06/2014
                                            if (dsLog.Tables[0].Rows[i]["DATA_TYPE"].ToString() == "DATE")
                                            {
                                                DateTime d = Convert.ToDateTime(values[i], App.Environment.ServerInfo.Culture);
                                                valuesFLDCD.Add(String.Format("{0:dd/MM/yyyy}", d));
                                            }
                                            else
                                            {
                                                valuesFLDCD.Add(values[i].ToString());
                                            }
                                            
                                            valuesFLDCD.Add(dsLog.Tables[0].Rows[i]["DATA_TYPE"].ToString());
                                            OracleHelper.ExecuteStoreProcedure(ConnectionString, Session, "sp_tllogfld_ins", valuesFLDCD.ToArray());
                                        }

                                    }
                                }
                                //SendMail(moduleID, maintainInfo.SubModule, null, null, null);
                            }
                            catch (Exception ex)
                            {                                
                                OracleHelper.ExecuteStoreProcedure(ConnectionString, Session, "sp_tllog_udp", txnum);
                                throw ex;
                            }
                           
                        }
                        else
                        {
                            OracleHelper.FillDataSet(ConnectionString, Session, maintainInfo.EditUpdateStore, out ds, values.ToArray());
                            if (ds.Tables.Count == 1)
                                container = new DataContainer() { DataTable = ds.Tables[0] };
                            if (maintainInfo.SendEmail == CODES.DEFMOD.SENDMAIL.YES)
                            {
                                //DataTable resultTable;
                                //string RptName = string.Empty;
                                //string secid = null;
                                //OracleHelper.FillDataTable(ConnectionString, Session, "SP_RPTMASTER_SEL_BY_RPTID", out resultTable, values.ToArray());

                                //if (maintainInfo.ModuleID == "02033" || maintainInfo.ModuleID == "02032")
                                //{
                                //    OracleHelper.FillDataTable(ConnectionString, Session, "SP_RPTNAME_SEL_EX", out resultTable, out secid, values.ToArray());
                                //}
                                //else
                                //{
                                //    OracleHelper.FillDataTable(ConnectionString, Session, "SP_RPTNAME_SEL", out resultTable, values.ToArray());
                                //}

                                //if (resultTable.Rows.Count > 0)
                                //{
                                //    RptName = resultTable.Rows[0][0].ToString();
                                //    //add by trungtt - 23.12.2014 - get Secid
                                //    if(resultTable.Columns.Count == 2)
                                //        secid = resultTable.Rows[0][1].ToString();
                                //    //end trungtt
                                //}

                                
                                SendMail(moduleID, maintainInfo.SubModule);
                            }            
                        }                        
                        break;
                }
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {               
                throw ErrorUtils.CreateError(ex);
            }
        }

        

        //add by trungtt - 28.3.2013
        [OperationContract]
        public void ExecApprove(out DataContainer container, string moduleID, string subModule,string secID, List<string> values)
        {
            try
            {
                var maintainInfo = (MaintainModuleInfo)ModuleUtils.GetModuleInfo(moduleID, subModule);
                CheckRole(maintainInfo);
                container = null;
                DataSet ds;
                switch (subModule)
                {
                    case CODES.DEFMOD.SUBMOD.MAINTAIN_ADD:
                        OracleHelper.FillDataSet(ConnectionString, Session, maintainInfo.AddInsertStore, out ds, values.ToArray());
                        if (ds.Tables.Count == 1)
                            container = new DataContainer() { DataTable = ds.Tables[0] };
                        //SendMail(moduleID, maintainInfo.SubModule, maintainInfo.Approve, null,secID);
                        break;
                    case CODES.DEFMOD.SUBMOD.MAINTAIN_EDIT:
                        OracleHelper.FillDataSet(ConnectionString, Session, maintainInfo.EditUpdateStore, out ds, values.ToArray());
                        if (ds.Tables.Count == 1)
                            container = new DataContainer() { DataTable = ds.Tables[0] };
                        //SendMail(moduleID, maintainInfo.SubModule, maintainInfo.Approve, null,secID);
                        break;
                }
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }
        //end trungtt
 
        //add by TrungTT - 28.11.2011 - Execute Procedure Fill Dataset
        [OperationContract]
        public void ExecuteProcedureFillDataset(out DataContainer container, string storeData, List<string> values)
        {
            try
            {
                container = null;
                DataSet ds;
                OracleHelper.FillDataSet(ConnectionString, Session, storeData, out ds, values.ToArray());
                if (ds.Tables.Count == 1)
                {
                    //TUDQ them
                    //CheckDataRole(ds.Tables[0]);
                    //
                    container = new DataContainer() { DataTable = ds.Tables[0] };
                }

            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }
        //End TrungTT

        [OperationContract]
        public void ExecuteChartMaster(out DataContainer executeResult, string moduleID, string subModule, List<string> values)
        {
            try
            {
                var chartModuleInfo = (ChartModuleInfo)ModuleUtils.GetModuleInfo(moduleID, subModule);
                CheckRole(chartModuleInfo);

                DataTable resultTable;
                OracleHelper.FillDataTable(ConnectionString, Session, chartModuleInfo.ChartDataStore, out resultTable, values.ToArray());
                //TUDQ them
                CheckDataRole(resultTable);
                //
                executeResult = new DataContainer { DataTable = resultTable };
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void ExecuteProcedure(string moduleID, string subModule, List<string> values)
        {
            try
            {
                var execProcInfo = (ExecProcModuleInfo)ModuleUtils.GetModuleInfo(moduleID, subModule);
                CheckRole(execProcInfo);
                OracleHelper.ExecuteStoreProcedure(ConnectionString, Session, execProcInfo.ExecuteStore, values.ToArray());               
                if (execProcInfo.SendEmail == CODES.DEFMOD.SENDMAIL.YES)
                {
                    DataTable resultTable;
                    string RptName = string.Empty;
                    string secid = null;
                    // Doan nay hardcode me rui
                    if (moduleID == "01285" || moduleID == "01255")
                    {
                        OracleHelper.FillDataTable(ConnectionString, Session, "SP_RPTNAME_SEL_EX", out resultTable, out secid, values.ToArray());
                    }
                    else
                    {
                        OracleHelper.FillDataTable(ConnectionString, Session, "SP_RPTNAME_SEL", out resultTable, values.ToArray());
                    }
                    
                    if (resultTable.Rows.Count > 0)
                    {
                        RptName = resultTable.Rows[0][0].ToString();
                    }
                    //SendMail(moduleID, "MMN", null, RptName, secid);  
                }
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void ExecuteInstallModule(string modulePackageData)
        {
            try
            {
                var moduleInfo = ModuleUtils.GetModuleInfo(STATICMODULE.IEMODULE, CODES.DEFMOD.SUBMOD.MODULE_MAIN);
                CheckRole(moduleInfo);

                var ds = new DataSet();
                ds.ReadXml(new StringReader(modulePackageData));

                // Process Script
                foreach (DataRow row in ds.Tables["SCRIPT"].Rows)
                {
                    if (row["STOREBODY"] != DBNull.Value)
                    {
                        var query = string.Format("CREATE OR REPLACE\r\n{0}", row["STOREBODY"]);
                        var conn = new OracleConnection(ConnectionString);
                        var comm = new OracleCommand(query, conn);
                        conn.Open();
                        comm.ExecuteNonQuery();
                        conn.Close();
                    }
                }

                // Execute Script
                foreach (DataTable dt in ds.Tables)
                {
                    if (dt.TableName != "TABLE_NAME" && dt.TableName != "SCRIPT")
                    {
                        var whereQuery = "";
                        var sep2 = "";

                        using (var conn = new OracleConnection(ConnectionString))
                        {
                            conn.Open();

                            var tableKey = (string)ds.Tables["TABLE_NAME"].Select("TABLE_NAME = '" + dt.TableName + "'")[0]["TABLE_KEY"];
                            foreach (DataColumn col in dt.Columns)
                            {
                                if (tableKey.Contains(col.ColumnName))
                                {
                                    whereQuery += string.Format("{1}({0} = :{0} OR :{0} IS NULL)", col.ColumnName, sep2);
                                    sep2 = " AND ";
                                }
                            }

                            foreach (DataRow row in dt.Rows)
                            {
                                var comm = new OracleCommand
                                {
                                    Connection = conn,
                                    BindByName = true,
                                    CommandText = string.Format("DELETE FROM {0} WHERE {1}", dt.TableName, whereQuery)
                                };

                                foreach (DataColumn col in dt.Columns)
                                {
                                    if (tableKey.Contains(col.ColumnName))
                                        comm.Parameters.Add(":" + col.ColumnName, row[col]);
                                }

                                comm.ExecuteNonQuery();
                            }

                            conn.Close();
                        }
                    }
                }

                foreach (DataTable dt in ds.Tables)
                {
                    if (dt.TableName != "TABLE_NAME" && dt.TableName != "SCRIPT")
                    {
                        var beginQuery = "";
                        var endQuery = "";
                        var sep = "";

                        foreach (DataColumn dataCol in dt.Columns)
                        {
                            beginQuery += string.Format("{1}{0}", dataCol.ColumnName, sep);
                            endQuery += string.Format("{1}:{0}", dataCol.ColumnName, sep);

                            sep = ", ";
                        }

                        var query = string.Format("INSERT INTO {0}({1}) VALUES ({2})", dt.TableName, beginQuery, endQuery);

                        foreach (DataRow dataRow in dt.Rows)
                        {
                            var conn = new OracleConnection(ConnectionString);
                            conn.Open();

                            var comm = new OracleCommand(query, conn) { BindByName = true };

                            foreach (DataColumn col in dt.Columns)
                            {
                                comm.Parameters.Add(":" + col.ColumnName, dataRow[col]);
                            }

                            comm.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                }
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void ExecuteUninstallModule(string moduleID)
        {
            try
            {
                var moduleInfo = ModuleUtils.GetModuleInfo(STATICMODULE.IEMODULE, CODES.DEFMOD.SUBMOD.MODULE_MAIN);
                CheckRole(moduleInfo);

                OracleHelper.ExecuteStoreProcedure(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.MODULE_UNINSTALL, moduleID);
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void ExecuteSaveLanguage(string langID, string langName, string langValue)
        {
            try
            {
                var moduleInfo = ModuleUtils.GetModuleInfo(STATICMODULE.EDITLANG, CODES.DEFMOD.SUBMOD.MODULE_MAIN);
                CheckRole(moduleInfo);
                OracleHelper.ExecuteStoreProcedure(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.UPDATE_DEFLANG, langID, langName, langValue);
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void ExecuteGenerateModulePackage(string moduleID, out string generatedPackage)
        {
            try
            {
                var moduleInfo = ModuleUtils.GetModuleInfo(STATICMODULE.IEMODULE, CODES.DEFMOD.SUBMOD.MODULE_MAIN);
                CheckRole(moduleInfo);

                DataSet ds;
                OracleHelper.FillDataSet(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.GENERATE_PACKGE, out ds, moduleID);

                var tableList = ds.Tables[0];
                tableList.TableName = "TABLE_NAME";
                for (var i = 0; i < tableList.Rows.Count; i++)
                {
                    ds.Tables[1 + i].TableName = tableList.Rows[i][0].ToString();
                }

                var sw = new StringWriter();
                ds.WriteXml(sw, XmlWriteMode.WriteSchema);
                generatedPackage = sw.ToString();
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void ValidateFieldInfoSyntax(string moduleID, string subModule, string fieldID, List<string> values)
        {
            try
            {
                var moduleInfo = ModuleUtils.GetModuleInfo(STATICMODULE.IEMODULE, CODES.DEFMOD.SUBMOD.MODULE_MAIN);
                CheckRole(moduleInfo);

                var fieldInfo = FieldUtils.GetModuleFieldByID(moduleID, fieldID);
                var validateName = FieldUtils.GetValidateName(moduleInfo, fieldInfo);
                validateName = ExpressionUtils.ParseScript(validateName).StoreProcName;
                var validateInfo = FieldUtils.GetValidateInfo(validateName);

                OracleHelper.ExecuteStoreProcedure(ConnectionString, Session, validateInfo.StoreValidate, values.ToArray());
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void CallbackQuery(out DataContainer executeResult, string moduleID, string callbackFieldID, List<string> values)
        {
            try
            {
                var moduleInfo = ModuleUtils.GetModuleInfo(STATICMODULE.IEMODULE, CODES.DEFMOD.SUBMOD.MODULE_MAIN);
                CheckRole(moduleInfo);

                var storeFieldInfo =
                    FieldUtils.GetModuleFieldByID(
                        moduleID,
                        callbackFieldID
                    );

                executeResult = new DataContainer();
                if (storeFieldInfo != null)
                {
                    DataTable table;
                    OracleHelper.FillDataTable(ConnectionString, Session, storeFieldInfo.Callback, out table, values.ToArray());
                    executeResult.DataTable = table;
                }
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        public List<OracleParam> BuildOracleParamsInfo()
        {
            try
            {
                var stores = OracleHelper.ExecuteStoreProcedure<OracleStore>(ConnectionString, null, SYSTEM_STORE_PROCEDURES.LIST_STOREPROC);
                var oracleParams = new List<OracleParam>();

                foreach (var store in stores)
                {
                    try
                    {
                        OracleHelper.DiscoveryParameters(ConnectionString, store.StoreName, oracleParams);
                    }
                    catch
                    {
                    }
                }
                return oracleParams;
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void ListOracleParameter(out List<OracleParam> oracleParams)
        {
            oracleParams = AllCaches.OracleParamsInfo;
        }

        private string BuildSearchExtension(SearchModuleInfo searchInfo, SearchConditionInstance conditionIntance)
        {
            var fields = FieldUtils.GetModuleFields(searchInfo.ModuleID);
            using (var conn = new OracleConnection(ConnectionString))
            {
                using (var comm = new OracleCommand(searchInfo.WhereExtension, conn))
                {
                    conn.Open();
                    comm.CommandType = CommandType.StoredProcedure;
                    OracleHelper.DiscoveryParameters(comm);

                    foreach (var field in
                        fields.Where(field => field.WhereExtension == CODES.DEFMODFLD.WHEREEXTENSION.YES))
                    {
                        comm.Parameters[field.FieldName].Value = DBNull.Value;
                        foreach (var condition in conditionIntance.SubCondition)
                        {
                            if (condition.ConditionID == field.FieldID && string.IsNullOrEmpty(condition.SQLLogic))
                            {
                                comm.Parameters[field.FieldName].Value = condition.Operator;
                            }
                        }
                    }
                    comm.ExecuteNonQuery();
                    return comm.Parameters["RETURN_VALUE"].Value.ToString();
                }
            }
        }

        [OperationContract]
        public void FetchAllSearchResult(out DataContainer searchResult, string moduleID, string subModule, string searchResultKey, DateTime searchTime, int fromRow)
        {
            try
            {
                var moduleInfo = ModuleUtils.GetModuleInfo(moduleID, subModule);
                CheckRole(moduleInfo);

                var cacheResult = (from item in CachedSearchResult
                                   where
                                        item.SearchKey == searchResultKey &&
                                        item.TimeSearch == searchTime &&
                                        item.SessionKey == Session.SessionKey
                                   select item).SingleOrDefault();

                if (cacheResult != null)
                {
                    //TUDQ them
                    //cacheResult.CachedResult = CheckDataRole(cacheResult.CachedResult);
                    //END
                    lock (cacheResult.CachedResult)
                    {
                        cacheResult.BufferData(fromRow + CONSTANTS.MAX_ROWS_IN_BUFFER);

                        var resultTable = cacheResult.CachedResult.Clone();
                        var rows = cacheResult.CachedResult.Rows.OfType<DataRow>().Skip(fromRow).Take(CONSTANTS.MAX_ROWS_IN_BUFFER).ToArray();
                        foreach (var t in rows)
                        {
                            resultTable.ImportRow(t);
                        }
                        //TUDQ them
                        //     resultTable=  CheckDataRole(resultTable);
                        //
                        searchResult = new DataContainer { DataTable = resultTable };
                    }
                }
                else
                {
                    throw ErrorUtils.CreateError(ERR_SYSTEM.ERR_SEARCH_RESULT_NOT_FOUND);
                }
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void FetchSearchResult(out DataContainer searchResult, out int bufferSize, out int minPage, out int maxPage, out int startRow, string moduleID, string subModule, string searchResultKey, DateTime searchTime, int selectedPage, int maxPageSize)
        {
            try
            {
                var searchInfo = ModuleUtils.GetModuleInfo(moduleID, subModule);
                CheckRole(searchInfo);

                var cacheResult = (from item in CachedSearchResult
                                   where
                                        item.SearchKey == searchResultKey &&
                                        item.TimeSearch == searchTime &&
                                        item.SessionKey == Session.SessionKey
                                   select item).FirstOrDefault();

                if (cacheResult != null)
                {
#if DEBUG
                    var startTime = DateTime.Now;
#endif
                    searchResult = new DataContainer();

                    if (cacheResult.IsBufferMode)
                    {
                        try
                        {
                            //TUDQ them
                            cacheResult.CachedResult = CheckDataRole(cacheResult.CachedResult);
                            //END
                            searchResult.DataTable = cacheResult.GetSearchResult(selectedPage * maxPageSize, maxPageSize);
                        }
                        catch
                        {
                        }

                        startRow = selectedPage * maxPageSize;
                        minPage = 0;
                        maxPage = (int)Math.Ceiling(1.0 * cacheResult.CachedResult.Rows.Count / maxPageSize) - 1;
                    }
                    else
                    {
                        startRow = selectedPage * maxPageSize;
                        minPage = Math.Max(0, selectedPage - CONSTANTS.PAGE_VISIBLE_COUNT / 2);

                        try
                        {
                            cacheResult.BufferData((minPage + CONSTANTS.PAGE_VISIBLE_COUNT) * maxPageSize);
                            //TUDQ them
                            cacheResult.CachedResult = CheckDataRole(cacheResult.CachedResult);
                            //END
                            searchResult.DataTable = cacheResult.GetSearchResult(selectedPage * maxPageSize, maxPageSize);
                        }
                        catch
                        {
                        }

                        maxPage = (int)Math.Ceiling(1.0 * cacheResult.CachedResult.Rows.Count / maxPageSize) - 1;
                        maxPage = Math.Min(maxPage, minPage + CONSTANTS.PAGE_VISIBLE_COUNT - 1);
                    }
                    bufferSize = cacheResult.CachedResult.Rows.Count;
#if DEBUG
                    WriteLog(
                        "Search: " + searchResultKey.Replace("&", "&&"),
                        string.Format("Fetch {0} row(s), from {1}", maxPageSize, selectedPage * maxPageSize),
                        string.Format("{0:#,0.000} second(s)", (DateTime.Now - startTime).TotalSeconds),
                        "y"
                    );
#endif
                }
                else
                {
                    throw ErrorUtils.CreateError(ERR_SYSTEM.ERR_SEARCH_RESULT_NOT_FOUND);
                }
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void DisposeSearchResult(string moduleID, string subModule, string searchResultKey, DateTime searchTime)
        {
            try
            {
                var searchInfo = (SearchModuleInfo)ModuleUtils.GetModuleInfo(moduleID, subModule);
                CheckRole(searchInfo);

                var cacheResult = (from item in CachedSearchResult
                                   where
                                        item.SearchKey == searchResultKey &&
                                        item.TimeSearch == searchTime &&
                                        item.SessionKey == Session.SessionKey
                                   select item).SingleOrDefault();

                if (cacheResult != null)
                {
                    CachedSearchResult.Remove(cacheResult);
                    cacheResult.Dispose();
                }
            }
            catch
            {
            }
        }

        private void DiscoveryParametersForSearch(OracleCommand command, SearchModuleInfo searchInfo, string queryFormat, SearchConditionInstance conditionIntance, List<SearchConditionInstance> staticConditionInstances)
        {
            command.BindByName = true;

            var whereExtension = "1 = 1";
            if (!string.IsNullOrEmpty(searchInfo.WhereExtension))
            {
                whereExtension = BuildSearchExtension(searchInfo, conditionIntance);
            }


            BuildStaticConditions(searchInfo, command, staticConditionInstances);

            var whereCondition = ModuleUtils.BuildSearchCondition(searchInfo, ref whereExtension, command, conditionIntance);
            if (string.IsNullOrEmpty(whereCondition)) whereCondition = "1 = 1";

            command.CommandText = string.Format(queryFormat, whereCondition, whereExtension);
            if (searchInfo.ModuleID == STATICMODULE.UPFILE_MODID)
            {
                command.CommandText = queryFormat + " and sessionskey = '" + Session.SessionKey + "'";
            }

        }

        [OperationContract]
        public void GetSearchStatistic(out DataContainer searchStatistic, string moduleID, string subModule, SearchConditionInstance conditionIntance, List<SearchConditionInstance> staticConditionInstances)
        {
            try
            {
                var searchInfo = (SearchModuleInfo)ModuleUtils.GetModuleInfo(moduleID, subModule);
                CheckRole(searchInfo);

                if (string.IsNullOrEmpty(searchInfo.StatisticQuery))
                {
                    searchStatistic = null;
                }
                else
                {
                    using (var conn = new OracleConnection(ConnectionString))
                    {
                        using (var comm = new OracleCommand())
                        {
                            comm.Connection = conn;
                            var adap = new OracleDataAdapter(comm);

                            DiscoveryParametersForSearch(comm, searchInfo, searchInfo.StatisticQuery, conditionIntance, staticConditionInstances);
#if DEBUG
                            var startTime = DateTime.Now;
                            try
                            {
                                Directory.CreateDirectory("Queries");
                                File.Delete(string.Format("Queries\\{0}-Statistic.txt", searchInfo.ModuleID));
                                File.WriteAllText(string.Format("Queries\\{0}-Statistic.txt", searchInfo.ModuleID), comm.CommandText);
                            }
                            catch
                            {
                            }

#endif

                            var table = new DataTable();
                            adap.Fill(table);
                            searchStatistic = new DataContainer { DataTable = table };
#if DEBUG
                            var searchResultKey = searchInfo.ModuleID + "-" + searchInfo.ModuleName + "?" + ModuleUtils.BuildSearchConditionKey(searchInfo, conditionIntance);
                            WriteLog(
                                "Search: " + searchResultKey.Replace("&", "&&"),
                                "Query Status SQL",
                                string.Format("{0:#,0.000} second(s)", (DateTime.Now - startTime).TotalSeconds),
                                "y"
                            );
#endif
                        }
                    }
                }
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void ExecuteSearch(out string searchResultKey, out DateTime searchTime, string moduleID, string subModule, SearchConditionInstance conditionIntance, List<SearchConditionInstance> staticConditionInstances)
        {
            try
            {
                var searchInfo = (SearchModuleInfo)ModuleUtils.GetModuleInfo(moduleID, subModule);
                CheckRole(searchInfo);

                var conn = new OracleConnection(ConnectionString);
                var comm = new OracleCommand();
                try
                {
                    var ds = new DataSet();
                    comm.Connection = conn;
                    conn.Open();

                    searchResultKey = searchInfo.ModuleID + "-" + searchInfo.ModuleName + "?" + ModuleUtils.BuildSearchConditionKey(searchInfo, conditionIntance);
                    DiscoveryParametersForSearch(comm, searchInfo, searchInfo.QueryFormat, conditionIntance, staticConditionInstances);
#if DEBUG
                    var startTime = DateTime.Now;
                    WriteLog(
                        searchResultKey.Replace("&", "&&"),
                        "Start search",
                        string.Format("By user: <b>{0}</b>, at <b>{1:HH:mm:ss dd/MM/yyyy}</b>", Session.Username, DateTime.Now),
                        "g"
                    );
                    WriteLog(
                        searchResultKey.Replace("&", "&&"),
                        "Key search",
                        searchResultKey
                    );
                    WriteLog(
                        searchResultKey.Replace("&", "&&"),
                        "SQL",
                        comm.CommandText,
                        "y"
                    );

                    try
                    {
                        Directory.CreateDirectory("Queries");
                        File.Delete(string.Format("Queries\\{0}.txt", searchInfo.ModuleID));
                        File.WriteAllText(string.Format("Queries\\{0}.txt", searchInfo.ModuleID), comm.CommandText);
                    }
                    catch
                    {
                    }
#endif
                    searchTime = DateTime.Now;
                    if (searchInfo.PageMode == CODES.MODSEARCH.PAGEMODE.PAGE_FROM_DATASET || searchInfo.PageMode == CODES.MODSEARCH.PAGEMODE.ALL_FROM_DATASET)
                    {
                        var adap = new OracleDataAdapter(comm);
                        adap.Fill(ds, "RESULT");
#if DEBUG
                        WriteLog(
                            "Search: " + searchResultKey.Replace("&", "&&"),
                            "Execute time",
                            string.Format("{0:#,0.000} second(s)", (DateTime.Now - startTime).TotalSeconds),
                            "y"
                        );
#endif
                        CachedSearchResult.Add(new SearchResult
                        {
                            SessionKey = Session.SessionKey,
                            SearchKey = searchResultKey,
                            TimeSearch = searchTime,
                            CachedResult = ds.Tables["RESULT"]
                        });
                    }
                    else
                    {
                        var dataReader = comm.ExecuteReader();
                        CachedSearchResult.Add(new SearchResult
                        {
                            SessionKey = Session.SessionKey,
                            SearchKey = searchResultKey,
                            TimeSearch = searchTime,
                            DataReader = dataReader,
                            DBConnection = conn
                        });
#if DEBUG
                        WriteLog(
                            "Search: " + searchResultKey.Replace("&", "&&"),
                            "Execute time",
                            string.Format("{0:#,0.000} second(s)", (DateTime.Now - startTime).TotalSeconds),
                            "y"
                        );
#endif
                    }
                }
                catch (FaultException)
                {
                    try
                    {
                        conn.Dispose();
                        comm.Dispose();
                    }
                    catch
                    {
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    try
                    {
                        conn.Dispose();
                        comm.Dispose();
                    }
                    catch
                    {
                    }
                    throw ErrorUtils.CreateErrorWithSubMessage(
                        ERR_SYSTEM.ERR_SYSTEM_EXECUTE_SEARCH_FAIL, ex.Message);
                }
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void ExecuteSearchEdit(string moduleID, string subModule, List<string> values)
        {
            try
            {
                var searchModuleInfo = (SearchModuleInfo)ModuleUtils.GetModuleInfo(moduleID, subModule);
                CheckRole(searchModuleInfo);

                OracleHelper.ExecuteStoreProcedure(ConnectionString, Session, searchModuleInfo.EditStore, values.ToArray());
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        private void BuildStaticConditions(SearchModuleInfo searchInfo, OracleCommand comm, List<SearchConditionInstance> staticConditionInstances)
        {
            var fields = FieldUtils.GetModuleFields(searchInfo.ModuleID, CODES.DEFMODFLD.FLDGROUP.PARAMETER);

            foreach (var field in fields)
            {
                switch (field.FieldName)
                {
                    case CONSTANTS.ORACLE_SESSION_USER:
                        comm.Parameters.Add(":" + field.ParameterName, Session.Username);
                        break;
                    case CONSTANTS.ORACLE_CURSOR_OUTPUT:
                        comm.Parameters.Add(new OracleParameter(":" + field.ParameterName, OracleDbType.RefCursor))
                            .Direction = ParameterDirection.Output;
                        break;
                }
            }

            fields = FieldUtils.GetModuleFields(searchInfo.ModuleID, CODES.DEFMODFLD.FLDGROUP.COMMON);
            foreach (var condition in staticConditionInstances)
            {
                foreach (var field in fields)
                {
                    if (field.FieldID == condition.ConditionID)
                    {
                        if (string.IsNullOrEmpty(condition.Value))
                            comm.Parameters.Add(":" + field.ParameterName, DBNull.Value);
                        else
                            comm.Parameters.Add(":" + field.ParameterName, condition.Value.Decode(field));
                    }
                }
            }
        }

        private void _ListUserRoles(out List<Role> roles, int userID)
        {
            try
            {
                //roles = OracleHelper.ExecuteStoreProcedure<Role>(ConnectionString, null, SYSTEM_STORE_PROCEDURES.LIST_USER_ROLE, userID);
                if (userID == Session.UserID)
                {
                    roles = OracleHelper.ExecuteStoreProcedure<Role>(ConnectionString, null, SYSTEM_STORE_PROCEDURES.LIST_USER_ROLE, userID);
                }
                else
                {
                    roles = OracleHelper.ExecuteStoreProcedure<Role>(ConnectionString, null,
                        SYSTEM_STORE_PROCEDURES.LIST_FUNCTION_USER_ROLE, userID, Session.UserID);
                }
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void ListGroupRoles(out List<Role> roles, int groupID)
        {
            try
            {
                var moduleInfo = ModuleUtils.GetModuleInfo(STATICMODULE.GROUP_ROLE_MODULE, CODES.DEFMOD.SUBMOD.MODULE_MAIN);
                CheckRole(moduleInfo);
                roles = OracleHelper.ExecuteStoreProcedure<Role>(ConnectionString, null,
                    SYSTEM_STORE_PROCEDURES.LIST_GROUP_ROLE, groupID, Session.UserID);
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void ListUserRoles(out List<Role> roles, int userID)
        {
            try
            {
                var moduleInfo = ModuleUtils.GetModuleInfo(STATICMODULE.USER_ROLE_MODULE, CODES.DEFMOD.SUBMOD.MODULE_MAIN);
                CheckRole(moduleInfo);
                _ListUserRoles(out roles, userID);
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void SaveGroupRoles(List<Role> roles, int groupID)
        {
            try
            {
                var moduleInfo = ModuleUtils.GetModuleInfo(STATICMODULE.USER_ROLE_MODULE, CODES.DEFMOD.SUBMOD.MODULE_MAIN);
                CheckRole(moduleInfo);

                OracleHelper.ExecuteStoreProcedure<Role>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.DELETE_GROUP_ROLE, groupID);
                foreach (var role in roles)
                {
                    if (role.RoleValue == "Y")
                        OracleHelper.ExecuteStoreProcedure<Role>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.INSERT_GROUP_ROLE, groupID, role.RoleID);
                }
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void SaveUserRoles(List<Role> roles, int userID)
        {
            try
            {
                var moduleInfo = ModuleUtils.GetModuleInfo(STATICMODULE.USER_ROLE_MODULE, CODES.DEFMOD.SUBMOD.MODULE_MAIN);
                CheckRole(moduleInfo);

                OracleHelper.ExecuteStoreProcedure<Role>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.DELETE_USER_ROLE, userID);
                foreach (var role in roles)
                {
#if DEBUG
                    OracleHelper.ExecuteStoreProcedure<Role>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.DEFROLE_UDP_PARENT, role.RoleID, role.CategoryID);
#endif
                    if (role.RoleValue == "Y")
                        OracleHelper.ExecuteStoreProcedure<Role>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.INSERT_USER_ROLE, userID, role.RoleID);
                }
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void ExecuteAlert(out DataContainer alertResult, string moduleID, string subModule)
        {
            try
            {
                var alertInfo = (AlertModuleInfo)ModuleUtils.GetModuleInfo(moduleID, subModule);
                CheckRole(alertInfo);
                DataTable tblAlert;
                //
                OracleHelper.FillDataTable(ConnectionString, Session, alertInfo.AlertStore, out tblAlert);
                //
                alertResult = new DataContainer { DataTable = tblAlert };
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void ExecuteBatch(string moduleID, string batchName)
        {
            try
            {
                var moduleInfo = ModuleUtils.GetModuleInfo(moduleID, CODES.DEFMOD.SUBMOD.MODULE_MAIN);
                CheckRole(moduleInfo);
                var batchInfo = OracleHelper.ExecuteStoreProcedure<BatchInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.LIST_BATCHINFO_BY_NAME, moduleID, batchName)[0];
                OracleHelper.ExecuteStoreProcedure(ConnectionString, Session, batchInfo.BatchStore, batchInfo.BatchName);
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        //add by trungtt - 1.6.2011 - batch dynamic market
        [OperationContract]
        public void ExecuteBatchMarket(string moduleID, string batchName, string market)
        {
            try
            {
                var moduleInfo = ModuleUtils.GetModuleInfo(moduleID, CODES.DEFMOD.SUBMOD.MODULE_MAIN);
                CheckRole(moduleInfo);
                var batchInfo = OracleHelper.ExecuteStoreProcedure<BatchInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.LIST_BATCHINFO_BY_NAME, moduleID, batchName)[0];
                OracleHelper.ExecuteStoreProcedure(ConnectionString, Session, batchInfo.BatchStore, market);
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }
        //end trungtt

        //add by TrungTT - 9.11.2011 - Batch put msg
        [OperationContract]
        public void ExecuteBatchPutMsg(out DataContainer container, string moduleID, string storeData, List<string> values)
        {
            try
            {
                var moduleInfo = ModuleUtils.GetModuleInfo(moduleID, CODES.DEFMOD.SUBMOD.MODULE_MAIN);
                CheckRole(moduleInfo);
                container = null;
                DataSet ds;
                OracleHelper.FillDataSet(ConnectionString, Session, moduleID, storeData, out ds, values.ToArray());
                if (ds.Tables.Count == 1)
                {
                    container = new DataContainer() { DataTable = ds.Tables[0] };
                }
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }
        //end TrungTT

        [OperationContract]
        public void ExecuteAlertClick(string moduleID, string subModule)
        {
            try
            {
                var alertInfo = (AlertModuleInfo)ModuleUtils.GetModuleInfo(moduleID, subModule);
                CheckRole(alertInfo);
                //
                OracleHelper.ExecuteStoreProcedure(ConnectionString, Session, alertInfo.ClickStore);
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void GetListSource(out List<NameValueItem> listSource, string moduleID, string subModule, string fieldID, List<string> values)
        {
            try
            {
                var moduleInfo = ModuleUtils.GetModuleInfo(moduleID, subModule);
                CheckRole(moduleInfo);

                var sql =
                    FieldUtils.GetModuleFieldByModule(
                        moduleInfo,
                        fieldID
                    ).ListSource;

                var match = Regex.Match(sql, "[^\\(]+");

                listSource = OracleHelper.ExecuteStoreProcedure<NameValueItem>(ConnectionString, Session, match.Groups[0].Value.Trim(), values.ToArray());
                //TUDQ them
                if ((match.Groups[0].Value.Trim().ToLower() == "sp_list_securities_by_code" && moduleID.Substring(0, 2) != CODES.DEFMOD.MODTYPE.SEARCHMASTER)
                    || (match.Groups[0].Value.Trim().ToLower() == "sp_list_sec_by_shortname" && moduleID.Substring(0, 2) != CODES.DEFMOD.MODTYPE.SEARCHMASTER))
                {
                    DataTable tableRole = null;
                    List<string> values2 = new List<string>();
                    values2.Add(Session.UserID.ToString());
                    OracleHelper.FillDataTable(ConnectionString, Session, "sp_DATAROLE_selbyid", out tableRole, values2.ToArray());
                    DataTable dtReturn = new DataTable();
                    List<NameValueItem> listSourceNew = new List<NameValueItem>();
                    //listSourceNew.Clear();
                    for (int j = 0; j < listSource.Count; j++)
                    {
                        bool flag = true;
                        for (int i = 0; i < tableRole.Rows.Count; i++)
                        {
                            if (tableRole.Rows[i]["SECID"].ToString() == listSource[j].Value.ToString() || tableRole.Rows[i]["SHORTNAME"].ToString() == listSource[j].Value.ToString())
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            listSourceNew.Add(listSource[j]);
                        }
                    }
                    listSource = listSourceNew;
                }

                //
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void SaveLayout(string moduleID, string subModule, string languageID, string layout)
        {
#if DEBUG
            try
            {
                OracleHelper.ExecuteStoreProcedure(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.UPDATE_LAYOUT,
                    moduleID, subModule, languageID, layout);
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
#endif
        }
        //[OperationContract]
        //public void SaveFile(string filename, Byte[] file)
        //{
        //    try
        //    {
        //        OracleHelper.ExecuteStoreProcedure(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.SAVE_FILE, filename, file);
        //    }
        //    catch (FaultException)
        //    {
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ErrorUtils.CreateError(ex);
        //    }
        //}

        [OperationContract]
        public void SaveFile(FileUpload file)
        {
            try
            {
                //File.AppendAllText("LastErrors.log", string.Format("{0}\r\n-------------\r\n", DateTime.Now.ToLongTimeString()));                     
                byte[] filedata = ReadFully(file.UploadStream);
                if (file.SecID > 0)
                {
                    SaveFileToDisk(file, filedata);
                }                
                OracleHelper.ExecuteStoreProcedure(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.SAVE_FILE, file.FileName, filedata,Session.SessionKey);
                //File.AppendAllText("LastErrors.log", string.Format("{0}\r\n-------------\r\n", DateTime.Now.ToLongTimeString()));                
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        private byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];            
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return  ms.ToArray();            
            }            
        }       
        private void SaveFileToDisk(FileUpload file,byte[] filedata)
        {
            try
            {
                List<string> values = new List<string>();
                values.Add(SYSVAR.GRNAME_SYS);
                values.Add(SYSVAR.VARNAME_RPTEXCELFILEPATH);
                DataContainer container = new DataContainer();
                ExecuteProcedureFillDataset(out container, "sp_sysvar_sel_bygrame", values);
                DataTable dt = container.DataSet.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    string _subPath = dt.Rows[0]["VARVALUE"].ToString();
                    //Tao thu muc cau truc SecID\Nam
                    _subPath = _subPath + "\\" + file.SecID + "\\" + file.RYear;
                    bool isExists = System.IO.Directory.Exists(_subPath);
                    if (!isExists)
                        System.IO.Directory.CreateDirectory(_subPath);
                    
                    string _FileName = _subPath + "\\" + file.FileName;
                    File.WriteAllBytes(_FileName, filedata);
                    OracleHelper.ExecuteStoreProcedure(ConnectionString, Session, "sp_reportfilelogs_ins", file.SecID, file.RptID, file.Term, file.TermNo, file.RYear,_FileName);
                }                
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        //trungtt - 20.12.2013 - sign file
        [OperationContract]
        public void SignFile(string filename, string certificateinfo, string worker, Byte[] file)
        {
            try
            {
                ClientWSService ws = new ClientWSService(certificateinfo);
                processData request = new processData();
                request.worker = worker.ToUpper();
                request.data = file;
                processDataResponse response = ws.processData(request);                
                OracleHelper.ExecuteStoreProcedure(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.SAVE_FILE, filename, response.@return.data);
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }
        //end trungtt

        [OperationContract]
        public void ExecuteSQL(out DataContainer container, string sqlQuery)
        {
#if DEBUG
            try
            {
                using (var conn = new OracleConnection(ConnectionString))
                {
                    conn.Open();
                    var comm = new OracleCommand(sqlQuery, conn);
                    comm.Parameters.Add("cur", OracleDbType.RefCursor, ParameterDirection.Output);
                    var adap = new OracleDataAdapter(comm);
                    var ds = new DataSet();
                    adap.Fill(ds);
                    if (ds.Tables.Count == 0)
                    {
                        container = null;
                    }
                    else
                    {
                        container = new DataContainer
                        {
                            DataTable = ds.Tables[0]
                        };
                    }
                }
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
#else
            throw ErrorUtils.CreateError(ERR_SYSTEM.ERR_SYSTEM_FUNCTION_ONLY_AVAILABLE_IN_DEBUG_MODE);
#endif
        }

        [OperationContract]
        public void ExecuteStoreProcedure(string storeProcedure, List<string> values)
        {
            try
            {
                OracleHelper.ExecuteStoreProcedure(ConnectionString, Session, storeProcedure, values.ToArray());
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void ResetCache()
        {
            try
            {
                var type = App.Environment.GetType();
                OracleHelper.m_CachedParameters.Clear();
                App.Environment = (AbstractEnvironment)type.GetConstructor(new Type[] { }).Invoke(new object[] { });
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void CheckUpdate(string clientVersion, out string fileContent)
        {
            try
            {
                var serverVersion = CommonUtils.MD5File(App.Configs.UpdatedToDateVersion);
                if (serverVersion != clientVersion)
                {
                    using (var f = File.OpenRead(App.Configs.UpdatedToDateVersion))
                    {
                        var buffer = new byte[f.Length];
                        f.Read(buffer, 0, buffer.Length);
                        fileContent = CommonUtils.EncodeTo64(buffer);
                    }
                    return;
                }

                fileContent = null;
            }
            catch
            {
                fileContent = null;
            }
        }

        [OperationContract, DebuggerStepThrough]
        public void NewsInfo(out List<NewsInfo> newsInfo)
        {
            try
            {
                newsInfo = OracleHelper.ExecuteStoreProcedure<NewsInfo>(ConnectionString, Session, "SP_NEWS_SEL");
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void GetCurrentUserProfile(out UserProfile userProfile)
        {
            try
            {
                userProfile = null;
                if (Session != null)
                {
                    userProfile = OracleHelper.ExecuteStoreProcedure<UserProfile>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.PROFILES_SEL, Session.Username)[0];
                }
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void ListUsersInGroup(out List<User> users, int groupID)
        {
            try
            {
                users = OracleHelper.ExecuteStoreProcedure<User>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.GROUP_LIST_USERS, groupID);
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

//#if DEBUG
        [OperationContract]
        public void ForceLoadModule(
            out List<ModuleInfo> modulesInfo,
            out List<ModuleFieldInfo> fieldsInfo,
            out List<ButtonInfo> buttonsInfo,
            out List<ButtonParamInfo> buttonParamsInfo,
            out List<LanguageInfo> languageInfo,
            out List<OracleParam> oracleParamsInfo,
            string moduleID)
        {
            try
            {
                modulesInfo = new List<ModuleInfo>();
                modulesInfo.AddRange(OracleHelper.ExecuteStoreProcedure<ModuleInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.GET_STATIC_MODULE, moduleID).ToArray());
                modulesInfo.AddRange(OracleHelper.ExecuteStoreProcedure<ModuleInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.GET_BATCH_MODULE, moduleID).ToArray());
                modulesInfo.AddRange(OracleHelper.ExecuteStoreProcedure<StatisticsModuleInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.GET_STATISTICS_MODULE, moduleID).ToArray());
                modulesInfo.AddRange(OracleHelper.ExecuteStoreProcedure<MaintainModuleInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.GET_MAINTAIN_MODULE, moduleID).ToArray());
                modulesInfo.AddRange(OracleHelper.ExecuteStoreProcedure<ReportModuleInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.GET_REPORT_MODULE, moduleID).ToArray());
                modulesInfo.AddRange(OracleHelper.ExecuteStoreProcedure<ChartModuleInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.GET_CHART_MODULE, moduleID).ToArray());
                modulesInfo.AddRange(OracleHelper.ExecuteStoreProcedure<SearchModuleInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.GET_SEARCHMASTER_MODULE, moduleID).ToArray());
                modulesInfo.AddRange(OracleHelper.ExecuteStoreProcedure<SwitchModuleInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.GET_SWITCH_MODULE, moduleID).ToArray());
                modulesInfo.AddRange(OracleHelper.ExecuteStoreProcedure<ImportModuleInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.GET_IMPORT_MODULE, moduleID).ToArray());
                modulesInfo.AddRange(OracleHelper.ExecuteStoreProcedure<ExecProcModuleInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.GET_EXECUTEPROC_MODULE, moduleID).ToArray());
                modulesInfo.AddRange(OracleHelper.ExecuteStoreProcedure<AlertModuleInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.GET_ALERT_MODULE, moduleID).ToArray());
                //TUDQ
                modulesInfo.AddRange(OracleHelper.ExecuteStoreProcedure<ModuleInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.GET_TREE_MODULE, moduleID).ToArray());
                modulesInfo.AddRange(OracleHelper.ExecuteStoreProcedure<ModuleInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.GET_EXPRESSION_MODULE, moduleID).ToArray());
                //END
                fieldsInfo = OracleHelper.ExecuteStoreProcedure<ModuleFieldInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.LIST_FIELD_INFO_BY_MODID, moduleID);
                buttonsInfo = OracleHelper.ExecuteStoreProcedure<ButtonInfo>(ConnectionString, null, SYSTEM_STORE_PROCEDURES.LIST_BUTTON_BY_MODID, moduleID);
                buttonParamsInfo = OracleHelper.ExecuteStoreProcedure<ButtonParamInfo>(ConnectionString, null, SYSTEM_STORE_PROCEDURES.LIST_BUTTON_PARAM_BY_MODID, moduleID);
                languageInfo = OracleHelper.ExecuteStoreProcedure<LanguageInfo>(ConnectionString, null, SYSTEM_STORE_PROCEDURES.LIST_LANGUAGE_BY_MODID, moduleID);

                var stores = OracleHelper.ExecuteStoreProcedure<OracleStore>(ConnectionString, null, SYSTEM_STORE_PROCEDURES.LIST_STOREPROC_BY_MODID, moduleID);
                oracleParamsInfo = new List<OracleParam>();

                foreach (var store in stores)
                {
                    try
                    {
                        OracleHelper.m_CachedParameters.Remove(store.StoreName);
                        OracleHelper.DiscoveryParameters(ConnectionString, store.StoreName, oracleParamsInfo);
                    }
                    catch
                    {
                    }
                }

                ModuleUtils.ForceLoad(moduleID,
                    modulesInfo,
                    fieldsInfo,
                    buttonsInfo,
                    buttonParamsInfo,
                    languageInfo,
                    oracleParamsInfo);
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }
//#endif

        [OperationContract]
        public void ExecuteStatistics(out string searchResultKey, out DateTime searchTime, string moduleID, string subModule, List<string> values)
        {
            try
            {
                var statisticsInfo = (StatisticsModuleInfo)ModuleUtils.GetModuleInfo(moduleID, subModule);
                CheckRole(statisticsInfo);

                var conn = new OracleConnection(ConnectionString);
                var oracleParams = ModuleUtils.GetOracleParams(statisticsInfo.StoreName);

                searchResultKey = statisticsInfo.ModuleID + "?";
                for (var i = 0; i < values.Count; i++)
                {
                    searchResultKey += oracleParams[i].Name + "=" + values[i] + "&";
                }
#if DEBUG
                var startTime = DateTime.Now;
#endif
                searchTime = DateTime.Now;
                if (statisticsInfo.PageMode == CODES.MODSEARCH.PAGEMODE.PAGE_FROM_DATASET)
                {
                    DataTable table;
#if DEBUG
                    WriteLog(
                        "Statistic: " + searchResultKey.Replace("&", "&&"),
                        "Execute time",
                        string.Format("{0:#,0.000} second(s)", (DateTime.Now - startTime).TotalSeconds),
                        "y"
                    );
#endif
                    OracleHelper.FillDataTable(ConnectionString, Session, statisticsInfo.StoreName, out table, values.ToArray());
                    CachedSearchResult.Add(new SearchResult
                    {
                        SessionKey = Session.SessionKey,
                        SearchKey = searchResultKey,
                        TimeSearch = searchTime,
                        CachedResult = table
                    });
                }
                else
                {
                    var dataReaders = OracleHelper.ExecuteReader(ConnectionString, Session, statisticsInfo.StoreName, values.ToArray());
                    if (dataReaders.Length == 2)
                        CachedSearchResult.Add(new SearchResult
                        {
                            SessionKey = Session.SessionKey,
                            SearchKey = searchResultKey,
                            TimeSearch = searchTime,
                            DataReader = dataReaders[0],
                            DataReader2 = dataReaders[1],
                            DBConnection = conn
                        });
                    else
                        CachedSearchResult.Add(new SearchResult
                        {
                            SessionKey = Session.SessionKey,
                            SearchKey = searchResultKey,
                            TimeSearch = searchTime,
                            DataReader = dataReaders[0],
                            DBConnection = conn
                        });
#if DEBUG
                    WriteLog(
                        "Search: " + searchResultKey.Replace("&", "&&"),
                        "Execute time",
                        string.Format("{0:#,0.000} second(s)", (DateTime.Now - startTime).TotalSeconds),
                        "y"
                    );
#endif
                }
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }
        //TuDQ sua
        [OperationContract]
        public void ExecuteChartQuery(out DataSet chartResult, string moduleID, string subModule, List<string> values)
        {
            //try
            //{
            //    var chartInfo = (ChartModuleInfo)ModuleUtils.GetModuleInfo(moduleID, subModule);
            //    CheckRole(chartInfo);

            //    DataTable table;
            //    OracleHelper.FillDataTable(ConnectionString, Session, chartInfo.ChartStore, out table, values.ToArray());
            //    chartResult = new DataTableContainer {Table = table};
            //}
            //catch (FaultException)
            //{
            //    throw;
            //}
            //catch (Exception ex)
            //{
            //    throw ErrorUtils.CreateError(ex);
            //}
            try
            {
                var chartInfo = (ChartModuleInfo)ModuleUtils.GetModuleInfo(moduleID, subModule);
                CheckRole(chartInfo);

                DataSet ds;
                OracleHelper.FillDataSet(ConnectionString, Session, chartInfo.ChartDataStore, out ds, values.ToArray());
                chartResult = ds;
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }
        [OperationContract]
        public void GetChartInf(out DataSet chartResult, List<string> values)
        {
            try
            {
                DataSet ds;
                OracleHelper.FillDataSet(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.GET_CHART_INF, out ds, values.ToArray());
                chartResult = ds;
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }
        //End

        //add by TrungTT - 27.12.2012 - Userlog
        [OperationContract]
        public void ExecuteUsersLog(string moduleID, string moduleName)
        {
            try
            {
                OracleHelper.ExecuteStoreProcedure(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.USERSLOG_INSERT, Session.Username, moduleID, moduleName, DateTime.Now);
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }
        //End TrungTT

        //Tudq them
        [OperationContract]
        public void GetTreeViewStore(out DataContainer executeResult, List<string> values)
        {
            try
            {

                DataTable table = null;
                OracleHelper.FillDataTable(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.GETMODULE_TREE, out table, values.ToArray());
                executeResult = new DataContainer { DataTable = table };
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void GetTreeViewLang(out DataContainer executeResult, List<string> values)
        {
            try
            {

                DataTable table = null;
                OracleHelper.FillDataTable(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.GETMODULE_TREELANG, out table, values.ToArray());
                executeResult = new DataContainer { DataTable = table };
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }
        //End


        //TUDQ them
        [OperationContract]
        public DataTable CheckDataRole(DataTable dt)
        {
            DataTable tableRole = null;
            List<string> values = new List<string>();
            values.Add(Session.UserID.ToString());
            OracleHelper.FillDataTable(ConnectionString, Session, "sp_DATAROLE_selbyid", out tableRole, values.ToArray());
            DataTable dtReturn = new DataTable();
            bool flag = false;
            bool flagRownum = false;
            bool flagStt = false;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName.ToUpper() == "SECID")
                {
                    flag = true;
                }
                if (dt.Columns[i].ColumnName.ToUpper() == "STT")
                {
                    flagStt = true;
                }
                if (dt.Columns[i].ColumnName.ToUpper() == "ROWNUM")
                {
                    flagRownum = true;
                }
            }
            // Loc du lieu theo secid
            if (flag)
            {
                dtReturn = dt.Clone();
                int rowNum = 0;
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    for (int k = 0; k < tableRole.Rows.Count; k++)
                    {
                        if (dt.Rows[j]["SECID"].ToString() == tableRole.Rows[k]["SECID"].ToString())
                        {
                            if (flagStt == true)
                            {
                                dt.Rows[j]["STT"] = rowNum + 1;
                                rowNum++;
                            }
                            if (flagRownum == true)
                            {
                                dt.Rows[j]["ROWNUM"] = rowNum + 1;
                                rowNum++;
                            }
                            dtReturn.ImportRow(dt.Rows[j]);
                            break;
                        }

                    }
                }
            }
            else
            {
            // Bo sung stt vao ket qua
                dtReturn = dt.Clone();
                int rowNum = 0;
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    for (int k = 0; k < tableRole.Rows.Count; k++)
                    {
                           if (flagStt == true)
                            {
                                dt.Rows[j]["STT"] = rowNum + 1;
                                rowNum++;
                            }                            
                            dtReturn.ImportRow(dt.Rows[j]);
                            break;                        
                    }
                }
                dtReturn = dt;
            }
            return dtReturn;
        }

        [OperationContract]
        public DataSet CheckDataRole1(DataSet ds)
        {
            DataTable tableRole = null;
            List<string> values = new List<string>();
            values.Add(Session.UserID.ToString());
            OracleHelper.FillDataTable(ConnectionString, Session, "sp_DATAROLE_selbyid", out tableRole, values.ToArray());
            DataSet dsResult = ds.Clone();
            bool flag = false;
            for (int count = 0; count < ds.Tables.Count; count++)
            {
                DataTable dt = ds.Tables[count];
                DataTable dtReturn = new DataTable();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName.ToUpper() == "SECID")
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    dtReturn = dt.Clone();
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        for (int k = 0; k < tableRole.Rows.Count; k++)
                        {
                            if (dt.Rows[j]["SECID"].ToString() == tableRole.Rows[k]["SECID"].ToString())
                            {
                                dtReturn.ImportRow(dt.Rows[j]);
                                break;
                            }
                        }
                    }
                    dsResult.Tables.Add(dtReturn);
                }
                else dsResult.Tables.Add(dt);
            }
            return dsResult;
        }

        //END
        public List<GroupSummaryInfo> BuildGroupSummaryInfo()
        {
            try
            {
                return OracleHelper.ExecuteStoreProcedure<GroupSummaryInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.LIST_GROUP_SUMMARY_INFO);
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        public List<ExportHeader> BuildExportHeaderInfo()
        {
            try
            {
                return OracleHelper.ExecuteStoreProcedure<ExportHeader>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.LIST_HEADER_EXPORT_INFO);
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }        
        }

        public List<SysvarInfo> BuildSysvarInfo()
        {
            try
            {
                return OracleHelper.ExecuteStoreProcedure<SysvarInfo>(ConnectionString, Session, SYSTEM_STORE_PROCEDURES.LIST_SYSVAR_INFO);
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        [OperationContract]
        public void ListGroupSummaryInfo(out List<GroupSummaryInfo> groupSummaryInfos)
        {
            groupSummaryInfos = AllCaches.GroupSummaryInfos;
        }

        [OperationContract]
        public void ListExportHeaderInfo(out List<ExportHeader> ExportHeaderInfos)
        {
            ExportHeaderInfos = AllCaches.ExportHeaders;
        }

        [OperationContract]
        public void ListSysvarInfo(out List<SysvarInfo> SysvarInfos)
        {
            SysvarInfos = AllCaches.SysvarsInfo;
        }

        [OperationContract]
        public void GetModImport(out DataSet executeResult, string rptID)
        {
            try
            {

                DataSet ds = null;
                OracleHelper.FillDataSet(ConnectionString, Session, "sp_modimport_sel", out ds, rptID);
                executeResult = ds;
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }
        //End

        // Gui mail sao ke tai khoan cho khach hang
        private void SendEmailSaoke(XtraReport report,string Subject,string Content,string client,string email,string payment_date,string statement_date,string contractno)
        {
            List<string> log = new List<string>();
            try
            {                
                MailMessage mail = new MailMessage();
                ConfigMail(mail);
                
                if (EmailIsValid(email))
                {                    
                    MemoryStream mem = new MemoryStream();
                    mem.SetLength(0);
                    report.ExportToPdf(mem);                    
                    mem.Seek(0, System.IO.SeekOrigin.Begin);                    
                    Attachment att = new Attachment(mem, "Statement_" + client + "_" + contractno +".pdf", "application/pdf");
                    mail.Attachments.Add(att);
                    mail.To.Add(email);
                    mail.Subject = string.Format(Subject,statement_date);
                    mail.Body = string.Format(Content, client, statement_date, payment_date);
                    mail.IsBodyHtml = true;
                    smtp.Send(mail);
                    log.Add("02248");
                    log.Add(string.Format("Đã gửi email tới địa chỉ {0} - {1} - {2}", email, contractno, client));
                    log.Add(statement_date);
                    WriteMailLogs(log);
                    mem.Close();
                }                                       
            }
            catch (Exception ex)
            {
                log.Add("02248");
                log.Add(string.Format("Gửi email thất bại tới địa chỉ {0}: Lỗi {1}", email,ex.Message.ToString()));
                log.Add(statement_date);
                WriteMailLogs(log);                
            }
        }
        // Send email thong bao       
        private void SendMail(string Modid, string SubMod)
        {
            try
            {
                MailMessage mail = new MailMessage();
                ConfigMail(mail);
                using (SAController ctrlSA = new SAController())
                {
                    List<string> values = new List<string>();
                    DataTable dtEmail = new DataTable();
                    DataTable dtContent = new DataTable();
                    string content = String.Empty ,subject = String.Empty;
                    DataContainer container;
                    values.Add(Modid);
                    // Get content
                    ctrlSA.ExecuteProcedureFillDataset(out container, "sp_mailrouter_sel", values);
                    dtContent = container.DataSet.Tables[0];
                    if (dtContent.Rows.Count != 0)
                    {
                        content = dtContent.Rows[0]["content"].ToString();
                        subject = dtContent.Rows[0]["subject"].ToString();
                    }

                    ctrlSA.ExecuteProcedureFillDataset(out container, "sp_list_email", values);
                    dtEmail = container.DataSet.Tables[0];
                    if (dtEmail.Rows.Count != 0)
                    {
                        for (int icount = 0; icount < dtEmail.Rows.Count; icount++)
                        {
                            if (EmailIsValid(dtEmail.Rows[icount]["email"].ToString()))
                            {
                                mail.To.Add(new MailAddress(dtEmail.Rows[icount]["email"].ToString(), dtEmail.Rows[icount]["displayname"].ToString()));
                                mail.Subject = subject;
                                mail.Body = content;
                                try
                                {
                                    smtp.Send(mail);
                                }
                                catch(Exception ex) {
                                    List<string> log = new List<string>();
                                    log.Add(Modid);
                                    log.Add(ex.Message.ToString());
                                    WriteMailLogs(log);
                                }                                
                            }
                        }
                    }
                }
            }
            catch (FaultException)
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ConfigMail(MailMessage mail)
        {
            string strMailServer, strMailUser, strMailPass;
            int iMailPort;
            DataTable dt = new DataTable();           
            DataContainer container;
            List<string> values = new List<string>();
            
            ExecuteProcedureFillDataset(out container, "sp_sysvar_list", values);
            dt = container.DataSet.Tables[0];
            if (dt.Rows.Count != 0)
            {
                DataRow row = dt.Rows[0];
                strMailServer = row["MAIL_SERVER"].ToString();
                strMailUser = row["MAIL_USER"].ToString();
                strMailPass = row["MAIL_PASSWORD"].ToString();
                iMailPort = int.Parse(row["MAIL_PORT"].ToString());               

                smtp = new SmtpClient(strMailServer);
                smtp.Port = iMailPort;
                smtp.Credentials = new System.Net.NetworkCredential(strMailUser, strMailPass);
                smtp.EnableSsl = false;
                mail.From = new MailAddress(strMailUser);
            }
            
        }

        private void WriteMailLogs(List<string> values)
        {
            OracleHelper.ExecuteStoreProcedure(ConnectionString, Session, "sp_emaillogs_add", values.ToArray());    
        }
        private static Regex CreateValidEmailRegex()
        {
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
        }
        internal static bool EmailIsValid(string emailAddress)
        {
            bool isValid = ValidEmailRegex.IsMatch(emailAddress);
            return isValid;
        }
        //add by trungtt - 10.9.2014 - send mail by module maintance
        private void SendMailByModuleMaintaince(List<string> values)
        {
            DataContainer container = null;
            string strListAddressTo = values[0];
            string[] strArrayAddressTo = strListAddressTo.Split(',');
            string strSubject = values[1];
            string strBody = values[2];
            //--Mail config
            DataSet dsResult2;
            values = new List<string>();
            ExecuteProcedureFillDataset(out container, "SP_MAILCONFIG_SEL", values);
            dsResult2 = container.DataSet;

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(dsResult2.Tables[0].Rows[0]["MAIL_ID"].ToString());
            mail.From = new MailAddress(dsResult2.Tables[0].Rows[0]["MAIL_NAME"].ToString());
            SmtpServer.Port = Int32.Parse(dsResult2.Tables[0].Rows[0]["PORT"].ToString());
            SmtpServer.Credentials = new System.Net.NetworkCredential(dsResult2.Tables[0].Rows[0]["MAIL_NAME"].ToString(), dsResult2.Tables[0].Rows[0]["PASSWORD"].ToString());                                    
            SmtpServer.EnableSsl = false;
            
            //--Send mail
            foreach (string item in strArrayAddressTo)
            {
                //--Subject and Body 
                mail.To.Add(item);
                mail.BodyEncoding = System.Text.Encoding.UTF8;                
                mail.Subject = strSubject;                
                mail.Body = strBody;
                mail.Priority = MailPriority.High;
                mail.IsBodyHtml = true;
                //--attachment
                List<string> listparam = new List<string>();
                listparam.Add(null);
                ExecuteProcedureFillDataset(out container, "SP_FILE_ATTACHMENT", listparam);                
                var resultTable = container.DataTable;
                if (resultTable.Rows.Count > 0)
                {
                    for (int i = 0; i <= resultTable.Rows.Count - 1; i++)
                    {
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream((Byte[])resultTable.Rows[i]["filestore"]))
                        {
                            mail.Attachments.Add(new Attachment(ms, resultTable.Rows[i]["filename"].ToString()));
                            try
                            {
                                SmtpServer.Send(mail);
                            }
                            catch (Exception ex)
                            {
                                //ShowError(ex);
                                throw ErrorUtils.CreateErrorWithSubMessage(148, "Gửi thư điện tử với file đính kèm thất bại !");
                            }
                            ms.Close();
                        }
                    }
                }                
                else
                {
                    try
                    {                      
                        SmtpServer.Send(mail);
                    }
                    catch (Exception ex)
                    {
                        //ShowError(ex);
                        throw ErrorUtils.CreateErrorWithSubMessage(148, "Gửi thư điện tử thất bại !");
                    }
                }
            }
            var valuesDeleteFile = new List<string>();
            ExecuteStoreProcedure("SP_DELETE_FILE_ATTACHMENT", valuesDeleteFile);
        }
        //end trungtt        
    }
}