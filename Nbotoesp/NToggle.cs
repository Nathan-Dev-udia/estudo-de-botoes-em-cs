using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace butoes.Nbotoesp
{
    class NToggle : CheckBox
    {
        //Fields
        private Color onBackColor = Color.MediumSlateBlue;
        private Color onToggleColor = Color.WhiteSmoke;
        private Color offBackColor = Color.Gray;
        private Color offToggleColor = Color.Red;
        private bool solidStyle = true;

        //Properties
        public Color OnBackColor
        {
            get { return onBackColor; }
            set { onBackColor = value; this.Invalidate(); }
        }

        public Color OnToggleColor
        {
            get { return onToggleColor; }
            set { onToggleColor = value; this.Invalidate(); }
        }

        public Color OffBackColor
        {
            get { return offBackColor; }  // CORRETO
            set { offBackColor = value; this.Invalidate(); }  // CORRETO
        }

        public Color OffToggleColor
        {
            get { return offToggleColor; }
            set { offToggleColor = value; this.Invalidate(); }
        }

        public override string Text
        {
            get { return base.Text; }
            set {  }
        }

        [DefaultValue(true)]
        public bool SolidStyle
        {
            get { return solidStyle; }
            set { solidStyle = value; this.Invalidate(); }
        }

        //Constructor
        public NToggle()
        {
            this.MinimumSize = new Size(45, 22);
            //this.Size = new Size(45, 22);
            //this.BackColor = Color.Gray;
            //this.ForeColor = Color.White;
            //this.Font = new Font("Segoe UI", 10);
            //this.TextAlign = ContentAlignment.MiddleCenter;
            //this.Text = "OFF";
        }
        //Methods
        private GraphicsPath GetFigurePath()
        {
            int arcSize = this.Height - 1;
            Rectangle leftArc = new Rectangle(0, 0, arcSize, arcSize);
            Rectangle rightArc = new Rectangle(this.Width - arcSize - 2, 0, arcSize, arcSize);
            
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(leftArc, 90, 180);
            path.AddArc(rightArc, 270, 180);
            path.CloseFigure();
            return path;
        }
        protected override void OnPaint(PaintEventArgs pevent)
        {
            int toggleSize = this.Height - 5;
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            pevent.Graphics.Clear(this.Parent.BackColor);
            if (this.Checked) //ON
            {
                //Draw the control surface
                if (solidStyle)
                    pevent.Graphics.FillPath(new SolidBrush(this.onBackColor), GetFigurePath());
                else pevent.Graphics.DrawPath(new Pen(onBackColor, 2), GetFigurePath());
                //Draw the toggle
                pevent.Graphics.FillEllipse(new SolidBrush(this.onToggleColor), new Rectangle(this.Width - this.Height + 1, 2, toggleSize, toggleSize));
            }
            else //OFF
            {
                //Draw the control surface
                if (solidStyle)
                    pevent.Graphics.FillPath(new SolidBrush(this.offBackColor), GetFigurePath());
                else pevent.Graphics.DrawPath(new Pen(offBackColor, 2), GetFigurePath());
                //Draw the toggle
                pevent.Graphics.FillEllipse(new SolidBrush(this.offToggleColor), 
                    new Rectangle(2, 2, toggleSize, toggleSize));
            }
        }
    }
}