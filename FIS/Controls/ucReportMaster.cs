using DevExpress.LookAndFeel;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using FIS.AppClient.Interface;
using FIS.Base;
using FIS.Controllers;
using FIS.Entities;
using FIS.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;


namespace FIS.AppClient.Controls
{
    public partial class ucReportMaster : ucModule,
        IParameterFieldSupportedModule,
        ICommonFieldSupportedModule
    {
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
            if (ModuleInfo.SendEmail == "Y") { btnReport.Text = "Gửi Mail"; }
            if (ActiveControl is ComboBoxEdit)
                LoadComboxListSource((ActiveControl as ComboBoxEdit).Properties);
        }

        //add by TrungTT - 27.7.2010
        protected override void BuildButtons()
        {
            if (ModuleInfo.UIType == CODES.DEFMOD.UITYPE.POPUP)
            {
#if DEBUG
                //SetupSaveLayout(reportLayout);
                //EDIT BY TRUNGTT - 21.04.2011
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
                //END TRUNGTT
#endif
            }
        }
        //end TrungTT
        #endregion

        public ucReportMaster()
        {
            InitializeComponent();

            //add by TrungTT - 27.7.2010 
            reportLayout.AllowCustomizationMenu = true;
            btnView.Visible = false;
            btnRepair.Visible = false;
#if DEBUG
            btnRepair.Visible = true;
#endif
            //end TrungTT
        }

