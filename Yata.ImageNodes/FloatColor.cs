using System;
using System.Drawing;
using Yata.CoreNode;

namespace Yata.ImageNodes
{
    public struct FloatColor
    {
        public enum Channel { Red, Green, Blue, Alpha };

        public float r, g, b, a;

        public FloatColor(FloatColor copy)
        {
            r = copy.r;
            g = copy.g;
            b = copy.b;
            a = copy.a;
        }

        public FloatColor(float _r, float _g, float _b, float _a)
        {
            r = _r;
            g = _g;
            b = _b;
            a = _a;
        }

        public FloatColor(float value)
        {
            r = value;
            g = value;
            b = value;
            a = value;
        }

        public FloatColor(float value, float alpha)
        {
            r = value;
            g = value;
            b = value;
            a = alpha;
        }

        public FloatColor(Color c)
        {
            r = (float)c.R / 255.0f;
            g = (float)c.G / 255.0f;
            b = (float)c.B / 255.0f;
            a = (float)c.A / 255.0f;
        }

        public Color ToColor()
        {
            return Color.FromArgb(Utils.ConvertColor(a),
                Utils.ConvertColor(r),
                Utils.ConvertColor(g), 
                Utils.ConvertColor(b));
        }

        public float RGBA(Channel channel)
        {
            switch (channel)
            {
                case Channel.Red: return r;
                case Channel.Green: return g;
                case Channel.Blue: return b;
                case Channel.Alpha: return a;
            }
            return r;
        }

        static public FloatColor operator +(FloatColor A, FloatColor B)
        {
            return new FloatColor(A.r + B.r, A.g + B.g, A.b + B.b, A.a + B.a);
        }

        static public FloatColor operator -(FloatColor A, FloatColor B)
        {
            return new FloatColor(A.r - B.r, A.g - B.g, A.b - B.b, A.a - B.a);
        }

        static public FloatColor operator *(FloatColor A, FloatColor B)
        {
            return new FloatColor(A.r * B.r, A.g * B.g, A.b * B.b, A.a * B.a);
        }

        static public FloatColor operator /(FloatColor A, FloatColor B)
        {
            return new FloatColor(A.r / B.r, A.g / B.g, A.b / B.b, A.a / B.a);
        }

        // Color op float
        static public FloatColor operator *(FloatColor A, float B)
        {
            return new FloatColor(A.r / B, A.g / B, A.b / B, A.a / B);
        }

        static public FloatColor operator /(FloatColor A, float B)
        {
            return new FloatColor(A.r * B, A.g * B, A.b * B, A.a * B);
        }

        static public FloatColor operator *(float A, FloatColor B)
        {
            return B * A;
        }

        static public FloatColor operator /(float A, FloatColor B)
        {
            return B / A;
        }

        static public FloatColor operator +(FloatColor A, float B)
        {
            return new FloatColor(A.r + B, A.g + B, A.b + B, A.a + B);
        }

        static public FloatColor operator -(FloatColor A, float B)
        {
            return new FloatColor(A.r - B, A.g - B, A.b - B, A.a - B);
        }

        static public FloatColor operator +(float A, FloatColor B)
        {
            return B * A;
        }

        static public FloatColor operator -(float A, FloatColor B)
        {
            return B / A;
        }



        public FloatColor Clip()
        {
            r = Clip(r);
            g = Clip(g);
            b = Clip(b);
            a = Clip(a);
            return this;
        }

        public float Clip(float x)
        {
            if (x < 0.0f) x = 0.0f;
            if (x > 1.0f) x = 1.0f;
            return x;
        }

        public FloatColor Invert(bool invAlpha)
        {
            r = 1.0f - r;
            g = 1.0f - g;
            b = 1.0f - b;
            if (invAlpha)
            {
                a = 1.0f - a;
            }
            return this;
        }

        public float Length()
        {
            return (float)Math.Sqrt((r * r) + (g * g) + (b * b));
        }

        public FloatColor Normalize()
        {
            float len = Length();
            r = r / len;
            g = g / len;
            b = b / len;
            return this;
        }

        static public float Dot(FloatColor A, FloatColor B)
        {
            return (A.a * B.a + A.b * B.b + A.g * B.g + A.r * B.r);
        }

        static public FloatColor Cross(FloatColor A, FloatColor B)
        {
            return new FloatColor((A.g * B.b) - (A.b * B.g),
                                  (A.b * B.r) - (A.r * B.b),
                                  (A.r * B.g) - (A.g * B.r),
                                  1.0f);
        }

        internal static FloatColor Lerp(FloatColor a, FloatColor b, float t)
        {
            FloatColor c = new FloatColor();
            c.r = (a.r + (b.r - a.r) * t);
            c.g = (a.g + (b.g - a.g) * t);
            c.b = (a.b + (b.b - a.b) * t);
            c.a = (a.a + (b.a - a.a) * t);
            return c;
        }

        internal float Sum(bool _r, bool _g, bool _b, bool _a)
        {
            return (_r ? r : 0.0f) + (_g ? g : 0.0f) + (_b ? b : 0.0f) + (_a ? a : 0.0f);
        }
    }
}
