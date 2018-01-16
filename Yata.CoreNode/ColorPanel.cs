using System.Drawing;
using System.Windows.Forms;

namespace Yata.CoreNode
{
    public class ColorPanel : Panel
    {
        private Color color;

        public delegate void ChangeColorDelegate(Color c);
        public event ChangeColorDelegate ColorChanged;

        public Color Color
        {
            get { return color; }
            set { color = value; Invalidate(); }
        }
        
        public ColorPanel()
            : base()
        {
            Paint += new PaintEventHandler(ColorPanel_Paint);
            Click += new System.EventHandler(ColorPanel_Click);
        }

        private void ColorPanel_Click(object sender, System.EventArgs e)
        {
            ColorPicker picker = new ColorPicker(color);
            if (picker.ShowDialog() == DialogResult.OK)
            {
                Color = picker.Color;
                if (ColorChanged != null)
                {
                    ColorChanged(color);
                }
            }
        }

        private void ColorPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(color), new Rectangle(0, 0, Width, Height));
        }
    }
}
