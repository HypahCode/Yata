using System.Drawing;

namespace Yata
{
    public class Utils
    {
        private Utils() { }

        public static bool PointInsideRect(Point p, Rectangle r)
        {
            return ((p.X > r.Left) && (p.Y > r.Top) && (p.X < r.Right) && (p.Y < r.Bottom));
        }

        public static bool PointInsideRect(Point p, RectangleF r)
        {
            return ((p.X > r.Left) && (p.Y > r.Top) && (p.X < r.Right) && (p.Y < r.Bottom));
        }

        public static Rectangle RectFToRect(RectangleF r)
        {
            return new Rectangle((int)r.X, (int)r.Y, (int)r.Width, (int)r.Height);
        }

        public static Point GetRectCenterPoint(Rectangle r)
        {
            return new Point(r.X + r.Width / 2, r.Y + r.Height / 2);
        }

        /// <summary>
        /// YColor R, G, B or A to some int (value * 255)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ConvertColor(float value)
        {
            int v = (int)(value * 255.0);
            if (v < 0) v = 0;
            if (v > 255) v = 255;
            return (int)v;
        }
    }
}
