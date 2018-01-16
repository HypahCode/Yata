using System.Drawing;

namespace Yata.CoreNode
{
    public interface IRenderContext
    {
        PointF Offset { get; }
        float Zoom { get; }

        void DrawLine(Color color, Point p1, Point p2);
        void FillRectangle(Brush b, Rectangle boundingRect);
        void DrawImage(Bitmap bmp, Rectangle rect);
        void DrawString(string text, SolidBrush textBrush, Point location);
        void FillEllipse(Brush circleBrush, Rectangle bounds);
        float MeasureString(string text);
    }
}
