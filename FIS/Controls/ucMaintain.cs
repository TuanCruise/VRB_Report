using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
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
        RichEditControl richEdit;
        string filetempname = null;
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
            lnkFileContextMenu.Items.Add("Lưu file", ThemeUtils.Image16.Images["SAVE"]).Click +=
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
                                   ctrlSA.ExecuteProcedureFillDataset(out container, StoreName, values);
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
            if (InvokeRequired)
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

        }

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
                                     case CODES.DEFMOD.SUBMOD.MAINTAIN_VIEW:
                                         try
                                         {
                                             // Lay thong tin module trong tllog
                                             List<string> value = new List<string>();
                                             string v_Submod = null;
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
                                                 ctrlSA.ExecApprove(out container, ModuleInfo.ModuleID, v_Submod, SecID, values);

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
                                                 ctrlSA.ExecApprove(out container, ModuleInfo.ModuleID, v_Submod, SecID, values);

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
                                             ctrlSA.ExecuteStoreProcedure(MaintainInfo.EXECTRANSCTIONSTORE, values1);
                                             repeatInput = false;
                                         }
                                         catch (Exception ex)
                                         {
                                             ShowError(ex);
                                             UnLockUserAction();
                                         }
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

        public override void LockUserAction()
        {
            base.LockUserAction();

            if (!InvokeRequired)
            {
                Enabled = false;
                ShowWaitingBox();
            }
        }

        public override void UnLockUserAction()
        {
            base.UnLockUserAction();

            if (!InvokeRequired)
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

        private void btnGetData_Click(object sender, EventArgs e)
        {

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
    }
}