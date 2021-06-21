using DevExpress.Skins;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using FIS.AppClient.Interface;
using FIS.Base;
using FIS.Controllers;
using FIS.Entities;
using FIS.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.ServiceModel;
using System.Threading;
using System.Windows.Forms;

namespace FIS.AppClient.Controls
{
    public partial class ucChartManager : ucModule,
        ICommonFieldSupportedModule
    {
        public ucChartManager()
        {
            InitializeComponent();
#if DEBUG
            chartLayout.AllowCustomizationMenu = true;
#endif
        }

        #region Properties & Members

        //TuDQ them
        public DataTable dtChartInf { get; set; }
        //End

        public ChartModuleInfo ChartInfo
        {
            get
            {
                return (ChartModuleInfo)ModuleInfo;
            }
        }
        #endregion

        #region Override methods
        protected override void InitializeGUI(Skin skin)
        {
            base.InitializeGUI(skin);
            lbTitle.Appearance.BackColor = skin.Colors.GetColor(CommonColors.Highlight);
            //lbTitle.Appearance.ForeColor = skin.Colors.GetColor(CommonColors.HighligthText);
        }

        protected override void BuildFields()
        {
            base.BuildFields();

            if (Parent is ContainerControl)
                ((ContainerControl)Parent).ActiveControl = chartLayout;
        }

        protected override void BuildButtons()
        {
            //if (ModuleInfo.UIType == CODES.DEFMOD.UITYPE.POPUP)
            //{
            //    var frmOwner = (XtraForm)Parent;
            //    frmOwner.AcceptButton = btnCommit;
#if DEBUG
            //SetupSaveLayout(chartLayout);
            //SetupSaveAllLayout(chartLayout);
            SetupContextMenu(chartLayout);
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
            SetupSaveLayout(chartLayout);
#endif
            //}
            //end trungtt
        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            lbTitle.BackColor = ThemeUtils.BackTitleColor;
            lbTitle.ForeColor = ThemeUtils.TitleColor;
        }

        protected override void InitializeModuleData()
        {
            base.InitializeModuleData();
            lbTitle.Text = Language.Title;

            if (ActiveControl is ComboBoxEdit)
                LoadComboxListSource((ActiveControl as ComboBoxEdit).Properties);
        }

        protected override void SetDefaultValues(List<ModuleFieldInfo> fields)
        {
            base.SetDefaultValues(fields);

            foreach (var field in fields)
            {
                if (field.DefaultValue != null)
                {
                    switch (field.FieldGroup)
                    {
                        case CODES.DEFMODFLD.FLDGROUP.COMMON:
                            this[field.FieldID] = FieldUtils.Convert(field, field.DefaultValue);
                            break;
                    }
                }
            }
        }
        #endregion

        #region Events

        private DateTime lastSwitch;

        private void btnCommit_Click(object sender, EventArgs e)
        {
            //if (ValidateModule())
            //{
            //    if (ChartInfo.ShowAddButton.ToString() == "Y")
            //    {
            //        if (chartThread != null)
            //            chartThread.Abort();
            //        ParameterizedThreadStart starter = new ParameterizedThreadStart(GetDataHelper);
            //        chartThread = new Thread(starter);
            //        object secondsInterval = 1;
            //        chartThread.Start(secondsInterval);
            //    }
            //    else
            //    {
            //        ExecuteNonThread();
            //    }
            //}

            //TuDq THEM
            try
            {
                ExecuteDrawChart();
                //if (ValidateModule())
                //{
                //    List<ModuleFieldInfo> fields;
                //    fields = FieldUtils.GetModuleFields(ModuleInfo.ModuleID);
                //    int countChart = 0;
                //    using (var ctrlSA = new SAController())
                //    {
                //        List<string> Values;
                //        DataSet container;
                //        GetOracleParameterValues(out Values, ChartInfo.ChartDataStore);
                //        ctrlSA.ExecuteChartQuery(out container, ModuleInfo.ModuleID, ModuleInfo.SubModule, Values);
                //        dtChartInf.Clear();
                //        if (container.Tables.Count > 0)
                //        {
                //            chartMain.Series.Clear();
                //            foreach (var field in fields)
                //            {
                //                if (field.FieldGroup == CODES.DEFMODFLD.FLDGROUP.CHART & field.ReadOnlyOnView != "N")
                //                {
                //                    List<string> Values2 = new List<string>();
                //                    var tblResult = container.Tables[countChart];
                //                    if (tblResult.Rows.Count > 0)
                //                    {
                //                        DataSet dsInf = new DataSet();
                //                        Values2.Add(field.ModuleID);
                //                        Values2.Add(field.FieldID);
                //                        ctrlSA.GetChartInf(out dsInf, Values2);
                //                        if (dsInf.Tables.Count > 0)
                //                        {
                //                            for (var i = 0; i < dsInf.Tables[0].Rows.Count; i++)
                //                            {
                //                                var name = Convert.ToString(dsInf.Tables[0].Rows[i]["NAME"]);
                //                                dtChartInf.Rows.Add(name.Substring(name.IndexOf("$") + 1, name.Length - name.IndexOf("$") - 1), dsInf.Tables[0].Rows[i]["VALUE"].ToString());
                //                            }
                //                        }
                //                        switch (field.ControlType)
                //                        {
                //                            case CODES.MODCHART.CHARTTYPE.LINE_CHART:
                //                                buildChartLine_BAR(tblResult, 0, field.FieldType, field.DefaultValue, dsInf, field.FieldName);
                //                                break;
                //                            case CODES.MODCHART.CHARTTYPE.BAR_CHART:
                //                                buildChartLine_BAR(tblResult, 1, field.FieldType, field.DefaultValue, dsInf, field.FieldName);
                //                                break;
                //                            case CODES.MODCHART.CHARTTYPE.STOCK_CHART:
                //                                buildChartLine_BAR(tblResult, 2, field.FieldType, field.DefaultValue, dsInf, field.FieldName);
                //                                break;
                //                        }
                //                    }                                   
                //                }
                //                countChart++;
                //            }
                //        }
                //    }
                //}
                //chartMain.BeginInit();
                //XYDiagram diagraM1 = (XYDiagram)chartMain.Diagram;
                //if (chkLabel.Checked == false)
                //{
                //    foreach (Series seriesx in chartMain.Series)
                //    {
                //        seriesx.Label.Visible = false;
                //    }
                //}
                //if (chkDate.Checked == false)
                //    diagraM1.AxisX.Label.Visible = false;
                //else
                //    diagraM1.AxisX.Label.Visible = true;
                //chartMain.Legend.Font = new Font("Tahoma", 8);
                //chartMain.Legend.Visible = true;
                //chartMain.EndInit();
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
        #endregion

        private void buildChartLine_BAR(DataTable dtData, int type, string format_type, string lengend, DataSet dsInf, string chartName)
        {
            //type =0 : line
            //type =1 : bar
            //type =2 : stock
            var sw1 = new StringWriter();
            dtData.WriteXmlSchema(sw1);
            var schema1 = sw1.ToString();
            //--------------------------------------------------------------------------------
            var dataTables1 = new Dictionary<string, DataTable>();
            var dataTables2 = new Dictionary<string, DataTable>();
            XYDiagramPane panel = new XYDiagramPane();
            SecondaryAxisX axisX1 = new SecondaryAxisX();
            SecondaryAxisY axisY1 = new SecondaryAxisY();
            if (dtData.Columns.Contains("MAXVALUE"))
            {
                axisY1.Range.MaxValue = Int32.Parse(dtData.Rows[0]["MAXVALUE"].ToString());
                axisY1.Range.MinValue = Int32.Parse(dtData.Rows[0]["MINVALUE"].ToString());
            }

            var diagraM1 = new XYDiagram();
            chartMain.BeginInit();
            if (chartMain.Diagram == null)
            {
                chartMain.Diagram = diagraM1;
            }
            else
            {
                diagraM1 = (XYDiagram)chartMain.Diagram;
            }
            //diagraM1.Panes.Clear();
            diagraM1.Panes.Add(panel);
            panel.Name = chartName;
            diagraM1.AxisX.Label.Font = new Font("Tahoma", 7);
            if (format_type == "DTE")
            {
                //diagraM1.AxisX.DateTimeScaleMode = DateTimeScaleMode.Manual;
                axisX1.DateTimeScaleMode = DateTimeScaleMode.Manual;
            }
            else if (format_type == "DTI")
            {
                //diagraM1.AxisX.DateTimeScaleMode = DateTimeScaleMode.Manual;
                //diagraM1.AxisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Second;
                //diagraM1.AxisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Second;
                //diagraM1.AxisX.DateTimeOptions.Format = DateTimeFormat.Custom;
                //diagraM1.AxisX.DateTimeOptions.FormatString = "t";
                axisX1.DateTimeScaleMode = DateTimeScaleMode.Manual;
                axisX1.DateTimeGridAlignment = DateTimeMeasurementUnit.Second;
                axisX1.DateTimeMeasureUnit = DateTimeMeasurementUnit.Second;
                axisX1.DateTimeOptions.Format = DateTimeFormat.Custom;
                axisX1.DateTimeOptions.FormatString = "t";
            }

            diagraM1.DefaultPane.Visible = false;
            diagraM1.AxisX.Visible = false;
            diagraM1.AxisY.Visible = false;
            diagraM1.SecondaryAxesX.Add(axisX1);
            diagraM1.SecondaryAxesY.Add(axisY1);
            if (dsInf.Tables.Count > 0)
            {
                for (var m = 0; m < dsInf.Tables[0].Rows.Count; m++)
                {
                    var name = Convert.ToString(dsInf.Tables[0].Rows[m]["NAME"]);
                    if (chartName + ".Left" == name.Substring(name.IndexOf("$") + 1, name.Length - name.IndexOf("$") - 1))
                    {
                        axisY1.Title.Font = new Font("Tahoma", 8);
                        axisY1.Title.Text = Convert.ToString(dsInf.Tables[0].Rows[m]["VALUE"]);
                        axisY1.Title.Visible = true;
                    }
                }
            }
            axisX1.Alignment = AxisAlignment.Near;
            axisY1.Alignment = AxisAlignment.Near;
            //--------------------------------------------------------------------------------
            foreach (DataRow row in dtData.Rows)
            {

                var SYMBOL = row["CODE"].ToString();
                DataTable temp;
                if (!dataTables1.ContainsKey(SYMBOL))
                {
                    temp = new DataTable();
                    var sr = new StringReader(schema1);
                    temp.ReadXmlSchema(sr);
                    dataTables1.Add(SYMBOL, temp);
                }
                else
                    temp = dataTables1[SYMBOL];

                var newRow = temp.NewRow();
                newRow.ItemArray = row.ItemArray;
                temp.Rows.Add(newRow);
            }

            int i = 0;
            foreach (var pair in dataTables1)
            {
                var SYMBOL = pair.Key;
                var temp = pair.Value;
                if (type == 0)
                {
                    var Series = new Series(SYMBOL, ViewType.Line);
                    Series.ArgumentDataMember = "X";
                    Series.ValueDataMembers.AddRange(new string[] { "VALUE" });

                    if (format_type == "DTE" | format_type == "DTI")
                        Series.ArgumentScaleType = ScaleType.DateTime;
                    else
                        Series.ArgumentScaleType = ScaleType.Numerical;
                    Series.DataSource = temp;
                    ((LineSeriesView)Series.View).LineMarkerOptions.Size = 2;
                    ((LineSeriesView)Series.View).LineMarkerOptions.FillStyle.FillMode = FillMode.Solid;
                    chartMain.Series.Add(Series);
                    ((LineSeriesView)Series.View).Pane = panel;
                    ((LineSeriesView)Series.View).AxisY = axisY1;
                    ((LineSeriesView)Series.View).AxisX = axisX1;
                    if (lengend.Length >= 1 & lengend.Substring(0, 1) == "N")
                        Series.ShowInLegend = false;
                    Series.View.Color = color[i];
                }
                else if (type == 1)
                {
                    var Series = new Series(SYMBOL, ViewType.Bar);
                    Series.ArgumentDataMember = "X";
                    Series.ValueDataMembers.AddRange(new string[] { "VALUE" });

                    if (format_type == "DTE" | format_type == "DTI")
                        Series.ArgumentScaleType = ScaleType.DateTime;
                    else
                        Series.ArgumentScaleType = ScaleType.Numerical;
                    Series.DataSource = temp;
                    ((BarSeriesView)Series.View).FillStyle.FillMode = FillMode.Solid;
                    ((BarSeriesView)Series.View).Pane = panel;
                    ((BarSeriesView)Series.View).AxisY = axisY1;
                    ((BarSeriesView)Series.View).AxisX = axisX1;
                    chartMain.Series.Add(Series);
                    if (lengend.Length >= 1 & lengend.Substring(0, 1) == "N")
                        Series.ShowInLegend = false;
                    Series.View.Color = color[i];
                }
                else if (type == 2)
                {
                    var Series = new Series(SYMBOL, ViewType.Stock);
                    Series.ArgumentDataMember = "X";
                    Series.ValueDataMembers.AddRange(new string[] { "LOWPRICE", "HIGHPRICE", "OPENPRICE", "CLOSEPRICE" });
                    if (format_type == "DTE" | format_type == "DTI")
                        Series.ArgumentScaleType = ScaleType.DateTime;
                    else
                        Series.ArgumentScaleType = ScaleType.Numerical;
                    Series.DataSource = temp;
                    ((StockSeriesView)Series.View).Pane = panel;
                    ((StockSeriesView)Series.View).AxisY = axisY1;
                    ((StockSeriesView)Series.View).AxisX = axisX1;
                    ((StockSeriesView)Series.View).ReductionOptions.Visible = false;
                    chartMain.Series.Add(Series);
                    if (lengend.Length >= 1 & lengend.Substring(0, 1) == "N")
                        Series.ShowInLegend = false;
                }
                i++;
            }

            if (dtData.Columns.Contains("CODE2"))
            {
                SecondaryAxisY axisY2 = new SecondaryAxisY();
                {
                    axisY2.Range.MaxValue = Int32.Parse(dtData.Rows[0]["MAXVALUE2"].ToString());
                    axisY2.Range.MinValue = Int32.Parse(dtData.Rows[0]["MINVALUE2"].ToString());
                }
                diagraM1.SecondaryAxesY.Add(axisY2);
                if (dsInf.Tables.Count > 0)
                {
                    for (var n = 0; n < dsInf.Tables[0].Rows.Count; n++)
                    {
                        var name = Convert.ToString(dsInf.Tables[0].Rows[n]["NAME"]);
                        if (chartName + ".Right" == name.Substring(name.IndexOf("$") + 1, name.Length - name.IndexOf("$") - 1))
                        {
                            axisY2.Title.Font = new Font("Tahoma", 8);
                            axisY2.Title.Text = Convert.ToString(dsInf.Tables[0].Rows[n]["VALUE"]);
                            axisY2.Title.Visible = true;
                        }
                    }
                }
                axisY2.Alignment = AxisAlignment.Far;
                axisY2.GridLines.Visible = false;
                foreach (DataRow row in dtData.Rows)
                {

                    var SYMBOL = row["CODE2"].ToString();
                    DataTable temp;
                    if (!dataTables2.ContainsKey(SYMBOL))
                    {
                        temp = new DataTable();
                        var sr = new StringReader(schema1);
                        temp.ReadXmlSchema(sr);
                        dataTables2.Add(SYMBOL, temp);
                    }
                    else
                        temp = dataTables2[SYMBOL];

                    var newRow = temp.NewRow();
                    newRow.ItemArray = row.ItemArray;
                    temp.Rows.Add(newRow);
                }

                int j = 0;
                foreach (var pair in dataTables2)
                {
                    var SYMBOL = pair.Key;
                    var temp = pair.Value;
                    if (dtData.Rows[0]["TYPE_CHART2"].ToString() == "0")
                    {
                        var Series = new Series(SYMBOL, ViewType.Line);
                        Series.ArgumentDataMember = "X";
                        Series.ValueDataMembers.AddRange(new string[] { "VALUE2" });
                        if (format_type == "DTE" | format_type == "DTI")
                            Series.ArgumentScaleType = ScaleType.DateTime;
                        else
                            Series.ArgumentScaleType = ScaleType.Numerical;
                        Series.DataSource = temp;
                        ((LineSeriesView)Series.View).LineMarkerOptions.Size = 2;
                        ((LineSeriesView)Series.View).LineMarkerOptions.FillStyle.FillMode = FillMode.Solid;
                        chartMain.Series.Add(Series);
                        ((LineSeriesView)Series.View).Pane = panel;
                        ((LineSeriesView)Series.View).AxisX = axisX1;
                        ((LineSeriesView)Series.View).AxisY = axisY2;
                        if (lengend.Length >= 2 & lengend.Substring(1, 1) == "N")
                            Series.ShowInLegend = false;
                        Series.View.Color = color2[j];
                        j++;
                    }
                    else if (type == 1)
                    {
                        var Series = new Series(SYMBOL, ViewType.Bar);
                        Series.ArgumentDataMember = "X";
                        Series.ValueDataMembers.AddRange(new string[] { "VALUE2" });
                        if (format_type == "DTE" | format_type == "DTI")
                            Series.ArgumentScaleType = ScaleType.DateTime;
                        else
                            Series.ArgumentScaleType = ScaleType.Numerical;
                        Series.DataSource = temp;
                        ((BarSeriesView)Series.View).FillStyle.FillMode = FillMode.Solid;
                        chartMain.Series.Add(Series);
                        ((BarSeriesView)Series.View).Pane = panel;
                        ((BarSeriesView)Series.View).AxisX = axisX1;
                        ((BarSeriesView)Series.View).AxisY = axisY2;
                        if (lengend.Length >= 2 & lengend.Substring(1, 1) == "N")
                            Series.ShowInLegend = false;
                        Series.View.Color = color[j];
                        j++;
                    }
                    else if (type == 2)
                    {
                        var Series = new Series(SYMBOL, ViewType.Stock);
                        Series.ArgumentDataMember = "X";
                        Series.ValueDataMembers.AddRange(new string[] { "LOWPRICE", "HIGHPRICE", "OPENPRICE", "CLOSEPRICE" });
                        if (format_type == "DTE" | format_type == "DTI")
                            Series.ArgumentScaleType = ScaleType.DateTime;
                        else
                            Series.ArgumentScaleType = ScaleType.Numerical;
                        Series.DataSource = temp;
                        chartMain.Series.Add(Series);
                        ((StockSeriesView)Series.View).Pane = panel;
                        ((StockSeriesView)Series.View).AxisY = axisY2;
                        ((StockSeriesView)Series.View).AxisX = axisX1;
                        if (lengend.Length >= 2 & lengend.Substring(1, 1) == "N")
                            Series.ShowInLegend = false;
                        j++;
                    }
                }
            }

            chartMain.EndInit();
        }

        private void GetDataHelper(object secondsInterval)
        {
            do
            {
                Execute((int)secondsInterval);
                Application.DoEvents();
                Thread.Sleep(Int32.Parse(ChartInfo.SleepTime));
            }
            while (true);
        }

        private delegate void GetDataDelegate(int secondsInterval);
        Color[] color = new Color[] { Color.Blue, Color.Orange, Color.Green, Color.Yellow };
        Color[] color2 = new Color[] { Color.Red, Color.Azure, Color.Black, Color.Brown };
        public void ExecuteDrawChart()
        {
            DataContainer container;
            DataSet containerData;
            try
            {
                using (var ctrlSA = new SAController())
                {
                    List<string> ValuesSeries;
                    GetOracleParameterValues(out ValuesSeries, ChartInfo.ChartSeries);
                    ctrlSA.ExecuteProcedureFillDataset(out container, ChartInfo.ChartSeries, ValuesSeries);
                    DataTable tblResult = container.DataSet.Tables[0];
                    chartMain.Series.Clear();
                    chartMain.Titles.Clear();
                    if (tblResult.Rows.Count > 0)
                    {
                        chartMain.Titles.Add(new ChartTitle());
                        foreach (DataRow row in tblResult.Rows)
                        {
                            List<string> ValuesParam;
                            GetOracleParameterValues(out ValuesParam, ChartInfo.ChartDataStore);
                            ValuesParam[0] = row[0].ToString();
                            ctrlSA.ExecuteChartQuery(out containerData, ModuleInfo.ModuleID, ModuleInfo.SubModule, ValuesParam);
                            //-----------------
                            Series series = new Series(row[1].ToString(), ViewType.Line);

                            series.ArgumentScaleType = ScaleType.DateTime;
                            series.ArgumentDataMember = "TXDATE";
                            series.ValueScaleType = ScaleType.Numerical;
                            series.ValueDataMembers.AddRange(new string[] { "VAL" });
                            series.PointOptions.ValueNumericOptions.Format = NumericFormat.General;
                            series.PointOptions.ValueNumericOptions.Precision = 0;
                            series.DataSource = containerData.Tables[0];

                            if (chkLabel.Checked == true)
                                series.Label.Visible = true;
                            else
                                series.Label.Visible = false;

                            ((LineSeriesView)series.View).LineMarkerOptions.Size = 5;
                            ((LineSeriesView)series.View).LineMarkerOptions.FillStyle.FillMode = FillMode.Solid;
                            chartMain.Series.Add(series);

                            // Access the view-type-specific options of the series.
                            ((LineSeriesView)series.View).LineMarkerOptions.Kind = MarkerKind.Circle;
                            ((LineSeriesView)series.View).LineStyle.DashStyle = DashStyle.Solid;
                        }
                        //Tùy biến trục của đồ thị
                        XYDiagram diagram = (XYDiagram)chartMain.Diagram;
                        diagram.AxisX.Alignment = AxisAlignment.Zero;
                        diagram.AxisX.Label.Angle = -30;
                        diagram.AxisX.Label.Antialiasing = true;
                        //diagram.AxisX.Label.Staggered = true;


                        diagram.AxisY.Range.MaxValue = Int64.Parse(tblResult.Rows[0]["MAXVAL"].ToString());
                        diagram.AxisY.Range.MinValue = Int64.Parse(tblResult.Rows[0]["MINVAL"].ToString());
                        diagram.AxisY.NumericOptions.Precision = 1;
                        diagram.AxisY.Alignment = AxisAlignment.Zero;

                        // Access the type-specific options of the diagram.
                        diagram.EnableAxisXZooming = false;

                        //Tùy biến đồ thị
                        // Hide the legend (if necessary).
                        chartMain.Legend.Font = new Font("Tahoma", 7);
                        chartMain.Legend.Visible = true;

                        // Add a title to the chart (if necessary).
                        chartMain.Titles[0].Text = Language.Title;

                        // Add the chart to the form.
                        chartMain.Dock = DockStyle.Fill;
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

        //Execute Chart Non Thread
        public void ExecuteNonThread()
        {
            //TuDQ them
            // DataTableContainer container;
            DataSet container;
            //end
            try
            {
                var fieldSYMBOL = FieldUtils.GetModuleFieldsByName(ModuleInfo.ModuleID, "SYMBOL");
                if (fieldSYMBOL.Count == 1 && !CheckCountSymbol())
                {
                    throw ErrorUtils.CreateError(FIS.Common.ERR_SYSTEM.ERR_CHART_COUNT_SYMBOL);
                }
                else
                {
                    using (var ctrlSA = new SAController())
                    {
                        List<string> Values;
                        GetOracleParameterValues(out Values, ChartInfo.ChartDataStore);
                        ctrlSA.ExecuteChartQuery(out container, ModuleInfo.ModuleID, ModuleInfo.SubModule, Values);

                        DataTable tblResult = container.Tables[0];
                        if (tblResult.Rows.Count > 0)
                        {
                            chartMain.DataSource = tblResult;
                            chartMain.Series.Clear();

                            #region Case & Draw Chart

                            switch (ChartInfo.ChartType)
                            {
                                case CODES.MODCHART.CHARTTYPE.LINE_CHART:
                                    //KHOI LUONG GIAO DICH
                                    Series volSec = new Series("Khối lượng GD", ViewType.Bar);
                                    volSec.ArgumentDataMember = "DATENO";
                                    volSec.ValueDataMembers.AddRange(new string[] { "VOL" });
                                    volSec.ArgumentScaleType = ScaleType.Numerical;

                                    //GIA TRI GIAO DICH
                                    Series valSec = new Series("Giá trị GD", ViewType.Bar);
                                    valSec.ArgumentDataMember = "DATENO";
                                    valSec.ValueDataMembers.AddRange(new string[] { "VAL" });
                                    valSec.ArgumentScaleType = ScaleType.Numerical;
                                    //HNX-INDEX
                                    Series index = new Series("HNX-INDEX", ViewType.Line);
                                    index.ArgumentDataMember = "DATENO";
                                    index.ValueDataMembers.AddRange(new string[] { "HNXINDEX" });
                                    index.ArgumentScaleType = ScaleType.Numerical;
                                    index.View.Color = Color.Red;
                                    ((LineSeriesView)index.View).LineMarkerOptions.FillStyle.FillMode = FillMode.Solid;
                                    //ADD SERIES VAO CHARTMAIN
                                    chartMain.Series.AddRange(new Series[] { volSec, valSec, index });

                                    //------------------------------------------------------------------------------
                                    //CREATE XYDIAGRAM
                                    XYDiagram diagram = (XYDiagram)chartMain.Diagram;
                                    diagram.Panes.Clear();

                                    //------------------------------------------------------------------------------
                                    diagram.AxisX.Alignment = AxisAlignment.Zero;

                                    SecondaryAxisY myAxisY = new SecondaryAxisY();
                                    diagram.SecondaryAxesY.Clear();
                                    diagram.SecondaryAxesY.Add(myAxisY);
                                    ((LineSeriesView)index.View).AxisY = myAxisY;
                                    myAxisY.GridLines.Visible = true;

                                    myAxisY.Range.MaxValue = Int32.Parse(tblResult.Rows[0]["MAXINDEX"].ToString());
                                    myAxisY.Range.MinValue = Int32.Parse(tblResult.Rows[0]["MININDEX"].ToString());

                                    SecondaryAxisY myAxisY1 = new SecondaryAxisY();
                                    diagram.SecondaryAxesY.Add(myAxisY1);
                                    ((BarSeriesView)valSec.View).AxisY = myAxisY1;
                                    myAxisY1.Alignment = AxisAlignment.Near;
                                    myAxisY1.GridLines.Visible = true;

                                    //--------------------------------------------------------------------------------
                                    //CREATE PANEL
                                    XYDiagramPane myPane = new XYDiagramPane();
                                    diagram.Panes.AddRange(new XYDiagramPane[] { myPane });
                                    ((XYDiagramSeriesViewBase)valSec.View).Pane = myPane;
                                    //---------------------------------------------------------------------------------
                                    //properties DiagramPane
                                    diagram.DefaultPane.SizeMode = PaneSizeMode.UseWeight;
                                    diagram.DefaultPane.Weight = Double.Parse(ChartInfo.SizePane);
                                    //Label OY
                                    diagram.AxisY.Title.Font = new Font("Tahoma", 8);
                                    diagram.AxisY.Title.Text = "Khối lượng (Triệu CP)";
                                    diagram.AxisY.Title.Visible = true;

                                    myAxisY.Title.Font = new Font("Tahoma", 8);
                                    myAxisY.Title.Text = "Chỉ số (HNX-INDEX)";
                                    myAxisY.Title.Visible = true;

                                    myAxisY1.Title.Font = new Font("Tahoma", 8);
                                    myAxisY1.Title.Text = "Giá trị (Tỷ VNĐ)";
                                    myAxisY1.Title.Visible = true;

                                    //--------------------------------------------------------------------------------
                                    diagram.AxisX.Label.Font = new Font("Tahoma", 7);
                                    //--------------------------------------------------------------------------------

                                    chartMain.CustomDrawAxisLabel += delegate (object sender, CustomDrawAxisLabelEventArgs e)
                                    {
                                        if (e.Item.Axis == diagram.AxisX)
                                        {
                                            e.Item.Text = "";
                                            foreach (DataRow rows in tblResult.Rows)
                                            {
                                                //if (rows["DATENO"].ToString() == e.Item.Text)
                                                //    e.Item.Text = rows["DAY"].ToString();
                                                if (rows["DATENO"].ToString() == e.Item.AxisValue.ToString())
                                                    e.Item.Text = rows["DAY"].ToString();
                                            }
                                        }
                                    };
                                    diagram.AxisX.Range.MinValue = Double.Parse(tblResult.Rows[0]["DATENO"].ToString()) - 0.35;
                                    int maxValue = tblResult.Rows.Count - 1;
                                    diagram.AxisX.Range.MaxValue = Double.Parse(tblResult.Rows[maxValue]["DATENO"].ToString()) + 0.35;
                                    //---------------------------------------------------------------------------------
                                    if (chkLabel.Checked == true)
                                    {
                                        volSec.Label.Visible = true;
                                        valSec.Label.Visible = true;
                                        index.Label.Visible = true;
                                    }
                                    else
                                    {
                                        volSec.Label.Visible = false;
                                        valSec.Label.Visible = false;
                                        index.Label.Visible = false;
                                    }
                                    ((LineSeriesView)index.View).LineMarkerOptions.Size = 2;
                                    if (chkDate.Checked == true)
                                    {
                                        diagram.AxisX.Label.Visible = true;
                                    }
                                    else
                                    {
                                        diagram.AxisX.Label.Visible = false;
                                    }

                                    chartMain.Legend.Font = new Font("Tahoma", 8);
                                    break;


                                case CODES.MODCHART.CHARTTYPE.LINE_BAR_BAR_URD_CHART:
                                    var sw = new StringWriter();
                                    tblResult.WriteXmlSchema(sw);
                                    var schema = sw.ToString();
                                    //--------------------------------------------------------------------------------
                                    var dataTables = new Dictionary<string, DataTable>();
                                    //--------------------------------------------------------------------------------
                                    Series valueSeries = null;
                                    Series volumeSeries = null;
                                    //--------------------------------------------------------------------------------
                                    XYDiagramPane pricePanel = new XYDiagramPane();
                                    XYDiagramPane valuePanel = new XYDiagramPane();
                                    XYDiagramPane volumePanel = new XYDiagramPane();
                                    SecondaryAxisY indexAxis = new SecondaryAxisY();
                                    SecondaryAxisY valueAxis = new SecondaryAxisY();
                                    SecondaryAxisY volumeAxis = new SecondaryAxisY();
                                    SecondaryAxisY priceAxis = new SecondaryAxisY();
                                    //------------------------------------------------------------------------------------
                                    //CUSTOM ROLE SECONDARYAXIS
                                    //PRICE
                                    priceAxis.Range.MaxValue = Int32.Parse(tblResult.Rows[0]["MAXPRICE"].ToString());
                                    priceAxis.Range.MinValue = Int32.Parse(tblResult.Rows[0]["MINPRICE"].ToString());
                                    //INDEX
                                    indexAxis.Range.MaxValue = Int32.Parse(tblResult.Rows[0]["MAXINDEX"].ToString());
                                    indexAxis.Range.MinValue = Int32.Parse(tblResult.Rows[0]["MININDEX"].ToString());
                                    //------------------------------------------------------------------------------------
                                    var diagraM = new XYDiagram();
                                    chartMain.BeginInit();
                                    chartMain.Diagram = diagraM;
                                    diagraM.Panes.Clear();
                                    diagraM.Panes.Add(pricePanel);
                                    diagraM.Panes.Add(volumePanel);
                                    diagraM.Panes.Add(valuePanel);

                                    diagraM.AxisX.Label.Font = new Font("Tahoma", 7);

                                    diagraM.DefaultPane.Visible = false;

                                    diagraM.SecondaryAxesY.Clear();
                                    diagraM.SecondaryAxesY.Add(priceAxis);
                                    diagraM.SecondaryAxesY.Add(indexAxis);
                                    diagraM.SecondaryAxesY.Add(valueAxis);
                                    diagraM.SecondaryAxesY.Add(volumeAxis);
                                    indexAxis.GridLines.Visible = true;
                                    //----------------------------------------------------------------------------------------
                                    diagraM.AxisY.Title.Font = new Font("Tahoma", 8);
                                    diagraM.AxisY.Title.Text = "Giá GD (Ngàn VNĐ)";
                                    diagraM.AxisY.Visible = false;

                                    priceAxis.Title.Font = new Font("Tahoma", 8);
                                    priceAxis.Title.Text = "Giá (Ngàn VNĐ)";
                                    priceAxis.Title.Visible = true;
                                    priceAxis.Alignment = AxisAlignment.Near;

                                    indexAxis.Title.Font = new Font("Tahoma", 8);
                                    indexAxis.Title.Text = "Chỉ số (HNX-INDEX)";
                                    indexAxis.Title.Visible = true;
                                    indexAxis.Alignment = AxisAlignment.Far;

                                    valueAxis.Title.Font = new Font("Tahoma", 8);
                                    valueAxis.Title.Text = "GTGD (Tỷ VNĐ)";
                                    valueAxis.Title.Visible = true;
                                    valueAxis.Alignment = AxisAlignment.Near;

                                    volumeAxis.Title.Font = new Font("Tahoma", 8);
                                    volumeAxis.Title.Text = "KLGD (Ngàn CP)";
                                    volumeAxis.Title.Visible = true;
                                    volumeAxis.Alignment = AxisAlignment.Near;
                                    //--------------------------------------------------------------------------------
                                    foreach (DataRow row in tblResult.Rows)
                                    {

                                        var SYMBOL = row["CODE"].ToString();
                                        DataTable temp;
                                        if (!dataTables.ContainsKey(SYMBOL))
                                        {
                                            temp = new DataTable();
                                            var sr = new StringReader(schema);
                                            temp.ReadXmlSchema(sr);
                                            dataTables.Add(SYMBOL, temp);
                                        }
                                        else
                                            temp = dataTables[SYMBOL];

                                        var newRow = temp.NewRow();
                                        newRow.ItemArray = row.ItemArray;
                                        temp.Rows.Add(newRow);
                                    }

                                    int i = 0;
                                    foreach (var pair in dataTables)
                                    {


                                        var SYMBOL = pair.Key;
                                        var temp = pair.Value;
                                        var priceSeries = new Series(SYMBOL, ViewType.Line);
                                        priceSeries.ArgumentDataMember = "DATENO";
                                        priceSeries.ValueDataMembers.AddRange(new string[] { "PRICE" });
                                        priceSeries.ArgumentScaleType = ScaleType.Numerical;
                                        priceSeries.DataSource = temp;
                                        ((LineSeriesView)priceSeries.View).LineMarkerOptions.Size = 2;
                                        ((LineSeriesView)priceSeries.View).LineMarkerOptions.FillStyle.FillMode = FillMode.Solid;
                                        chartMain.Series.Add(priceSeries);
                                        ((LineSeriesView)priceSeries.View).Pane = pricePanel;
                                        ((LineSeriesView)priceSeries.View).AxisY = priceAxis;
                                        priceSeries.ShowInLegend = false;
                                        priceSeries.View.Color = color[i];

                                        valueSeries = new Series(SYMBOL, ViewType.Bar);
                                        valuePanel.Assign(valueSeries);
                                        valueSeries.ArgumentDataMember = "DATENO";
                                        valueSeries.ValueDataMembers.AddRange(new string[] { "VAL" });
                                        valueSeries.ArgumentScaleType = ScaleType.Numerical;
                                        valueSeries.DataSource = temp;
                                        ((BarSeriesView)valueSeries.View).FillStyle.FillMode = FillMode.Solid;
                                        chartMain.Series.Add(valueSeries);
                                        ((BarSeriesView)valueSeries.View).Pane = valuePanel;
                                        ((BarSeriesView)valueSeries.View).AxisY = valueAxis;
                                        //valueSeries.View.Color = GetSeriesColor(chartMain.Series.IndexOf(priceSeries));
                                        valueSeries.View.Color = color[i];
                                        valueSeries.ShowInLegend = false;

                                        volumeSeries = new Series(SYMBOL, ViewType.Bar);
                                        volumePanel.Assign(volumeSeries);
                                        volumeSeries.ArgumentDataMember = "DATENO";
                                        volumeSeries.ValueDataMembers.AddRange(new string[] { "VOL" });
                                        volumeSeries.ArgumentScaleType = ScaleType.Numerical;
                                        volumeSeries.DataSource = temp;
                                        ((BarSeriesView)volumeSeries.View).FillStyle.FillMode = FillMode.Solid;
                                        chartMain.Series.Add(volumeSeries);
                                        ((BarSeriesView)volumeSeries.View).Pane = volumePanel;
                                        ((BarSeriesView)volumeSeries.View).AxisY = volumeAxis;
                                        //volumeSeries.View.Color = GetSeriesColor(chartMain.Series.IndexOf(priceSeries));
                                        volumeSeries.View.Color = color[i];

                                        i++;

                                    }

                                    var indexSeries = new Series("HNX-INDEX", ViewType.Line);
                                    chartMain.Series.Add(indexSeries);
                                    indexSeries.ArgumentDataMember = "DATENO";
                                    indexSeries.ValueDataMembers.AddRange(new string[] { "HNXINDEX" });
                                    indexSeries.ArgumentScaleType = ScaleType.Numerical;
                                    indexSeries.DataSource = tblResult;
                                    ((LineSeriesView)indexSeries.View).LineMarkerOptions.Size = 2;
                                    ((LineSeriesView)indexSeries.View).Pane = pricePanel;
                                    ((LineSeriesView)indexSeries.View).AxisY = indexAxis;
                                    indexSeries.View.Color = Color.Red;
                                    ((LineSeriesView)indexSeries.View).LineMarkerOptions.FillStyle.FillMode = FillMode.Solid;
                                    indexSeries.Assign(indexAxis);
                                    //----------------------------------------------------------------------------------------
                                    chartMain.CustomDrawAxisLabel += delegate (object sender, CustomDrawAxisLabelEventArgs e)
                                    {
                                        if (e.Item.Axis == diagraM.AxisX)
                                        {
                                            foreach (DataRow rows in tblResult.Rows)
                                            {
                                                if (rows["DATENO"].ToString() == e.Item.Text)
                                                    e.Item.Text = rows["DAY"].ToString();
                                            }
                                        }
                                    };
                                    diagraM.AxisX.Range.MinValue = Int32.Parse(tblResult.Rows[0]["DATENO"].ToString()) - 0.35;
                                    int MaxValue = tblResult.Rows.Count - 1;
                                    diagraM.AxisX.Range.MaxValue = Int32.Parse(tblResult.Rows[MaxValue]["DATENO"].ToString()) + 0.35;
                                    //----------------------------------------------------------------------------------------
                                    pricePanel.SizeMode = PaneSizeMode.UseWeight;
                                    pricePanel.Weight = Double.Parse(ChartInfo.SizePane);
                                    //----------------------------------------------------------------------------------------
                                    if (chkLabel.Checked == false)
                                    {
                                        foreach (Series seriesx in chartMain.Series)
                                        {
                                            seriesx.Label.Visible = false;
                                        }
                                    }
                                    if (chkDate.Checked == false)
                                        diagraM.AxisX.Label.Visible = false;
                                    else
                                        diagraM.AxisX.Label.Visible = true;
                                    //----------------------------------------------------------------------------------------
                                    chartMain.Legend.Font = new Font("Tahoma", 8);
                                    chartMain.Legend.Visible = true;

                                    chartMain.EndInit();
                                    break;
                                //TUDQ them
                                case CODES.MODCHART.CHARTTYPE.STOCK_CHART:
                                    var sw1 = new StringWriter();
                                    tblResult.WriteXmlSchema(sw1);
                                    var schema1 = sw1.ToString();
                                    //--------------------------------------------------------------------------------
                                    var dataTables1 = new Dictionary<string, DataTable>();
                                    //--------------------------------------------------------------------------------

                                    Series volumeSeries1 = null;
                                    //--------------------------------------------------------------------------------
                                    XYDiagramPane pricePanel1 = new XYDiagramPane();
                                    XYDiagramPane volumePanel1 = new XYDiagramPane();
                                    SecondaryAxisY volumeAxis1 = new SecondaryAxisY();
                                    SecondaryAxisY priceAxis1 = new SecondaryAxisY();

                                    //PRICE
                                    priceAxis1.Range.MaxValue = Int32.Parse(tblResult.Rows[0]["MAXPRICE"].ToString()) + 2000;
                                    priceAxis1.Range.MinValue = Int32.Parse(tblResult.Rows[0]["MINPRICE"].ToString()) > 1000 ? Int32.Parse(tblResult.Rows[0]["MINPRICE"].ToString()) - 1000 : Int32.Parse(tblResult.Rows[0]["MINPRICE"].ToString());
                                    //--------------------------------------------------------------------------------------
                                    var diagraM1 = new XYDiagram();
                                    chartMain.BeginInit();
                                    chartMain.Diagram = diagraM1;
                                    diagraM1.Panes.Clear();
                                    diagraM1.Panes.Add(pricePanel1);
                                    diagraM1.Panes.Add(volumePanel1);

                                    diagraM1.AxisX.Label.Font = new Font("Tahoma", 7);
                                    diagraM1.AxisX.DateTimeScaleMode = DateTimeScaleMode.Manual;
                                    diagraM1.DefaultPane.Visible = false;

                                    diagraM1.SecondaryAxesY.Clear();
                                    diagraM1.SecondaryAxesY.Add(priceAxis1);
                                    diagraM1.SecondaryAxesY.Add(volumeAxis1);

                                    //----------------------------------------------------------------------------------------
                                    diagraM1.AxisY.Title.Font = new Font("Tahoma", 8);
                                    diagraM1.AxisY.Title.Text = "Giá GD (Ngàn VNĐ)";
                                    diagraM1.AxisY.Visible = false;

                                    priceAxis1.Title.Font = new Font("Tahoma", 8);
                                    priceAxis1.Title.Text = "Giá (Ngàn VNĐ)";
                                    priceAxis1.Title.Visible = true;
                                    priceAxis1.Alignment = AxisAlignment.Near;

                                    volumeAxis1.Title.Font = new Font("Tahoma", 8);
                                    volumeAxis1.Title.Text = "KLGD (Ngàn CP)";
                                    volumeAxis1.Title.Visible = true;
                                    volumeAxis1.Alignment = AxisAlignment.Near;
                                    //series
                                    var priceSeries1 = new Series(tblResult.Rows[0]["Code"].ToString(), ViewType.Stock);
                                    priceSeries1.ArgumentScaleType = ScaleType.DateTime;
                                    //for (var j = 0; j < tblResult.Rows.Count; j++)
                                    //{
                                    //    priceSeries1.Points.Add(new SeriesPoint(DateTime.Parse(tblResult.Rows[j]["TXDATE"].ToString()), new object[] { tblResult.Rows[j]["LOWPRICE"].ToString(), tblResult.Rows[j]["HIGHPRICE"].ToString(), tblResult.Rows[j]["OPENPRICE"].ToString(), tblResult.Rows[j]["CLOSEPRICE"].ToString() }));
                                    //}
                                    priceSeries1.ArgumentDataMember = "TXDATE";
                                    priceSeries1.ValueDataMembers.AddRange(new string[] { "LOWPRICE", "HIGHPRICE", "OPENPRICE", "CLOSEPRICE" });
                                    priceSeries1.DataSource = tblResult;
                                    chartMain.Series.Add(priceSeries1);
                                    ((StockSeriesView)priceSeries1.View).Pane = pricePanel1;
                                    ((StockSeriesView)priceSeries1.View).AxisY = priceAxis1;
                                    ((StockSeriesView)priceSeries1.View).ReductionOptions.Visible = false;
                                    priceSeries1.ShowInLegend = false;
                                    //KHOI LUONG GIAO DICH
                                    volumeSeries1 = new Series(tblResult.Rows[0]["Code"].ToString(), ViewType.Bar);
                                    volumePanel1.Assign(volumeSeries1);
                                    volumeSeries1.ArgumentDataMember = "TXDATE";
                                    volumeSeries1.ValueDataMembers.AddRange(new string[] { "TOTALVOL" });
                                    volumeSeries1.ArgumentScaleType = ScaleType.DateTime;
                                    volumeSeries1.DataSource = tblResult;
                                    chartMain.Series.Add(volumeSeries1);
                                    ((BarSeriesView)volumeSeries1.View).FillStyle.FillMode = FillMode.Solid;
                                    ((BarSeriesView)volumeSeries1.View).Pane = volumePanel1;
                                    ((BarSeriesView)volumeSeries1.View).AxisY = volumeAxis1;
                                    volumeSeries1.View.Color = color[0];
                                    //----------------------------------------------------------------------------------------

                                    //          chartMain.Series.AddRange((new Series[] {priceSeries1,volumeSeries1});
                                    //----------------------------------------------------------------------------------------
                                    if (chkLabel.Checked == false)
                                    {
                                        foreach (Series seriesx in chartMain.Series)
                                        {
                                            seriesx.Label.Visible = false;
                                        }
                                    }
                                    if (chkDate.Checked == false)
                                        diagraM1.AxisX.Label.Visible = false;
                                    else
                                        diagraM1.AxisX.Label.Visible = true;
                                    chartMain.Legend.Font = new Font("Tahoma", 8);
                                    chartMain.Legend.Visible = true;

                                    chartMain.EndInit();

                                    //if (chkLabel.Checked == true) series.Label.Visible = true;
                                    //else series.Label.Visible = false;

                                    //ADD vao chart main
                                    //  chartMain.Series.AddRange(new Series[] { series, volSec1});

                                    // Access the view-type-specific options of the series.
                                    //if (ChartInfo.ChartType == CODES.MODCHART.CHARTTYPE..STOCK_CHART)
                                    //{
                                    //    StockSeriesView myView = (StockSeriesView)series.View;
                                    //    myView.ReductionOptions.Visible = false;
                                    //    myView.LineThickness = 2;
                                    //    myView.LevelLineLength = 0.25;
                                    //}
                                    //else
                                    //{
                                    //    CandleStickSeriesView myView = (CandleStickSeriesView)series.View;
                                    //    myView.ReductionOptions.Visible = false;
                                    //    myView.LineThickness = 1;
                                    //    myView.LevelLineLength = 0.1;

                                    //}

                                    //------------------------------------------------------------------------------
                                    //CREATE XYDIAGRAM
                                    //XYDiagram diagramXY = (XYDiagram)chartMain.Diagram;
                                    // diagramXY.Panes.Clear();

                                    ////------------------------------------------------------------------------------
                                    //diagramXY.AxisX.Alignment = AxisAlignment.Zero;


                                    //diagramXY.AxisY.Range.MaxValue = Int32.Parse(tblResult.Rows[0]["MAXPRICE"].ToString()) + 2000;
                                    //diagramXY.AxisY.Range.MinValue = Int32.Parse(tblResult.Rows[0]["MINPRICE"].ToString())>1000?Int32.Parse(tblResult.Rows[0]["MINPRICE"].ToString())-1000:Int32.Parse(tblResult.Rows[0]["MINPRICE"].ToString());

                                    //if (chkDate.Checked == false)
                                    //    diagramXY.AxisX.Label.Visible = false;
                                    //else
                                    //    diagramXY.AxisX.Label.Visible = true;



                                    break;

                                    //End
                            }
                            #endregion

                            chartMain.Dock = DockStyle.None;
                        }
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
        //End Execute Chart Non Thread

        //Execute Chart with Thread
        public void Execute(int secondsInterval)
        {
            //TuDQ them
            // DataTableContainer container;
            DataSet container;
            //end
            try
            {
                if (this.InvokeRequired)
                {
                    GetDataDelegate getDataDelegate = new GetDataDelegate(Execute);
                    object[] parameters = { secondsInterval };
                    this.Invoke(getDataDelegate, parameters);
                }
                else
                {
                    if (DateTime.Now.Subtract(lastSwitch).Seconds >= secondsInterval)
                    {

                        var fieldSYMBOL = FieldUtils.GetModuleFieldsByName(ModuleInfo.ModuleID, "SYMBOL");
                        if (fieldSYMBOL.Count == 1 && !CheckCountSymbol())
                        {
                            throw ErrorUtils.CreateError(FIS.Common.ERR_SYSTEM.ERR_CHART_COUNT_SYMBOL);
                        }
                        else
                        {
                            using (var ctrlSA = new SAController())
                            {
                                List<string> Values;
                                GetOracleParameterValues(out Values, ChartInfo.ChartDataStore);
                                ctrlSA.ExecuteChartQuery(out container, ModuleInfo.ModuleID, ModuleInfo.SubModule, Values);

                                DataTable tblResult = container.Tables[0];
                                if (tblResult.Rows.Count > 0)
                                {
                                    chartMain.DataSource = tblResult;
                                    chartMain.Series.Clear();

                                    #region Case & Draw Chart
                                    switch (ChartInfo.ChartType)
                                    {
                                        case CODES.MODCHART.CHARTTYPE.LINE_BAR_BAR_DAY_CHART:
                                            var sw1 = new StringWriter();
                                            tblResult.WriteXmlSchema(sw1);
                                            var schema1 = sw1.ToString();
                                            //--------------------------------------------------------------------------------
                                            var dataTables1 = new Dictionary<string, DataTable>();
                                            //--------------------------------------------------------------------------------
                                            Series valueSeries1 = null;
                                            Series volumeSeries1 = null;
                                            //--------------------------------------------------------------------------------
                                            XYDiagramPane pricePanel1 = new XYDiagramPane();
                                            XYDiagramPane valuePanel1 = new XYDiagramPane();
                                            XYDiagramPane volumePanel1 = new XYDiagramPane();
                                            SecondaryAxisY indexAxis1 = new SecondaryAxisY();
                                            SecondaryAxisY valueAxis1 = new SecondaryAxisY();
                                            SecondaryAxisY volumeAxis1 = new SecondaryAxisY();
                                            SecondaryAxisY priceAxis1 = new SecondaryAxisY();

                                            //--------------------------------------------------------------------------------------
                                            //CUSTOM ROLE SECONDARYAXIS
                                            //PRICE
                                            priceAxis1.Range.MaxValue = Int32.Parse(tblResult.Rows[0]["MAXPRICE"].ToString());
                                            priceAxis1.Range.MinValue = Int32.Parse(tblResult.Rows[0]["MINPRICE"].ToString());
                                            //INDEX
                                            indexAxis1.Range.MaxValue = Int32.Parse(tblResult.Rows[0]["MAXINDEX"].ToString());
                                            indexAxis1.Range.MinValue = Int32.Parse(tblResult.Rows[0]["MININDEX"].ToString());
                                            //--------------------------------------------------------------------------------------

                                            var diagraM1 = new XYDiagram();
                                            chartMain.BeginInit();
                                            chartMain.Diagram = diagraM1;
                                            diagraM1.Panes.Clear();
                                            diagraM1.Panes.Add(pricePanel1);
                                            diagraM1.Panes.Add(volumePanel1);
                                            diagraM1.Panes.Add(valuePanel1);

                                            //---------------------------------------------------------------------------------------
                                            diagraM1.AxisX.Label.Font = new Font("Tahoma", 7);

                                            diagraM1.AxisX.DateTimeScaleMode = DateTimeScaleMode.Manual;
                                            diagraM1.AxisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Second;
                                            diagraM1.AxisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Second;
                                            diagraM1.AxisX.DateTimeOptions.Format = DateTimeFormat.Custom;
                                            diagraM1.AxisX.DateTimeOptions.FormatString = "t";

                                            diagraM1.DefaultPane.Visible = false;

                                            diagraM1.SecondaryAxesY.Clear();
                                            diagraM1.SecondaryAxesY.Add(priceAxis1);
                                            diagraM1.SecondaryAxesY.Add(indexAxis1);
                                            diagraM1.SecondaryAxesY.Add(valueAxis1);
                                            diagraM1.SecondaryAxesY.Add(volumeAxis1);
                                            indexAxis1.GridLines.Visible = true;
                                            //----------------------------------------------------------------------------------------
                                            diagraM1.AxisY.Title.Font = new Font("Tahoma", 8);
                                            diagraM1.AxisY.Title.Text = "Giá GD (Ngàn VNĐ)";
                                            diagraM1.AxisY.Visible = false;

                                            priceAxis1.Title.Font = new Font("Tahoma", 8);
                                            priceAxis1.Title.Text = "Giá (Ngàn VNĐ)";
                                            priceAxis1.Title.Visible = true;
                                            priceAxis1.Alignment = AxisAlignment.Near;

                                            indexAxis1.Title.Font = new Font("Tahoma", 8);
                                            indexAxis1.Title.Text = "Chỉ số (HNX-INDEX)";
                                            indexAxis1.Title.Visible = true;
                                            indexAxis1.Alignment = AxisAlignment.Far;

                                            valueAxis1.Title.Font = new Font("Tahoma", 8);
                                            valueAxis1.Title.Text = "GTGD (Triệu VNĐ)";
                                            valueAxis1.Title.Visible = true;
                                            valueAxis1.Alignment = AxisAlignment.Near;

                                            volumeAxis1.Title.Font = new Font("Tahoma", 8);
                                            volumeAxis1.Title.Text = "KLGD (Ngàn CP)";
                                            volumeAxis1.Title.Visible = true;
                                            volumeAxis1.Alignment = AxisAlignment.Near;
                                            //--------------------------------------------------------------------------------
                                            foreach (DataRow row in tblResult.Rows)
                                            {

                                                var SYMBOL = row["CODE"].ToString();
                                                DataTable temp;
                                                if (!dataTables1.ContainsKey(SYMBOL))
                                                {
                                                    temp = new DataTable();
                                                    var sr = new StringReader(schema1);
                                                    temp.ReadXmlSchema(sr);
                                                    dataTables1.Add(SYMBOL, temp);
                                                }
                                                else
                                                    temp = dataTables1[SYMBOL];

                                                var newRow = temp.NewRow();
                                                newRow.ItemArray = row.ItemArray;
                                                temp.Rows.Add(newRow);
                                            }
                                            int i = 0;
                                            foreach (var pair in dataTables1)
                                            {
                                                var SYMBOL = pair.Key;
                                                var temp = pair.Value;
                                                var priceSeries = new Series(SYMBOL, ViewType.Line);
                                                priceSeries.ArgumentDataMember = "DAY";
                                                priceSeries.ValueDataMembers.AddRange(new string[] { "PRICE" });
                                                priceSeries.ArgumentScaleType = ScaleType.DateTime;
                                                priceSeries.DataSource = temp;
                                                ((LineSeriesView)priceSeries.View).LineMarkerOptions.Size = 2;
                                                ((LineSeriesView)priceSeries.View).LineMarkerOptions.FillStyle.FillMode = FillMode.Solid;
                                                chartMain.Series.Add(priceSeries);
                                                ((LineSeriesView)priceSeries.View).Pane = pricePanel1;
                                                ((LineSeriesView)priceSeries.View).AxisY = priceAxis1;
                                                priceSeries.ShowInLegend = false;
                                                priceSeries.View.Color = color[i];

                                                valueSeries1 = new Series(SYMBOL, ViewType.Bar);
                                                valuePanel1.Assign(valueSeries1);
                                                valueSeries1.ArgumentDataMember = "DAY";
                                                valueSeries1.ValueDataMembers.AddRange(new string[] { "VAL" });
                                                valueSeries1.ArgumentScaleType = ScaleType.DateTime;
                                                valueSeries1.DataSource = temp;
                                                ((BarSeriesView)valueSeries1.View).FillStyle.FillMode = FillMode.Solid;
                                                chartMain.Series.Add(valueSeries1);
                                                ((BarSeriesView)valueSeries1.View).Pane = valuePanel1;
                                                ((BarSeriesView)valueSeries1.View).AxisY = valueAxis1;
                                                //valueSeries1.View.Color = GetSeriesColor(chartMain.Series.IndexOf(priceSeries));
                                                valueSeries1.View.Color = color[i];
                                                valueSeries1.ShowInLegend = false;

                                                volumeSeries1 = new Series(SYMBOL, ViewType.Bar);
                                                volumePanel1.Assign(volumeSeries1);
                                                volumeSeries1.ArgumentDataMember = "DAY";
                                                volumeSeries1.ValueDataMembers.AddRange(new string[] { "VOL" });
                                                volumeSeries1.ArgumentScaleType = ScaleType.DateTime;
                                                volumeSeries1.DataSource = temp;
                                                ((BarSeriesView)volumeSeries1.View).FillStyle.FillMode = FillMode.Solid;
                                                chartMain.Series.Add(volumeSeries1);
                                                ((BarSeriesView)volumeSeries1.View).Pane = volumePanel1;
                                                ((BarSeriesView)volumeSeries1.View).AxisY = volumeAxis1;
                                                //volumeSeries1.View.Color = GetSeriesColor(chartMain.Series.IndexOf(priceSeries));
                                                volumeSeries1.View.Color = color[i];
                                                i++;
                                            }

                                            var indexSeries1 = new Series("HNX-INDEX", ViewType.Line);
                                            chartMain.Series.Add(indexSeries1);
                                            indexSeries1.ArgumentDataMember = "DAY";
                                            indexSeries1.ValueDataMembers.AddRange(new string[] { "HNXINDEX" });
                                            indexSeries1.ArgumentScaleType = ScaleType.DateTime;
                                            indexSeries1.DataSource = tblResult;
                                            ((LineSeriesView)indexSeries1.View).LineMarkerOptions.Size = 2;
                                            ((LineSeriesView)indexSeries1.View).Pane = pricePanel1;
                                            ((LineSeriesView)indexSeries1.View).AxisY = indexAxis1;
                                            indexSeries1.View.Color = Color.Red;
                                            ((LineSeriesView)indexSeries1.View).LineMarkerOptions.FillStyle.FillMode = FillMode.Solid;
                                            indexSeries1.Assign(indexAxis1);
                                            //----------------------------------------------------------------------------------------
                                            pricePanel1.SizeMode = PaneSizeMode.UseWeight;
                                            pricePanel1.Weight = Double.Parse(ChartInfo.SizePane);
                                            //----------------------------------------------------------------------------------------
                                            if (chkLabel.Checked == false)
                                            {
                                                foreach (Series seriesx in chartMain.Series)
                                                {
                                                    seriesx.Label.Visible = false;
                                                }
                                            }
                                            if (chkDate.Checked == false)
                                                diagraM1.AxisX.Label.Visible = false;
                                            else
                                                diagraM1.AxisX.Label.Visible = true;
                                            //----------------------------------------------------------------------------------------
                                            chartMain.Legend.Font = new Font("Tahoma", 8);
                                            chartMain.Legend.Visible = true;

                                            chartMain.EndInit();
                                            break;
                                        case CODES.MODCHART.CHARTTYPE.LINE_LINE_BAR_DAY_CHART:
                                            //KHOI LUONG GIAO DICH
                                            Series volSec1 = new Series("Khối lượng GD", ViewType.Bar);
                                            volSec1.ArgumentDataMember = "DAY";
                                            volSec1.ValueDataMembers.AddRange(new string[] { "VOL" });
                                            volSec1.ArgumentScaleType = ScaleType.DateTime;

                                            //GIA TRI GIAO DICH
                                            Series valSec1 = new Series("Giá trị GD", ViewType.Bar);
                                            valSec1.ArgumentDataMember = "DAY";
                                            valSec1.ValueDataMembers.AddRange(new string[] { "VAL" });
                                            valSec1.ArgumentScaleType = ScaleType.DateTime;
                                            //HNX-INDEX
                                            Series index1 = new Series("HNX-INDEX", ViewType.Line);
                                            index1.ArgumentDataMember = "DAY";
                                            index1.ValueDataMembers.AddRange(new string[] { "HNXINDEX" });
                                            index1.ArgumentScaleType = ScaleType.DateTime;
                                            index1.View.Color = Color.Red;
                                            ((LineSeriesView)index1.View).LineMarkerOptions.FillStyle.FillMode = FillMode.Solid;
                                            //ADD SERIES VAO CHARTMAIN
                                            chartMain.Series.AddRange(new Series[] { volSec1, valSec1, index1 });

                                            //------------------------------------------------------------------------------
                                            //CREATE XYDIAGRAM
                                            XYDiagram diagram1 = (XYDiagram)chartMain.Diagram;
                                            diagram1.Panes.Clear();

                                            //------------------------------------------------------------------------------
                                            //diagram1.AxisX.Alignment = AxisAlignment.Zero;

                                            SecondaryAxisY myAxisY5 = new SecondaryAxisY();
                                            diagram1.SecondaryAxesY.Clear();
                                            diagram1.SecondaryAxesY.Add(myAxisY5);
                                            ((LineSeriesView)index1.View).AxisY = myAxisY5;
                                            myAxisY5.GridLines.Visible = true;
                                            myAxisY5.Range.MaxValue = Int32.Parse(tblResult.Rows[0]["MAXINDEX"].ToString());
                                            myAxisY5.Range.MinValue = Int32.Parse(tblResult.Rows[0]["MININDEX"].ToString());

                                            SecondaryAxisY myAxisY6 = new SecondaryAxisY();
                                            diagram1.SecondaryAxesY.Add(myAxisY6);
                                            ((BarSeriesView)valSec1.View).AxisY = myAxisY6;
                                            myAxisY6.Alignment = AxisAlignment.Near;
                                            myAxisY6.GridLines.Visible = true;

                                            //--------------------------------------------------------------------------------
                                            //CREATE PANEL
                                            XYDiagramPane myPane5 = new XYDiagramPane();
                                            diagram1.Panes.AddRange(new XYDiagramPane[] { myPane5 });
                                            ((XYDiagramSeriesViewBase)valSec1.View).Pane = myPane5;
                                            //---------------------------------------------------------------------------------
                                            //properties
                                            diagram1.DefaultPane.SizeMode = PaneSizeMode.UseWeight;
                                            diagram1.DefaultPane.Weight = Double.Parse(ChartInfo.SizePane);
                                            //Label OY
                                            diagram1.AxisY.Title.Font = new Font("Tahoma", 8);
                                            diagram1.AxisY.Title.Text = "Khối lượng (Ngàn CP)";
                                            diagram1.AxisY.Title.Visible = true;

                                            myAxisY5.Title.Font = new Font("Tahoma", 8);
                                            myAxisY5.Title.Text = "Chỉ số (HNX-INDEX)";
                                            myAxisY5.Title.Visible = true;

                                            myAxisY6.Title.Font = new Font("Tahoma", 8);
                                            myAxisY6.Title.Text = "Giá trị (Triệu VNĐ)";
                                            myAxisY6.Title.Visible = true;

                                            //--------------------------------------------------------------------------------
                                            diagram1.AxisX.Label.Font = new Font("Tahoma", 7);

                                            //---------------------------------------------------------------------------------
                                            //Create Zoom
                                            //diagram1.EnableZooming = true;
                                            diagram1.AxisX.DateTimeScaleMode = DateTimeScaleMode.Manual;
                                            diagram1.AxisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Second;
                                            diagram1.AxisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Second;
                                            diagram1.AxisX.DateTimeOptions.Format = DateTimeFormat.Custom;
                                            diagram1.AxisX.DateTimeOptions.FormatString = "t";

                                            //---------------------------------------------------------------------------------
                                            if (chkLabel.Checked == true)
                                            {
                                                volSec1.Label.Visible = true;
                                                valSec1.Label.Visible = true;
                                                index1.Label.Visible = true;
                                            }
                                            else
                                            {
                                                volSec1.Label.Visible = false;
                                                valSec1.Label.Visible = false;
                                                index1.Label.Visible = false;
                                            }
                                            ((LineSeriesView)index1.View).LineMarkerOptions.Size = 1;
                                            if (chkDate.Checked == true)
                                            {
                                                diagram1.AxisX.Label.Visible = true;
                                            }
                                            else
                                            {
                                                diagram1.AxisX.Label.Visible = false;
                                            }

                                            chartMain.Legend.Font = new Font("Tahoma", 8);
                                            break;
                                    }
                                    #endregion

                                    chartMain.Dock = DockStyle.None;
                                }
                            }
                        }
                        lastSwitch = DateTime.Now;
                        Refresh();
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
        // End Execute Chart with Thread

        private Color GetSeriesColor(int seriesIndex)
        {
            Palette currentPallete = chartMain.PaletteRepository[chartMain.PaletteName];
            return currentPallete[seriesIndex].Color;
        }


        private void chartMain_ObjectHotTracked(object sender, HotTrackEventArgs e)
        {
            SeriesPoint point = e.AdditionalObject as SeriesPoint;
            try
            {
                //switch (ChartInfo.ChartType)
                //{
                //    case CODES.MODCHART.CHARTTYPE..LINE_CHART:
                //        if (point != null)
                //        {
                //            DataRowView rowView = (DataRowView)point.Tag;
                //            string strRealVol = String.Format("{0:###,###}", rowView["REALVOL"]);
                //            string strRealVal = String.Format("{0:###,###}", rowView["REALVAL"]);

                //            string s = "Tổng giá trị GD: " + strRealVal + " VND" +
                //                "\r\nTổng khối lượng GD: " + strRealVol + " CP" +
                //                "\r\nHNX-INDEX: " + rowView["HNXINDEX"].ToString() + " điểm" +
                //                "\r\nNgày GD: " + rowView["DAY"].ToString();
                //            toolTipController1.ShowHint(s);
                //        }
                //        else
                //            toolTipController1.HideHint();
                //        break;

                //    case CODES.MODCHART.CHARTTYPE..LINE_BAR_BAR_URD_CHART:
                //        if (point != null)
                //        {
                //            Series series = e.HitInfo.Series as Series;
                //            DataRowView rowView = (DataRowView)point.Tag;
                //            string strRealVol = String.Format("{0:###,###}", rowView["REALVOL"]);
                //            string strRealVal = String.Format("{0:###,###}", rowView["REALVAL"]);
                //            string strRealPrice = String.Format("{0:###,###}", rowView["REALPRICE"]);
                //            string s;
                //            if (series.Name != "HNX-INDEX")
                //            {
                //                    s = "Mã CK: " + rowView["CODE"].ToString() +
                //                    "\r\nGiá GD: " + strRealPrice + " VND" +
                //                    "\r\nGiá trị GD: " + strRealVal + " VND" +
                //                    "\r\nKhối lượng GD: " + strRealVol + " CP" +
                //                    "\r\nHNX-INDEX: " + rowView["HNXINDEX"].ToString() + " điểm" +
                //                    "\r\nNgày GD: " + rowView["DAY"].ToString();
                //            }
                //            else
                //            {
                //                    s = "HNX-INDEX: " + rowView["HNXINDEX"].ToString() + " điểm" +
                //                    "\r\nNgày GD: " + rowView["DAY"].ToString();
                //            }
                //            toolTipController1.ShowHint(s);
                //        }
                //        else
                //            toolTipController1.HideHint();
                //        break;
                //    case CODES.MODCHART.CHARTTYPE..LINE_LINE_BAR_DAY_CHART:
                //        if (point != null)
                //        {
                //            DataRowView rowView = (DataRowView)point.Tag;
                //            string strRealVol = String.Format("{0:###,###}", rowView["REALVOL"]);
                //            string strRealVal = String.Format("{0:###,###}", rowView["REALVAL"]);
                //            string strTime = String.Format("{0:T}", rowView["DAY"]);

                //           string s = "Tổng giá trị GD: " + strRealVal + " VND" +
                //                "\r\nTổng khối lượng GD: " + strRealVol + " CP" +
                //                "\r\nHNX-INDEX: " + rowView["HNXINDEX"].ToString() + " điểm" +
                //                "\r\nThời gian GD: " + strTime;
                //            toolTipController1.ShowHint(s);
                //        }
                //        else
                //            toolTipController1.HideHint();
                //        break;
                //    case CODES.MODCHART.CHARTTYPE..LINE_BAR_BAR_DAY_CHART:
                //        if (point != null)
                //        {
                //            Series series = e.HitInfo.Series as Series;
                //            DataRowView rowView = (DataRowView)point.Tag;
                //            string strRealVol = String.Format("{0:###,###}", rowView["REALVOL"]);
                //            string strRealVal = String.Format("{0:###,###}", rowView["REALVAL"]);
                //            string strRealPrice = String.Format("{0:###,###}", rowView["REALPRICE"]);
                //            string strTime;
                //            string s;
                //            if(series.Name != "HNX-INDEX")
                //            {
                //                strTime = String.Format("{0:T}", rowView["DAY"]);
                //                s = "Mã CK: " + rowView["CODE"].ToString() +
                //                    "\r\nGiá GD: " + strRealPrice + " VND" +
                //                    "\r\nGiá trị GD: " + strRealVal + " VND" +
                //                    "\r\nKhối lượng GD: " + strRealVol + " CP" +
                //                    "\r\nHNX-INDEX: " + rowView["HNXINDEX"].ToString() + " điểm" +
                //                    "\r\nThời gian GD: " + strTime; 
                //            }
                //            else
                //            {
                //                strTime = String.Format("{0:T}", rowView["DAY"]);
                //                s = "HNX-INDEX: " + rowView["HNXINDEX"].ToString() + " điểm" +
                //                      "\r\nThời gian GD: " + strTime; 
                //            }
                //            toolTipController1.ShowHint(s);
                //        }
                //        else
                //            toolTipController1.HideHint();
                //        break;
                //    //TuDQ Them
                //    case CODES.MODCHART.CHARTTYPE..STOCK_CHART:
                //        if (point != null && point.Length == 4)
                //        {
                //            Series series = e.HitInfo.Series as Series;
                //            DataRowView rowView = (DataRowView)point.Tag;
                //            string strLowPrice = String.Format("{0:###,###}", rowView["LOWPRICE"]);
                //            string strHighPrice = String.Format("{0:###,###}", rowView["HIGHPRICE"]);
                //            string strOpenPrice = String.Format("{0:###,###}", rowView["OPENPRICE"]);
                //            string strClosePrice = String.Format("{0:###,###}", rowView["CLOSEPRICE"]);
                //            string s;
                //            s = "Mã CK: " + rowView["CODE"].ToString() +
                //                "\r\nGiá thấp nhất: " + strLowPrice + " VND" +
                //                "\r\nGiá cao nhất: " + strHighPrice + " VND" +
                //                "\r\nGiá mở cửa: " + strOpenPrice + " VND" +
                //                "\r\nGiá đóng cửa: " + strClosePrice + " VND";   
                //            toolTipController1.ShowHint(s);
                //        }
                //        else if (point != null && point.Length == 1)
                //        {
                //            Series series = e.HitInfo.Series as Series;
                //            DataRowView rowView = (DataRowView)point.Tag;
                //            string s;
                //            string strTotalVol = String.Format("{0:###,###}", rowView["TOTALVOL"]);
                //            s = "Khối lượng GD: " + strTotalVol + " CP";
                //            toolTipController1.ShowHint(s);
                //        }
                //        else
                //            toolTipController1.HideHint();
                //        break;
                //        //END
                //}
                if (point != null)
                {
                    DataRowView rowView = (DataRowView)point.Tag;
                    string strName = rowView["NAME"].ToString();
                    string strVAL = String.Format("{0:###,###}", rowView["VAL"]);
                    string[] strArr = (rowView["TXDATE"].ToString()).Split(' ');

                    string s = "Quỹ đầu tư: " + strName +
                        "\r\nGiá trị NAV: " + strVAL +
                        "\r\nThời gian: " + strArr[0];
                    toolTipController1.ShowHint(s);
                    //XYDiagramPane panel = (XYDiagramPane)e.HitInfo.NonDefaultPane;
                    //if (panel != null)
                    //{
                    //    DataRowView rowView = (DataRowView)point.Tag;
                    //    for (var i = 0; i < dtChartInf.Rows.Count; i++)
                    //    {
                    //        if (panel.Name + ".Tooltip" == Convert.ToString(dtChartInf.Rows[i]["NAME"]))
                    //        {
                    //            //string s = "";
                    //            string result = Convert.ToString(dtChartInf.Rows[i]["VALUE"]);
                    //            //string[] temps = value.Split('}');
                    //            //foreach (string temp in temps)
                    //            //{
                    //            //    string[] temps1 = temp.Split('{');
                    //            //    s = s + temp[0];
                    //            //    if (rowView[temp[1]] != null)
                    //            //    { 
                    //            //        bool isNUm
                    //            //    }
                    //            //}
                    //            for (var j = 0; j < rowView.DataView.Table.Columns.Count; j++)
                    //            {
                    //                string temp = Convert.ToString(rowView[j]);
                    //                double num;
                    //                bool isNum = double.TryParse(temp,out num);
                    //                if(isNum) temp = String.Format("{0:###,###}", rowView[j]);
                    //                result = result.Replace("{" + rowView.DataView.Table.Columns[j].ColumnName + "}", temp);
                    //            }
                    //            toolTipController1.ShowHint(result.Replace("\\r\\n",Environment.NewLine));
                    //            break;
                    //        }
                    //    }
                    //}
                    //else toolTipController1.HideHint();
                }
                else toolTipController1.HideHint();
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

        #region Export Chart
        private void btnExport_Click(object sender, EventArgs e)
        {
            var folderDialog = new SaveFileDialog();
            folderDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\ExportChart";
            folderDialog.Filter = "Image Files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            folderDialog.FilterIndex = 1;
            Directory.CreateDirectory(folderDialog.InitialDirectory);
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                chartMain.ExportToImage(folderDialog.FileName, ImageFormat.Jpeg);
            }
        }
        #endregion

        #region Check Count Symbol
        private bool CheckCountSymbol()
        {
            bool result = true;
            int i = 0;

            var field = FieldUtils.GetModuleFieldsByName(ModuleInfo.ModuleID, "SYMBOL")[0];
            string stringInput = this[field.FieldID].ToString();

            char charInput = ',';

            foreach (char c in stringInput)
            {
                if (c == charInput)
                    i++;
            }
            if (i >= 4)
                result = false;
            return result;
        }
        #endregion

        #region ICommonFieldSupportedModule Members

        public bool ValidateRequire
        {
            get { return true; }
        }

        public LayoutControl CommonLayout
        {
            get { return chartLayout; }
        }

        public string CommonLayoutStoredData
        {
            get { return Language.Layout; }
        }

        #endregion

        private void ucChartManager_Load(object sender, EventArgs e)
        {
            dtChartInf = new DataTable();
            dtChartInf.Columns.Add("NAME", typeof(string));
            dtChartInf.Columns.Add("VALUE", typeof(string));
            List<ModuleFieldInfo> fields;
            fields = FieldUtils.GetModuleFields(ModuleInfo.ModuleID);
            foreach (var field in fields)
            {
                if (field.ControlType == CODES.MODCHART.CHARTTYPE.STOCK_CHART && field.ReadOnlyOnView != "N")
                {
                    btnStock.Visible = true;
                    btnCandleStick.Visible = true;
                    btnStock.Visible = true;
                    break;
                }
            }
        }

        private void btnStock_Click(object sender, EventArgs e)
        {
            foreach (Series seriesx in chartMain.Series)
            {
                if (seriesx.View.ToString().Replace(" ", "") == ViewType.CandleStick.ToString())
                {
                    seriesx.ChangeView(ViewType.Stock);
                }
            }
        }

        private void btnCandleStick_Click(object sender, EventArgs e)
        {
            foreach (Series seriesx in chartMain.Series)
            {
                if (seriesx.View.ToString() == ViewType.Stock.ToString())
                {
                    seriesx.ChangeView(ViewType.CandleStick);
                }
            }
        }
    }
}
