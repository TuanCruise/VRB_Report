using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using FIS.AppClient.Interface;
using FIS.Base;
using FIS.Common;
using FIS.Controllers;
using FIS.Entities;
using FIS.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.ServiceModel;

namespace FIS.AppClient.Controls
{
    public partial class ucReportMaster : ucModule,
        IParameterFieldSupportedModule,
        ICommonFieldSupportedModule
    {
        string baseRptPath;
        string ExportPath;
        #region Properties & Members

        public ReportModuleInfo ReportInfo
        {
            get
            {
                return (ReportModuleInfo)ModuleInfo;
            }
        }
        #endregion

        #region Override methods
        protected override void InitializeModuleData()
        {
            base.InitializeModuleData();
            lbTitle.Text = Language.Title;
            if (ActiveControl is ComboBoxEdit)
                LoadComboxListSource((ActiveControl as ComboBoxEdit).Properties);
            baseRptPath = Program.strAppStartUpPath + @"\Reports\" + ReportInfo.ReportName;
            ExportPath = Program.strAppStartUpPath + @"\Reports\Export";
            if (ReportInfo.ModuleID != "02248") btnExport.Visible = false;
        }

        protected override void BuildButtons()
        {
            if (ModuleInfo.UIType == CODES.DEFMOD.UITYPE.POPUP)
            {
#if DEBUG
                SetupContextMenu(reportLayout);
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
                SetupSaveLayout(reportLayout);
#endif
            }
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
        #endregion

        public ucReportMaster()
        {
            InitializeComponent();
            reportLayout.AllowCustomizationMenu = true;
            btnView.Visible = false;
            btnRepair.Visible = false;
#if DEBUG
            btnRepair.Visible = true;
#endif
        }

        public override void Execute()
        {
            try
            {
                if (ModuleInfo.SendEmail == CONSTANTS.Yes)
                {
                    ExecuteReportAndSendMail();
                }
                else
                {
                    using (SAController ctrlSA = new SAController())
                    {
                        DataContainer container;
                        #region Create Data
                        List<string> Values;
                        GetOracleParameterValues(out Values, ReportInfo.StoreName);
                        ctrlSA.ExecuteReport(out container, ModuleInfo.ModuleID, ModuleInfo.SubModule, Values);

                        var dsResult = container.DataSet;

                        var dt = new DataTable(CONSTANTS.REPORT_PARAMETER);
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
                            row[field.FieldName] = this[field.FieldID];
                        }

                        #endregion
                        #region Create Report
                        dsResult.WriteXml(baseRptPath + ".xml", XmlWriteMode.WriteSchema);
                        var report = XtraReport.FromFile(baseRptPath + ".repx", true);
                        report.XmlDataPath = baseRptPath + ".xml";

                        string strProcedureName = ReportInfo.StoreName.ToString();
                        string strReportName = ReportInfo.ReportName.ToString();

                        #endregion
                        report.RequestParameters = false;
                        report.ShowPreviewDialog();
                    }
                }
                //RequireRefresh = true;                
            }
            catch (FaultException ex)
            {
                ShowError(ex);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void ExecuteReportAndSendMail()
        {
            CurrentThread = new WorkerThread(
              delegate
              {
                  LockUserAction();
                  DataContainer container;
                  try
                  {
                      using (SAController ctrlSA = new SAController())
                      {
                          List<string> Values;
                          GetOracleParameterValues(out Values, ReportInfo.StoreName);
                          ctrlSA.ExecuteMaintainReport(out container, ModuleInfo.ModuleID, ModuleInfo.SubModule, Values);
                          // Show success
                          frmInfo.ShowInfo("Gửi Email sao kê", "Đã gửi sao kê thành công !", this);
                          CloseModule();
                      }
                  }
                  catch (FaultException ex)
                  {
                      ShowError(ex);
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
            CurrentThread.Start();
        }

        private void ExportReportToPdf()
        {
            DataContainer container;
            string pStatementDate= DateTime.Now.ToString();
            try
            {
                using (SAController ctrlSA = new SAController())
                {
                    List<string> Values;
                    GetOracleParameterValues(out Values, ReportInfo.StoreName);

                    var fields = GetModuleFields();
                    foreach (var field in fields)
                    {
                        if (!string.IsNullOrEmpty(field.ParameterName) && field.ParameterName.ToUpper()== CONSTANTS.SAOKE_DAY_PARAM)
                        {
                            pStatementDate = this[field.FieldID].Encode(field);
                        }
                    }
                    //Tao tung bao cao mot va ket xuat ra file PDF
                    List<string> paras = new List<string>(Values[0].ToString().Split(','));
                    #region Create Data
                    foreach (var para in paras)
                    {
                        List<string> value = new List<string>();
                        value.Add(para);
                        value.Add(Values[1].ToString());
                        ctrlSA.ExecuteReport(out container, ModuleInfo.ModuleID, ModuleInfo.SubModule, value);

                        var dsResult = container.DataSet;

                        var dt = new DataTable(CONSTANTS.REPORT_PARAMETER);
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
                            row[field.FieldName] = this[field.FieldID];
                        }

                        #endregion
                        #region Create Report
                        dsResult.WriteXml(baseRptPath + ".xml", XmlWriteMode.WriteSchema);
                        var report = XtraReport.FromFile(baseRptPath + ".repx", true);
                        report.XmlDataPath = baseRptPath + ".xml";
                        var fileName = para + "_" + Convert.ToDateTime(pStatementDate, CultureInfo.InvariantCulture).ToShortDateString().Replace("/", "") + ".pdf";

                        System.IO.Directory.CreateDirectory(ExportPath);

                        report.ExportToPdf(ExportPath + "\\" + fileName);
                        #endregion
                    }
                }
            }
            catch (FaultException ex)
            {
                ShowError(ex);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void ViewReport()
        {
            try
            {
                if (File.Exists(baseRptPath + ".xml"))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(baseRptPath + ".xml");
                    if (ds.Tables.Count != 0)
                    {
                        var report = XtraReport.FromFile(baseRptPath + ".repx", true);
                        report.XmlDataPath = baseRptPath + ".xml";

                        string strProcedureName = ReportInfo.StoreName;
                        string strReportName = ReportInfo.ReportName;

                        var dt = ds.Tables[CONSTANTS.REPORT_PARAMETER];

                        report.RequestParameters = false;
                        report.ShowPreviewDialog(UserLookAndFeel.Default);
                    }
                }
            }
            catch (FaultException ex)
            {
                ShowError(ex);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            if (ValidateModule())
            {
                Execute();
            }
        }
        private void btnView_Click(object sender, EventArgs e)
        {
            ViewReport();
        }
        private void ucReportMaster_Load(object sender, EventArgs e)
        {

        }

        private void btnRepair_Click(object sender, EventArgs e)
        {
            try
            {
                var frmDesigner = new XRDesignFormEx();

                if (File.Exists(baseRptPath + ".repx"))
                {
                    var report = XtraReport.FromFile(baseRptPath + ".repx", true);
                    report.XmlDataPath = baseRptPath + ".xml";
                    frmDesigner.OpenReport(report);
                    frmDesigner.FileName = baseRptPath + ".repx";
                    frmDesigner.ShowDialog();
                }
            }
            catch (FaultException ex)
            {
                ShowError(ex);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        public bool ValidateRequire
        {
            get { return true; }
        }

        public LayoutControl CommonLayout
        {
            get { return reportLayout; }
        }

        public string CommonLayoutStoredData
        {
            get { return Language.Layout; }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            CurrentThread = new WorkerThread(
               delegate
               {
                   LockUserAction();
                   try
                   {
                       ExportReportToPdf();
                       // Show success
                       frmInfo.ShowInfo("Kết xuất sao kê ra file PDF", "Đã kết xuất thành công !", this);
                       CloseModule();
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

            CurrentThread.Start();
        }
    }
}
