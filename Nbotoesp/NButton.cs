using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace butoes.Nbotoesp
{
    class NButton : Button
    {
        private int borderSize = 3; // Tamanho da borda
        private int borderRadius = 20; // Raio da curvatura do botão
        private Color borderColor = Color.MediumSlateBlue; // Cor da borda do botão
        private Color originalBackColor; // Cor de fundo original do botão
        private Color clickedBackColor = Color.LightSlateGray; // Cor do fundo quando o botão for clicado
        private Color clickedBorderColor = Color.Cyan; // Cor da borda quando o botão for clicado
        private Color clickedTextColor = Color.Black; // Cor do texto quando o botão for clicado

        // Propriedades públicas para edição via Designer
        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; Invalidate(); }
        }

        public Color ClickedBackColor
        {
            get { return clickedBackColor; }
            set { clickedBackColor = value; Invalidate(); }
        }

        public Color ClickedBorderColor
        {
            get { return clickedBorderColor; }
            set { clickedBorderColor = value; Invalidate(); }
        }

        public Color ClickedTextColor
        {
            get { return clickedTextColor; }
            set { clickedTextColor = value; Invalidate(); }
        }

        public NButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.BackColor = Color.MediumSlateBlue; // Cor de fundo do botão
            this.ForeColor = Color.White; // Cor do texto do botão
            this.originalBackColor = this.BackColor; // Armazenar a cor original

            this.Resize += (s, e) => this.Invalidate();
            this.MouseDown += (s, e) => this.OnMouseDown(e); // Evento para detectar quando o botão for pressionado
            this.MouseUp += (s, e) => this.OnMouseUp(e); // Evento para detectar quando o botão for solto
        }

        // Evento quando o botão for pressionado (fica pálido)
        private void OnMouseDown(MouseEventArgs e)
        {
            this.BackColor = clickedBackColor; // Altera a cor de fundo para o clicado
            this.borderColor = clickedBorderColor; // Altera a cor da borda para o clicado
            this.ForeColor = clickedTextColor; // Altera a cor do texto para o clicado
            this.Invalidate(); // Força o botão a ser repintado com a nova cor
        }

        // Evento quando o botão for solto (volta para a cor original)
        private void OnMouseUp(MouseEventArgs e)
        {
            this.BackColor = originalBackColor; // Restaura a cor original
            this.borderColor = Color.MediumSlateBlue; // Restaura a cor original da borda
            this.ForeColor = Color.White; // Restaura a cor original do texto
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            int radius = Math.Min(borderRadius, Math.Min(this.Width, this.Height) / 2); // Evita deformações na curvatura
            Rectangle rectSurface = new Rectangle(0, 0, this.Width, this.Height);
            Rectangle rectBorder = new Rectangle(1, 1, this.Width - 2, this.Height - 2);

            // Desenhar o botão com bordas arredondadas
            using (GraphicsPath pathSurface = GetFigurePath(rectSurface, radius))
            using (GraphicsPath pathBorder = GetFigurePath(rectBorder, radius - 1))
            using (Pen penBorder = new Pen(borderColor, borderSize))
            {
                penBorder.Alignment = PenAlignment.Inset;
                this.Region = new Region(pathSurface); // Aplica a curvatura ao botão

                pevent.Graphics.FillPath(new SolidBrush(this.BackColor), pathSurface); // Preenche o botão com a cor de fundo
                pevent.Graphics.DrawPath(penBorder, pathBorder); // Desenha a borda do botão
            }

            // Desenhar o texto no botão
            using (Brush textBrush = new SolidBrush(this.ForeColor))
            {
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                pevent.Graphics.DrawString(this.Text, this.Font, textBrush, rectSurface, sf); // Desenha o texto centralizado
            }
        }

        private GraphicsPath GetFigurePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int curveSize = radius * 2;

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();

            return path;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (this.Parent != null)
                this.Parent.BackColorChanged += (s, ev) => this.Invalidate();
        }
    }
}