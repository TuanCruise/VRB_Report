using Aspose.Cells;
using FIS.Common;
using FIS.Entities;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;


namespace FIS.Utils
{
    public class DataTableExporter
    {
        public string[] Columns { get; set; }
        public string[] Headers { get; set; }
        public ModuleFieldInfo[] Fields { get; set; }
        public ModuleInfo ModuleInfo { get; private set; }
        private Style[] OddStyles { get; set; }
        private Style[] EvenStyles { get; set; }
        private Dictionary<object, string>[] ListSources { get; set; }
        protected List<ExportHeader> ExportHeaderInfo { get; set; }
        private List<GroupSummaryInfo> GroupSummaries { get; set; }
        //Tu them
        public DataTable dt = null;
        public int levelMaxMergeExport = 0;
        //End
        public DataTableExporter(string[] columns, string[] headers, ModuleInfo moduleInfo, ModuleFieldInfo[] fields)
        {
            Columns = columns;
            Headers = headers;
            Fields = fields;
            ModuleInfo = moduleInfo;
            ListSources = new Dictionary<object, string>[columns.Length];
            OddStyles = new Style[columns.Length];
            EvenStyles = new Style[columns.Length];
        }

        public Workbook CreateWorkBook()
        {
            var book = new Workbook();
            book.Worksheets.Clear();

            var borderColor = Color.FromArgb(79, 129, 189);
            var textColor = Color.FromArgb(0, 0, 0);
            var headerBackgroundColor = Color.FromArgb(79, 129, 189);
            var headerTextColor = Color.FromArgb(255, 255, 255);
            var oddBackgroundColor = Color.FromArgb(220, 230, 241);
            var evenBackgroundColor = Color.FromArgb(255, 255, 255);

            book.ChangePalette(borderColor, 50);
            book.ChangePalette(textColor, 51);
            book.ChangePalette(headerBackgroundColor, 52);
            book.ChangePalette(headerTextColor, 53);
            book.ChangePalette(oddBackgroundColor, 54);
            book.ChangePalette(evenBackgroundColor, 55);

            book.DefaultStyle.Font.Name = "Tahoma";
            book.DefaultStyle.Font.Size = 10;
            book.DefaultStyle.Font.Color = textColor;
            ApplyBorder(book.DefaultStyle, borderColor, CellBorderType.Thin);

            var style = book.Styles[book.Styles.Add()];
            style.Name = "HeaderStyle";
            style.Font.IsBold = true;
            style.Pattern = BackgroundType.Solid;
            style.Font.Color = headerTextColor;
            style.ForegroundColor = headerBackgroundColor;
            style.Pattern = BackgroundType.Solid;
            style.HorizontalAlignment = TextAlignmentType.Center;
            ApplyBorder(style, borderColor, CellBorderType.Thin);

            for (var i = 0; i < Columns.Length; i++)
            {
                style = book.Styles[book.Styles.Add()];
                OddStyles[i] = style;
                style.Name = "OddStyle" + Columns[i];
                style.Pattern = BackgroundType.Solid;
                style.ForegroundColor = oddBackgroundColor;
                style.Pattern = BackgroundType.Solid;
                style.HorizontalAlignment = ApplyAlign(Fields[i].TextAlign, Fields[i].FieldType);
                ApplyBorder(style, borderColor, CellBorderType.Thin);
                ApplyFormat(style, Fields[i], ref ListSources[i]);
            }

            for (var i = 0; i < Columns.Length; i++)
            {
                style = book.Styles[book.Styles.Add()];
                EvenStyles[i] = style;
                style.Name = "EvenStyle" + Columns[i];
                style.Pattern = BackgroundType.Solid;
                style.ForegroundColor = evenBackgroundColor;
                style.Pattern = BackgroundType.Solid;
                style.HorizontalAlignment = ApplyAlign(Fields[i].TextAlign, Fields[i].FieldType);
                ApplyBorder(style, borderColor, CellBorderType.Thin);
                ApplyFormat(style, Fields[i], ref ListSources[i]);
            }

            return book;
        }
        private static TextAlignmentType ApplyAlign(string textalign, string fldtype)
        {
            switch (textalign)
            {
                case CODES.DEFMODFLD.TEXTALIGN.LEFT:
                    return TextAlignmentType.Left;
                case CODES.DEFMODFLD.TEXTALIGN.RIGHT:
                    return TextAlignmentType.Right;
                default:
                    if (fldtype == CODES.DEFMODFLD.FLDTYPE.STRING || fldtype == CODES.DEFMODFLD.FLDTYPE.DATE)
                    {
                        return TextAlignmentType.Left;
                    }
                    else
                    {
                        return TextAlignmentType.Right;
                    }
            }
        }

