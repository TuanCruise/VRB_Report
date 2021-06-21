using DevExpress.Utils;
using DevExpress.XtraCharts;
using DevExpress.XtraLayout;
using FIS.AppClient.Interface;
using FIS.Base;
using FIS.Controllers;
using FIS.Entities;
using FIS.Extensions;
using FIS.Utils;
using System;
using System.Collections.Generic;
using System.Data;

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
            if (ValidateModule())
            {
                switch (ChartModuleInfo.ChartType)
                {
                    case CODES.MODCHART.CHARTTYPE.YIELD_CURVE_WITH_FIT_OPTIONS:
                    case CODES.MODCHART.CHARTTYPE.YIELD_CURVE_NO_FIT_OPTIONS:
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
                    if (!string.IsNullOrEmpty(curveType) && mainChart.Series.Count == 3)
                    {
                        switch (curveType)
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
