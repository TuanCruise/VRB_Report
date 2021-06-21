using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.BandedGrid;
using FIS.AppClient.Interface;
using FIS.Base;
using FIS.Controllers;
using FIS.Entities;
using FIS.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;
namespace FIS.AppClient.Controls
{
    public partial class ucExpression : ucModule, IParameterFieldSupportedModule
    {
        internal DataTable ResultTable { get; set; }
        internal string LastSearchResultKey { get; set; }
        internal string MID { get; set; }
        internal DateTime LastSearchTime { get; set; }
        public GridControl gcMain { get; set; }
        public List<GridBand> Bands { get; set; }
        public List<string[]> listLayout { get; set; }
        public string CurrentExpress = String.Empty;
        public string CurrentExpressEx = string.Empty;
        public ModuleInfo MaintainInfo
        {
            get
            {
                return (ModuleInfo)ModuleInfo;
            }
        }
        public ucExpression()
        {
            InitializeComponent();
        }

        protected override void InitializeGUI(DevExpress.Skins.Skin skin)
        {
            base.InitializeGUI(skin);
            lstExecuteResult.ImageList = ThemeUtils.Image16;
            lbTitle.Text = Language.Title;
        }

        private void ucExpression_Load(object sender, EventArgs e)
        {
            LookUpColumnInfoCollection coll = lookUpEdit1.Properties.Columns;
            // A column to display the ProductID field's values.
            coll.Add(new LookUpColumnInfo("TEXT", 0));
            lookUpEdit1.Properties.SearchMode = SearchMode.AutoComplete;


            LoadData();

        }

