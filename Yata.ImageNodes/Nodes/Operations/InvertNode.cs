using System.Drawing;
using Yata.CoreNode;

namespace Yata.ImageNodes.Nodes.Operations
{
    [NodeUsage(@"Image.Operations", nodeName)]
    public class InvertNode : ImageNodeBase
    {
        private const string nodeName = @"Invert";

        private FloatColorInput input;

        public InvertNode()
            : base(nodeName)
        {
            input = AddInput("Input");
            outputs.Add(new FloatColorOutput(this, "Output"));
            Init();
        }

        public override FloatColor GetPixel(float x, float y, bool preview)
        {
            return input.GetPixel(x, y, preview).Invert(false);
        }

        public static Bitmap Icon
        {
            get { return new Bitmap(typeof(InvertNode), "Icons.Invert.png"); }
        }
    }
}
