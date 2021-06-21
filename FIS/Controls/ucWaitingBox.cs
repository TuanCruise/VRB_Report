using DevExpress.XtraEditors;
using System.Drawing;

namespace FIS.AppClient.Controls
{
    public partial class ucWaitingBox : PanelControl
    {
        public string StatusText
        {
            get { return lbProgressText.Text; }
            set { lbProgressText.Text = value; }
        }
        public void InitializeControls()
        {
            Controls.Add(lbProgressText);
            Controls.Add(marqueeProgressBarControl1);

            SuspendLayout();
            marqueeProgressBarControl1.Location = new Point(5, 30);
            ResumeLayout();
        }

        public ucWaitingBox()
        {
            InitializeComponent();
            InitializeControls();
        }
    }
}