        private void LoadData()
        {
            try
            {


                using (var ctrlSA = new SAController())
                {

                    List<string> moduleid = new List<string>();
                    moduleid.Add(ModuleInfo.ModuleID);

                    DataContainer dcType;
                    ctrlSA.ExecuteProcedureFillDataset(out dcType, "sp_MODEXPRESSION_sel", moduleid);
                    DataTable dtType = dcType.DataTable;

                    // Doan nay rat lom dang nhe phai dinh nghia them ModExpression nhung luoi qua
                    List<string> values2 = new List<string>(); ;
                    GetOracleParameterValues(out values2, "sp_list_treeindicatorbyid");
                    List<string> values = new List<string>();


                    DataContainer con;
                    DataContainer con1;

                    if (dtType.Rows.Count > 0)
                    {
                        values.Add(dtType.Rows[0]["TYPE"].ToString());
                        ctrlSA.ExecuteProcedureFillDataset(out con1, "sp_list_treeindicatorbytype", values);
                    }
                    else
                    {
                        if (values2[0] != null)
                        {
                            ctrlSA.ExecuteProcedureFillDataset(out con1, "sp_list_treeindicatorbyid", values2);
                        }
                        else
                            ctrlSA.ExecuteProcedureFillDataset(out con1, "sp_list_treeindicator", values);
                    }


                    ctrlSA.ExecuteProcedureFillDataset(out con, "sp_list_treeindicator_all", values);

                    AssignFieldValuesFromResult(con);
                    DataTable dt = con.DataTable;
                    DataTable dt1 = con1.DataTable;

                    if (dt1.Rows.Count > 0)
                    {
                        lookUpEdit1.Properties.DataSource = dt1;
                        lookUpEdit1.Properties.DisplayMember = "TEXT";
                        lookUpEdit1.Properties.ValueMember = "VALUE";
                        lookUpEdit1.EditValue = dt1.Rows[0]["VALUE"];


                        gridControl1.DataSource = dt;
                        textEdit1.Text = "[" + lookUpEdit1.EditValue + "] == ";
                        lookUpEdit1_EditValueChanged(null, null);
                        gridView1.BestFitColumns();
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
        private void lookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (textEdit1.Text != "")
                textEdit1.Text = "[" + lookUpEdit1.EditValue + "] == ";
            CurrentExpress = "";
            using (var ctrlSA = new SAController())
            {
                string fldid = Regex.Split(textEdit1.Text, "==")[0].Trim().Replace("[", "").Replace("]", "");
                List<string> values = new List<string>();
                values.Add(fldid);
                DataContainer con;
                ctrlSA.ExecuteProcedureFillDataset(out con, "sp_formular_selbyfldid", values);
                AssignFieldValuesFromResult(con);
                DataTable dt = con.DataTable;
                if (dt.Rows.Count > 0)
                {
                    string formula = dt.Rows[0]["FORMULA"].ToString();
                    DataTable dtSource = (DataTable)gridControl1.DataSource;
                    for (int i = 0; i < dtSource.Rows.Count; i++)
                    {
                        formula = formula.Replace("[" + dtSource.Rows[i]["RPTFLDID"].ToString() + "]", "[" + dtSource.Rows[i]["VALUE"].ToString() + "]");
                    }
                    convertExpression(formula);
                    textEdit1.Text = textEdit1.Text + formula;
                }
            }
        }

        private void listBoxControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //string s = textEdit1.SelectedText;
            //if (s != "")
            //{
            //    if (s != "+" && s != "-" && s != "*" && s != "/" && s != "(" && s != ")")
            //    {
            //        if (s.Length > 1)
            //        {
            //            string first = s.Substring(0, 1);
            //            string last = s.Substring(s.Length - 1, 1);
            //            if ((first != "[" && first != "]" && first != "+" && first != "-" && first != "*" && first != "/" && first != "(" && first != ")") && (last != "[" && last != "]" &&  last != "+" && last != "-" && last != "*" && last != "/" && last != "(" && last != ")"))
            //            {
            //                string fldid = Regex.Split(textEdit1.Text, "==")[0].Trim().Replace("[", "").Replace("]", "");
            //                if (s != fldid)
            //                {
            //                    string fr = textEdit1.Text.Substring(textEdit1.SelectionStart - 1, 1);
            //                    string ls = textEdit1.Text.Substring(textEdit1.SelectionStart + textEdit1.SelectionLength , 1);
            //                    if (fr == "[" && ls == "]")
            //                    {
            //                        textEdit1.Text = textEdit1.Text.Substring(0, textEdit1.SelectionStart) + gridView1.GetDataRow(gridView1.FocusedRowHandle)["VALUE"].ToString() + textEdit1.Text.Substring(textEdit1.SelectionStart + textEdit1.SelectionLength, textEdit1.Text.Length - textEdit1.SelectionStart - textEdit1.SelectionLength );                                   
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    if (CurrentExpress == ")")
            //    {
            //    }
            //    else if (CurrentExpress == "=")
            //    {
            //        textEdit1.Text = textEdit1.Text + " [" + gridView1.GetDataRow(gridView1.FocusedRowHandle)["VALUE"].ToString() + "]";
            //        CurrentExpress = gridView1.GetDataRow(gridView1.FocusedRowHandle)["VALUE"].ToString();
            //    }
            //    else if (CurrentExpress != "(" && CurrentExpress != "+" && CurrentExpress != "-" && CurrentExpress != "-" && CurrentExpress != "/" && CurrentExpress != "*")
            //    {
            //        string preStr = textEdit1.Text.Substring(0, textEdit1.Text.LastIndexOf(CurrentExpress));
            //        if (preStr.Substring(preStr.Length - 1, 1) != "[")
            //            preStr = preStr + " [";
            //        textEdit1.Text = preStr + gridView1.GetDataRow(gridView1.FocusedRowHandle)["VALUE"].ToString() + "]";
            //        CurrentExpress = gridView1.GetDataRow(gridView1.FocusedRowHandle)["VALUE"].ToString();
            //    }
            //    else
            //    {
            //        textEdit1.Text = textEdit1.Text + " [" + gridView1.GetDataRow(gridView1.FocusedRowHandle)["VALUE"].ToString() + "]";
            //        CurrentExpress = gridView1.GetDataRow(gridView1.FocusedRowHandle)["VALUE"].ToString();
            //    }
            //}
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            string lastStr = textEdit1.Text.Trim().Substring(textEdit1.Text.Trim().Length - 1, 1);
            RemoveExp(lastStr);
        }

        private void Expression(string exp)
        {
            string s = textEdit1.SelectedText;
            if (s != "")
            {
                if (s == "+" || s == "-" || s == "*" || s == "/")
                {
                    textEdit1.Text = textEdit1.Text.Substring(0, textEdit1.SelectionStart) + exp + textEdit1.Text.Substring(textEdit1.SelectionStart + textEdit1.SelectionLength, textEdit1.Text.Length - textEdit1.SelectionStart - textEdit1.SelectionLength);
                }
            }
            else
            {
                if (CurrentExpress == "" || CurrentExpress == "=" || CurrentExpress == "(")
                {

                }
                else if (CurrentExpress != "+" && CurrentExpress != "-" && CurrentExpress != "-" && CurrentExpress != "/" && CurrentExpress != "*")
                {
                    textEdit1.Text = textEdit1.Text + " " + exp;
                    CurrentExpress = exp;
                }
                else
                {
                    string preStr = textEdit1.Text.Substring(0, textEdit1.Text.LastIndexOf(CurrentExpress));
                    textEdit1.Text = preStr + exp;
                    CurrentExpress = exp;
                }
            }
        }

        private void RemoveExp(string exp)
        {
            if (exp == "=" || exp == "")
            {

            }
            else if ((exp != ")" && exp != "+" && exp != "-" && exp != "-" && exp != "/" && exp != "*") || exp == "(")
            {
                textEdit1.Text = textEdit1.Text.Substring(0, textEdit1.Text.LastIndexOf(CurrentExpress) - 1).Trim();
                CurrentExpress = textEdit1.Text.Trim().Substring(textEdit1.Text.Length - 1, 1);
            }
            else
            {
                textEdit1.Text = textEdit1.Text.Substring(0, textEdit1.Text.LastIndexOf(exp) - 1).Trim();
                //CurrentExpress = textEdit1.Text.Substring(;
                CurrentExpress = textEdit1.Text.Substring(textEdit1.Text.LastIndexOf("[") + 1, textEdit1.Text.Length - textEdit1.Text.LastIndexOf("[") - 2);
            }
        }


        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Expression("+");
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            Expression("-");
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Expression("*");
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Expression("/");
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            if (CurrentExpress == "=" || CurrentExpress == "")
            {
                textEdit1.Text = textEdit1.Text + " (";
                CurrentExpress = "(";
            }
            else if ((CurrentExpress == "(" || CurrentExpress == ")") || (CurrentExpress != "+" && CurrentExpress != "-" && CurrentExpress != "-" && CurrentExpress != "/" && CurrentExpress != "*"))
            {

            }
            else
            {
                textEdit1.Text = textEdit1.Text.Trim() + " (";
                CurrentExpress = "(";
            }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            if (CurrentExpress == "=" || CurrentExpress == "")
            {
            }
            else if (CurrentExpress != "(" && CurrentExpress != "+" && CurrentExpress != "-" && CurrentExpress != "-" && CurrentExpress != "/" && CurrentExpress != "*")
            {
                textEdit1.Text = textEdit1.Text.Trim() + " ) ";
                CurrentExpress = ")";
            }
            else
            {

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string result = Regex.Split(textEdit1.Text, "==")[1];
            if (CurrentExpress == "+" || CurrentExpress == "-" || CurrentExpress == "*" || CurrentExpress == "/" || CurrentExpress == "(")
            {
                MessageBox.Show("Công thức không hợp lệ.Không thể kết thúc biểu thức bằng + - * / hay (", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            char[] a = { '(' };

            String[] res = result.Split(a);
            int count = res.Length;

            char[] a1 = { ')' };

            String[] res1 = result.Split(a1);
            int count1 = res1.Length;
            if (count != count1)
            {
                MessageBox.Show("Số lượng ( và ) không bằng nhau!");
                return;
            }
            DataTable dtSource = (DataTable)gridControl1.DataSource;
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                result = result.Replace("[" + dtSource.Rows[i]["VALUE"].ToString() + "]", "[" + dtSource.Rows[i]["RPTFLDID"].ToString() + "]");
                //result = result.Replace("[" + dtSource.Rows[i]["VALUE"].ToString() + "]", "[" + dtSource.Rows[i]["ROWID"].ToString() + "]");
            }
            //if(result.Trim()!="")
            //{
            //    result = result.Replace(" ", "").Replace("[", "").Replace("]", "");
            //}

            using (var ctrlSA = new SAController())
            {
                string fldid = Regex.Split(textEdit1.Text, "==")[0].Trim().Replace("[", "").Replace("]", "");
                List<string> values = new List<string>();
                values.Add(fldid);
                DataContainer con;
                ctrlSA.ExecuteProcedureFillDataset(out con, "sp_formular_selbyfldid", values);
                AssignFieldValuesFromResult(con);
                DataTable dt = con.DataTable;

                object row = lookUpEdit1.Properties.GetDataSourceRowByKeyValue(lookUpEdit1.EditValue);

                values.Add(result.Trim());
                values.Add((row as DataRowView)["TYPE2"].ToString());
                if (dt.Rows.Count == 0)
                {
                    ctrlSA.ExecuteStoreProcedure("sp_fomular_ins", values);
                }
                else
                {
                    ctrlSA.ExecuteStoreProcedure("sp_fomular_udpbyfldid", values);
                }

                this.CloseModule();
            }
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            this.CloseModule();
        }

        private void convertExpression(string exp)
        {
            exp = exp.Trim().Replace(" ", "").Replace("[", "").Replace("]", "");
            if (exp.Substring(exp.Length - 1, 1) == ")")
                CurrentExpress = ")";
            else
            {
                int max = 0;
                int i = exp.LastIndexOf("+");
                int j = exp.LastIndexOf("-");
                int k = exp.LastIndexOf("*");
                int l = exp.LastIndexOf("/");
                if (i > max) max = i;
                if (j > max) max = j;
                if (k > max) max = k;
                if (l > max) max = l;
                string lastExp = "";
                if (max == i) lastExp = "+";
                else if (max == j) lastExp = "-";
                else if (max == k) lastExp = "*";
                else if (max == l) lastExp = "/";
                CurrentExpress = exp.Substring(exp.LastIndexOf(lastExp) + 1, exp.Length - exp.LastIndexOf(lastExp) - 1);
            }
            //exp = "[" + exp + "]";
            //string exp1 = exp.Replace("+", "] + [").Replace("-", "] - [").Replace("*", "] * [").Replace("/", "] / [");
            //string exp2 = exp1.Replace("[(", "( [").Replace(")]", "] )");
            //string exp3 = exp2.Replace("[(", "( [").Replace(")]", "] )");
            //string exp4 = exp3.Replace("[(", "( [").Replace(")]", "] )");
            //string exp5 = exp4.Replace("[(", "( [").Replace(")]", "] )");
            //string exp6 = exp5.Replace("[(", "( [").Replace(")]", "] )");
            //string exp7 = exp6.Replace("[(", "( [").Replace(")]", "] )");
            //return exp7;
        }

        private void lookUpEdit2_EditValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }



        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            string s = textEdit1.SelectedText;
            if (s != "")
            {
                if (s != "+" && s != "-" && s != "*" && s != "/" && s != "(" && s != ")")
                {
                    if (s.Length > 1)
                    {
                        string first = s.Substring(0, 1);
                        string last = s.Substring(s.Length - 1, 1);
                        if ((first != "[" && first != "]" && first != "+" && first != "-" && first != "*" && first != "/" && first != "(" && first != ")") && (last != "[" && last != "]" && last != "+" && last != "-" && last != "*" && last != "/" && last != "(" && last != ")"))
                        {
                            string fldid = Regex.Split(textEdit1.Text, "==")[0].Trim().Replace("[", "").Replace("]", "");
                            if (s != fldid)
                            {
                                string fr = textEdit1.Text.Substring(textEdit1.SelectionStart - 1, 1);
                                string ls = textEdit1.Text.Substring(textEdit1.SelectionStart + textEdit1.SelectionLength, 1);
                                if (fr == "[" && ls == "]")
                                {
                                    textEdit1.Text = textEdit1.Text.Substring(0, textEdit1.SelectionStart) + gridView1.GetDataRow(gridView1.FocusedRowHandle)["VALUE"].ToString() + textEdit1.Text.Substring(textEdit1.SelectionStart + textEdit1.SelectionLength, textEdit1.Text.Length - textEdit1.SelectionStart - textEdit1.SelectionLength);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (CurrentExpress == ")")
                {
                }
                else if (CurrentExpress == "=")
                {
                    textEdit1.Text = textEdit1.Text + " [" + gridView1.GetDataRow(gridView1.FocusedRowHandle)["VALUE"].ToString() + "]";
                    CurrentExpress = gridView1.GetDataRow(gridView1.FocusedRowHandle)["VALUE"].ToString();
                }
                else if (CurrentExpress != "(" && CurrentExpress != "+" && CurrentExpress != "-" && CurrentExpress != "-" && CurrentExpress != "/" && CurrentExpress != "*")
                {
                    string preStr = textEdit1.Text.Substring(0, textEdit1.Text.LastIndexOf(CurrentExpress));
                    if (preStr.Substring(preStr.Length - 1, 1) != "[")
                        preStr = preStr + " [";
                    textEdit1.Text = preStr + gridView1.GetDataRow(gridView1.FocusedRowHandle)["VALUE"].ToString() + "]";
                    CurrentExpress = gridView1.GetDataRow(gridView1.FocusedRowHandle)["VALUE"].ToString();
                }
                else
                {
                    textEdit1.Text = textEdit1.Text + " [" + gridView1.GetDataRow(gridView1.FocusedRowHandle)["VALUE"].ToString() + "]";
                    CurrentExpress = gridView1.GetDataRow(gridView1.FocusedRowHandle)["VALUE"].ToString();
                }
            }
        }


    }
}
