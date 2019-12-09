using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;
using FIS.Common;
using FIS.Controllers;
using FIS.Entities;
using FIS.Utils;
using DevExpress.XtraEditors.Controls;
using System.Threading;
using FIS.AppClient.Interface;
using FIS.Base;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;

namespace FIS.AppClient.Controls
{
    public partial class ucBatchProcess : ucModule,
        IParameterFieldSupportedModule
    {
        private Dictionary<int, FaultException> ProcessErrors { get; set; }
        
        public string Markets
        {
            get
            {
                var field = FieldUtils.GetModuleFieldsByName(ModuleInfo.ModuleID, "AMARCODE")[0];
                return (string)this[field.FieldID];
            }
        }

        public ucBatchProcess()
        {
            InitializeComponent();
            ProcessErrors = new Dictionary<int, FaultException>();
        }

        protected override void BuildButtons()
        {
#if DEBUG
            ContextMenuStrip = Context;
            SetupModuleEdit();
            SetupLanguageTool();
#endif
        }

        protected override void InitializeGUI(DevExpress.Skins.Skin skin)
        {
            base.InitializeGUI(skin);
            lstExecuteResult.ImageList = ThemeUtils.Image16;
            //var field = FieldUtils.GetModuleFieldsByName(ModuleInfo.ModuleID, "AMARCODE")[0];
            //lbTitle.Text = Language.Title + field.FieldID.ToString();            
        }

        protected override void BuildFields()
        {
            base.BuildFields();
            BuildBatch();
        }

        private void BuildBatch()
        {
            List<BatchInfo> colBatchs;
            using(var ctrlSA = new SAController())
            {
                ctrlSA.ListBatchInfo(out colBatchs, ModuleInfo.ModuleID);
            }

            chkLstBatchName.BeginUpdate();
            foreach(var batch in colBatchs)
            {
                var caption = Language.GetLabelText(batch.BatchName);
                chkLstBatchName.Items.Add(new CheckedListBoxItem(batch, caption));
            }

            chkLstBatchName.CheckAll();
            chkLstBatchName.EndUpdate();
        }

        private void btnBatch_Click(object sender, EventArgs e)
        {
            Execute();
        }

        public override void Execute()
        {
            base.Execute();

            btnBatch.Enabled = false;
            chkLstBatchName.Enabled = false;
            tabbedControlGroup1.SelectedTabPage = tabBatchLog;
            
            var batchThread = new BatchThread(
                (from CheckedListBoxItem item in chkLstBatchName.CheckedItems
                 select item.Value as BatchInfo).ToArray());

            //batchThread.strMarket = Markets;
            batchThread.EndBatch += delegate
            {
                Invoke(new BatchEndDelegate(BatchEnd));
            };

            batchThread.ProcessComplete += delegate(BatchInfo batch, bool isDone, FaultException ex)
            {
                try
                {
                    if (isDone)
                        Invoke(new BatchCompleteDelegate(BatchComplete), batch);
                    else
                        Invoke(new BatchErrorDelegate(BatchError), batch, ex);
                }
                catch
                {
                }
            };

            batchThread.Start();
        }

        public delegate void BatchEndDelegate();
        public void BatchEnd()
        {
            btnBatch.Enabled = true;
            chkLstBatchName.Enabled = true;
        }

        public delegate void BatchErrorDelegate(BatchInfo batch, FaultException ex);

        public void BatchError(BatchInfo batch, FaultException ex)
        {
            var item = chkLstBatchName.Items[batch];
            if (item != null)
            {
                var index = lstExecuteResult.Items.Add(item.Description, ThemeUtils.GetImage16x16Index("ERROR"));
                ProcessErrors[index] = ex;
            }
        }

        public delegate void BatchCompleteDelegate(BatchInfo batch);

        public void BatchComplete(BatchInfo batch)
        {
            var item = chkLstBatchName.Items[batch];
            if (item != null)
            {
                lstExecuteResult.Items.Add(item.Description, ThemeUtils.GetImage16x16Index("DONE"));
                chkLstBatchName.Items.Remove(item);
            }
        }

        class BatchThread 
        {
            private readonly BatchInfo[] m_Batchs;
            public event OnProcessComplete ProcessComplete;
            public event EventHandler EndBatch;
            public delegate void OnProcessComplete(BatchInfo batch, bool isDone, FaultException ex);
            private Thread thread { get; set; }
            public string strMarket;

            public BatchThread(BatchInfo[] batchs)
            {
                m_Batchs = batchs;
            }

            private void Process()
            {
                if (strMarket == null)
                {
                    foreach (var batch in m_Batchs)
                    {
                        try
                        {                           
                                using (var ctrlSA = new SAController())
                                {
                                    ctrlSA.ExecuteBatch(batch.ModuleID, batch.BatchName);
                                }                                
                            
                            if (ProcessComplete != null)
                            {
                                ProcessComplete(batch, true, null);
                            }
                        }
                        catch (FaultException ex)
                        {
                            ProcessComplete(batch, false, ex);
                        }
                        catch (Exception ex)
                        {
                            ProcessComplete(batch, false, ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_UNKNOWN, ex.Message));
                        }
                    }
                }
                else 
                {
                    foreach (var batch in m_Batchs)
                    {
                        try
                        {                            
                            using (var ctrlSA = new SAController())
                            {
                                ctrlSA.ExecuteBatchMarket(batch.ModuleID, batch.BatchName, strMarket);
                                //ctrlSA.ExecuteBatchMarket(batch.ModuleID, strMarket);
                            }
                            
                            if (ProcessComplete != null)
                            {
                                ProcessComplete(batch, true, null);
                            }                            
                        }
                        catch (FaultException ex)
                        {
                            ProcessComplete(batch, false, ex);
                            return;
                        }
                        catch (Exception ex)
                        {
                            ProcessComplete(batch, false, ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_UNKNOWN, ex.Message));
                            return;
                        }
                    }
                }
                if (EndBatch != null) EndBatch(this, new EventArgs());
            }

            public void Start()
            {
                thread = new Thread(Process);
                thread.Start();
            }
        }

        private void lstExecuteResult_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (ProcessErrors.ContainsKey(lstExecuteResult.SelectedIndex) && ProcessErrors[lstExecuteResult.SelectedIndex] != null)
            {
                frmInfo.ShowError(Language.Title, ProcessErrors[lstExecuteResult.SelectedIndex], this);
            }
        }

        private void chkCheckAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCheckAll.Checked == true)
            {
                chkLstBatchName.CheckAll();
            }
            else
            {
                chkLstBatchName.UnCheckAll();
            }
        }

      
        
    }
}
