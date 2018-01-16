using System.Windows.Forms;
using System.Drawing;

namespace Yata.CoreNode
{
    public class EnvenlopeControl : Panel
    {
        private Envelope envenlope;

        public Envelope Env { get { return envenlope; } }

        public EnvenlopeControl(Envelope env)
        {
            envenlope = env;

            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            Paint += new PaintEventHandler(EnvenlopeControl_Paint);

            Invalidate();
        }

        private void EnvenlopeControl_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(64, 64, 64)), new Rectangle(0, 0, Width, Height));

            for (int l = 0; l < envenlope.GetLines().Count; l++)
            {
                Yata.CoreNode.Envelope.EnvelopeLine line = envenlope.GetLines()[l];
                Point[] points = new Point[Width];

                for (int x = 0; x < Width; x++)
                {
                    int y = (int)(line.GetValue((float)x / (float)Width) * ((float) Height));
                    points[x] = new Point(x, Height - y);
                }
                Pen pen = new Pen(Color.White);
                e.Graphics.DrawLines(pen, points);
            }
        }        
    }
}
