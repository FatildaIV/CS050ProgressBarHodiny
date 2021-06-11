using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSOWNProgressBarHodiny
{
    public partial class ProgressBarForm : Form
    {
        private System.Windows.Forms.Timer RotateTimer = null;
        private float RotationAngle1 = 90F;
        private float RotationAngle2 = 0F;
        public bool RotateFigures = false;
        int progress;
        class TransparentPanel : Panel
        {
            internal const int WS_EX_TRANSPARENT = 0x00000020;

            public TransparentPanel() => InitializeComponent();

            protected void InitializeComponent()
            {
                this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                              ControlStyles.Opaque |
                              ControlStyles.ResizeRedraw |
                              ControlStyles.SupportsTransparentBackColor |
                              ControlStyles.UserPaint, true);
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
            }

            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams parameters = base.CreateParams;
                    parameters.ExStyle |= WS_EX_TRANSPARENT;
                    return parameters;
                }
            }
        }
        public ProgressBarForm()
        {
            InitializeComponent();
            RotateTimer = new Timer();
            RotateTimer.Interval = 50;
            RotateTimer.Enabled = false;
            RotateTimer.Tick += new EventHandler(this.RotateTick);
        }

        protected void RotateTick(object sender, EventArgs e)
        {
            RotationAngle1 += 10F;
            RotationAngle2 += 10F;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            progress = 25;

            RotateTimer.Enabled = !RotateTimer.Enabled;
            if (RotateTimer.Enabled == false)
            {
                RotateFigures = false;
                RotationAngle1 = 90F;
                RotationAngle2 = 0F;
            }
            else
            {
                RotateFigures = true;
            }
        }

        private void transparentPanel1_Paint(object sender, PaintEventArgs e)
        {
            if (!RotateFigures) return;
            e.Graphics.RotateTransform(-90);
            e.Graphics.TranslateTransform(this.Width / 2, this.Height / 2);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.CompositingMode = CompositingMode.SourceOver;
            Rectangle rect = new Rectangle(0 - this.Width / 2 + 20, 0 - this.Height / 2 + 20, this.Width - 40, this.Height - 40);

            Pen pen = new Pen(Color.Teal, 8);

            e.Graphics.DrawArc(pen, rect, 0, (int)(this.progress * 3.6));

                   using (Pen transpPen = new Pen(transparentPanel1.Parent.BackColor, 10))
                   using (Pen penOuter = new Pen(Color.SteelBlue, 8))
                   using (Pen penInner = new Pen(Color.Teal, 8))
                   using (Matrix m1 = new Matrix())
                   using (Matrix m2 = new Matrix())
                   {
                      m1.RotateAt(-RotationAngle1, new PointF(rect.Width / 2, rect.Height / 2));
                      m2.RotateAt(RotationAngle1, new PointF(rect.Width / 2, rect.Height / 2));
                      rect.Inflate(-(int)penOuter.Width, -(int)penOuter.Width);
                      rectInner.Inflate(-(int)penOuter.Width * 3, -(int)penOuter.Width * 3);
                      
                      e.Graphics.Transform = m2;
                      e.Graphics.DrawArc(transpPen, rectInner, 190, (int)(this.progress * 3.6));
                      e.Graphics.DrawArc(penInner, rectInner, 180, (int)(this.progress * 3.6) + 20);
                  }
        }

        private void CircularProgressBar_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.CompositingMode = CompositingMode.SourceOver;
            Rectangle rect = transparentPanel1.ClientRectangle;
            Rectangle rectInner = rect;
    
            Pen transpPen = new Pen(transparentPanel1.Parent.BackColor, 10);
            Pen penOuter = new Pen(Color.SteelBlue, 8);
            Pen penInner = new Pen(Color.Teal, 8);

            e.Graphics.DrawArc(transpPen, rectInner, 10, (int)(this.progress * 3.6));
            e.Graphics.DrawArc(penInner, rectInner, 0, (int)(this.progress * 3.6) - 10);
        }
    }
}