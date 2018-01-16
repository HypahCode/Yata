using System.Drawing;

namespace Yata.ImageNodes.Nodes.Operations
{
    public class EdgeDetectionNode : ImageNodeBase
    {
        private FloatColorInput input;
        private PointF offset = new PointF(0.01f, 0.01f);
        private FloatColor intensity = new FloatColor(0.8f, 0.0f);

        public EdgeDetectionNode()
            : base(FriendlyName)
        {
            input = AddInput("Input");

            outputs.Add(new FloatColorOutput(this, "Output"));

            Init();
        }

        public override void PrepareForRender(int w, int h)
        {
            base.PrepareForRender(w, h);
            offset.X = 1.0f / w;
            offset.Y = 1.0f / h;
        }

        public override FloatColor GetPixel(float x, float y, bool preview)
        {
            FloatColor col00 = input.GetPixel(x - offset.X, y - offset.Y, preview); 
            FloatColor col01 = input.GetPixel(x, y - offset.Y, preview);
            FloatColor col02 = input.GetPixel(x + offset.X, y - offset.Y, preview);

            FloatColor col10 = input.GetPixel(x - offset.X, y, preview);
            FloatColor col11 = input.GetPixel(x, y, preview);
            FloatColor col12 = input.GetPixel(x + offset.X, y, preview);

            FloatColor col20 = input.GetPixel(x - offset.X, y + offset.Y, preview);
            FloatColor col21 = input.GetPixel(x, y + offset.Y, preview);
            FloatColor col22 = input.GetPixel(x + offset.X, y + offset.Y, preview);

            FloatColor c = (col00+(col01+col01)+col02-col20-(col21+col21)-col22)+(col02+(col12+col12)+col22-col01-(col10+col10)-col20);
            c.a = 1.0f;

            return c;
        }

        public static string FriendlyName
        {
            get { return "Edge detection"; }
        }

        public static string SubMenuPath
        {
            get { return "Image.Operations"; }
        }
    }
}
