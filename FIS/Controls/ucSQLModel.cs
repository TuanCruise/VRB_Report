using FIS.AppClient.Interface;
using FIS.AppClient.Properties;
using FIS.Base;
using FIS.Controllers;
using FIS.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace FIS.AppClient.Controls
{
    public partial class ucSQLModel : ucModule, IParameterFieldSupportedModule
    {
        public ucSQLModel()
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


        public PaintObject<string, OracleParameter> CreatePaintObject(string storeName, Color connectorColor)
        {
            using (var ctrlSA = new SAController())
            {
                DataContainer container;

                ctrlSA.ExecuteSQL(out container, "SELECT * FROM USER_ARGUMENTS WHERE OBJECT_NAME = '" + storeName + "' ORDER BY POSITION");

                var table = container.DataTable;

                var oracleParams = (from DataRow row in table.Rows
                                    select new OracleParameter
                                    {
                                        ParameterName = (string)row["ARGUMENT_NAME"],
                                        ParameterType = (OracleParameterType)Enum.Parse(typeof(OracleParameterType), (string)row["IN_OUT"])
                                    }).ToList();

                var storePaintObject = new PaintObject<string, OracleParameter>
                {
                    Title = storeName,
                    Parent = storeName,
                    Childs = (from oracleParam in oracleParams select oracleParam).ToList(),
                    Icon = Resources.Database,
                    BeginEnd = BeginEndMode.Begin,
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

        protected override void InitializeModuleData()
        {
            base.InitializeModuleData();
            try
            {
                lbTitle.Text = string.Format("Store procedure \"{0}\"", this["P01"]);

                GetStoreSource();
                using (var ctrlSA = new SAController())
                {
                    DataContainer container;

                    reMain.BeginPaintObjects = new List<AbstractPaintObject>
                                                   {
                                                       CreatePaintObject((string) this["P01"], Color.Blue)
                                                   };
                    reMain.EndPaintObjects = new List<AbstractPaintObject>();

                    ctrlSA.ExecuteSQL(out container, "SELECT * FROM USER_TABLES");
                    var table = container.DataTable;
                    foreach (DataRow row in table.Rows)
                    {
                        cboTablesName.Properties.Items.Add(row["TABLE_NAME"]);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void cboTablesName_SelectedValueChanged(object sender, EventArgs e)
        {
            using (var ctrlSA = new SAController())
            {
                var tableName = (string)cboTablesName.EditValue;
                reMain.EndPaintObjects.Clear();
                DataContainer container;
                ctrlSA.ExecuteSQL(out container, "SELECT * FROM USER_TAB_COLS WHERE TABLE_NAME = '" + tableName + "' ORDER BY COLUMN_ID");

                var table = container.DataTable;
                var tablePaintObject = new PaintObject<string, ColumnInfo>
                {
                    BeginEnd = BeginEndMode.End,
                    Childs = (from DataRow row in table.Rows
                              select new ColumnInfo
                              {
                                  ColumnType = ColumnType.COLUMN,
                                  ColumnName = (string)row["COLUMN_NAME"]
                              }).ToList(),
                    ConnectorColor = Color.Red,
                    Icon = Resources.Database,
                    Parent = tableName,
                    Title = tableName
                };

                tablePaintObject.CustomLabel += tablePaintObject_CustomLabel;
                reMain.EndPaintObjects.Add(tablePaintObject);
            }
        }

        static void tablePaintObject_CustomLabel(object sender, PaintObject<string, ColumnInfo>.CustomLabelEventArgs e)
        {
            e.Text = "(" + e.Child.ColumnType + ")" + e.Child.ColumnName;
            if (e.Child.ColumnType == ColumnType.FILTER || e.Child.ColumnType == ColumnType.FILTERVAL)
                e.ForeColor = Color.Red;
            else if (e.Child.ColumnType == ColumnType.VALUE)
                e.ForeColor = Color.Blue;
        }

        private void reMain_UpdateConnect(object sender, ucRelationEditor.UpdateConnectEventArgs e)
        {
            var columnInfo = e.BeginPoint.Child as ColumnInfo;
            var oracleParameter = e.EndPoint.Child as OracleParameter;
            if (columnInfo != null && oracleParameter != null && oracleParameter.ParameterName == columnInfo.ParameterName)
            {
                e.IsConnect = true;
                if (columnInfo.ColumnType == ColumnType.FILTER || columnInfo.ColumnType == ColumnType.FILTERVAL)
                {
                    e.ConnectorColor = Color.Red;
                }
                else
                {
                    e.ConnectorColor = Color.Black;
                }
            }
        }

        private void reMain_ItemClicked(object sender, ItemClickedEventArgs e)
        {
            var columnInfo = e.Position.Child as ColumnInfo;
            if (columnInfo == null) return;

            columnInfo.ColumnType = (ColumnType)(((int)columnInfo.ColumnType + 1) % 4);
        }

        private void reMain_ConnectChange(object sender, ucRelationEditor.UpdateConnectEventArgs e)
        {
            var columnInfo = e.BeginPoint.Child as ColumnInfo;
            var oracleParameter = e.EndPoint.Child as OracleParameter;
            if (columnInfo != null && oracleParameter != null)
            {
                columnInfo.ParameterName = oracleParameter.ParameterName;
            }
        }

        private void btnAutoMatch_Click(object sender, EventArgs e)
        {
            foreach (PaintObject<string, OracleParameter> beginObject in reMain.BeginPaintObjects)
            {
                foreach (PaintObject<string, ColumnInfo> endObject in reMain.EndPaintObjects)
                {
                    foreach (var beginChild in beginObject.Childs)
                    {
                        foreach (var endChild in endObject.Childs)
                        {
                            if (beginChild.ParameterName.EndsWith(endChild.ColumnName))
                            {
                                endChild.ParameterName = beginChild.ParameterName;
                            }
                        }
                    }
                }
            }

            reMain.Refresh();
        }

        private void btnCompile_Click(object sender, EventArgs e)
        {
            using (var ctrlSA = new SAController())
            {
                DataContainer container;
                ctrlSA.ExecuteSQL(out container, "CREATE OR REPLACE \r\n" + txtStoreSource.Text.Replace("\r\n", "\n"));
            }
        }

        private void btnAppend_Click(object sender, EventArgs e)
        {
            string txtAppend = null;
            switch (cboScriptType.Text)
            {
                case "SELECT * Query":
                    txtAppend = CreateSelectAllColumnQuery();
                    break;
                case "SELECT Query":
                    txtAppend = CreateSelectQuery();
                    break;
                case "INSERT Query":
                    txtAppend = CreateInsertQuery();
                    break;
                case "DELETE Query":
                    txtAppend = CreateDeleteQuery();
                    break;
                case "UPDATE Query":
                    txtAppend = CreateUpdateQuery();
                    break;
                case "REPLACE Query":
                    txtAppend = CreateSelectQuery();
                    break;
            }

            txtStoreSource.Text = Regex.Replace(txtStoreSource.Text, "END;[^;]*$", txtAppend + "\r\nEND;");
        }

        private string CreateSelectAllColumnQuery()
        {
            var colStrFilter = new List<string>();

            foreach (PaintObject<string, ColumnInfo> endPoint in reMain.EndPaintObjects)
            {
                foreach (var col in endPoint.Childs)
                {
                    if (!string.IsNullOrEmpty(col.ParameterName))
                    {
                        switch (col.ColumnType)
                        {
                            case ColumnType.COLUMN:
                            case ColumnType.FILTER:
                                colStrFilter.Add(string.Format(txtFormatName.Text + " = {1}", col.ColumnName, col.ParameterName));
                                break;
                        }
                    }
                    switch (col.ColumnType)
                    {
                        case ColumnType.FILTERVAL:
                            colStrFilter.Add(string.Format(txtFormatName.Text + " = '{0}'", col.ColumnName));
                            break;
                    }
                }

                if (colStrFilter.Count == 0)
                {
                    return string.Format(@"    OPEN cur FOR
        SELECT
            *
        FROM {0};",
                        endPoint.Title);
                }

                return string.Format(@"    OPEN cur FOR
        SELECT
            *
        FROM {0}
        WHERE
            {1};",
                                     endPoint.Title,
                                     string.Join(" AND\r\n            ", colStrFilter.ToArray()));
            }

            return null;
        }

        private string CreateSelectQuery()
        {
            var colStrSelect = new List<string>();
            var colStrFilter = new List<string>();

            foreach (PaintObject<string, ColumnInfo> endPoint in reMain.EndPaintObjects)
            {
                foreach (var col in endPoint.Childs)
                {
                    if (!string.IsNullOrEmpty(col.ParameterName))
                    {
                        switch (col.ColumnType)
                        {
                            case ColumnType.FILTER:
                                colStrFilter.Add(string.Format(txtFormatName.Text + " = {1}", col.ColumnName, col.ParameterName));
                                break;
                        }
                    }
                    switch (col.ColumnType)
                    {
                        case ColumnType.COLUMN:
                            colStrSelect.Add(string.Format(txtFormatName.Text, col.ColumnName));
                            break;
                        case ColumnType.FILTERVAL:
                            colStrFilter.Add(string.Format(txtFormatName.Text + " = '{0}'", col.ColumnName));
                            break;
                    }
                }

                if (colStrFilter.Count == 0)
                {
                    return string.Format(@"    OPEN cur FOR
        SELECT
            {0}
        FROM {1};",
                        string.Join(",\r\n            ", colStrSelect.ToArray()),
                        endPoint.Title);
                }

                return string.Format(@"    OPEN cur FOR
        SELECT
            {0}
        FROM {1}
        WHERE
            {2};",
                                     string.Join(",\r\n               ", colStrSelect.ToArray()),
                                     endPoint.Title,
                                     string.Join(" AND\r\n            ", colStrFilter.ToArray()));
            }

            return null;
        }

        public string CreateInsertQuery()
        {
            var colStrField = new List<string>();
            var colStrValue = new List<string>();

            foreach (PaintObject<string, ColumnInfo> endPoint in reMain.EndPaintObjects)
            {
                foreach (var col in endPoint.Childs)
                {
                    if (!string.IsNullOrEmpty(col.ParameterName))
                    {
                        switch (col.ColumnType)
                        {
                            case ColumnType.COLUMN:
                                colStrField.Add(string.Format(txtFormatName.Text, col.ColumnName));
                                colStrValue.Add(col.ParameterName);
                                break;
                        }
                    }
                    switch (col.ColumnType)
                    {
                        case ColumnType.VALUE:
                            colStrField.Add(string.Format(txtFormatName.Text, col.ColumnName));
                            colStrValue.Add(string.Format("'{0}'", col.ColumnName));
                            break;
                    }
                }

                return string.Format(
                        @"    INSERT INTO {0}(
        {1}
    )
    VALUES
    (
        {2}
    );",
                    endPoint.Title,
                    string.Join(",\r\n        ", colStrField.ToArray()),
                    string.Join(",\r\n        ", colStrValue.ToArray()));
            }

            return null;
        }

        public string CreateUpdateQuery()
        {
            var colStrUpdate = new List<string>();
            var colStrFilter = new List<string>();

            foreach (PaintObject<string, ColumnInfo> endPoint in reMain.EndPaintObjects)
            {
                foreach (var col in endPoint.Childs)
                {
                    if (!string.IsNullOrEmpty(col.ParameterName))
                    {
                        switch (col.ColumnType)
                        {
                            case ColumnType.COLUMN:
                                colStrUpdate.Add(string.Format(txtFormatName.Text + " = {1}", col.ColumnName, col.ParameterName));
                                break;
                            case ColumnType.FILTER:
                                colStrFilter.Add(string.Format(txtFormatName.Text + " = {1}", col.ColumnName, col.ParameterName));
                                break;
                        }
                    }
                    else
                    {
                        switch (col.ColumnType)
                        {
                            case ColumnType.VALUE:
                                colStrUpdate.Add(string.Format(txtFormatName.Text + " = '{0}'", col.ColumnName));
                                break;
                            case ColumnType.FILTERVAL:
                                colStrFilter.Add(string.Format(txtFormatName.Text + " = '{0}'", col.ColumnName));
                                break;
                        }
                    }
                }

                if (colStrFilter.Count == 0)
                {
                    return string.Format(@"    UPDATE {0} SET
        {1};",
                        endPoint.Title,
                        string.Join(",\r\n    ", colStrUpdate.ToArray()));
                }

                return string.Format(@"    UPDATE {0} SET
        {1}
    WHERE
        {2};",
                                     endPoint.Title,
                                     string.Join(",\r\n        ", colStrUpdate.ToArray()),
                                     string.Join(" AND\r\n        ", colStrFilter.ToArray()));
            }

            return null;
        }

        public string CreateDeleteQuery()
        {
            var colStrFilter = new List<string>();

            foreach (PaintObject<string, ColumnInfo> endPoint in reMain.EndPaintObjects)
            {
                foreach (var col in endPoint.Childs)
                {
                    if (!string.IsNullOrEmpty(col.ParameterName))
                    {
                        switch (col.ColumnType)
                        {
                            case ColumnType.COLUMN:
                            case ColumnType.FILTER:
                                colStrFilter.Add(string.Format(txtFormatName.Text + " = {1}", col.ColumnName, col.ParameterName));
                                break;
                        }
                    }
                    switch (col.ColumnType)
                    {
                        case ColumnType.FILTERVAL:
                            colStrFilter.Add(string.Format(txtFormatName.Text + " = '{0}'", col.ColumnName));
                            break;
                    }
                }

                if (colStrFilter.Count == 0)
                {
                    return string.Format(@"        DELETE FROM {0};",
                        endPoint.Title);
                }

                return string.Format(@"        DELETE FROM {0}
        WHERE
            {1};",
                                     endPoint.Title,
                                     string.Join(" AND\r\n            ", colStrFilter.ToArray()));
            }

            return null;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GetStoreSource();
        }

        private void GetStoreSource()
        {
            using (var ctrlSA = new SAController())
            {
                DataContainer container;
                ctrlSA.ExecuteSQL(out container, "SELECT DEV_FN_STORE_BODY('" + this["P01"] + "') FROM DUAL");

                var table = container.DataTable;
                if (table.Rows.Count > 0 && table.Rows[0][0] != DBNull.Value)
                {
                    txtStoreSource.Text = ((string)table.Rows[0][0]).Replace("\n", "\r\n");
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            CloseModule();
        }
    }

    public enum ColumnType
    {
        COLUMN = 0,
        VALUE = 1,
        FILTER = 2,
        FILTERVAL = 3
    }

    public class ColumnInfo
    {
        public ColumnType ColumnType { get; set; }
        public string ColumnName { get; set; }
        public string ParameterName { get; set; }
    }
}
