using System;
using System.Windows.Forms;
using System.Drawing;

namespace Yata.CoreNode
{
    public partial class ColorPicker : Form
    {
        private Color gradientColor = Color.FromArgb(255, 0, 0, 255);
        private Color selectedColor = Color.Red;

        private Bitmap colorGradient = new Bitmap(255, 255);
        private Bitmap colorSpectrum = new Bitmap(32, 255);

        public Color Color
        {
            get { return selectedColor; }
        }

        public ColorPicker(Color color)
        {
            selectedColor = color;

            InitializeComponent();
            gradientPanel.Paint += new PaintEventHandler(gradientPanel_Paint);
            gradientPanel.MouseDown += new MouseEventHandler(gradientPanel_MouseDown);

            spectrumPanel.Paint += new PaintEventHandler(spectrumPanel_Paint);
            spectrumPanel.MouseDown += new MouseEventHandler(spectrumPanel_MouseDown);

            colorPanel.Paint += new PaintEventHandler(colorPanel_Paint);

            gradientPanel.Invalidate();
            spectrumPanel.Invalidate();
        }

        private void gradientPanel_MouseDown(object sender, MouseEventArgs e)
        {
            selectedColor = GetGradientColor(e.X, e.Y);
            colorPanel.Invalidate();
        }

        private void colorPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(new SolidBrush(selectedColor), new Rectangle(0, 0, colorPanel.Width, colorPanel.Height));
        }

        private void spectrumPanel_MouseDown(object sender, MouseEventArgs e)
        {
            int y = e.Y;
            gradientColor = GetSpectrumColor(y);
            gradientPanel.Invalidate();
        }

        private void spectrumPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            UpdateSpectrum();
            g.DrawImage(colorSpectrum, new Rectangle(0, 0, colorSpectrum.Width, colorSpectrum.Height));
        }

        private void gradientPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            UpdateGradient();
            g.DrawImage(colorGradient, new Rectangle(0, 0, colorGradient.Width, colorGradient.Height));
        }

        private void UpdateSpectrum()
        {
            for (int y = 0; y < colorSpectrum.Height; y++)
            {
                Color c = GetSpectrumColor(y);
                for (int x = 0; x < colorSpectrum.Width; x++)
                {
                    colorSpectrum.SetPixel(x, y, c);
                }
            }
        }

        private Color GetSpectrumColor(int y)
        {
            Color[] colors = { Color.Red, Color.Magenta, Color.Blue, Color.Cyan, Color.Lime, Color.Yellow };
            int steps = (int)(255.0f / (float)colors.Length);


            Color c1 = Color.Red, c2 = Color.Blue;
            for (int i = 0; i < colors.Length; i++)
            {
                if (y < (steps * (i + 1)))
                {
                    c1 = colors[i % colors.Length];
                    c2 = colors[(i + 1) % colors.Length];
                    break;
                }
            }

            int yy = y % steps;
            Color c = ColorLerp(c1, c2, (int)(((float)yy / steps) * 255.0f));
            return c;
        }

        private void UpdateGradient()
        {
            Color c00 = Color.FromArgb(255, 255, 255);
            Color c01 = Color.FromArgb(0, 0, 0);
            Color c10 = gradientColor;
            Color c11 = Color.FromArgb(0, 0, 0);

            for (int y = 0; y < colorGradient.Height; y++)
            {
                Color c1 = ColorLerp(c00, c01, y);
                Color c2 = ColorLerp(c10, c11, y);
                for (int x = 0; x < colorGradient.Height; x++)
                {
                    Color c = ColorLerp(c1, c2, x);
                    colorGradient.SetPixel(x, y, c);
                }
            }
        }

        private Color GetGradientColor(int x, int y)
        {
            Color c00 = Color.FromArgb(255, 255, 255);
            Color c01 = Color.FromArgb(0, 0, 0);
            Color c10 = gradientColor;
            Color c11 = Color.FromArgb(0, 0, 0);

            Color c1 = ColorLerp(c00, c01, y);
            Color c2 = ColorLerp(c10, c11, y);

            return ColorLerp(c1, c2, x);
        }

        private Color NormalizeColor(Color c)
        {
            float len = (float)Math.Sqrt(c.R * c.R + c.G * c.G + c.B * c.B);
            float r = (float)c.R / len;
            float g = (float)c.G / len;
            float b = (float)c.B / len;
            return Color.FromArgb((int)(r * 255.0f), (int)(g * 255.0f), (int)(b * 255.0f));
        }

        private Color ColorLerp(Color c1, Color c2, int t)
        {
            float amount = (float)t / 255.0f;
            int _r = (int)((float)c1.R + ((float)c2.R - (float)c1.R) * amount);
            int _g = (int)((float)c1.G + ((float)c2.G - (float)c1.G) * amount);
            int _b = (int)((float)c1.B + ((float)c2.B - (float)c1.B) * amount);

            return Color.FromArgb(_r, _g, _b);
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
