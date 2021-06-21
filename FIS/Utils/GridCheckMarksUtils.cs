using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace FIS.AppClient.Utils
{
    public class GridCheckMarksUtils
    {
        protected GridView m_GridView;
        protected ArrayList m_Selection;
        private GridColumn m_Column;
        private RepositoryItemCheckEdit m_Edit;
        private readonly bool m_ClearOnMouseDown;
        public event EventHandler SelectChanged;


        public GridCheckMarksUtils(bool clearOnMouseDown)
        {
            m_Selection = new ArrayList();
            m_ClearOnMouseDown = clearOnMouseDown;
        }

        public GridCheckMarksUtils(GridView gridView, bool clearOnMouseDown)
            : this(clearOnMouseDown)
        {
            m_ClearOnMouseDown = clearOnMouseDown;
            grid_view = gridView;
        }

        protected virtual void Attach(GridView gridView)
        {
            if (gridView == null) return;
            m_Selection.Clear();
            m_GridView = gridView;
            m_Edit = gridView.GridControl.RepositoryItems.Add("CheckEdit") as RepositoryItemCheckEdit;
            m_Edit.EditValueChanged += edit_EditValueChanged;

            m_Column = gridView.Columns.Add();
            m_Column.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            m_Column.VisibleIndex = int.MaxValue;
            m_Column.FieldName = "CheckMarkSelection";
            m_Column.Caption = "Mark";
            m_Column.OptionsColumn.ShowCaption = false;
            m_Column.UnboundType = DevExpress.Data.UnboundColumnType.Boolean;
            m_Column.ColumnEdit = m_Edit;


            gridView.Click += View_Click;
            gridView.CustomDrawColumnHeader += View_CustomDrawColumnHeader;
            gridView.CustomDrawGroupRow += View_CustomDrawGroupRow;
            gridView.CustomUnboundColumnData += view_CustomUnboundColumnData;
            gridView.RowStyle += view_RowStyle;
            if (m_ClearOnMouseDown) gridView.MouseDown += view_MouseDown;
        }

        protected virtual void Detach()
        {
            if (m_GridView == null) return;
            if (m_Column != null)
                m_Column.Dispose();
            if (m_Edit != null)
            {
                m_GridView.GridControl.RepositoryItems.Remove(m_Edit);
                m_Edit.Dispose();
            }

            m_GridView.Click -= View_Click;
            m_GridView.CustomDrawColumnHeader -= View_CustomDrawColumnHeader;
            m_GridView.CustomDrawGroupRow -= View_CustomDrawGroupRow;
            m_GridView.CustomUnboundColumnData -= view_CustomUnboundColumnData;
            m_GridView.RowStyle -= view_RowStyle;
            if (m_ClearOnMouseDown) m_GridView.MouseDown -= view_MouseDown;

            m_GridView = null;
        }

        protected void DrawCheckBox(Graphics g, Rectangle r, bool @checked)
        {
            var info = (CheckEditViewInfo)m_Edit.CreateViewInfo();
            var painter = (CheckEditPainter)m_Edit.CreatePainter();
            info.EditValue = @checked;
            info.Bounds = r;
            info.CalcViewInfo(g);
            var args = new ControlGraphicsInfoArgs(info, new DevExpress.Utils.Drawing.GraphicsCache(g), r);
            painter.Draw(args);
            args.Cache.Dispose();
        }

        private void View_Click(object sender, EventArgs e)
        {
            var pt = m_GridView.GridControl.PointToClient(Control.MousePosition);
            var info = m_GridView.CalcHitInfo(pt);
            if (info.InColumn && info.Column == m_Column)
            {
                if (SelectedCount == m_GridView.DataRowCount)
                    ClearSelection();
                else
                    SelectAll();
            }
            if (info.InRow && info.HitTest != GridHitTest.RowGroupButton)
            {
                if (m_GridView.IsGroupRow(info.RowHandle))
                {
                    var selected = IsGroupRowSelected(info.RowHandle);
                    SelectGroup(info.RowHandle, !selected);
                }
                else
                {
                    var selected = IsRowSelected(info.RowHandle);
                    SelectRow(info.RowHandle, !selected);
                }
            }
            if (SelectChanged != null) SelectChanged(m_GridView, new EventArgs());
        }

        private void view_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1 && e.Button == MouseButtons.Left)
            {
                var pt = m_GridView.GridControl.PointToClient(Control.MousePosition);
                var info = m_GridView.CalcHitInfo(pt);
                if (info.InRow && info.Column != m_Column && m_GridView.IsDataRow(info.RowHandle))
                {
                    grid_view.PostEditor();
                    ClearSelection();
                    SelectRow(info.RowHandle, true);
                }
            }
        }

        private void View_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column == m_Column)
            {
                e.Info.InnerElements.Clear();
                e.Painter.DrawObject(e.Info);
                DrawCheckBox(e.Graphics, e.Bounds, SelectedCount == m_GridView.DataRowCount);
                e.Handled = true;
            }
        }

        private void View_CustomDrawGroupRow(object sender, RowObjectCustomDrawEventArgs e)
        {
            var info = e.Info as GridGroupRowInfo;
            if (info != null)
            {
                info.GroupText = "         " + info.GroupText.TrimStart();
                e.Info.Paint.FillRectangle(e.Graphics, e.Appearance.GetBackBrush(e.Cache), e.Bounds);
                e.Painter.DrawObject(e.Info);

                var r = info.ButtonBounds;
                r.Offset(r.Width * 2, 0);
                DrawCheckBox(e.Graphics, r, IsGroupRowSelected(e.RowHandle));
                e.Handled = true;
            }
        }

        private void view_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (IsRowSelected(e.RowHandle))
            {
                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
            }
        }

        public GridView grid_view
        {
            get
            {
                return m_GridView;
            }
            set
            {
                if (m_GridView != value)
                {
                    Detach();
                    Attach(value);
                }
            }
        }

        public GridColumn CheckMarkColumn
        {
            get
            {
                return m_Column;
            }
        }

        public int SelectedCount
        {
            get
            {
                return m_Selection.Count;
            }
        }

        public object GetSelectedRow(int index)
        {
            return m_Selection[index];
        }

        public int GetSelectedIndex(object row)
        {
            return m_Selection.IndexOf(row);
        }

        public void ClearSelection()
        {
            m_Selection.Clear();
            Invalidate();
        }

        private void Invalidate()
        {
            m_GridView.BeginUpdate();
            m_GridView.EndUpdate();
        }

        public void SelectAll()
        {
            m_Selection.Clear();
            if (m_GridView.DataSource is ICollection)
                m_Selection.AddRange(((ICollection)m_GridView.DataSource));  // fast
            else
                for (var i = 0; i < m_GridView.DataRowCount; i++)  // slow
                    m_Selection.Add(m_GridView.GetRow(i));
            Invalidate();
        }

        public void SelectGroup(int rowHandle, bool select)
        {
            if (IsGroupRowSelected(rowHandle) && select) return;
            for (var i = 0; i < m_GridView.GetChildRowCount(rowHandle); i++)
            {
                var childRowHandle = m_GridView.GetChildRowHandle(rowHandle, i);
                if (m_GridView.IsGroupRow(childRowHandle))
                    SelectGroup(childRowHandle, select);
                else
                    SelectRow(childRowHandle, select, false);
            }
            Invalidate();
        }

        public void SelectRow(int rowHandle, bool select)
        {
            SelectRow(rowHandle, select, true);
        }

        private void SelectRow(int rowHandle, bool select, bool invalidate)
        {
            if (IsRowSelected(rowHandle) == select) return;
            var row = m_GridView.GetRow(rowHandle);
            if (select)
                m_Selection.Add(row);
            else
                m_Selection.Remove(row);
            if (invalidate)
            {
                Invalidate();
            }
        }

        public bool IsGroupRowSelected(int rowHandle)
        {
            for (var i = 0; i < m_GridView.GetChildRowCount(rowHandle); i++)
            {
                var row = m_GridView.GetChildRowHandle(rowHandle, i);
                if (m_GridView.IsGroupRow(row))
                {
                    if (!IsGroupRowSelected(row)) return false;
                }
                else
                    if (!IsRowSelected(row)) return false;
            }
            return true;
        }

        public bool IsRowSelected(int rowHandle)
        {
            if (m_GridView.IsGroupRow(rowHandle))
                return IsGroupRowSelected(rowHandle);

            var row = m_GridView.GetRow(rowHandle);
            return GetSelectedIndex(row) != -1;
        }

        private void view_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            if (e.Column == CheckMarkColumn)
            {
                if (e.IsGetData)
                    e.Value = IsRowSelected(m_GridView.GetRowHandle(e.ListSourceRowIndex));
                // e.Value = IsRowSelected(e.RowHandle);
                else
                    SelectRow(m_GridView.GetRowHandle(e.ListSourceRowIndex), (bool)e.Value);
                //SelectRow(e.RowHandle, (bool)e.Value);
            }
        }

        private void edit_EditValueChanged(object sender, EventArgs e)
        {
            m_GridView.PostEditor();
            if (SelectChanged != null) SelectChanged(m_GridView, new EventArgs());
        }
    }
}