        private void ApplyFormat(Style style, ModuleFieldInfo field, ref Dictionary<object, string> listSource)
        {
            if (field.FieldType == CODES.DEFMODFLD.FLDTYPE.DATE)
            {
                style.Custom = "dd/MM/yyyy";
            }

            if (field.FieldType == CODES.DEFMODFLD.FLDTYPE.DATETIME)
            {
                style.Custom = "dd/MM/yyyy HH:mm:ss";
            }

            if (field.ListSource != null)
            {
                listSource = new Dictionary<object, string>();
                if (!string.IsNullOrEmpty(field.ListSource))
                {
                    var match = Regex.Match(field.ListSource, "^:([^.]+).([^.]+)$");

                    if (match.Success)
                    {
                        var codeType = match.Groups[1].Value;
                        var codeName = match.Groups[2].Value;
                        var codes = CodeUtils.GetCodes(codeType, codeName);
                        foreach (var code in codes)
                        {
                            listSource.Add(
                                code.CodeValue.Decode(field), LangUtils.Translate(
                                LangType.DEFINE_CODE,
                                code.CodeType,
                                code.CodeName,
                                code.CodeValueName));
                        }
                    }
                    else
                    {
                        var procExpression = ExpressionUtils.ParseScript(field.ListSource);
                        if (procExpression != null && procExpression.StoreProcName != null)
                        {
                            var count = (from op in procExpression.Operands
                                         where op.Type == OperandType.NAME
                                         select 1).Count();
                            if (count == 0)
                            {
                                var sourceList = App.Environment.GetSourceList(ModuleInfo, field,
                                    (from operand in procExpression.Operands
                                     select operand.NameOrValue).ToList());

                                foreach (var item in sourceList)
                                {
                                    listSource.Add(item.Value.Decode(field), item.Text);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void ApplyBorder(Style style, Color color, CellBorderType cellBorderType)
        {
            style.Borders[BorderType.TopBorder].Color = color;
            style.Borders[BorderType.TopBorder].LineStyle = cellBorderType;
            style.Borders[BorderType.BottomBorder].Color = color;
            style.Borders[BorderType.BottomBorder].LineStyle = cellBorderType;
            style.Borders[BorderType.LeftBorder].Color = color;
            style.Borders[BorderType.LeftBorder].LineStyle = cellBorderType;
            style.Borders[BorderType.RightBorder].Color = color;
            style.Borders[BorderType.RightBorder].LineStyle = cellBorderType;
        }

        public void ExportDataTable(Worksheet workSheet, DataTable table)
        {
            var headerStyle = workSheet.Workbook.Styles["HeaderStyle"];
            //workSheet.Cells.StandardHeight = 30;
            int MaxRowHeader = 0;
            var expInfo = SysvarUtils.GetVarValue(SYSVAR.GRNAME_SYS, SYSVAR.VARNAME_EXPORT_LOGO);
            if (expInfo == CONSTANTS.Yes)
            {
                int pictureIndex = workSheet.Pictures.Add(0, 0, System.Environment.CurrentDirectory + "\\Theme\\Logo.jpg");
                Aspose.Cells.Picture picture = workSheet.Pictures[pictureIndex];
                picture.Height = 100;
                picture.Width = 600;
                MaxRowHeader = 6;
            }

            //write header manual
            ExportHeaderInfo = GetExportHeaderParams(ModuleInfo.ModuleID);
            if (ExportHeaderInfo.Count > 0)
            {
                foreach (var headerRow in ExportHeaderInfo)
                {
                    workSheet.Cells.Merge(headerRow.StartRow, headerRow.StartCol, headerRow.RowNum, headerRow.ColNum);
                    MaxRowHeader = headerRow.MaxRow;
                    workSheet.Cells[headerRow.StartRow, headerRow.StartCol].PutValue(headerRow.Text);
                    workSheet.Cells[headerRow.StartRow, headerRow.StartCol].SetStyle(headerStyle);
                }
            }
            else
            {
                for (var i = 0; i < Headers.Length; i++)
                {
                    workSheet.Cells[MaxRowHeader, i].PutValue(Headers[i]);
                    workSheet.Cells[MaxRowHeader, i].SetStyle(headerStyle);
                }
            }

            var rowIndex = MaxRowHeader + 1;
            foreach (DataRow row in table.Rows)
            {
                for (var i = 0; i < Columns.Length; i++)
                {
                    var cellValue = row[Columns[i]];
                    if (ListSources[i] != null)
                    {
                        if (ListSources[i].ContainsKey(cellValue))
                            workSheet.Cells[rowIndex, i].PutValue(ListSources[i][cellValue]);
                    }
                    else
                        //workSheet.Cells[rowIndex, i].PutValue(cellValue);
                        workSheet.Cells[rowIndex, i].PutValue(cellValue.DecodeAny(Fields[i]));

                    if (rowIndex % 2 == 1)
                        workSheet.Cells[rowIndex, i].SetStyle(OddStyles[i]);
                    else
                        workSheet.Cells[rowIndex, i].SetStyle(EvenStyles[i]);

                    switch (Fields[i].FieldType)
                    {
                        case CONSTANTS.FLDTYPEDEC:
                            if (Fields[i].FieldName != "STT")
                            {
                                workSheet.Cells[rowIndex, i].Style.Custom = CONSTANTS.FLDFORMATDEC;
                            }
                            break;
                        case CONSTANTS.FLDTYPEDTE:
                            workSheet.Cells[rowIndex, i].Style.Custom = Fields[i].FieldFormat;
                            break;
                    }
                }

                rowIndex++;
            }
            var maxRow = rowIndex++;

            string CellAt;
            for (var i = 0; i < Columns.Length; i++)
            {
                string cellname = CellsHelper.ColumnIndexToName(i);
                string CellNameStart = cellname + 1;
                string CellNameStop = cellname + maxRow.ToString();

                if (maxRow % 2 == 1)
                    workSheet.Cells[maxRow, i].SetStyle(OddStyles[i]);
                else
                    workSheet.Cells[maxRow, i].SetStyle(EvenStyles[i]);
                //wrap                
                if (Fields[i].WrapText == CONSTANTS.Yes)
                {
                    for (var j = 0; j <= maxRow; j++)
                    {
                        CellAt = cellname + j;
                        Cell obj = workSheet.Cells[CellAt];
                        Style style = obj.GetStyle();
                        style.IsTextWrapped = true;
                        obj.SetStyle(style);
                    }
                }
                // end wrap
                //workSheet.Cells[maxRow, 0].PutValue(maxRow);                                                                      
                //GroupSummaries = FieldUtils.GetModuleGroupSummary(ModuleInfo.ModuleID, ModuleInfo.ModuleType);

                if (Fields[i].Group_Summary == CONSTANTS.Yes)
                {
                    workSheet.Cells[maxRow, 1].PutValue(CONSTANTS.SUMTEXT);
                    if (Fields[i].FieldName == "ROA" || Fields[i].FieldName == "ROE")
                    {
                        workSheet.Cells[maxRow, i].Formula = "=SUM(" + CellNameStart + ":" + CellNameStop + ")/" + table.Rows.Count;
                    }
                    else
                    {
                        workSheet.Cells[maxRow, i].Formula = "=SUM(" + CellNameStart + ":" + CellNameStop + ")";
                    }
                    workSheet.Cells[maxRow, i].Style.Custom = CONSTANTS.FLDFORMATDEC;
                }
            }

            MaxRowHeader = MaxRowHeader + 1;
            int iRows = MaxRowHeader + 1;
            int startRowGroup = MaxRowHeader + 1;
            int endRowGroup = MaxRowHeader + 1;
            //// Group rao lai da
            //for (var j = 0; j < Columns.Length; j++)
            //{
            //    iRows = MaxRowHeader + 1; startRowGroup = MaxRowHeader + 1; endRowGroup = MaxRowHeader + 1; cellValuePre = null;
            //    if (Fields[j].GroupOnSearch == CONSTANTS.Yes)
            //    {
            //        foreach (DataRow row in table.Rows)
            //        {
            //            string cellValue = row[Columns[j]].ToString();
            //            if (cellValuePre != cellValue)
            //            {
            //                workSheet.Cells.InsertRow(startRowGroup - 1);
            //                workSheet.Cells[startRowGroup - 1, 1].PutValue(cellValue);
            //                cellValuePre = cellValue;

            //            }

            //            //if (cellValuePre != cellValue)
            //            //{
            //            //    cellValuePre = cellValue;
            //            //    if (startRowGroup < endRowGroup)
            //            //    {                                
            //            //        workSheet.Cells.InsertRow(endRowGroup + 1);
            //            //        iRows++;
            //            //    }
            //            //    startRowGroup = iRows;
            //            //    endRowGroup = iRows;
            //            //}
            //            //else
            //            //{
            //            //    endRowGroup = iRows;
            //            //}
            //            startRowGroup = iRows + 1;
            //            iRows++;
            //        }

            //        //iRows = MaxRowHeader + 1; startRowGroup = MaxRowHeader + 1; endRowGroup = MaxRowHeader + 1; cellValuePre = null;
            //        //foreach (DataRow row in table.Rows)
            //        //{
            //        //    string cellValue = row[Columns[j]].ToString();
            //        //    if (cellValuePre != cellValue)
            //        //    {
            //        //        cellValuePre = cellValue;
            //        //        if (startRowGroup < endRowGroup)
            //        //        {
            //        //            workSheet.Cells.GroupRows(startRowGroup, endRowGroup, false);
            //        //            iRows++;
            //        //        }
            //        //        startRowGroup = iRows;
            //        //        endRowGroup = iRows;
            //        //    }
            //        //    else
            //        //    {
            //        //        endRowGroup = iRows;
            //        //    }
            //        //    iRows++;
            //        //}
            //    }
            //}                       

            workSheet.Outline.SummaryRowBelow = false;
            workSheet.AutoFitRows();

        }

        public static List<ExportHeader> GetExportHeaderParams(string moduleID)
        {
            return (from param in AllCaches.ExportHeaders
                    where param.ModuleID == moduleID
                    select param).ToList();
        }
    }
}

