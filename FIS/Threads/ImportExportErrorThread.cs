using Aspose.Cells;
using FIS.AppClient.Controls;
using FIS.Utils;
using System;
using System.Data;
using System.Windows.Forms;

namespace FIS.AppClient.Threads
{
    public class ImportExportErrorThread : WorkerThread
    {
        public new ucIMWizard Parent { get; private set; }
        public string FileName { get; set; }
        public DataTable ExcelBufferTable { get; set; }

        public ImportExportErrorThread(Control parent) : base(null, parent)
        {
            Parent = (ucIMWizard)parent;
            Worker = ExecuteExport;
        }

        public void ExecuteExport(WorkerThread worker)
        {
            try
            {
                Parent.LockUserAction();

                var workbook = new Workbook();
                var worksheet = workbook.Worksheets[0];

                for (var i = 0; i < ExcelBufferTable.Columns.Count; i++)
                {
                    for (var j = 0; j < ExcelBufferTable.Rows.Count; j++)
                    {
                        worksheet.Cells[j, i].PutValue(ExcelBufferTable.Rows[j][i]);
                    }
                }

                workbook.Save(FileName);
            }
            catch (Exception ex)
            {
                Parent.ShowError(ex);
            }
            finally
            {
                PercentComplete = 100.0f;
                ExecuteUpdateGUI(true);
                Parent.UnLockUserAction();
            }
        }
    }
}
