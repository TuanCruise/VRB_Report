using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace FIS.AppClient.Controls
{
    public partial class ucRelationEditor : DevExpress.XtraEditors.XtraUserControl
    {
        public delegate void UpdateConnectEventHandler(object sender, UpdateConnectEventArgs e);
        public class UpdateConnectEventArgs : EventArgs
        {
            public ChildPosition BeginPoint { get; set; }
            public ChildPosition EndPoint { get; set; }
            public bool IsConnect { get; set; }
            public Color ConnectorColor { get; set; }
            public UpdateConnectEventArgs(ChildPosition endPoint, ChildPosition beginPoint)
            {
                EndPoint = beginPoint;
                BeginPoint = endPoint;
                IsConnect = false;
            }
        }

        public delegate void ItemClickedEventHandler(object sender, ItemClickedEventArgs e);

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<ChildPosition> BeginPoints { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<ChildPosition> EndPoints { get; set; }

        ChildPosition BeginPoint { get; set; }
        ChildPosition EndPoint { get; set; }
        PointF EndMousePoint { get; set; }

        public List<AbstractPaintObject> BeginPaintObjects { get; set; }
        public List<AbstractPaintObject> EndPaintObjects { get; set; }
        public event UpdateConnectEventHandler UpdateConnect;
        public event UpdateConnectEventHandler ConnectChange;
        public event ItemClickedEventHandler ItemClicked;
        public event ItemClickedEventHandler ObjectTitleClicked;

        public ucRelationEditor()
        {
            InitializeComponent();
            BeginPoints = new List<ChildPosition>();
            EndPoints = new List<ChildPosition>();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (!DesignMode && EndPaintObjects != null && BeginPaintObjects != null)
            {
                BeginPoints.Clear();
                EndPoints.Clear();
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                var top = 40F;
                var beginHeight = 40F;
                var maxBeginWidth = 0F;
                foreach (var beginObject in BeginPaintObjects)
                {
                    var beginSize = beginObject.Paint(e.Graphics, BeginPoints, EndPoints, 40, top);
                    if (maxBeginWidth < beginSize.Width)
                    {
                        maxBeginWidth = beginSize.Width;
                    }
                    top += beginSize.Height + 10;
                    beginHeight += beginSize.Height + 10;
                }

                var endHeight = 40F;
                var controlWidth = maxBeginWidth + 240;
                foreach (var endObject in EndPaintObjects)
                {
                    var endSize = endObject.Paint(e.Graphics, BeginPoints, EndPoints, maxBeginWidth + 240, endHeight);
                    if (controlWidth < maxBeginWidth + 240 + endSize.Width + 10)
                    {
                        controlWidth = maxBeginWidth + 240 + endSize.Width + 10;
                    }
                    endHeight += endSize.Height + 10;
                }
                var controlHeight = Math.Max(endHeight, beginHeight);

                foreach (var beginPoint in BeginPoints)
                {
                    foreach (var endPoint in EndPoints)
                    {
                        var ev = new UpdateConnectEventArgs(beginPoint, endPoint);
                        UpdateConnect(this, ev);
                        if (ev.IsConnect)
                        {
                            e.Graphics.DrawLine(new Pen(ev.ConnectorColor), endPoint.Position, beginPoint.Position);
                        }
                    }
                }

                if (BeginPoint != null)
                {
                    var beginPos = BeginPoint.Position;
                    e.Graphics.DrawEllipse(Pens.Blue, beginPos.X - 5, beginPos.Y - 5, 10, 10);
                    if (EndPoint != null)
                    {
                        var endPos = EndPoint.Position;
                        e.Graphics.DrawEllipse(Pens.Blue, endPos.X - 5, endPos.Y - 5, 10, 10);
                        e.Graphics.DrawLine(Pens.Blue, beginPos.X, beginPos.Y, endPos.X, endPos.Y);
                    }
                    else
                    {
                        e.Graphics.DrawLine(Pens.Blue, beginPos.X, beginPos.Y, EndMousePoint.X, EndMousePoint.Y);
                    }
                }

                Size = new Size(Convert.ToInt32(controlWidth), Convert.ToInt32(controlHeight));
            }
        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == MouseButtons.None)
            {
                BeginPoint = null;
                EndPoint = null;
                if (BeginPaintObjects != null)
                {
                    foreach (var beginPoint in BeginPoints)
                    {
                        var point = beginPoint.Position;
                        if (beginPoint.IsNearPoint(e.X, e.Y))
                        {
                            BeginPoint = beginPoint;
                            EndMousePoint = point;
                        }
                    }
                }

                if (EndPaintObjects != null)
                {
                    foreach (var endPoint in EndPoints)
                    {
                        var point = endPoint.Position;
                        if (endPoint.IsNearPoint(e.X, e.Y))
                        {
                            BeginPoint = endPoint;
                            EndMousePoint = point;
                        }
                    }
                }
            }
            else
            {
                EndPoint = null;

                foreach (var beginPoint in BeginPoints)
                {
                    var point = beginPoint.Position;
                    if (beginPoint.IsNearPoint(e.X, e.Y))
                    {
                        EndPoint = beginPoint;
                    }

                    EndMousePoint = point;
                }

                EndMousePoint = new PointF(e.X, e.Y);
            }

            Refresh();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (BeginPoint != null)
            {
                EndMousePoint = new PointF(e.X, e.Y);
                EndPoint = null;
                Refresh();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            foreach (var paintObject in EndPaintObjects.Union(BeginPaintObjects))
            {
                if (paintObject.TitleBar.Contains(e.Location))
                {
                    if (ObjectTitleClicked != null)
                        ObjectTitleClicked(this, new ItemClickedEventArgs(new ChildPosition
                        {
                            Parent = paintObject.Parent
                        }));
                }
            }

            if (BeginPoint != null && EndPoint != null && BeginPoint.Child != EndPoint.Child)
            {
                if (ConnectChange != null && BeginPoint != EndPoint)
                    ConnectChange(this, new UpdateConnectEventArgs(EndPoint, BeginPoint));
                BeginPoint = null;
                Refresh();
            }
            else
            {
                ChildPosition beginPoint = null;

                foreach (var point in BeginPoints.Union(EndPoints))
                {
                    if (point.IsNearPoint(e.X, e.Y))
                    {
                        if (ItemClicked != null)
                        {
                            beginPoint = point;
                        }
                    }
                }

                if (beginPoint != null)
                    ItemClicked(this, new ItemClickedEventArgs(beginPoint));
            }
        }
    }
    public class ItemClickedEventArgs : EventArgs
    {
        public ChildPosition Position { get; set; }

        public ItemClickedEventArgs(ChildPosition position)
        {
            Position = position;
        }
    }
    public class ChildPosition
    {
        public PointF Position { get; set; }
        public object Child { get; set; }
        public Color Color { get; set; }
        public object Parent { get; set; }
        public bool IsNearPoint(float x, float y)
        {
            return Math.Abs(Position.X - x) <= 10 && Math.Abs(Position.Y - y) <= 10;
        }

        public bool IsNearPoint(PointF point)
        {
            return IsNearPoint(point.X, point.Y);
        }
    }
    public enum BeginEndMode
    {
        End, Begin
    }

    public abstract class AbstractPaintObject
    {
        public object Parent { get; set; }
        public RectangleF TitleBar { get; set; }
        public abstract SizeF Paint(Graphics g, List<ChildPosition> BeginPoints, List<ChildPosition> EndPoints, float left, float top);
    }

    public class PaintObject<ParentType, ChildType> : AbstractPaintObject
    {
        public string Title { get; set; }
        public new ParentType Parent
        {
            get { return (ParentType)base.Parent; }
            set { base.Parent = value; }
        }
        public List<ChildType> Childs { get; set; }
        public Image Icon { get; set; }
        public BeginEndMode BeginEnd { get; set; }
        public Color ConnectorColor { get; set; }

        public delegate void CustomLabelEventHandler(object sender, CustomLabelEventArgs e);
        public class CustomLabelEventArgs : EventArgs
        {
            public ParentType Parent { get; set; }
            public ChildType Child { get; set; }
            public Color ForeColor { get; set; }
            public string Text { get; set; }
        }

        public event CustomLabelEventHandler CustomLabel;

        public override SizeF Paint(Graphics g, List<ChildPosition> BeginPoints, List<ChildPosition> EndPoints, float left, float top)
        {
            var childsLabel = new List<string>();
            var childsColor = new List<Color>();

            foreach (var child in Childs)
            {
                var e = new CustomLabelEventArgs();
                e.Parent = Parent;
                e.Child = child;
                e.ForeColor = Color.Black;

                CustomLabel(this, e);
                childsLabel.Add(e.Text);
                childsColor.Add(e.ForeColor);
            }

            var titleFont = new Font("Tahoma", 6, FontStyle.Bold);
            var textFont = new Font("Courier New", 8);
            var titleWidth = g.MeasureString(Title, titleFont).Width + 26;

            foreach (var childLabel in childsLabel)
            {
                var textWidth = g.MeasureString(childLabel, textFont).Width + 8;
                if (titleWidth < textWidth) titleWidth = textWidth;
            }
            TitleBar = new RectangleF(left, top, titleWidth, 16);
            var recTitleText = new RectangleF(left + 18, top, titleWidth, 16);
            var recTitleIcon = new RectangleF(left + 2, top, 16, 16);

            g.FillRectangle(Brushes.Black, TitleBar);
            g.DrawImage(Icon, recTitleIcon);
            g.DrawString(Title, titleFont, Brushes.White, recTitleText, new StringFormat { LineAlignment = StringAlignment.Center });

            var textTop = top + 14;
            for (var i = 0; i < childsLabel.Count; i++)
            {
                var recText = new RectangleF(left + 2, textTop + 4, titleWidth, textTop + 24);
                g.DrawString(childsLabel[i], textFont, new SolidBrush(childsColor[i]), recText);
                textTop += 16;
            }

            g.DrawRectangle(Pens.Black, left, top, titleWidth, textTop - top + 4);

            textTop = top + 23;
            var brush = new SolidBrush(ConnectorColor);
            for (int index = 0; index < childsLabel.Count; index++)
            {
                if (BeginEnd == BeginEndMode.End)
                {
                    g.FillEllipse(brush, TitleBar.Left - 3, textTop, 6, 6);
                    BeginPoints.Add(
                        new ChildPosition
                        {
                            Parent = Parent,
                            Position = new PointF(TitleBar.Left, textTop + 3),
                            Child = Childs[index],
                            Color = ConnectorColor
                        });
                }
                else
                {
                    g.FillEllipse(brush, TitleBar.Right - 3, textTop, 6, 6);
                    EndPoints.Add(
                        new ChildPosition
                        {
                            Parent = Parent,
                            Position = new PointF(TitleBar.Right, textTop + 3),
                            Child = Childs[index],
                            Color = ConnectorColor
                        });
                }

                textTop += 16;
            }

            return new SizeF(titleWidth, textTop - top + 4);
        }
    }
}
