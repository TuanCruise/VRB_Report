using System;
using System.Collections.Generic;
using System.Data;
using DevExpress.Utils;
using DevExpress.XtraCharts;
using DevExpress.XtraLayout;
using FIS.AppClient.Interface;
using FIS.Base;
using FIS.Controllers;
using FIS.Entities;
using FIS.Extensions;
using FIS.Utils;

namespace FIS.AppClient.Controls
{
    public partial class ucChartMaster : ucModule,
        IParameterFieldSupportedModule,
        ICommonFieldSupportedModule
    {
        public ChartModuleInfo ChartModuleInfo
        {
            get
            {
                return (ChartModuleInfo)ModuleInfo;
            }
        }

        private DataContainer m_ResultContainer;

        public ucChartMaster()
        {
            InitializeComponent();
        }

        protected override void InitializeModuleData()
        {
            base.InitializeModuleData();
            lbTitle.Text = Language.Title;
        }

        protected override void BuildButtons()
        {
#if DEBUG
            SetupContextMenu(commonLayout);
            SetupParameterFields();
            SetupCommonFields();
            SetupLanguageTool();
            SetupSaveLayout(commonLayout);
#endif
        }
        
        public override void Execute()
        {
            if(ValidateModule())
            {
                switch(ChartModuleInfo.ChartType)
                {
                    case CODES.MODCHART.CHARTTYPE.YIELD_CURVE_WITH_FIT_OPTIONS:
                    case CODES.MODCHART.CHARTTYPE.YIELD_CURVE_NO_FIT_OPTIONS:
                        DrawSvenssonChart();
                        break;
                }
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            switch (ChartModuleInfo.ChartType)
            {
                case CODES.MODCHART.CHARTTYPE.YIELD_CURVE_WITH_FIT_OPTIONS:
                case CODES.MODCHART.CHARTTYPE.YIELD_CURVE_NO_FIT_OPTIONS:
                    var fldCurveType = GetModuleFieldByName(CODES.DEFMODFLD.FLDGROUP.COMMON, "YC_CURVETYPE");
                    var curveType = this[fldCurveType.FieldID] as string;
                    if(!string.IsNullOrEmpty(curveType) && mainChart.Series.Count == 3)
                    {
                        switch(curveType)
                        {
                            case CODES.MODCHART.YC_CURVETYPE.FORWARD_RATES_CURVE:
                                mainChart.Series["ForwardRates"].Visible = true;
                                mainChart.Series["ZeroRates"].Visible = false;
                                break;
                            case CODES.MODCHART.YC_CURVETYPE.ZERO_RATES_CURVE:
                                mainChart.Series["ForwardRates"].Visible = false;
                                mainChart.Series["ZeroRates"].Visible = true;
                                break;
                            case CODES.MODCHART.YC_CURVETYPE.ALL_CURVE:
                                mainChart.Series["ForwardRates"].Visible = true;
                                mainChart.Series["ZeroRates"].Visible = true;
                                break;
                        }
                    }
                    break;
            }
        }
        #region YC.Drawing
        public void DrawSvenssonChart()
        {
            LockUserAction();
            CurrentThread = new WorkerThread(delegate(WorkerThread thread)
            {
                try
                {
                    using (var ctrlSA = new SAController())
                    {
                        List<string> values;

                        GetOracleParameterValues(out values, ChartModuleInfo.ChartDataStore);
                        ctrlSA.ExecuteChartMaster(out m_ResultContainer, ModuleInfo.ModuleID, ModuleInfo.SubModule, values);
                    }

                    thread.IsSuccessful = true;
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
        }

        void CurrentThread_ProcessComplete(object sender, EventArgs e)
        {
            var thread = sender as WorkerThread;

            if (thread != null && thread.IsSuccessful)
            {
                var ds = m_ResultContainer.DataSet;
                var ycDataTable = ds.Tables["YCData"];
                var marketDataTable = ds.Tables["MarketData"];
                var minTimeToMarturity = Convert.ToDouble(ds.Tables["ChartDetail"].Rows[0]["MinTimeToMarturity"]);
                var maxTimeToMarturity = Convert.ToDouble(ds.Tables["ChartDetail"].Rows[0]["MaxTimeToMarturity"]);
                var forwardRateVariance = Convert.ToDouble(ds.Tables["ChartDetail"].Rows[0]["ZeroRatesVariance"]);
                var zeroRateVariance = Convert.ToDouble(ds.Tables["ChartDetail"].Rows[0]["ForwardRatesVariance"]);

                mainChart.BeginInit();

                var diagram = new XYDiagram
                {
                    AxisX =
                    {
                        Tickmarks = { MinorVisible = false },
                        GridSpacingAuto = false,
                        GridSpacing = 1,
                        Range =
                        {
                            MinValue = Math.Floor(minTimeToMarturity),
                            MaxValue = Math.Ceiling(maxTimeToMarturity),
                            ScrollingRange =
                            {
                                MinValue = Math.Floor(minTimeToMarturity),
                                MaxValue = Math.Ceiling(maxTimeToMarturity),
                            }
                        }
                    },
                };

                mainChart.Diagram = diagram;
                diagram.EnableAxisXZooming = true;
                diagram.EnableAxisXScrolling = true;

                var series = new Series
                {
                    ArgumentDataMember = "TimeToMaturity",
                    ValueScaleType = ScaleType.Numerical,
                    ArgumentScaleType = ScaleType.Numerical,
                    Label =
                    {
                        Visible = false
                    },
                    View = new PointSeriesView(),
                    DataSource = marketDataTable,
                    Name = "MarketData"
                };
                series.ValueDataMembers.AddRange("YTM");
                mainChart.Series.Add(series);

                AddSplineLine("ForwardRates");
                AddSplineLine("ZeroRates");

                mainChart.Series["MarketData"].LegendText = Language.GetLabelText("MarketDataLegend");
                mainChart.Series["ForwardRates"].LegendText = string.Format(Language.GetLabelText("ForwardRatesLegend"), forwardRateVariance);
                mainChart.Series["ZeroRates"].LegendText = string.Format(Language.GetLabelText("ZeroRatesLegend"), zeroRateVariance);

                mainChart.DataSource = ycDataTable;
                mainChart.EndInit();
                Refresh();
            }
        }

        public void AddSplineLine(string valueFieldName)
        {
            var series = new Series
            {
                ArgumentDataMember = "TimeToMaturity",
                ValueScaleType = ScaleType.Numerical,
                ArgumentScaleType = ScaleType.Numerical,
                Label =
                {
                    Visible = false
                },
                View = new SplineSeriesView
                {
                    LineMarkerOptions =
                    {
                        Size = 3
                    }
                },
                Name = valueFieldName
            };
            series.ValueDataMembers.AddRange(valueFieldName);
            mainChart.Series.Add(series);
        }

        private void mainChart_ObjectHotTracked(object sender, HotTrackEventArgs e)
        {
            var point = e.AdditionalObject as SeriesPoint;
            if (point != null)
            {
                var dataRowView = point.Tag as DataRowView;
                if (dataRowView != null)
                {
                    var superTip = ctrlToolTip.GetSuperTip(mainChart);

                    if (((Series)e.HitInfo.Series).Name == "MarketData")
                    {
                        var superTipItem = superTip.Items[1] as ToolTipItem;
                        if (superTipItem != null)
                        {
                            var fmtRow = new RowFormattable(dataRowView.Row);
                            superTipItem.Text = string.Format(Language.GetLabelText("MarketData"), fmtRow);
                        }
                    }

                    ctrlToolTip.ShowHint(new ToolTipControllerShowEventArgs
                                             {
                                                 SuperTip = superTip,
                                                 ToolTipType = ToolTipType.SuperTip
                                             });
                }
            }
            else
                ctrlToolTip.HideHint();
        }

        #endregion

        public bool ValidateRequire
        {
            get { return true; }
        }

        public LayoutControl CommonLayout
        {
            get { return commonLayout; }
        }

        public string CommonLayoutStoredData
        {
            get { return Language.Layout; }
        }

        public override void LockUserAction()
        {
            base.LockUserAction();

            if (!InvokeRequired)
            {
                ShowWaitingBox();
                ClearChartData();
                Enabled = false;
            }
        }

        public void ClearChartData()
        {
            mainChart.BeginInit();
            mainChart.Series.Clear();
            mainChart.DataSource = null;
            mainChart.EndInit();
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

        private void btnExecute_Click(object sender, EventArgs e)
        {
            Execute();
        }
    }
}