        public override void Execute()
        {

            try
            {
                if (ModuleInfo.SendEmail == "Y")
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
                            row[field.FieldName] = this[field.FieldID];
                        }

                        #endregion

                        #region Create Report
                        dsResult.WriteXml(Program.strAppStartUpPath + @"\Reports\" + ReportInfo.ReportName + ".xml", XmlWriteMode.WriteSchema);
                        var report = XtraReport.FromFile(Program.strAppStartUpPath + @"\Reports\" + ReportInfo.ReportName + ".repx", true);
                        report.XmlDataPath = Program.strAppStartUpPath + @"\Reports\" + ReportInfo.ReportName + ".xml";

                        string strProcedureName = ReportInfo.StoreName.ToString();
                        string strReportName = ReportInfo.ReportName.ToString();
                        //if (strProcedureName.Length > 9)
                        //{
                        //    CreateReportMultiDetail(strProcedureName, strReportName, report, dsResult);
                        //}

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
            DataContainer container;
            try
            {
                using (SAController ctrlSA = new SAController())
                {
                    List<string> Values;
                    GetOracleParameterValues(out Values, ReportInfo.StoreName);
                    ctrlSA.ExecuteMaintainReport(out container, ModuleInfo.ModuleID, ModuleInfo.SubModule, Values);
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
                if (File.Exists(Program.strAppStartUpPath + @"\Reports\" + ReportInfo.ReportName + ".xml"))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(Program.strAppStartUpPath + @"\Reports\" + ReportInfo.ReportName + ".xml");
                    if (ds.Tables.Count != 0)
                    {
                        var report = XtraReport.FromFile(Program.strAppStartUpPath + @"\Reports\" + ReportInfo.ReportName + ".repx", true);
                        report.XmlDataPath = Program.strAppStartUpPath + @"\Reports\" + ReportInfo.ReportName + ".xml";

                        string strProcedureName = ReportInfo.StoreName;
                        string strReportName = ReportInfo.ReportName;
                        if (strProcedureName.Length > 9)
                        {
                            CreateReportMultiDetail(strProcedureName, strReportName, report, ds);
                        }

                        var dt = ds.Tables["ReportParameter"];

                        //foreach (var field in CommonFields)
                        //{
                        //    string name = field.FieldName;
                        //    report.Parameters[name].Value = dt.Columns[name].DefaultValue.Encode(field);
                        //}
                        report.RequestParameters = false;
                        report.ShowPreviewDialog(UserLookAndFeel.Default);
                    }
                    else
                    {
                        //throw ErrorUtils.CreateError(ERR_SYSTEM.ERR_REPORT_WAS_NOT_CREATE);
                    }
                }
                else
                {
                    //throw ErrorUtils.CreateError(ERR_SYSTEM.ERR_REPORT_WAS_NOT_CREATE);
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
        //ADD BY TRUNGTT - 19.7.2010
        private void btnView_Click(object sender, EventArgs e)
        {
            ViewReport();
        }
        //END TRUNGTT
        private void ucReportMaster_Load(object sender, EventArgs e)
        {

        }

        //13.8.2010
        private void btnRepair_Click(object sender, EventArgs e)
        {
            try
            {
                var frmDesigner = new XRDesignFormEx();
                if (File.Exists(Program.strAppStartUpPath + @"\Reports\" + ReportInfo.ReportName + ".repx"))
                {
                    var report = XtraReport.FromFile(Program.strAppStartUpPath + @"\Reports\" + ReportInfo.ReportName + ".repx", true);
                    report.XmlDataPath = Program.strAppStartUpPath + @"\Reports\" + ReportInfo.ReportName + ".xml";
                    //report.ShowDesignerDialog();
                    frmDesigner.OpenReport(report);
                    frmDesigner.FileName = Program.strAppStartUpPath + @"\Reports\" + ReportInfo.ReportName + ".repx";
                    frmDesigner.ShowDialog();
                }
                else
                {
                    //throw ErrorUtils.CreateError(ERR_SYSTEM.ERR_REPORT_WAS_NOT_CREATE);                    
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
        //END TRUNGTT

        //CREATE BY TRUNGTT 25.10.2010
        private void CreateReportMultiDetail(string strProcedureName, string strReportName, XtraReport reportMaster, DataSet ds)
        {
            XRSubreport subReport;
            XtraReport reportDetail;
            int n = Int32.Parse(strProcedureName.Substring(9, 2).ToString());

            if (!strProcedureName.Contains('C'))
            {
                for (int i = 1; i <= n; i++)
                {
                    reportDetail = XtraReport.FromFile(Program.strAppStartUpPath + @"\Reports\" + ReportInfo.ReportName + "_" + i + ".repx", true);
                    reportDetail.XmlDataPath = Program.strAppStartUpPath + @"\Reports\" + ReportInfo.ReportName + ".xml";
                    int j = i - 1;
                    if (j == 0)
                        reportDetail.DataMember = "Table";
                    else
                        reportDetail.DataMember = "Table" + j;
                    subReport = ((XRSubreport)reportMaster.FindControl("subreport" + i, true));

                    subReport.ReportSource = reportDetail;
                }
            }
            else if (strProcedureName.Contains('C'))
            {
                int position = Int32.Parse(strProcedureName.Substring(12, 1).ToString());
                for (int i = 1; i <= n; i++)
                {
                    if (position == i)
                    {
                        string dataMember = null;
                        int j = i - 1;
                        if (j == 0)
                            dataMember = "Table";
                        else
                            dataMember = "Table" + j;
                        if (ds.Tables[0].Rows.Count != 0)
                            DrawChart(reportMaster, ds, "Table");
                    }
                    else
                    {
                        reportDetail = XtraReport.FromFile(Program.strAppStartUpPath + @"\Reports\" + ReportInfo.ReportName + "_" + i + ".repx", true);
                        reportDetail.XmlDataPath = Program.strAppStartUpPath + @"\Reports\" + ReportInfo.ReportName + ".xml";

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
        }

        private void DrawChart(XtraReport reportMaster, DataSet ds, string dataMember)
        {
            int MaxWidth = reportMaster.PageWidth;

            XRChart chartMain = ((XRChart)reportMaster.FindControl("chart1", true));

            chartMain.DataSource = ds;
            chartMain.DataMember = dataMember;

            chartMain.Series.Clear();

            Series volSeries = new Series("Khối lượng giao dịch", ViewType.Bar);
            Series indexSeries = new Series("HNX Index", ViewType.Line);

            chartMain.Series.AddRange(new Series[] { volSeries, indexSeries });

            volSeries.ValueDataMembers.AddRange(new string[] { "BAR" });
            volSeries.ArgumentDataMember = "DATENO";
            volSeries.ArgumentScaleType = ScaleType.Numerical;
            ((BarSeriesView)volSeries.View).FillStyle.FillMode = FillMode.Solid;
            volSeries.View.Color = Color.LightBlue;

            indexSeries.ValueDataMembers.AddRange(new string[] { "LINE" });
            indexSeries.ArgumentDataMember = "DATENO";
            indexSeries.ArgumentScaleType = ScaleType.Numerical;
            ((LineSeriesView)indexSeries.View).LineMarkerOptions.FillStyle.FillMode = FillMode.Solid;
            indexSeries.View.Color = Color.Red;

            // Create two secondary axes, and add them to the chart's Diagram.
            SecondaryAxisY indexAxisY = new SecondaryAxisY();

            indexAxisY.Range.MaxValue = Int32.Parse(ds.Tables[0].Rows[0]["MAXLINE"].ToString());
            indexAxisY.Range.MinValue = Int32.Parse(ds.Tables[0].Rows[0]["MINLINE"].ToString());

            ((XYDiagram)chartMain.Diagram).SecondaryAxesY.Clear();
            ((XYDiagram)chartMain.Diagram).SecondaryAxesY.Add(indexAxisY);

            ((LineSeriesView)indexSeries.View).AxisY = indexAxisY;
            //-------------------------------------------------------------------
            ((XYDiagram)chartMain.Diagram).AxisY.Title.Font = new Font("Tahoma", 8);
            ((XYDiagram)chartMain.Diagram).AxisY.Title.Text = "Khối lượng giao dịch";
            ((XYDiagram)chartMain.Diagram).AxisY.Title.Visible = true;
            ((XYDiagram)chartMain.Diagram).AxisY.Label.EndText = "M";

            indexAxisY.Title.Font = new Font("Tahoma", 8);
            indexAxisY.Title.Text = "HNX Index";
            indexAxisY.Title.Visible = true;

            ((LineSeriesView)indexSeries.View).LineMarkerOptions.Size = 2;
            //-------------------------------------------------------------------
            ((XYDiagram)chartMain.Diagram).AxisX.Label.Angle = -45;
            //-------------------------------------------------------------------
            chartMain.CustomDrawAxisLabel += delegate (object sender, CustomDrawAxisLabelEventArgs e)
            {
                if (e.Item.Axis == ((XYDiagram)chartMain.Diagram).AxisX)
                {
                    foreach (DataRow rows in ds.Tables[dataMember].Rows)
                    {
                        if (rows["DATENO"].ToString() == e.Item.Text)
                            e.Item.Text = rows["NGAY"].ToString();
                    }
                }
            };
            ((XYDiagram)chartMain.Diagram).AxisX.Range.MinValue = Int32.Parse(ds.Tables[dataMember].Rows[0]["DATENO"].ToString()) - 0.35;
            int MaxValue = ds.Tables[dataMember].Rows.Count - 1;
            ((XYDiagram)chartMain.Diagram).AxisX.Range.MaxValue = Int32.Parse(ds.Tables[dataMember].Rows[MaxValue]["DATENO"].ToString()) + 0.35;
            //-------------------------------------------------------------------

            volSeries.Label.Visible = false;
            indexSeries.Label.Visible = false;

            ChartTitle titles = new ChartTitle();
            titles.Text = "CHỈ SỐ THỊ TRƯỜNG, KHỐI LƯỢNG GIAO DỊCH <BR> THEO THỜI GIAN";
            titles.Dock = ChartTitleDockStyle.Top;
            titles.Alignment = StringAlignment.Center;
            titles.Font = new Font("Tahoma", 10);
            chartMain.Titles.Add(titles);

            chartMain.Legend.Visible = true;
            chartMain.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Center;
            chartMain.Legend.AlignmentVertical = LegendAlignmentVertical.BottomOutside;

        }
        //END TRUNGTT
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

        }
    }
}
