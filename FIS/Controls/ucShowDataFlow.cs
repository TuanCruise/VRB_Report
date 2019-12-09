using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Windows;
using FIS.AppClient.Interface;
using FIS.AppClient.Properties;
using FIS.Base;
using FIS.Common;
using FIS.Controllers;
using FIS.Entities;
using FIS.Utils;
using FIS.AppClient.Utils;

namespace FIS.AppClient.Controls
{
    public partial class ucShowDataFlow : ucModule,
        IParameterFieldSupportedModule
    {
        public ucShowDataFlow()
        {
            InitializeComponent();
        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            lbTitle.BackColor = ThemeUtils.BackTitleColor;
            lbTitle.ForeColor = ThemeUtils.TitleColor;
        }

        protected override void BuildButtons()
        {
#if DEBUG
            ContextMenuStrip = Context;
            SetupSaveSize();
#endif
        }

        protected override void InitializeModuleData()
        {
            base.InitializeModuleData();
            try
            {
                reMain.BeginPaintObjects = new List<AbstractPaintObject>();
#if DEBUG
                MainProcess.ForceLoad((string)this["P01"]);
#endif
                var module = ModuleUtils.GetModuleInfo((string)this["P01"], (string)this["P02"]);
                var fields = (from field in FieldUtils.GetModuleFields((string)this["P01"])
                              where field.FieldGroup == CODES.DEFMODFLD.FLDGROUP.PARAMETER
                              select field).ToList();

                if (fields.Count > 0)
                    reMain.BeginPaintObjects.Add(CreatePaintObject("Parameters", fields, module));

                fields = (from field in FieldUtils.GetModuleFields((string)this["P01"])
                          where
                              field.FieldGroup == CODES.DEFMODFLD.FLDGROUP.COMMON &&
                              field.ControlType != CODES.DEFMODFLD.CTRLTYPE.DEFINEDGROUP
                          select field).ToList();

                if (fields.Count > 0)
                    reMain.BeginPaintObjects.Add(CreatePaintObject("Controls", fields, module));

                var endStores = new List<string>();

                switch (module.ModuleType)
                {
                    case CODES.DEFMOD.MODTYPE.MAINTAIN:
                        if (module.SubModule == "MED")
                        {
                            var maintainModule = (MaintainModuleInfo) module;
                            endStores.Add(maintainModule.EditSelectStore);
                            endStores.Add(maintainModule.EditUpdateStore);
                        }
                        if (module.SubModule == "MAD")
                        {
                            var maintainModule = (MaintainModuleInfo)module;
                            endStores.Add(maintainModule.AddSelectStore);
                            endStores.Add(maintainModule.AddInsertStore);
                        }
                        if (module.SubModule == "MVW")
                        {
                            var maintainModule = (MaintainModuleInfo)module;
                            endStores.Add(maintainModule.ViewSelectStore);
                        }
                        break;
                    case CODES.DEFMOD.MODTYPE.STOREEXECUTE:
                        var execModule = ((ExecProcModuleInfo)module);
                        endStores.Add(execModule.ExecuteStore);
                        break;
                }

                reMain.EndPaintObjects = new List<AbstractPaintObject>();
                var count = 0;
                foreach (string store in endStores)
                {
                    if(!string.IsNullOrEmpty(store))
                    {
                        cboStoresName.Properties.Items.Add(store);
                        reMain.EndPaintObjects.Add(CreatePaintObject(store, count == 0 ? Color.Green : Color.Red));
                        count++;
                    }
                }

                lbTitle.Text = string.Format("\"{0}\" Data Flow", LangUtils.TranslateModuleItem(LangType.MODULE_TITLE, module));
            }
            catch(Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        public PaintObject<ModuleInfo, ModuleFieldInfo> CreatePaintObject(string title, List<ModuleFieldInfo> fields, ModuleInfo module)
        {
            var modulePaintObject = new PaintObject<ModuleInfo, ModuleFieldInfo>
            {
                Title = title,
                Parent = module,
                Childs = (from field in fields
                          where field.ControlType != CODES.DEFMODFLD.CTRLTYPE.DEFINEDGROUP
                          select field).ToList(),
                Icon = Resources.Form,
                BeginEnd = BeginEndMode.Begin,
                ConnectorColor = Color.Black
            };

            modulePaintObject.CustomLabel += modulePaintObject_CustomLabel;
            return modulePaintObject;
        }

        static void modulePaintObject_CustomLabel(object sender, PaintObject<ModuleInfo, ModuleFieldInfo>.CustomLabelEventArgs e)
        {
            if(e.Parent.SubModule == CODES.DEFMOD.SUBMOD.MAINTAIN_EDIT)
            {
                e.Text = string.Format("({0}){1}", e.Child.ReadOnlyOnEdit, e.Child.FieldName);
            }
            else if (e.Parent.SubModule == CODES.DEFMOD.SUBMOD.MAINTAIN_ADD)
            {
                e.Text = string.Format("({0}){1}", e.Child.ReadOnlyOnAdd, e.Child.FieldName);
            }
            else if (e.Parent.SubModule == CODES.DEFMOD.SUBMOD.MAINTAIN_VIEW)
            {
                e.Text = string.Format("({0}){1}", e.Child.ReadOnlyOnView, e.Child.FieldName);
            }
            else
            {
                e.Text = string.Format("(RO){0}", e.Child.FieldName);
            }

            if (e.Text.StartsWith("(RO)"))e.ForeColor = Color.Red;
        }

        public PaintObject<string, OracleParameter> CreatePaintObject(string storeName, Color connectorColor)
        {
            using(var ctrlSA = new SAController())
            {
                DataContainer container;
                
                ctrlSA.ExecuteSQL(out container, "SELECT * FROM USER_ARGUMENTS WHERE OBJECT_NAME = '" + storeName + "' ORDER BY POSITION");

                var table = container.DataTable;
                
                var oracleParams = (from DataRow row in table.Rows
                                    select new OracleParameter
                                               {
                                                   ParameterName = (string) row["ARGUMENT_NAME"],
                                                   ParameterType = (OracleParameterType) Enum.Parse(typeof (OracleParameterType), (string) row["IN_OUT"]),
                                                   ParamterDataType = (string)row["DATA_TYPE"]
                                               }).ToList();

                oracleParams.Add(new OracleParameter
                {
                    ParameterName = "[+]New Parameter...",
                    ParameterType = OracleParameterType.NEW
                });

                oracleParams.Add(new OracleParameter
                {
                    ParameterName = "New Cursor",
                    ParameterType = OracleParameterType.NEW
                });

                var storePaintObject = new PaintObject<string, OracleParameter>
                {
                    Title = storeName,
                    Parent = storeName,
                    Childs = (from oracleParam in oracleParams select oracleParam).ToList(),
                    Icon = Resources.Database,
                    BeginEnd = BeginEndMode.End,
                    ConnectorColor = connectorColor
                };

                storePaintObject.CustomLabel += storePaintObject_CustomLabel;
                return storePaintObject;
            }
        }

        static void storePaintObject_CustomLabel(object sender, PaintObject<string, OracleParameter>.CustomLabelEventArgs e)
        {
            e.Text = e.Child.GetLabel();
            if (e.Text.StartsWith("(SYS)") || e.Text.StartsWith("(OUT)"))
            {
                e.ForeColor = Color.DarkGray;
            }
            else if (e.Text.StartsWith("(NEW)"))
            {
                e.ForeColor = Color.Blue;
            }
        }

        private void reMain_UpdateConnect(object sender, ucRelationEditor.UpdateConnectEventArgs e)
        {
            var field = (ModuleFieldInfo)e.EndPoint.Child;
            var param = (OracleParameter)e.BeginPoint.Child;
            if (field.ParameterName == param.ParameterName)
            {
                e.IsConnect = true;
                e.ConnectorColor = e.BeginPoint.Color;
            }
        }

        private void reMain_ItemClicked(object sender, ItemClickedEventArgs e)
        {
            var fieldInfo = e.Position.Child as ModuleFieldInfo;
            if(fieldInfo != null)
            {
                var ucModule = MainProcess.CreateModuleInstance("02904", "MED");
                ucModule["P01"] = fieldInfo.ModuleID;
                ucModule["C01"] = fieldInfo.FieldID;
                ucModule.ShowDialogModule(this);
            }
            else
            {
                var paramInfo = e.Position.Child as OracleParameter;
                if(paramInfo != null)
                {
                    switch(paramInfo.ParameterName)
                    {
                        case "New Cursor":
                            break;
                    }
                }
            }
        }

        private void reMain_ConnectChange(object sender, ucRelationEditor.UpdateConnectEventArgs e)
        {
            var param = e.BeginPoint.Child as OracleParameter;
            var field = e.EndPoint.Child as ModuleFieldInfo;

            if(field != null && param != null && !string.IsNullOrEmpty(field.ParameterName))
            {
                string paramType;
                switch (field.FieldType)
                {
                    case CODES.DEFMODFLD.FLDTYPE.STRING:
                        paramType = "VARCHAR2";
                        break;
                    case CODES.DEFMODFLD.FLDTYPE.DATE:
                    case CODES.DEFMODFLD.FLDTYPE.DATETIME:
                        paramType = "DATE";
                        break;
                    default:
                        paramType = "NUMBER";
                        break;
                }
                
                if (param.ParameterType == OracleParameterType.NEW)
                {
                    foreach(PaintObject<string, OracleParameter> endPoints in reMain.EndPaintObjects)
                    {
                        if(endPoints.Childs.Contains(param))
                        {
                            var insertPosition = endPoints.Childs.Count - 1;
                            endPoints.Childs.Insert(insertPosition,
                                new OracleParameter
                                    {
                                        ParameterName = field.ParameterName,
                                        ParameterType = OracleParameterType.IN,
                                        ParamterDataType = paramType
                                    });
                        }
                    }
                }
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ctrlSA = new SAController())
                {
                    foreach(PaintObject<string, OracleParameter> paintObject in reMain.EndPaintObjects)
                    {
                        DataContainer container;
                        ctrlSA.ExecuteSQL(out container, "SELECT DEV_FN_STORE_BODY('" + paintObject.Title.ToUpper() + "') FROM DUAL");

                        var paramHeads = new List<string>();

                        foreach (var param in paintObject.Childs)
                        {
                            if(param.ParameterType == OracleParameterType.IN)
                            {
                                if (param.ParameterType != OracleParameterType.NEW)
                                {
                                    paramHeads.Add("\n    " + param.ParameterName + " " + param.ParamterDataType);
                                }
                            }
                        }

                        foreach (var param in paintObject.Childs)
                        {
                            if (param.ParameterType == OracleParameterType.OUT)
                            {
                                paramHeads.Add("\n    " + param.ParameterName + " OUT PKG_DATA.CURSOR");
                            }
                        }

                        var replaceHead =
                            "CREATE OR REPLACE\n" +
                            "PROCEDURE " + paintObject.Title + "\n(" +
                            string.Join(",", paramHeads.ToArray()) + "\n";

                        var table = container.DataTable;
                        if (table.Rows.Count > 0 && table.Rows[0][0] != DBNull.Value)
                        {
                            var source = (string)table.Rows[0][0];
                            source = Regex.Replace(source, "^[^)]+", replaceHead);
                            ctrlSA.ExecuteSQL(out container, source);
                        }
                        else
                        {
                            var source =
                                replaceHead + ")\n" +
                                "IS\n" +
                                "BEGIN\n" +
                                "    NULL;\n" +
                                "END;";
                            ctrlSA.ExecuteSQL(out container, source);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void btnDesign_Click(object sender, EventArgs e)
        {
            try
            {
                var ucModule = MainProcess.CreateModuleInstance(STATICMODULE.SQL_MODEL_DESIGNER);
                ucModule["P01"] = cboStoresName.EditValue;
                ucModule.ShowDialogModule(this);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            CloseModule();
        }

        private void reMain_ObjectTitleClicked(object sender, ItemClickedEventArgs e)
        {
            Clipboard.SetText((string)e.Position.Parent);
        }
    }

    public enum OracleParameterType
    {
        IN, OUT, NEW, DESIGN
    }

    public class OracleParameter
    {
        public string ParameterName {get; set;}
        public OracleParameterType ParameterType{get;set;}
        public string ParamterDataType { get; set; }
        public string GetLabel()
        {
            if(ParameterName == "SESSIONINFO_USERNAME")
            {
                return "(SYS)" + ParameterName;
            }
            return "(" + ParameterType + ")" + ParameterName;
        }
    }
}
