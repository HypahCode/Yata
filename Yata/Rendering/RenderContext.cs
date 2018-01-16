using System.Drawing;
using Yata.CoreNode;

namespace Yata.Rendering
{
    internal class RenderContext : IRenderContext
    {
        private Graphics graphics;
        private float zoom = 1.0f;
        private Font unchangedFont = new Font("Arial", 8);
        private Font font = new Font("Arial", 8);

        public RenderContext()
        {
            graphics = null;
            Offset = new PointF(0.0f, 0.0f);
            Zoom = zoom;
        }

        public PointF Offset { get; set; }
        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; ResizeFonts(); }
        }

        private void ResizeFonts()
        {
            font = new Font("Arial", 8 * zoom);
        }

        public Point PointToScreen(Point p)
        {
            float xx = (p.X * Zoom) + Offset.X;
            float yy = (p.Y * Zoom) + Offset.Y;
            return new Point((int)xx, (int)yy);
        }

        public PointF PointToScreen(float x, float y)
        {
            float xx = (x * Zoom) + Offset.X;
            float yy = (y * Zoom) + Offset.Y;
            return new Point((int)xx, (int)yy);
        }

        public Point ScreenToPoint(Point p)
        {
            PointF pF = new PointF((p.X - Offset.X) / zoom, (p.Y - Offset.Y) / zoom);
            return new Point((int)pF.X, (int)pF.Y);
        }

        public Rectangle RectangleToScreen(Rectangle r)
        {
            return new Rectangle(PointToScreen(r.Location), new Size((int)(r.Width * Zoom), (int)(r.Height * Zoom)));
        }

        public void DrawLine(Color color, Point p1, Point p2)
        {
            graphics.DrawLine(new Pen(color, (int)(3.0f * Zoom)), PointToScreen(p1), PointToScreen(p2));
        }

        public void FillRectangle(Brush brush, Rectangle rect)
        {
            graphics.FillRectangle(brush, RectangleToScreen(rect));
        }

        public void DrawImage(Bitmap bmp, Rectangle rect)
        {
            graphics.DrawImage(bmp, RectangleToScreen(rect));
        }

        public void DrawString(string text, SolidBrush textBrush, Point location)
        {
            graphics.DrawString(text, font, textBrush, PointToScreen(location));
        }

        public void FillEllipse(Brush brush, Rectangle rect)
        {
            graphics.FillEllipse(brush, RectangleToScreen(rect));
        }

        public float MeasureString(string text)
        {
            return graphics.MeasureString(text, unchangedFont).Width;
        }

        internal void SetGraphics(Graphics g)
        {
            graphics = g;
        }
    }
}
