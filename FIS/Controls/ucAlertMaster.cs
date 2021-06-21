using DevExpress.XtraBars;
using FIS.AppClient.Utils;
using FIS.Base;
using FIS.Common;
using FIS.Controllers;
using FIS.Entities;
using FIS.Extensions;
using FIS.Utils;
using System;
using System.Data;
using System.ServiceModel;
using System.Threading;

namespace FIS.AppClient.Controls
{
    public partial class ucAlertMaster : ucModule
    {
        #region Property & Members
        private DataRow m_LastResultRow;
        private FaultException m_LastException;

        public int LastAlertCount { get; set; }
        public BarButtonItem StatusLed { get; set; }
        public AlertModuleInfo AlertInfo
        {
            get
            {
                return (AlertModuleInfo)ModuleInfo;
            }
        }
        #endregion

        #region Initialize Methods
        protected override void InitializeGUI(DevExpress.Skins.Skin skin)
        {
            base.InitializeGUI(skin);
            StatusLed = new BarButtonItem
            {
                Alignment = BarItemLinkAlignment.Left,
                Glyph = ThemeUtils.Image16.Images[Language.Icon],
                PaintStyle = BarItemPaintStyle.CaptionGlyph
            };

            StatusLed.ItemClick += StatusLed_ItemClick;

            MainProcess.RegisterButton(StatusLed);
        }

        protected override void InitializeModuleData()
        {
            base.InitializeModuleData();
            CurrentThread = new WorkerThread(delegate
                {
                    while (!CurrentThread.AbortRequest)
                    {
                        try
                        {
                            using (var ctrlSA = new SAController())
                            {
                                DataContainer alertContainer;
                                ctrlSA.ExecuteAlert(out alertContainer, ModuleInfo.ModuleID, ModuleInfo.SubModule);

                                var rows = alertContainer.DataTable.Rows;

                                if (rows.Count > 0)
                                    m_LastResultRow = alertContainer.DataTable.Rows[0];
                            }
                        }
                        catch (FaultException ex)
                        {
                            m_LastException = ex;
                        }
                        catch (Exception ex)
                        {
                            m_LastException = ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_UNKNOWN, ex.Message);
                        }

                        CurrentThread.ExecuteUpdateGUI();
                        Thread.Sleep(AlertInfo.SleepTime);
                    }
                }, MainProcess.GetMainForm());
            CurrentThread.DoUpdateGUI += CurrentThread_DoUpdateGUI;
            CurrentThread.Start();
        }
        #endregion

        #region Events
        /// <summary>
        /// Sự kiện: Click vào nút Alert dưới StatusBar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void StatusLed_ItemClick(object sender, ItemClickEventArgs e)
        {
            AlertClick();
        }

        /// <summary>
        /// Sự kiện: Click cửa số Alert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void alertCtrlMain_AlertClick(object sender, DevExpress.XtraBars.Alerter.AlertClickEventArgs e)
        {
            AlertClick();
            e.AlertForm.Close();
        }

        private void AlertClick()
        {
            try
            {
                MainProcess.ExecuteModule(AlertInfo.CallModuleID, AlertInfo.CallSubModule, true);

                if (!string.IsNullOrEmpty(AlertInfo.ClickStore))
                {
                    using (var ctrlSA = new SAController())
                    {
                        ctrlSA.ExecuteAlertClick(ModuleInfo.ModuleID, ModuleInfo.SubModule);
                        LastAlertCount = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        /// <summary>
        /// Cập nhật giao diện sau khi Store được thực thi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CurrentThread_DoUpdateGUI(object sender, EventArgs e)
        {
            try
            {
                if (m_LastException != null)
                {
                    StatusLed.Glyph = ThemeUtils.Image16.Images[Language.Icon];
                }

                var rf = new RowFormattable(m_LastResultRow);
                if (m_LastResultRow.Table.Columns.Contains(AlertInfo.CountField) &&
                    m_LastResultRow[AlertInfo.CountField] != DBNull.Value)
                {
                    var newAlertCount = int.Parse(rf.Row[AlertInfo.CountField].ToString());

                    if (LastAlertCount != newAlertCount)
                    {
                        alertCtrlMain.Show(
                            MainProcess.GetMainForm(),
                            Language.Title,
                            string.Format(Language.Content, rf),
                            ThemeUtils.Image16.Images[Language.Icon]
                        );

                        LastAlertCount = newAlertCount;
                    }
                }

                StatusLed.Caption = string.Format(Language.Status, rf);
            }
            catch
            {
            }
        }
        #endregion

        public ucAlertMaster()
        {
            InitializeComponent();
            LastAlertCount = 0;
        }

        protected override void DestroyHandle()
        {
            MainProcess.CancelRegisterButton(StatusLed);
            base.DestroyHandle();
        }
    }
}