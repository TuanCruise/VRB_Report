using Aspose.Cells;
using DevExpress.XtraEditors.Controls;
using FIS.AppClient.Controls;
using FIS.Common;
using FIS.Utils;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FIS.AppClient.Threads
{
    public class ImportReadFileThread : WorkerThread
    {
        public DataTable ExcelBufferTable { get; set; }
        public ImageComboBoxItem[] ImportComboBoxItems { get; set; }
        public string FileName { get; set; }
        public new ucIMWizard Parent { get; private set; }

        public ImportReadFileThread(Control parent)
            : base(null, parent)
        {
            Parent = (ucIMWizard)parent;
            Worker = ExecuteReadFile;
        }

        public void ExecuteReadFile(WorkerThread worker)
        {
            try
            {
                Parent.LockUserAction();
                try
                {
                    ExcelBufferTable = new DataTable();

                    var workbook = new Workbook();
                    workbook.Open(FileName);
                    var worksheet = workbook.Worksheets[0];

                    var rowLength = worksheet.Cells.MaxRow + 1;
                    var colLength = worksheet.Cells.MaxColumn + 1;

                    var befcol = 'A' - 1;
                    var col = 'A';
                    for (var i = 0; i < colLength; i++)
                    {
                        if (col > 'Z')
                        {
                            col = 'A';
                            befcol++;
                        }

                        if (befcol == 'A' - 1)
                            ExcelBufferTable.Columns.Add(new DataColumn(string.Format("{0}", char.ConvertFromUtf32(col)), typeof(object)));
                        else
                            ExcelBufferTable.Columns.Add(new DataColumn(string.Format("{0}{1}", char.ConvertFromUtf32(befcol), char.ConvertFromUtf32(col)), typeof(object)));

                        col++;
                    }

                    ImportComboBoxItems =
                        (from DataColumn column in ExcelBufferTable.Columns
                         select new ImageComboBoxItem
                         {
                             ImageIndex = ThemeUtils.GetImage16x16Index("COLUMN"),
                             Value = column.ColumnName,
                             Description = column.ColumnName
                         }).ToArray();

                    for (var i = 0; i < rowLength; i++)
                    {
                        var row = ExcelBufferTable.NewRow();
                        ExcelBufferTable.Rows.Add(row);

                        for (var j = 0; j < colLength; j++)
                        {
                            row[j] = worksheet.Cells[i, j].Value;
                        }


                        PercentComplete = i * 100.0f / rowLength;
                        if (worksheet.Cells[i, 1].Value != null)
                        {
                            StatusText = worksheet.Cells[i, 1].Value.ToString();
                        }
                        ExecuteUpdateGUI();
                    }

                    ExcelBufferTable.Columns.Add(new DataColumn("COLUMN_ERROR", typeof(string))
                    {
                        Caption = Parent.Language.GetLabelText("COLUMN_ERROR")
                    });

                    PercentComplete = 100;
                    ExecuteUpdateGUI(true);
                }
                catch (IOException ex)
                {
                    ExcelBufferTable = null;
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_IMPORT.ERR_FILE_OPEN_ERROR, ex.Message);
                }
                catch (Exception ex)
                {
                    ExcelBufferTable = null;
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_UNKNOWN, ex.Message);
                }
            }
            catch (Exception ex)
            {
                Parent.ShowError(ex);
            }
            finally
            {
                Parent.UnLockUserAction();
            }
        }
    }
}
