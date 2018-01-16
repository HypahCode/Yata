using System;
using System.Collections.Generic;
using Yata.CoreNode;
using Yata.CoreNode.PropertiesUi;

namespace Yata.ImageNodes.Nodes.Generators
{
    public class VoronoiGeneratorNode : ImageNodeBase
    {
        private struct Point
        {
            public float x;
            public float y;
            public Point(float _x, float _y)
            {
                x = _x;
                y = _y;
            }
        }

        private const int maxShift = 1000;

        public enum Distribution { Random, Square, SquareStaticShift, SquareRandomShift }
        public enum DistanceFunction { Normal, Manhattan, Chebyshev }

        [DataTypeUISlider("Total points", 2, 300, 10)]
        private int totalPoints = 10;
        [DataTypeUINumeric("Random seed", 0, 100000)]
        private int randomSeed = 0;
        [DataTypeUIEnum("Distribution")]
        private Distribution distribution = Distribution.Random;
        [DataTypeUIEnum("Distance function")]
        private DistanceFunction distanceFunc = DistanceFunction.Normal;
        [DataTypeUISlider("Shift X", 0, maxShift, 10)]
        public int shiftX = 10;
        [DataTypeUISlider("Shift Y", 0, maxShift, 10)]
        public int shiftY = 10;


        private List<Point> points = new List<Point>();

        private FloatColor black = new FloatColor(0.0f, 1.0f);
        private FloatColor white = new FloatColor(1.0f, 1.0f);

        public VoronoiGeneratorNode()
            : base(FriendlyName)
        {
            outputs.Add(new FloatColorOutput(this, "Output"));
            Init();
        }

        protected override void Setup()
        {
            base.Setup();
            GeneratePoints();
        }

        public override FloatColor GetPixel(float x, float y, bool preview)
        {
            x = WrapCoord(x);
            y = WrapCoord(y);

            SortedList<float, Point> sortedPoints = GetClosestPoints(new Point(x, y));

            float d1 = sortedPoints.Keys[0];
            float d2 = sortedPoints.Keys[1];
            float totalDist = (d1 + d2) / 2.0f;
            float t = d1 / totalDist;
            return FloatColor.Lerp(black, white, t);
        }

        private SortedList<float, Point> GetClosestPoints(Point point)
        {
            SortedList<float, Point> closest = new SortedList<float, Point>();
            foreach (Point p in points)
            {
                float distance = DistWrapped(p, point);

                if (!closest.ContainsKey(distance))
                {
                    closest.Add(distance, p);
                }
            }
            return closest;
        }

        private float DistWrapped(Point p1, Point p2)
        {
            float d = Dist(p1, p2);
            float dTmp;
            float x = p1.x;
            float y = p1.y;

            // X = var, Y = -1.0f
            dTmp = Dist(new Point(x - 1.0f, y - 1.0f), p2);
            if (dTmp < d) d = dTmp;

            dTmp = Dist(new Point(x, y - 1.0f), p2);
            if (dTmp < d) d = dTmp;

            dTmp = Dist(new Point(x + 1.0f, y - 1.0f), p2);
            if (dTmp < d) d = dTmp;

            // X = var, Y = 0.0f
            dTmp = Dist(new Point(x - 1.0f, y), p2);
            if (dTmp < d) d = dTmp;

            dTmp = Dist(new Point(x + 1.0f, y), p2);
            if (dTmp < d) d = dTmp;

            // X = var, Y = -1.0f
            dTmp = Dist(new Point(x - 1.0f, y + 1.0f), p2);
            if (dTmp < d) d = dTmp;

            dTmp = Dist(new Point(x, y + 1.0f), p2);
            if (dTmp < d) d = dTmp;

            dTmp = Dist(new Point(x + 1.0f, y + 1.0f), p2);
            if (dTmp < d) d = dTmp;

            return d;
        }

        private float Dist(Point p1, Point p2)
        {
            if (distanceFunc == DistanceFunction.Normal)
                return DistNormal(p1, p2);
            else if (distanceFunc == DistanceFunction.Manhattan)
                return DistManhattan(p1, p2);
            else
                return DistChebyshev(p1, p2);
        }

        private float DistNormal(Point p1, Point p2)
        {
            float a = p1.x - p2.x;
            float b = p1.y - p2.y;
            return (float)Math.Sqrt((a * a) + (b * b));
        }

        private float DistManhattan(Point p1, Point p2)
        {
            float a = Math.Abs(p1.x - p2.x);
            float b = Math.Abs(p1.y - p2.y);
            return a + b;
        }

        private float DistChebyshev(Point p1, Point p2)
        {
            float a = Math.Abs(p1.x - p2.x);
            float b = Math.Abs(p1.y - p2.y);
            return Math.Max(a, b);
        }

        private void GeneratePoints()
        {
            switch (distribution)
            {
                case Distribution.Random:
                    GeneratePointsRandom();
                    break;
                case Distribution.Square:
                    GeneratePointsSquare();
                    break;
                case Distribution.SquareStaticShift:
                    GeneratePointsSquare();
                    ShiftStatic();
                    break;
                case Distribution.SquareRandomShift:
                    GeneratePointsSquare();
                    ShiftRandom();
                    break;
            }
        }

        private void GeneratePointsRandom()
        {
            points.Clear();
            Random rand = new Random(randomSeed);
            for (int i = 0; i < totalPoints; i++)
            {
                float x = (float)rand.NextDouble();
                float y = (float)rand.NextDouble();
                points.Add(new Point(x, y));
            }
        }

        private void GeneratePointsSquare()
        {
            int pAxis = (int)Math.Sqrt(totalPoints);
            if (pAxis < 1) { pAxis = 1; }

            points.Clear();
            for (int x = 0; x < pAxis; x++)
            {
                for (int y = 0; y < pAxis; y++)
                {
                    float xx = (float)x / pAxis;
                    float yy = (float)y / pAxis;
                    points.Add(new Point(xx, yy));
                }
            }
        }

        private void ShiftStatic()
        {
            for (int i = 0; i < points.Count; i++)
            {
                Point p = points[i];
                p.x = points[i].x + shiftX;
                p.y = points[i].y + shiftY;
                points[i] = p;
            }
        }

        private void ShiftRandom()
        {
            Random rand = new Random(randomSeed);
            for (int i = 0; i < points.Count; i++)
            {
                Point p = points[i];
                p.x = points[i].x + (float)rand.NextDouble() * shiftX;
                p.y = points[i].y + (float)rand.NextDouble() * shiftY;
                points[i] = p;
            }
        }

        public static string FriendlyName
        {
            get { return "Voronoi"; }
        }

        public static string SubMenuPath
        {
            get { return "Image.Generator"; }
        }

        public override bool ShowPropertiesDialog()
        {
            base.ShowPropertiesDialog();
            PropertiesFormWrapper form = new PropertiesFormWrapper(this, FriendlyName);
            return form.Show();
        }
    }
}
