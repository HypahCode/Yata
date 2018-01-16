using System;
using System.Drawing;

namespace Yata.ImageNodes.Nodes.Generators
{
    public class Flare : ImageNodeBase
    {
        public Flare()
            : base(FriendlyName)
        {
            outputs.Add(new FloatColorOutput(this, "Output"));

            Init();
        }

        public override FloatColor GetPixel(float x, float y, bool preview)
        {
            x = WrapCoord(x);
            y = WrapCoord(y);
            x -= 0.5f;
            y -= 0.5f;
            float grey = 1.0f - (float)Math.Sqrt(x * x + y * y) * 2;
            return new FloatColor(grey, 1.0f).Clip();
        }

        public static string FriendlyName
        {
            get { return "Flare"; }
        }

        public static string SubMenuPath
        {
            get { return "Image.Generator"; }
        }

        public static Bitmap Icon
        {
            get { return new Bitmap(typeof(Flare), "Icons.Flare.png"); }
        }
    }
}
