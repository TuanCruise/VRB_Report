using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit;
using FIS.AppClient.Interface;
using FIS.AppClient.Utils;
using FIS.Base;
using FIS.Common;
using FIS.Controllers;
using FIS.Entities;
using FIS.Extensions;
using FIS.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;


namespace FIS.AppClient.Controls
{
    public partial class ucMaintain : ucModule,
        IParameterFieldSupportedModule,
        ICommonFieldSupportedModule
    {
        #region ICommonFieldSupportedModule Members
        
        public bool ValidateRequire
        {
            get { return true; }
        }

        public LayoutControl CommonLayout
        {
            get { return mainLayout; }
        }
        
        public string CommonLayoutStoredData
        {
            get
            {
                switch (ModuleInfo.SubModule)
                {
                    case CODES.DEFMOD.SUBMOD.MAINTAIN_ADD:
                        return Language.AddLayout;
                    case CODES.DEFMOD.SUBMOD.MAINTAIN_EDIT:
                        return Language.EditLayout;
                    case CODES.DEFMOD.SUBMOD.MAINTAIN_VIEW:
                        return Language.ViewLayout;
                    //TUDQ them
                    case CODES.DEFMOD.SUBMOD.TRANSACTION_VIEW:
                        return Language.ViewLayout;
                    //
                    default:
                        return null;
                }
            }
        }
        #endregion

        #region Properties & Members
        //add by TrungTT - 07.12.2011 - add RichEdit for Load Report
        RichEditControl richEdit;
        string strRPT_ACTION = null;
        string filetempname = null;
        //end TrungTT
        //add by TrungTT - 4.01.2012 - SwapDataReport
        WebBrowser webBrowser1 = new WebBrowser();
        WebBrowser webBrowser2 = new WebBrowser();
        WebBrowser webBrowser3 = new WebBrowser();
        private ContextMenuStrip lnkFileContextMenu;

        public MaintainModuleInfo MaintainInfo
        {
            get
            {
                return (MaintainModuleInfo)ModuleInfo;
            }
        }

        #endregion

        #region Override methods
        protected override void BuildFields()
        {
            //add by TrungTT - 07.12.2011 - add RichEdit for Load Report
            var fldDetailColumn = MaintainInfo.Report;
            if (fldDetailColumn == "T" || fldDetailColumn == "Y")
            {
                btnLoadReport.Visible = true;
                btnLoadReport.Text = "Tạo tin";

                //btnSave.Visible = true;
                //btnSave.Text = "Lưu bản tạm";

                btnCommit.Text = "Gửi tin";

                richEdit = new RichEditControl
                {
                    Name = "rtxLoadReport"
                };
                
                mainLayout.Controls.Add(richEdit);

                if (CommonLayoutStoredData == null)
                {
                    mainLayout.AddItem(fldDetailColumn, richEdit);
                }
            }
            
            if (fldDetailColumn == "T")
            {
                btnGetData.Text = "Tải BCTC";
                btnGetData.Visible = true;
            }
            if (fldDetailColumn == "M")
            {
                richEdit = new RichEditControl { Name = "rtxLoadReport" };
                mainLayout.Controls.Add(richEdit);
                if (CommonLayoutStoredData == null)
                {
                    mainLayout.AddItem(fldDetailColumn, richEdit);
                }
            }
            if (fldDetailColumn == "SML")
            {
                btnCommit.Text = "Gửi thư";
                richEdit = new RichEditControl { Name = "rtxLoadReport" };
                mainLayout.Controls.Add(richEdit);
                if (CommonLayoutStoredData == null)
                {
                    mainLayout.AddItem(fldDetailColumn, richEdit);
                }
            }
            //End trungTT.
            //if (MaintainInfo.Approve == CODES.MODMAINTAIN.APROVE.YES && !string.IsNullOrEmpty(Program.txnum) ) {                
            //    btnCommit.Visible = true;
            //    btnCommit.Text = "Duyệt";
            //}

            if (!string.IsNullOrEmpty(MaintainInfo.ButtonText))
            {
                btnCommit.Text = MaintainInfo.ButtonText; 
            }
            
            base.BuildFields();

            if (Parent is ContainerControl)
                ((ContainerControl)Parent).ActiveControl = mainLayout;

            
        }

        protected override void BuildButtons()
        {
//#if DEBUG
                SetupContextMenu(mainLayout);
                SetupModuleEdit();
                SetupGenenerateScript();
                SetupSeparator();
                SetupParameterFields();
                SetupCommonFields();
                SetupSeparator();
                SetupFieldMaker();
                SetupFieldsSuggestion();
                SetupSeparator();
                SetupLanguageTool();
                SetupSaveLayout(mainLayout);
                SetupSaveAllLayout(mainLayout);
//#endif
        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            lbTitle.BackColor = ThemeUtils.BackTitleColor;
            lbTitle.ForeColor = ThemeUtils.TitleColor;
            SaveAsFile();            
        }
         private void SaveAsFile()
        {
            lnkFileContextMenu = new ContextMenuStrip();            
            lnkFile.ContextMenuStrip = lnkFileContextMenu;
            lnkFileContextMenu.Items.Clear();
            lnkFileContextMenu.Items.Add("Lưu file", ThemeUtils.Image16.Images["SAVE"]).Click+=            
               delegate
               {
                   try
                   {
                       bool allow = false;
                       foreach (var i in (FieldUtils.GetModuleFields(
                           ModuleInfo.ModuleID,
                           CODES.DEFMODFLD.FLDGROUP.PARAMETER)))
                       {
                           strFLDID = i.FieldName;
                           allow = true;
                       }
                       if (allow)
                       {
                           var fields = FieldUtils.GetModuleFields(ModuleInfo.ModuleID, CODES.DEFMODFLD.FLDGROUP.PARAMETER);
                           List<string> values = new List<string>();

                           string StoreName;                           
                           if (MaintainInfo.Approve == CODES.MODMAINTAIN.APROVE.YES && !string.IsNullOrEmpty(Program.txnum))
                           {
                               values.Add(Program.txnum);
                               StoreName = "sp_tllog_file_sel";
                           }
                           else
                           {
                               foreach (var field in fields)
                               {
                                   values.Add(this[field.FieldID].ToString());
                               }
                               StoreName = MaintainInfo.ReportStore;
                           }  
                          
                           // End TuanLM
                           if (values.Count > 0)
                           {
                               using (var ctrlSA = new SAController())
                               {
                                   DataContainer container = null;
                                   ctrlSA.ExecuteProcedureFillDataset(out container,StoreName, values);
                                   if (container != null && container.DataTable != null)
                                   {
                                       var resultTable = container.DataTable;

                                       if (resultTable.Rows.Count > 0)
                                       {
                                           for (int i = 0; i <= resultTable.Rows.Count - 1; i++)
                                           {
                                               using (System.IO.MemoryStream ms = new System.IO.MemoryStream((Byte[])resultTable.Rows[i]["filestore"]))
                                               {
                                                   StreamReader memoryReader = new StreamReader(ms);

                                                   SaveFileDialog dlg = new SaveFileDialog();
                                                   dlg.FileName = resultTable.Rows[i]["filename"].ToString();
                                                   dlg.Filter = resultTable.Rows[i]["filetype"].ToString();
                                                   if (dlg.ShowDialog() == DialogResult.OK)
                                                   {
                                                       ms.WriteTo(dlg.OpenFile());                                                       
                                                   }                                                  
                                               }
                                           };
                                       }
                                       else
                                       {
                                           throw ErrorUtils.CreateError(ERR_FILE.ERR_FILE_IS_NOT_ATTACKED);
                                       }
                                   }

                               }
                           }
                       }
                       else
                       {
                           throw ErrorUtils.CreateError(ERR_FILE.ERR_FILE_IS_NOT_ATTACKED);
                       }

                   }
                   catch (Exception ex)
                   {
                       ShowError(ex);
                   }
               };  
        }         
        private void LoadData(string storeName)
        {
            using (var ctrlSA = new SAController())
            {
                if (!string.IsNullOrEmpty(storeName))
                {
                    List<string> values;
                    GetOracleParameterValues(out values, storeName);

                    DataContainer con;
                    ctrlSA.ExecuteMaintainQuery(out con, MaintainInfo.ModuleID, MaintainInfo.SubModule, values);
                    AssignFieldValuesFromResult(con);
                    DataTable dt = con.DataTable;
                    //if ((MaintainInfo.SubModule == "MED" || MaintainInfo.SubModule == "MVW") && MaintainInfo.Report == "M")
                    //{
                    //    richEdit.HtmlText = dt.Rows[0]["TXNOTE"].ToString();
                    //}
                    //else if (MaintainInfo.SubModule == "MED" && (MaintainInfo.Report == "T" || MaintainInfo.Report == "R"))
                    //{
                    //    if (dt.Rows[0]["ORIENTION"].ToString() == "L")
                    //    {
                    //        richEdit.Document.HtmlText = dt.Rows[0]["FILE_REPORT"].ToString();
                    //        richEdit.Document.Sections[0].Page.PaperKind = System.Drawing.Printing.PaperKind.LetterExtra;
                    //        richEdit.Document.Sections[0].Page.Landscape = true;
                    //    }
                    //    else
                    //    {
                    //        richEdit.Document.HtmlText = dt.Rows[0]["FILE_REPORT"].ToString();
                    //        richEdit.Document.Sections[0].Page.PaperKind = System.Drawing.Printing.PaperKind.LetterExtra;
                    //    }
                    //}
                }
            }
        }

        protected override void InitializeModuleData()
        {
            base.InitializeModuleData();
            try
            {
                StopCallback = true;
                switch (ModuleInfo.SubModule)
                {
                    case CODES.DEFMOD.SUBMOD.MAINTAIN_EDIT:
                        lbTitle.Text = Language.EditTitle;
                        LoadData(MaintainInfo.EditSelectStore);
                        break;
                    case CODES.DEFMOD.SUBMOD.MAINTAIN_VIEW:
                        lbTitle.Text = Language.ViewTitle;
                        if (MaintainInfo.Report == "F")
                        {
                            lnkFile.Visible = true;
                            lnkFile.Properties.Appearance.BackColor = Color.Transparent;                            
                            SaveAsFileToDisk(false);
                            LoadData(MaintainInfo.ViewSelectStore);
                        }
                        else if (MaintainInfo.Report == "V")
                        {
                            SaveAsFileToDisk(true);
                            IsFile = true;
                        }
                        else
                        {

                            if (MaintainInfo.Approve == CODES.MODMAINTAIN.APROVE.YES && !string.IsNullOrEmpty(Program.txnum))
                                LoadDataTransaction(SYSTEM_STORE_PROCEDURES.TRANS_STOREPROC);
                            else
                                LoadData(MaintainInfo.ViewSelectStore);
                        }                                                                     
                        break;
                    case CODES.DEFMOD.SUBMOD.MAINTAIN_ADD:
                        lbTitle.Text = Language.AddTitle;
                        LoadData(MaintainInfo.AddSelectStore);
                        break;
                    //TUDQ them
                    case CODES.DEFMOD.SUBMOD.TRANSACTION_VIEW:
                        lbTitle.Text = Language.AddTitle;
                        LoadDataTransaction(MaintainInfo.TRANSATIONSTORE);
                        break;
                    //END
                }
                
                // Configuration lbTitle
                switch (ModuleInfo.SubModule)
                {
                    case CODES.DEFMOD.SUBMOD.MAINTAIN_EDIT:
                        lbTitle.Text = string.Format(lbTitle.Text, this);
                        break;
                    case CODES.DEFMOD.SUBMOD.MAINTAIN_VIEW:
                        lbTitle.Text = string.Format(lbTitle.Text, this);
                        SAController ctrl = new SAController();//HUYVQ: 2014/06/09
                        Session s;
                        ctrl.GetCurrentSessionInfo(out s);
                        if (MaintainInfo.Approve == CODES.MODMAINTAIN.APROVE.YES && !string.IsNullOrEmpty(Program.txnum) && s.Type == 1)
                        {
                            btnCommit.Visible = true;
                            btnCommit.Text = "Duyệt";
                        }
                        //add by trungtt 27.5.2014
                        else if (MaintainInfo.Report == "R" && MaintainInfo.SubModule == CODES.DEFMOD.SUBMOD.MAINTAIN_VIEW)
                        {
                            btnCommit.Visible = true;
                            btnCommit.Text = "Thông tin";
                            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucMaintain));
                            btnCommit.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadReport.Image")));
                        }
                        //end trungtt
                        else
                        {
                            btnCommit.Visible = false;
                        }                       
                        break;
                }

                if (ActiveControl is ComboBoxEdit)
                    LoadComboxListSource((ActiveControl as ComboBoxEdit).Properties);
            }
            catch
            {
                CloseModule();
                throw;
            }
            finally
            {
                StopCallback = false;
            }
        }

        protected delegate void ResetModuleDataInvoker();
        protected void ResetModuleData()
        {
            if(InvokeRequired)
            {
                Invoke(new ResetModuleDataInvoker(ResetModuleData));
                return;
            }

            InitializeModuleData();
            mainLayout.FocusHelper.FocusFirst();
        }

        protected override void SetDefaultValues(List<ModuleFieldInfo> fields)
        {
            base.SetDefaultValues(fields);

            foreach (var field in fields)
            {
                if (field.DefaultValue != null && field.ControlType != CODES.DEFMODFLD.CTRLTYPE.CHECKEDCOMBOBOX)
                {
                    switch (field.FieldGroup)
                    {
                        case CODES.DEFMODFLD.FLDGROUP.COMMON:
                            this[field.FieldID] = FieldUtils.Convert(field, field.DefaultValue);
                            break;
                    }
                }
                else if (field.DefaultValue != null && field.ControlType == CODES.DEFMODFLD.CTRLTYPE.CHECKEDCOMBOBOX)              
                {
                    using (var ctrlSA = new SAController())
                    {

                        List<string> values;                        
                        DataContainer con;
                        GetOracleParameterValues(out values, field.DefaultValue);
                        ctrlSA.ExecuteProcedureFillDataset(out con, field.DefaultValue, values);
                        var dsResult = con.DataSet;                         
                        this[field.FieldID] = FieldUtils.Convert(field, dsResult.Tables[0].Rows[0][0].ToString());                                                    
                    }
                }
            }
        }
        #endregion

        #region Events
        private void btnCommit_Click(object sender, EventArgs e)
        {
            if (btnCommit.Text == "Duyệt")
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn phê duyệt thông tin này ?", "Phê duyệt thông tin", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Execute(); 
                }
                //else
                //{
                //    // user clicked no
                //}
            }
            //add by trungtt - 27.5.2014
            else if (btnCommit.Text == "Thông tin")
            {
                LoadProfile();
            }
            //end trungtt
            else
            {
                Execute();
            }           
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Program.txnum = null;
            if (MaintainInfo.ModuleID == "02913")
            {
                MainProcess.LogoutFromSystem(true);
            }
            if (MaintainInfo.Report == CODES.MODMAINTAIN.REPORT.FILE)
            {
                if (!string.IsNullOrEmpty(filetempname))
                {
                    DeleteFile(filetempname);
                }               
                using (var ctrlSA = new SAController())
                {
                    List<string> values = new List<string>();
                    //values.Add(Program.FileName);
                    ctrlSA.ExecuteStoreProcedure("sp_delfile_all", values);
                    CloseModule();
                }
            }
            else
                CloseModule();           
        }

        #endregion

        public ucMaintain()
        {
            InitializeComponent();            
        }

        public delegate void PInvoke();
        public void P()
        {
            if (InvokeRequired)
            {
                Invoke(new PInvoke(P));
                return;
            }

            try
            {
                richEdit.SaveDocument(Program.strAppStartUpPath + @"\Reports\" + MaintainInfo.ReportName + ".html", DocumentFormat.Html);
                string sitePath2 = Program.strAppStartUpPath + @"\Reports\" + MaintainInfo.ReportName + ".html";
                webBrowser3.Navigate(sitePath2);
                webBrowser3.DocumentCompleted += webBrowser3_DocumentCompleted;
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }
        
        void webBrowser3_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                using (var ctrlSA = new SAController())
                {
                    HtmlElementCollection elems3 = webBrowser3.Document.GetElementsByTagName("span");
                    if (MaintainInfo.ModuleID == "02101" || MaintainInfo.ModuleID == "02102" || MaintainInfo.ModuleID == "02103" || MaintainInfo.ModuleID == "02104")
                    {
                        GetRecordReport_BCHD(webBrowser3);
                    }
                    else if (MaintainInfo.ModuleID == "02105")
                    {
                        GetRecordReport(webBrowser3,"02105",6, 6, 46);                        
                    }
                    else if (MaintainInfo.ModuleID == "02106")
                    {
                        GetRecordReport(webBrowser3,"02106", 1, 8, 54);                        
                    }
                    else if (MaintainInfo.ModuleID == "02107")
                    {
                        GetRecordReport(webBrowser3,"02107", 2, 6, 46);                        
                    }
                    else if (MaintainInfo.ModuleID == "02108")
                    {
                        GetRecordReport(webBrowser3,"02108",2, 6, 46);                        
                    }
                    else if (MaintainInfo.ModuleID == "02109")
                    {
                        GetRecordReport(webBrowser3,"02109", 4, 6, 52); 
                    }
                    foreach (HtmlElement elem3 in elems3)
                    {
                        if (elem3.InnerText != null)
                        {
                            string strNumber = elem3.InnerText.ToString();
                            double dblNumber;
                            bool isNumber = double.TryParse(strNumber, out dblNumber);
                            if (isNumber)
                            {
                                elem3.InnerText = String.Format("{0:###,##0.##}", double.Parse(strNumber));
                            }
                        }
                    }
                    File.WriteAllText(Program.strAppStartUpPath + @"\Reports\\" + MaintainInfo.ReportName + ".html", webBrowser3.Document.Body.Parent.OuterHtml, Encoding.GetEncoding(webBrowser3.Document.Encoding));
                    //};
                    richEdit.LoadDocument(Program.strAppStartUpPath + @"\Reports\\" + MaintainInfo.ReportName + ".html", DocumentFormat.Html);

                    List<string> values;
                    DataContainer container;
                    GetOracleParameterValues(out values, MaintainInfo.AddInsertStore);
                    values[81] = richEdit.HtmlText;
                    ctrlSA.ExecuteMaintain(out container, ModuleInfo.ModuleID, "MAD", values);

                    if (MaintainInfo.ModuleID == "02101" || MaintainInfo.ModuleID == "02102" || MaintainInfo.ModuleID == "02103" || MaintainInfo.ModuleID == "02104")
                    {
                        List<string> valuesDataReport;
                        GetOracleParameterValues(out valuesDataReport, MaintainInfo.DataReportStore);
                        valuesDataReport[4] = strArray;
                        ctrlSA.ExecuteStoreProcedure(MaintainInfo.DataReportStore, valuesDataReport);
                    }
                    else if (MaintainInfo.ModuleID == "02105" || MaintainInfo.ModuleID == "02106" || MaintainInfo.ModuleID == "02107" || MaintainInfo.ModuleID == "02108" || MaintainInfo.ModuleID == "02109")
                    {
                        List<string> valuesDataReport;
                        GetOracleParameterValues(out valuesDataReport, MaintainInfo.DataReportStore);
                        valuesDataReport[5] = strArray;
                        ctrlSA.ExecuteStoreProcedure(MaintainInfo.DataReportStore, valuesDataReport);
                    }
                    webBrowser3.DocumentCompleted -= webBrowser3_DocumentCompleted;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            } 
        }
        string strArray;
        private void GetRecordReport_BCHD(WebBrowser wb1)
        {            
            HtmlElementCollection elems3 = wb1.Document.GetElementsByTagName("span");
            foreach (HtmlElement elem3 in elems3)
            {
                if (elem3.InnerText != null)
                {
                    string strNumber = elem3.InnerText.ToString();
                    double dblNumber;
                    bool isNumber = double.TryParse(strNumber, out dblNumber);
                    if (isNumber)
                    {
                        strArray += elem3.InnerText + ",";
                    }
                    else if (strNumber == "cursor")
                    {
                        strArray += "cursor,";
                    }
                }
            }            
        }
        private void GetRecordReport(WebBrowser wb1, string strModID, int h1, int h2, int h3)
        {
            HtmlElementCollection elems = wb1.Document.GetElementsByTagName("p");
            strArray = "cursor,";
            int i;
            if (strModID == "02106")
            {
                i = h1;
                foreach (HtmlElement elem in elems)
                {
                    if ((i + 3) > h3 && ((i + 3) % h2) == 0)
                    {
                        HtmlElementCollection x = elem.GetElementsByTagName("span");

                        string strNumber = elem.InnerText.ToString();
                        double dblNumber;
                        bool isNumber = double.TryParse(strNumber, out dblNumber);

                        foreach (HtmlElement y in x)
                        {
                            if (y.InnerText == "cursor")
                                strArray += "cursor,";
                            else if (isNumber)
                            {
                                strArray += y.InnerText + ",";
                            }

                        }
                    }
                    i++;
                }
                i = h1;
                foreach (HtmlElement elem in elems)
                {
                    if ((i + 2) > h3 && ((i + 2) % h2) == 0)
                    {
                        HtmlElementCollection x = elem.GetElementsByTagName("span");

                        string strNumber = elem.InnerText.ToString();
                        double dblNumber;
                        bool isNumber = double.TryParse(strNumber, out dblNumber);

                        foreach (HtmlElement y in x)
                        {
                            if (y.InnerText == "cursor")
                                strArray += "cursor,";
                            else if (isNumber)
                            {
                                strArray += y.InnerText + ",";
                            }

                        }
                    }
                    i++;
                }
            }

            i = h1;
            foreach (HtmlElement elem in elems)
            {
                if ((i + 1) > h3 && ((i + 1) % h2) == 0)
                {
                    HtmlElementCollection x = elem.GetElementsByTagName("span");

                    string strNumber = elem.InnerText.ToString();
                    double dblNumber;
                    bool isNumber = double.TryParse(strNumber, out dblNumber);

                    foreach (HtmlElement y in x)
                    {
                        if (y.InnerText == "cursor")    
                            strArray += "cursor,";
                        else if (isNumber)
                        {
                            strArray += y.InnerText + ",";
                        }

                    }
                }
                i++;
            }
            i = h1;
            foreach (HtmlElement elem in elems)
            {
                if (i > h3 && (i % h2) == 0)
                {
                    HtmlElementCollection x = elem.GetElementsByTagName("span");

                    string strNumber = elem.InnerText.ToString();
                    double dblNumber;
                    bool isNumber = double.TryParse(strNumber, out dblNumber);

                    foreach (HtmlElement y in x)
                    {
                        if (y.InnerText == "cursor")
                            strArray += "cursor,";
                        else if (isNumber)
                        {
                            strArray += y.InnerText + ",";
                        }
                    }
                }
                i++;
            }
        }

        //public override void Execute()
        public void Execute()
        {
            if (ValidateModule())
            {
                LockUserAction();

                new WorkerThread(
                    delegate
                     {
                         try
                         {
                             using (var ctrlSA = new SAController())
                             {
                                 List<string> values;
                                 var repeatInput = false;
                                 DataContainer container = null;
                                 switch (ModuleInfo.SubModule)
                                 {
                                     case CODES.DEFMOD.SUBMOD.MAINTAIN_ADD:
                                         if (MaintainInfo.Report == "Y")
                                         {
                                             try
                                             {
                                                 P();
                                             }                                             
                                             catch (Exception ex)
                                             {
                                                 ShowError(ex);
                                             }
                                         }
                                         //else if (MaintainInfo.Report == "R")
                                         //{                                             
                                         //    P();
                                         //}
                                         else if (MaintainInfo.Report == "T")
                                         {
                                             P();
                                         }
                                         else if (MaintainInfo.Report == "M")
                                         {
                                             GetOracleParameterValues(out values, MaintainInfo.AddInsertStore);
                                             values[73] = richEdit.HtmlText;
                                             ctrlSA.ExecuteMaintain(out container, ModuleInfo.ModuleID, ModuleInfo.SubModule, values);
                                         }
                                         else if(MaintainInfo.Report == "SML")
                                         {
                                             List<string> sendMailPrm = new List<string>();
                                             sendMailPrm.Add(this["C01"].ToString());
                                             sendMailPrm.Add(this["C02"].ToString());
                                             sendMailPrm.Add(richEdit.HtmlText);
                                             ctrlSA.ExecuteMaintain(out container, ModuleInfo.ModuleID, ModuleInfo.SubModule, sendMailPrm);
                                             //string strListAddressTo = this["S01"].ToString();
                                             //string[] strArrayAddressTo = strListAddressTo.Split(',');

                                             ////--Mail config
                                             //DataSet dsResult2;
                                             //values = new List<string>();
                                             //ctrlSA.ExecuteProcedureFillDataset(out container, "SP_MAILCONFIG_SEL", values);
                                             //dsResult2 = container.DataSet;

                                             //SmtpClient SmtpServer = new SmtpClient(dsResult2.Tables[0].Rows[0]["MAIL_ID"].ToString());
                                             //SmtpServer.Port = Int32.Parse(dsResult2.Tables[0].Rows[0]["PORT"].ToString());
                                             //string[] strMailName = dsResult2.Tables[0].Rows[0]["MAIL_NAME"].ToString().Split('@');
                                             //SmtpServer.Credentials = new System.Net.NetworkCredential(strMailName[0].ToString(), dsResult2.Tables[0].Rows[0]["PASSWORD"].ToString());
                                             //SmtpServer.EnableSsl = false;
                                             //string strAddress = dsResult2.Tables[0].Rows[0]["MAIL_NAME"].ToString();
                                             ////--Send mail
                                             //foreach (string item in strArrayAddressTo)
                                             //{

                                             //    MailMessage mail = new MailMessage(new MailAddress(strAddress), new MailAddress(item));

                                             //    //--Subject and Body
                                             //    mail.BodyEncoding = Encoding.Default;
                                             //    //mail.Subject = lbTitle.Text;
                                             //    mail.Subject = this["C02"].ToString();
                                             //    //mail.Body = this["S03"].ToString();
                                             //    mail.Body = richEdit.HtmlText;
                                             //    mail.Priority = MailPriority.High;
                                             //    mail.IsBodyHtml = true;
                                             //    //--attachment
                                             //    List<string> listparam = new List<string>();
                                             //    listparam.Add(null);
                                             //    ctrlSA.ExecuteProcedureFillDataset(out container, "SP_FILE_ATTACHMENT", listparam);

                                             //    if (container != null && container.DataTable != null)
                                             //    {
                                             //        var resultTable = container.DataTable;

                                             //        if (resultTable.Rows.Count > 0)
                                             //        {
                                             //            for (int i = 0; i <= resultTable.Rows.Count - 1; i++)
                                             //            {
                                             //                using (System.IO.MemoryStream ms = new System.IO.MemoryStream((Byte[])resultTable.Rows[i]["filestore"]))
                                             //                {
                                             //                    mail.Attachments.Add(new Attachment(ms, resultTable.Rows[i]["filename"].ToString()));
                                             //                    try
                                             //                    {
                                             //                        SmtpServer.Send(mail);
                                             //                    }
                                             //                    catch (Exception ex)
                                             //                    {
                                             //                        //ShowError(ex);
                                             //                        throw ErrorUtils.CreateErrorWithSubMessage(148, "Gửi thư điện tử với file đính kèm thất bại !");
                                             //                    }
                                             //                    ms.Close();
                                             //                }
                                             //            }
                                             //        }
                                             //    }
                                             //    else
                                             //    {
                                             //        try
                                             //        {
                                             //            SmtpServer.Send(mail);
                                             //        }
                                             //        catch (Exception ex)
                                             //        {
                                             //            //ShowError(ex);
                                             //            throw ErrorUtils.CreateErrorWithSubMessage(148, "Gửi thư điện tử thất bại !");
                                             //        }
                                             //    }


                                             //}
                                             //var valuesDeleteFile = new List<string>();
                                             //ctrlSA.ExecuteStoreProcedure("SP_DELETE_FILE_ATTACHMENT", valuesDeleteFile);
                                             CloseModule();
                                         }
                                         else
                                         {
                                             GetOracleParameterValues(out values, MaintainInfo.AddInsertStore);
                                             List<string> temp = new List<string>();
                                             //TUDQ them
                                             if (MaintainInfo.ModuleID == "02913")
                                             {
                                                 foreach (string value in values)
                                                 {
                                                     temp.Add(CommonUtils.EncryptStringBySHA1(value));
                                                 }
                                                 values = temp;
                                             }
                                             if (MaintainInfo.TRANSACTION_MODE == "Y")
                                             {
                                                 values[values.Count - 1] = MaintainInfo.ModuleID;
                                                 values.Add(null);
                                             }
                                             //END
                                             ctrlSA.ExecuteMaintain(out container, ModuleInfo.ModuleID, ModuleInfo.SubModule, values);
                                         }
                                         if (MaintainInfo.AddRepeatInput == CODES.MODMAINTAIN.REPEATINPUT.YES)
                                            repeatInput = true;
                                         break;
                                         
                                     case CODES.DEFMOD.SUBMOD.MAINTAIN_EDIT:
                                         GetOracleParameterValues(out values, MaintainInfo.EditUpdateStore);
                                         ctrlSA.ExecuteMaintain(out container, ModuleInfo.ModuleID, ModuleInfo.SubModule, values);
                                                                                  
                                         if (MaintainInfo.EditRepeatInput == CODES.MODMAINTAIN.REPEATINPUT.YES)
                                            repeatInput = true;
                                         break;
                                         //TUDQ Them
                                     //add by TrungTT 23.8.2013;                           
                                     case CODES.DEFMOD.SUBMOD.MAINTAIN_VIEW:
                                         try
                                         {
                                             // Lay thong tin module trong tllog
                                             List<string> value = new List<string>();
                                             string v_Submod = null ;
                                             value.Add(Program.txnum);
                                             ctrlSA.ExecuteProcedureFillDataset(out container, "sp_tllog_sel_basic", value);
                                             if (container != null && container.DataTable != null)
                                                {
                                                    var resultTable = container.DataTable;
                                                    if (resultTable.Rows.Count > 0)
                                                    {
                                                        v_Submod = resultTable.Rows[0]["SUBMOD"].ToString();
                                                    }
                                                }

                                             if (v_Submod == CODES.DEFMOD.SUBMOD.MAINTAIN_ADD)
                                             {
                                                 GetOracleParameterValues(out values, MaintainInfo.AddInsertStore);
                                                 ctrlSA.ExecApprove(out container, ModuleInfo.ModuleID, v_Submod,SecID, values);

                                                 // Cap nhat tllogs
                                                 List<string> valueAcceptTrans = new List<string>();
                                                 valueAcceptTrans.Add(Program.txnum);
                                                 valueAcceptTrans.Add(App.Environment.ClientInfo.UserName);
                                                 ctrlSA.ExecuteStoreProcedure("sp_accept_trans", valueAcceptTrans);

                                                 List<string> values1 = new List<string>();
                                                 values1.Add(null);
                                                 values1.Add(Program.txnum);
                                                 // Cap nhat file dinh kem vao bang Bussiness tuong ung
                                                 if (!string.IsNullOrEmpty(MaintainInfo.EXECTRANSCTIONSTORE))
                                                 {
                                                     ctrlSA.ExecuteStoreProcedure(MaintainInfo.EXECTRANSCTIONSTORE, values1);
                                                 }
                                                 Program.txnum = null;
                                             }
                                             else if (v_Submod == CODES.DEFMOD.SUBMOD.MAINTAIN_EDIT)
                                             {
                                                 GetOracleParameterValues(out values, MaintainInfo.EditUpdateStore);
                                                 string v_ROWID = values[0].ToString();

                                                 List<string> values1 = new List<string>();
                                                 values1.Add(v_ROWID);
                                                 values1.Add(Program.txnum);
                                                 // Duyet thong tin
                                                 ctrlSA.ExecApprove(out container, ModuleInfo.ModuleID, v_Submod,SecID, values);

                                                 // Cap nhat tllogs
                                                 List<string> valueAcceptTrans = new List<string>();
                                                 valueAcceptTrans.Add(Program.txnum);
                                                 valueAcceptTrans.Add(App.Environment.ClientInfo.UserName);
                                                 ctrlSA.ExecuteStoreProcedure("sp_accept_trans", valueAcceptTrans);

                                                 // Cap nhat file dinh kem vao bang Bussiness tuong ung
                                                 if (!string.IsNullOrEmpty(MaintainInfo.EXECTRANSCTIONSTORE))
                                                 {
                                                     ctrlSA.ExecuteStoreProcedure(MaintainInfo.EXECTRANSCTIONSTORE, values1);
                                                 }
                                                 Program.txnum = null;
                                             }                                             
                                         }
                                         catch (Exception ex)
                                         {
                                             ShowError(ex);
                                             UnLockUserAction();
                                         }
                                         break;
                                     //end trungtt
                                     case CODES.DEFMOD.SUBMOD.TRANSACTION_VIEW:
                                         try
                                         {
                                             List<string> values1 = new List<string>();
                                             values1.Add(Program.txnum);
                                             values1.Add(ModuleInfo.ModuleID);
                                             values1.Add(App.Environment.ClientInfo.UserName);
                                             ctrlSA.ExecuteStoreProcedure(MaintainInfo.EXECTRANSCTIONSTORE,values1);
                                             repeatInput = false;
                                         }
                                         catch (Exception ex)
                                         {
                                             ShowError(ex);
                                             UnLockUserAction();
                                         }
                                         break;
                                     //case CODES.DEFMOD.SUBMOD.MAINTAIN_VIEW:
                                     //    // Chi dung cho phan duyet
                                     //    try
                                     //    {
                                     //        if (MaintainInfo.Approve == CODES.MODMAINTAIN.APROVE.YES && !string.IsNullOrEmpty(Program.txnum))
                                     //        {
                                     //            GetOracleParameterValues(out values, MaintainInfo.EditUpdateStore);
                                     //            ctrlSA.ExecuteApproved(out container, ModuleInfo.ModuleID, CODES.DEFMOD.SUBMOD.MAINTAIN_EDIT, values);
                                     //        }
                                     //    }
                                     //    catch (Exception ex)
                                     //    {
                                     //        ShowError(ex);
                                     //        UnLockUserAction();
                                     //    }
                                     //    break;
                                         //End
                                 }

                                 RequireRefresh = true;

                                 if(MaintainInfo.ShowSuccess == CODES.MODMAINTAIN.SHOWSUCCESS.YES)
                                 {
                                     RowFormattable fmtRow = null;

                                     if(container != null)
                                     {
                                         var rows = container.DataTable.Rows;
                                         if(rows.Count == 1)
                                             fmtRow = new RowFormattable(rows[0]);
                                     }

                                     if(fmtRow != null)
                                     {
                                         frmInfo.ShowInfo(Language.Title, string.Format(Language.SuccessStatus, fmtRow), this);
                                     }
                                     else
                                         frmInfo.ShowInfo(Language.Title, Language.SuccessStatus, this);
                                 }

                                 if (!repeatInput)
                                 {
                                     CloseModule();
                                 }
                                 else
                                 {
                                     ResetModuleData();
                                     UnLockUserAction();
                                 }
                             }
                         }
                         catch (Exception ex)
                         {
                             ShowError(ex);
                             UnLockUserAction();
                         }
                     }, this).Start();
            }
        }

        private void webBrowser3_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            System.Windows.Forms.HtmlDocument document = this.webBrowser3.Document;
        }
        public override void LockUserAction()
        {
            base.LockUserAction();

            if(!InvokeRequired)
            {
                Enabled = false;
                ShowWaitingBox();
            }
        }

        public override void UnLockUserAction()
        {
            base.UnLockUserAction();

            if(!InvokeRequired)
            {
                Enabled = true;
                HideWaitingBox();
                ActiveControl = mainLayout;
                mainLayout.Focus();
            }
        }

        string strFLDID;
        //edit by TrungTT - 8.10.2013 - adding output param into fuction
        private void SaveAsFileToDisk(bool _open)
        {
            try
            {
                bool allow = false;                
                foreach (var i in (FieldUtils.GetModuleFields(
                    ModuleInfo.ModuleID,
                    //CODES.DEFMODFLD.FLDGROUP.COMMON)))
                    CODES.DEFMODFLD.FLDGROUP.PARAMETER)))
                {
                    //if (i.FieldName == "ROWID")
                    strFLDID = i.FieldName;
                    allow = true;
                }
                if (allow)
                {                    
                    var fields = FieldUtils.GetModuleFields(ModuleInfo.ModuleID, CODES.DEFMODFLD.FLDGROUP.PARAMETER);
                    string StoreName;
                    List<string> values = new List<string>();
                    if (MaintainInfo.Approve == CODES.MODMAINTAIN.APROVE.YES && !string.IsNullOrEmpty(Program.txnum))
                    {
                        values.Add(Program.txnum);
                        StoreName = "sp_tllog_file_sel";
                    }
                    else
                    {
                        foreach (var field in fields)
                        {
                            if (this[field.FieldID] != null) //HUYVQ: 05/06/2014
                            {
                                values.Add(this[field.FieldID].ToString());
                            }
                        }
                        StoreName = MaintainInfo.ReportStore;
                    }                                    
                    // End TuanLM
                    if (values.Count > 0)
                    {
                        using (var ctrlSA = new SAController())
                        {
                            DataContainer container = null;

                            ctrlSA.ExecuteProcedureFillDataset(out container, StoreName, values);
                            if (container != null && container.DataTable != null)
                            {
                                var resultTable = container.DataTable;

                                if (resultTable.Rows.Count > 0)
                                {
                                    for (int i = 0; i <= resultTable.Rows.Count - 1; i++)
                                    {
                                        if (!string.IsNullOrEmpty(resultTable.Rows[i]["filestore"].ToString()))
                                        {
                                            using (System.IO.MemoryStream ms = new System.IO.MemoryStream((Byte[])resultTable.Rows[i]["filestore"]))
                                            {
                                                filetempname = System.Environment.GetEnvironmentVariable("TEMP") + "\\" + RandomString(10, false) + resultTable.Rows[i]["filename"].ToString();
                                                FileStream flStream = new FileStream(filetempname, FileMode.OpenOrCreate);
                                                StreamReader memoryReader = new StreamReader(ms);

                                                ms.WriteTo(flStream);
                                                lnkFile.Text = filetempname;
                                                flStream.Close();
                                                flStream.Dispose();
                                            }
                                            if (_open)
                                            {
                                                OpenFile(filetempname);
                                            }
                                        }
                                        else
                                        {
                                            lnkFile.Visible = false;
                                        }                                                                       
                                    }
                                }
                                //else
                                //{
                                //    throw ErrorUtils.CreateError(ERR_FILE.ERR_FILE_IS_NOT_ATTACKED);                                   
                                //}
                            }

                        }
                    }
                }
                else
                {
                    throw ErrorUtils.CreateError(ERR_FILE.ERR_FILE_IS_NOT_ATTACKED);                    
                }

            }
            catch (Exception ex)
            {                
                ShowError(ex);
            }
        
        }
        private void DeleteFile(string _filename)
        {
            System.IO.File.Delete(_filename);
        }
        
        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder sb = new StringBuilder();
            char c;
            Random rand = new Random();
            for (int i = 0; i < size; i++)
            {
                c = Convert.ToChar(Convert.ToInt32(rand.Next(65, 87)));
                sb.Append(c);
            }
            if (lowerCase)
                return sb.ToString().ToLower();
            return sb.ToString();

        }
        private void OpenFile(string _filename)
        {
            ProcessStartInfo pi = new ProcessStartInfo(_filename);
            pi.Arguments = Path.GetFileName(_filename);
            pi.UseShellExecute = true;
            pi.WorkingDirectory = Path.GetDirectoryName(_filename);
            pi.FileName = _filename;
            pi.Verb = "OPEN";
            System.Diagnostics.Process.Start(pi);
        }

        private void btnLoadReport_Click(object sender, EventArgs e)
        {
            strRPT_ACTION = "0";
            /*foreach (var field in CommonFields)
            {
                if (field.FieldName == "RPT_ACTION")
                {
                    strRPT_ACTION = this[field.FieldID].ToString();
                }
            }*/

            //richEdit.DoubleClick +=new EventHandler(richEdit_DoubleClick);
            //richEdit.HtmlTextChanged +=new EventHandler(richEdit_HtmlTextChanged);

            if (strRPT_ACTION == "0")
            {
                if (ValidateModule())
                {
                    LockUserAction();
                    try
                    {
                        using (var ctrlSA = new SAController())
                        {
                            List<string> values;
                            var repeatInput = false;
                            DataContainer container = null;

                            if (MaintainInfo.Report == "Y")
                            {
                                try
                                {
                                    #region Create Data
                                    List<string> Values_Rpt;
                                    GetOracleParameterValues(out Values_Rpt, MaintainInfo.ReportStore);
                                    ctrlSA.ExecuteMaintainReport(out container, ModuleInfo.ModuleID, ModuleInfo.SubModule, Values_Rpt);

                                    var dsResult = container.DataSet;

                                    var dt = new DataTable("ReportParameter");
                                    foreach (var field in CommonFields)
                                    {
                                        var name = field.FieldName;
                                        var col = new DataColumn(name, FieldUtils.GetType(field.FieldType));
                                        dt.Columns.Add(col);
                                    }

                                    var row = dt.NewRow();
                                    dt.Rows.Add(row);
                                    dsResult.Tables.Add(dt);

                                    foreach (var field in CommonFields)
                                    {
                                        if (field.FieldName == "MESSAGEID")
                                            row[field.FieldName] = 0;
                                        else
                                        row[field.FieldName] = this[field.FieldID];
                                    }

                                    #endregion

                                    #region Create Report
                                    dsResult.WriteXml(Program.strAppStartUpPath + @"\Reports\" + MaintainInfo.ReportName + ".xml", XmlWriteMode.WriteSchema);
                                    var report = XtraReport.FromFile(Program.strAppStartUpPath + @"\Reports\" + MaintainInfo.ReportName + ".repx", true);
                                    report.XmlDataPath = Program.strAppStartUpPath + @"\Reports\" + MaintainInfo.ReportName + ".xml";

                                    string strProcedureName = MaintainInfo.ReportStore.ToString();
                                    string strReportName = MaintainInfo.ReportName.ToString();
                                    if (strProcedureName.Length > 9)
                                    {
                                        XRSubreport subReport;
                                        XtraReport reportDetail;
                                        int n = Int32.Parse(strProcedureName.Substring(9, 2).ToString());
                                        for (int i = 1; i <= n; i++)
                                        {
                                            reportDetail = XtraReport.FromFile(Program.strAppStartUpPath + @"\Reports\" + MaintainInfo.ReportName + "_" + i + ".repx", true);
                                            reportDetail.XmlDataPath = Program.strAppStartUpPath + @"\Reports\" + MaintainInfo.ReportName + ".xml";
                                            int j = i - 1;
                                            if (j == 0)
                                                reportDetail.DataMember = "Table";
                                            else
                                                reportDetail.DataMember = "Table" + j;
                                            subReport = ((XRSubreport)report.FindControl("subreport" + i, true));

                                            subReport.ReportSource = reportDetail;
                                        }
                                    }

                                    #endregion

                                    report.ExportToHtml(Program.strAppStartUpPath + @"\Reports\\" + MaintainInfo.ReportName + ".html");
                                
                                    string strOriention = null;
                                    foreach (var field in ParameterFields)
                                    {
                                        if (field.FieldName == "ORIENTION")
                                            strOriention = field.DefaultValue;
                                    }
                                    if (strOriention == "L")
                                    {
                                        richEdit.LoadDocument(Program.strAppStartUpPath + @"\Reports\\" + MaintainInfo.ReportName + ".html", DocumentFormat.Html);
                                        richEdit.Document.Sections[0].Page.PaperKind = System.Drawing.Printing.PaperKind.A4;
                                        richEdit.Document.Sections[0].Page.Landscape = true;                                        
                                    }
                                    else
                                    {
                                        richEdit.LoadDocument(Program.strAppStartUpPath + @"\Reports\\" + MaintainInfo.ReportName + ".html", DocumentFormat.Html);
                                        richEdit.Document.Sections[0].Page.PaperKind = System.Drawing.Printing.PaperKind.LetterExtra;
                                    }
                                    UnLockUserAction();

                                }
                                catch (Exception ex)
                                {
                                    ShowError(ex);
                                }
                            }
                            else if (MaintainInfo.Report == "R")
                            {
                                try
                                {
                                    #region Create Data
                                    List<string> Values_Rpt;
                                    GetOracleParameterValues(out Values_Rpt, MaintainInfo.ReportStore);
                                    ctrlSA.ExecuteMaintainReport(out container, ModuleInfo.ModuleID, ModuleInfo.SubModule, Values_Rpt);

                                    var dsResult = container.DataSet;

                                    var dt = new DataTable("ReportParameter");
                                    foreach (var field in CommonFields)
                                    {
                                        var name = field.FieldName;
                                        var col = new DataColumn(name, FieldUtils.GetType(field.FieldType));
                                        dt.Columns.Add(col);
                                    }

                                    var row = dt.NewRow();
                                    dt.Rows.Add(row);
                                    dsResult.Tables.Add(dt);

                                    foreach (var field in CommonFields)
                                    {
                                        if(field.ControlType != "GV")
                                        {
                                            if (field.FieldFormat != null && this[field.FieldID] == null)
                                            { }
                                            else
                                                row[field.FieldName] = this[field.FieldID];
                                        }
                                    }

                                    #endregion

                                    #region Create Report
                                    dsResult.WriteXml(Program.strAppStartUpPath + @"\Reports\" + MaintainInfo.ReportName + ".xml", XmlWriteMode.WriteSchema);
                                    var report = XtraReport.FromFile(Program.strAppStartUpPath + @"\Reports\" + MaintainInfo.ReportName + ".repx", true);
                                    report.XmlDataPath = Program.strAppStartUpPath + @"\Reports\" + MaintainInfo.ReportName + ".xml";

                                    string strProcedureName = MaintainInfo.ReportStore.ToString();
                                    string strReportName = MaintainInfo.ReportName.ToString();
                                    if (strProcedureName.Length > 9)
                                    {
                                        XRSubreport subReport;
                                        XtraReport reportDetail;
                                        int n = Int32.Parse(strProcedureName.Substring(9, 2).ToString());
                                        for (int i = 1; i <= n; i++)
                                        {
                                            reportDetail = XtraReport.FromFile(Program.strAppStartUpPath + @"\Reports\" + MaintainInfo.ReportName + "_" + i + ".repx", true);
                                            reportDetail.XmlDataPath = Program.strAppStartUpPath + @"\Reports\" + MaintainInfo.ReportName + ".xml";
                                            int j = i - 1;
                                            if (j == 0)
                                                reportDetail.DataMember = "Table";
                                            else
                                                reportDetail.DataMember = "Table" + j;
                                            subReport = ((XRSubreport)report.FindControl("subreport" + i, true));

                                            subReport.ReportSource = reportDetail;
                                        }
                                    }

                                    #endregion

                                    UnLockUserAction();
                                    
                                    report.RequestParameters = false;
                                    report.ShowPreviewDialog();

                                }
                                catch (Exception ex)
                                {
                                    ShowError(ex);
                                    UnLockUserAction();
                                }
                            }
                            else if (MaintainInfo.Report == "T")
                            {
                                try
                                {
                                    if (check_Rpt == true)
                                    {
                                        string strDefmod = null;
                                        foreach (var field in ParameterFields)
                                        {
                                            if (field.FieldName == "DEFMOD")
                                                strDefmod = field.DefaultValue;
                                        }
                                        if (strDefmod == "02105")
                                            SwapDataReport(6, 6, 46);
                                        else if (strDefmod == "02106")
                                            SwapDataReport(1, 8, 54);
                                        else if (strDefmod == "02107")
                                            SwapDataReport(3, 6, 52);
                                        else if (strDefmod == "02108")
                                            SwapDataReport(2, 6, 46);
                                        else if (strDefmod == "02109")
                                            SwapDataReport(4, 6, 52);
                                        File.WriteAllText(Program.strAppStartUpPath + @"\Reports\\" + MaintainInfo.ReportName + ".html", webBrowser2.Document.Body.Parent.OuterHtml, Encoding.GetEncoding(webBrowser2.Document.Encoding));
                                    }
                                    richEdit.LoadDocument(Program.strAppStartUpPath + @"\Reports\\" + MaintainInfo.ReportName + ".html", DocumentFormat.Html);
                                    richEdit.Document.Sections[0].Page.PaperKind = System.Drawing.Printing.PaperKind.LetterExtra;
                                    UnLockUserAction();
                                }
                                catch (Exception ex)
                                {
                                    ShowError(ex);
                                }
                            }
                            if (MaintainInfo.AddRepeatInput == CODES.MODMAINTAIN.REPEATINPUT.YES)
                                repeatInput = true;
                            RequireRefresh = true;
                            if (MaintainInfo.ShowSuccess == CODES.MODMAINTAIN.SHOWSUCCESS.YES){                        
                            }
                            if (!repeatInput){                            
                            }
                            else
                            {
                                ResetModuleData();                            
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex);
                    }                    
                }
            }
            else
            { 
                try
                {
                    using (var ctrlSA = new SAController())
                    {
                        List<string> values;
                        DataContainer container = null;

                        if (MaintainInfo.Report == "R")
                        {
                            try
                            {
                                List<string> Values_Rpt;
                                GetOracleParameterValues(out Values_Rpt, "SP_RELOAD_RPT");
                                ctrlSA.ExecuteProcedureFillDataset(out container, "SP_RELOAD_RPT", Values_Rpt);
                                var dsResult = container.DataSet;                                

                                if (dsResult.Tables[0].Rows[0]["ORIENTATION"].ToString() == "L")
                                {
                                    richEdit.Document.HtmlText = dsResult.Tables[0].Rows[0]["FILE_REPORT"].ToString();
                                    richEdit.Document.Sections[0].Page.PaperKind = System.Drawing.Printing.PaperKind.LetterExtra;
                                    richEdit.Document.Sections[0].Page.Landscape = true;
                                }
                                else
                                {
                                    richEdit.Document.HtmlText = dsResult.Tables[0].Rows[0]["FILE_REPORT"].ToString();
                                    richEdit.Document.Sections[0].Page.PaperKind = System.Drawing.Printing.PaperKind.LetterExtra;
                                }
                            }
                            catch (Exception ex)
                            {
                                ShowError(ex);
                            }
                        }
                        else if (MaintainInfo.Report == "T")
                        {
                            try
                            {
                                List<string> Values_Rpt;
                                GetOracleParameterValues(out Values_Rpt, "SP_RELOAD_RPT");
                                ctrlSA.ExecuteProcedureFillDataset(out container, "SP_RELOAD_RPT", Values_Rpt);
                                var dsResult = container.DataSet;

                                if (dsResult.Tables[0].Rows[0]["ORIENTATION"].ToString() == "L")
                                {
                                    richEdit.Document.HtmlText = dsResult.Tables[0].Rows[0]["FILE_REPORT"].ToString();
                                    richEdit.Document.Sections[0].Page.PaperKind = System.Drawing.Printing.PaperKind.LetterExtra;
                                    richEdit.Document.Sections[0].Page.Landscape = true;
                                }
                                else
                                {
                                    richEdit.Document.HtmlText = dsResult.Tables[0].Rows[0]["FILE_REPORT"].ToString();
                                    richEdit.Document.Sections[0].Page.PaperKind = System.Drawing.Printing.PaperKind.LetterExtra;
                                }
                            }
                            catch (Exception ex)
                            {
                                ShowError(ex);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowError(ex);
                }
            } 
        }
        private void richEdit_DoubleClick(object sender, EventArgs e)
        {
            
        }
        private void richEdit_HtmlTextChanged(object sender, EventArgs e)
        {
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateModule())
            {
                LockUserAction();

                new WorkerThread(
                    delegate
                    {
                        try
                        {
                            using (var ctrlSA = new SAController())
                            {
                                List<string> values;
                                var repeatInput = false;
                                DataContainer container = null;
                                switch (ModuleInfo.SubModule)
                                {
                                    case CODES.DEFMOD.SUBMOD.MAINTAIN_ADD:
                                        if (MaintainInfo.Report == "R")
                                        {                                            
                                            GetOracleParameterValues(out values, MaintainInfo.AddInsertStore);
                                            values[81] = richEdit.HtmlText;
                                            ctrlSA.ExecuteMaintain(out container, ModuleInfo.ModuleID, ModuleInfo.SubModule, values);                                            
                                        }
                                        if (MaintainInfo.AddRepeatInput == CODES.MODMAINTAIN.REPEATINPUT.YES)
                                            repeatInput = true;
                                        break;
                                }

                                RequireRefresh = true;

                                if (MaintainInfo.ShowSuccess == CODES.MODMAINTAIN.SHOWSUCCESS.YES)
                                {
                                    RowFormattable fmtRow = null;

                                    if (container != null)
                                    {
                                        var rows = container.DataTable.Rows;
                                        if (rows.Count == 1)
                                            fmtRow = new RowFormattable(rows[0]);
                                    }

                                    if (fmtRow != null)
                                    {
                                        frmInfo.ShowInfo(Language.Title, string.Format(Language.SuccessStatus, fmtRow), this);
                                    }
                                    else
                                        frmInfo.ShowInfo(Language.Title, Language.SuccessStatus, this);
                                }

                                if (!repeatInput)
                                {
                                    CloseModule();
                                }
                                else
                                {
                                    ResetModuleData();
                                    UnLockUserAction();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ShowError(ex);
                            UnLockUserAction();
                        }
                    }, this).Start();
            }
        }
        //add by TrungTT - 4.01.2012 - Swap Data Report
        private void SwapDataReport(int h1, int h2, int h3)
        {
            List<string> listStr = new List<string>();
            HtmlElementCollection elems = webBrowser1.Document.GetElementsByTagName("p");
            int i = h1;
            foreach (HtmlElement elem in elems)
            {
                if (i > h3 && (i % h2) == 0)
                {
                    HtmlElementCollection x = elem.GetElementsByTagName("span");
                    foreach (HtmlElement y in x)
                    {
                        //y.InnerText = i.ToString();
                        listStr.Add(y.InnerHtml.ToString());
                    }
                }
                if (elem.Children.Count != 0)
                {
                    i++;
                }
            }
            //----------------------------
            HtmlElementCollection elems2 = webBrowser2.Document.GetElementsByTagName("p");
            int i2 = h1;
            int k = 0;
            foreach (HtmlElement elem2 in elems2)
            {
                if (i2 > h3 && (((i2 + 1) % h2) == 0))
                {
                    HtmlElementCollection x2 = elem2.GetElementsByTagName("span");

                    foreach (HtmlElement y2 in x2)
                    {
                        if (listStr[k].ToString() != "&nbsp;")
                        {
                            y2.InnerText = listStr[k].ToString();
                        }
                        //listStr.Add(y2.InnerHtml.ToString());
                        k++;
                    }
                }
                if (elem2.Children.Count != 0)
                {
                    i2++;
                }
            }
            //-----------------------------------
            HtmlElementCollection elems3 = webBrowser2.Document.GetElementsByTagName("span");
            foreach (HtmlElement elem3 in elems3)
            {
                string strNumber = elem3.InnerText.ToString();
                double dblNumber;
                bool isNumber = double.TryParse(strNumber, out dblNumber);
                if (isNumber)
                {
                    elem3.InnerText = String.Format("{0:###,##0.##}", double.Parse(strNumber));
                }
            }
        }
        bool check_Rpt;
        private void btnGetData_Click(object sender, EventArgs e)
        {
            strRPT_ACTION = null;
            foreach (var field in CommonFields)
            {
                if (field.FieldName == "RPT_ACTION")
                {
                    strRPT_ACTION = this[field.FieldID].ToString();
                }
            }
            richEdit.DoubleClick += new EventHandler(richEdit_DoubleClick);
            richEdit.HtmlTextChanged += new EventHandler(richEdit_HtmlTextChanged);
            if (strRPT_ACTION == "0")
            {
                if (ValidateModule())
                {
                    LockUserAction();
                    try
                    {
                        using (var ctrlSA = new SAController())
                        {
                            List<string> values;
                            var repeatInput = false;
                            DataContainer container = null;

                            if (MaintainInfo.Report == "T")
                            {
                                try
                                {
                                    List<string> Values_RptBCTC;
                                    GetOracleParameterValues(out Values_RptBCTC, "SP_RPT_PREVIOUS");
                                    if (Values_RptBCTC[0] != null)
                                    {
                                        ctrlSA.ExecuteProcedureFillDataset(out container, "SP_RPT_PREVIOUS", Values_RptBCTC);
                                        var dsResultBCTC = container.DataSet;
                                        if (dsResultBCTC.Tables[0].Rows.Count != 0)
                                        {
                                            RichEditControl RICHEDITCONTROL = new RichEditControl();
                                            RICHEDITCONTROL.HtmlText = dsResultBCTC.Tables[0].Rows[0]["FILE_REPORT"].ToString();
                                            RICHEDITCONTROL.SaveDocument(Program.strAppStartUpPath + @"\Reports\" + MaintainInfo.ReportName + "_PRE.html", DocumentFormat.Html);
                                            string sitePath1 = Program.strAppStartUpPath + @"\Reports\" + MaintainInfo.ReportName + "_PRE.html";
                                            webBrowser1.Navigate(sitePath1);
                                            check_Rpt = true;
                                        }
                                        else
                                        {
                                            webBrowser1.DocumentText = null;
                                            check_Rpt = false;
                                        }
                                    }
                                    else 
                                    {
                                        webBrowser1.DocumentText = null;
                                        check_Rpt = false;
                                    }

                                    container = null;
                                    #region Create Data
                                    List<string> Values_Rpt;
                                    GetOracleParameterValues(out Values_Rpt, MaintainInfo.ReportStore);
                                    ctrlSA.ExecuteMaintainReport(out container, ModuleInfo.ModuleID, ModuleInfo.SubModule, Values_Rpt);

                                    var dsResult = container.DataSet;

                                    var dt = new DataTable("ReportParameter");
                                    foreach (var field in CommonFields)
                                    {
                                        var name = field.FieldName;
                                        var col = new DataColumn(name, FieldUtils.GetType(field.FieldType));
                                        dt.Columns.Add(col);
                                    }

                                    var row = dt.NewRow();
                                    dt.Rows.Add(row);
                                    dsResult.Tables.Add(dt);

                                    foreach (var field in CommonFields)
                                    {
                                        if (field.FieldName == "MESSAGEID")
                                            row[field.FieldName] = 0;
                                        else
                                            row[field.FieldName] = this[field.FieldID];
                                    }
                                    
                                    #endregion

                                    #region Create Report
                                    dsResult.WriteXml(Program.strAppStartUpPath + @"\Reports\" + MaintainInfo.ReportName + ".xml", XmlWriteMode.WriteSchema);
                                    var report = XtraReport.FromFile(Program.strAppStartUpPath + @"\Reports\" + MaintainInfo.ReportName + ".repx", true);
                                    report.XmlDataPath = Program.strAppStartUpPath + @"\Reports\" + MaintainInfo.ReportName + ".xml";

                                    string strProcedureName = MaintainInfo.ReportStore.ToString();
                                    string strReportName = MaintainInfo.ReportName.ToString();
                                    if (strProcedureName.Length > 9)
                                    {
                                        XRSubreport subReport;
                                        XtraReport reportDetail;
                                        int n = Int32.Parse(strProcedureName.Substring(9, 2).ToString());
                                        for (int i = 1; i <= n; i++)
                                        {
                                            reportDetail = XtraReport.FromFile(Program.strAppStartUpPath + @"\Reports\" + MaintainInfo.ReportName + "_" + i + ".repx", true);
                                            reportDetail.XmlDataPath = Program.strAppStartUpPath + @"\Reports\" + MaintainInfo.ReportName + ".xml";
                                            int j = i - 1;
                                            if (j == 0)
                                                reportDetail.DataMember = "Table";
                                            else
                                                reportDetail.DataMember = "Table" + j;
                                            subReport = ((XRSubreport)report.FindControl("subreport" + i, true));

                                            subReport.ReportSource = reportDetail;
                                        }
                                    }

                                    #endregion

                                    report.ExportToHtml(Program.strAppStartUpPath + @"\Reports\\" + MaintainInfo.ReportName + ".html");
                                    //---
                                    RichEditControl RICHEDITCONTROL2 = new RichEditControl();
                                    RICHEDITCONTROL2.LoadDocument(Program.strAppStartUpPath + @"\Reports\\" + MaintainInfo.ReportName + ".html");
                                    RICHEDITCONTROL2.SaveDocument(Program.strAppStartUpPath + @"\Reports\" + MaintainInfo.ReportName + ".html", DocumentFormat.Html);
                                    string sitePath2 = Program.strAppStartUpPath + @"\Reports\" + MaintainInfo.ReportName + ".html";
                                    webBrowser2.Navigate(sitePath2);
                                    //---
                                    //string sitePath2 = Program.strAppStartUpPath + @"\Reports\" + MaintainInfo.ReportName + ".html";
                                    //webBrowser2.Navigate(sitePath2);

                                    UnLockUserAction();

                                }
                                catch (Exception ex)
                                {
                                    ShowError(ex);
                                }
                            }
                            if (MaintainInfo.AddRepeatInput == CODES.MODMAINTAIN.REPEATINPUT.YES)
                                repeatInput = true;
                            RequireRefresh = true;
                            if (MaintainInfo.ShowSuccess == CODES.MODMAINTAIN.SHOWSUCCESS.YES)
                            {
                            }
                            if (!repeatInput)
                            {
                            }
                            else
                            {
                                ResetModuleData();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex);
                    }
                }
            }
            else
            {
                try
                {
                    using (var ctrlSA = new SAController())
                    {
                        List<string> values;
                        DataContainer container = null;

                        if (MaintainInfo.Report == "R")
                        {
                            try
                            {
                                List<string> Values_Rpt;
                                GetOracleParameterValues(out Values_Rpt, "SP_RPT_PREVIOUS");
                                ctrlSA.ExecuteProcedureFillDataset(out container, "SP_RPT_PREVIOUS", Values_Rpt);
                                var dsResult = container.DataSet;

                                if (dsResult.Tables[0].Rows[0]["ORIENTATION"].ToString() == "L")
                                {
                                    richEdit.Document.HtmlText = dsResult.Tables[0].Rows[0]["FILE_REPORT"].ToString();
                                    richEdit.Document.Sections[0].Page.PaperKind = System.Drawing.Printing.PaperKind.A4;
                                    richEdit.Document.Sections[0].Page.Landscape = true;
                                }
                                else
                                {
                                    richEdit.Document.HtmlText = dsResult.Tables[0].Rows[0]["FILE_REPORT"].ToString();
                                    richEdit.Document.Sections[0].Page.PaperKind = System.Drawing.Printing.PaperKind.Letter;
                                }
                            }
                            catch (Exception ex)
                            {
                                ShowError(ex);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowError(ex);
                }
            }
        }
        //End TrungTT
        public static string readHTML(string url)
        {
            string html = null;
            using (StreamReader reader = new StreamReader(url))
            {
                html = reader.ReadToEnd();
            }
            return html;
        }

        private void GetMessageID()
        {
            bool allow = false;
            foreach (var i in (FieldUtils.GetModuleFields(
                ModuleInfo.ModuleID,
                //CODES.DEFMODFLD.FLDGROUP.COMMON)))
                CODES.DEFMODFLD.FLDGROUP.PARAMETER)))
            {
                if (i.FieldName == "MESSAGEID")
                    allow = true;
            }


            if (allow)
            {
                var field = FieldUtils.GetModuleFieldByName(
                    ModuleInfo.ModuleID,
                    //CODES.DEFMODFLD.FLDGROUP.COMMON,
                    CODES.DEFMODFLD.FLDGROUP.PARAMETER,
                    "MESSAGEID");

                var value = this[field.FieldID];
                if (value != null)
                {
                    Program.StrMessageID = value.ToString();
                }
            }
        }

        private void ucMaintain_Load(object sender, EventArgs e)
        {
            GetMessageID();
        }

        private void ucMaintain_ModuleClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Program.StrMessageID = "";            
        }

        //TUDQ them
        private void LoadDataTransaction(string storeName)
        {
            using (var ctrlSA = new SAController())
            {
                if (!string.IsNullOrEmpty(storeName))
                {
                    List<string> values = new List<string>();
                    values.Add(Program.txnum);

                    DataContainer con;
                    ctrlSA.ExecuteTransQuery(out con, MaintainInfo.ModuleID, MaintainInfo.SubModule, values);
                    AssignFieldValuesFromResult(con);
                    DataTable dt = con.DataTable;                   
                }
            }
        }
        //ENd

        //trungtt - 27.5.2014
        private void LoadProfile()
        {
            if (ValidateModule())
            {
                LockUserAction();
                try
                {
                    using (var ctrlSA = new SAController())
                    {
                        List<string> values;
                        var repeatInput = false;
                        DataContainer container = null;
                        try
                        {
                            #region Create Data
                            List<string> Values_Rpt;
                            GetOracleParameterValues(out Values_Rpt, MaintainInfo.ReportStore);
                            ctrlSA.ExecuteMaintainReport(out container, ModuleInfo.ModuleID, ModuleInfo.SubModule, Values_Rpt);

                            var dsResult = container.DataSet;

                            var dt = new DataTable("ReportParameter");
                            foreach (var field in CommonFields)
                            {
                                var name = field.FieldName;
                                var col = new DataColumn(name, FieldUtils.GetType(field.FieldType));
                                dt.Columns.Add(col);
                            }

                            var row = dt.NewRow();
                            dt.Rows.Add(row);
                            dsResult.Tables.Add(dt);

                            foreach (var field in CommonFields)
                            {
                                if (field.ControlType != "GV")
                                {
                                    if (field.FieldFormat != null && this[field.FieldID] == null)
                                    { }
                                    else
                                        row[field.FieldName] = this[field.FieldID];
                                }
                            }

                            #endregion

                            #region Create Report
                            dsResult.WriteXml(Program.strAppStartUpPath + @"\Reports\" + MaintainInfo.ReportName + ".xml", XmlWriteMode.WriteSchema);
                            var report = XtraReport.FromFile(Program.strAppStartUpPath + @"\Reports\" + MaintainInfo.ReportName + ".repx", true);
                            report.XmlDataPath = Program.strAppStartUpPath + @"\Reports\" + MaintainInfo.ReportName + ".xml";

                            string strProcedureName = MaintainInfo.ReportStore.ToString();
                            string strReportName = MaintainInfo.ReportName.ToString();
                            if (strProcedureName.Length > 9)
                            {
                                XRSubreport subReport;
                                XtraReport reportDetail;
                                int n = Int32.Parse(strProcedureName.Substring(9, 2).ToString());
                                for (int i = 1; i <= n; i++)
                                {
                                    reportDetail = XtraReport.FromFile(Program.strAppStartUpPath + @"\Reports\" + MaintainInfo.ReportName + "_" + i + ".repx", true);
                                    reportDetail.XmlDataPath = Program.strAppStartUpPath + @"\Reports\" + MaintainInfo.ReportName + ".xml";
                                    int j = i - 1;
                                    if (j == 0)
                                        reportDetail.DataMember = "Table";
                                    else
                                        reportDetail.DataMember = "Table" + j;
                                    subReport = ((XRSubreport)report.FindControl("subreport" + i, true));

                                    subReport.ReportSource = reportDetail;
                                }
                            }

                            #endregion

                            UnLockUserAction();

                            report.RequestParameters = false;
                            report.ShowPreviewDialog();

                        }
                        catch (Exception ex)
                        {
                            ShowError(ex);
                            UnLockUserAction();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowError(ex);
                }
            }
        }
        //end trungtt
    }
}