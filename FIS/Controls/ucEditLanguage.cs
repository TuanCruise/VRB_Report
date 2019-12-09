using System;
using System.Data;
using System.ServiceModel;
using FIS.Common;
using FIS.Controllers;
using FIS.Entities;
using FIS.Utils;
using System.Linq;

namespace FIS.AppClient.Controls
{
    public partial class ucEditLanguage : ucModule
    {
        private DataTable m_LanguageTable;
        public ucEditLanguage()
        {
            InitializeComponent();
        }

        protected override void BuildButtons()
        {
#if DEBUG
            ContextMenuStrip = Context;
#endif
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Execute();
        }

        public override void Execute()
        {
#if DEBUG
            CurrentThread = new WorkerThread(
                delegate
                    {
                        try
                        {
                            LockUserAction();

                            ((ClientEnvironment) App.Environment).GetServerInfo();
                            LangUtils.RefreshLanguage();
                            m_LanguageTable = new DataTable();

                            m_LanguageTable.Columns.Add("LANGID", typeof(string));
                            m_LanguageTable.Columns.Add("LANGNAME", typeof(string));
                            m_LanguageTable.Columns.Add("LANGVALUE", typeof(string));

                            foreach (var lang in LangUtils.CaptureLanguage)
                            {
                                if (!chkIcon.Checked && lang.EndsWith(".Icon")) continue;
                                if (!chkTip.Checked && lang.EndsWith(".Tip")) continue;
                                if (!chkHotkey.Checked && lang.EndsWith(".Hotkey")) continue;

                                var countModType =
                                    (from CodeInfo code in CodeUtils.GetCodes("DEFMOD", "MODTYPE")
                                     where lang.Contains(code.CodeValueName + ".") || lang.Contains(code.CodeValueName + "$")
                                     select code).Count();

                                if (chkModType.Checked && countModType == 0) continue;
                                if (!chkModType.Checked && countModType != 0) continue;

                                var row = m_LanguageTable.NewRow();
                                row["LANGID"] = App.Environment.ClientInfo.LanguageID;
                                row["LANGNAME"] = lang;
                                row["LANGVALUE"] = "";
                                m_LanguageTable.Rows.Add(row);
                            }
                        }
                        catch (Exception ex)
                        {
                            ShowError(ex);
                        }
                        finally
                        {
                            UnLockUserAction();
                        }
                    }, this);

            CurrentThread.ProcessComplete += CurrentThread_ProcessComplete;
            CurrentThread.Start();
#endif
        }

        public override void LockUserAction()
        {
            base.LockUserAction();

            if (!InvokeRequired)
            {
                ShowWaitingBox();
                Enabled = false;
            }
        }

        public override void UnLockUserAction()
        {
            base.UnLockUserAction();

            if (!InvokeRequired)
            {
                HideWaitingBox();
                Enabled = true;
            }
        }
        
        void CurrentThread_ProcessComplete(object sender, EventArgs e)
        {
            mainGrid.DataSource = m_LanguageTable;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
#if DEBUG
            CurrentThread = new WorkerThread(
                delegate
                {
                    try
                    {
                        LockUserAction();

                        foreach (DataRow row in m_LanguageTable.Rows)
                        {
                            if (row["LANGVALUE"] != null && row["LANGVALUE"] != DBNull.Value && !string.IsNullOrEmpty(row["LANGVALUE"].ToString()))
                            {
                                using (var ctrlSA = new SAController())
                                {
                                    ctrlSA.ExecuteSaveLanguage(row["LANGID"].ToString(), row["LANGNAME"].ToString(), row["LANGVALUE"].ToString());
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex);
                    }
                    finally
                    {
                        UnLockUserAction();
                    }
                }, this);

            CurrentThread.ProcessComplete += CurrentThread_ProcessComplete;
            CurrentThread.Start(); 
#endif
        }
    }
}
