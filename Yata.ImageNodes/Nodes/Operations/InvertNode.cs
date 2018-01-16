using System.Drawing;

namespace Yata.ImageNodes.Nodes.Operations
{
    public class InvertNode : ImageNodeBase
    {
        private FloatColorInput input;

        public InvertNode()
            : base(FriendlyName)
        {
            input = AddInput("Input");
            outputs.Add(new FloatColorOutput(this, "Output"));
            Init();
        }

        public override FloatColor GetPixel(float x, float y, bool preview)
        {
            return input.GetPixel(x, y, preview).Invert(false);
        }

        public static string FriendlyName
        {
            get { return "Invert"; }
        }

        public static string SubMenuPath
        {
            get { return "Image.Operations"; }
        }

        public static Bitmap Icon
        {
            get { return new Bitmap(typeof(InvertNode), "Icons.Invert.png"); }
        }
    }
}
