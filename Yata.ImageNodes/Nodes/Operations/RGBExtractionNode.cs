using Yata.CoreNode;
using Yata.CoreNode.PropertiesUi;

namespace Yata.ImageNodes.Nodes.Operations
{
    [NodeUsage(@"Image.Operations", nodeName)]
    public class RGBExtractionNode : ImageNodeBase
    {
        private const string nodeName = @"RGBA extraction";

        private class ChannelOutput : FloatColorOutput
        {
            private FloatColor blackValues;

            public ChannelOutput(Node parent, string name, FloatColor blacks)
                : base(parent, name)
            {
                blackValues = blacks;
            }

            public override FloatColor GetPixel(float x, float y, bool preview)
            {
                return ((ImageNodeBase)GetParent()).GetPixel(x, y, preview) * blackValues;
            }
        }

        [DataTypeUIBool("Ignore alpha for RGB")]
        private bool ignoreAlpha = true;

        private FloatColorInput input;

        public RGBExtractionNode()
            : base(nodeName)
        {
            input = AddInput("Input");

            outputs.Add(new ChannelOutput(this, "R", new FloatColor(1.0f, 0.0f, 0.0f, ignoreAlpha ? 1.0f : 0.0f)));
            outputs.Add(new ChannelOutput(this, "G", new FloatColor(0.0f, 1.0f, 0.0f, ignoreAlpha ? 1.0f : 0.0f)));
            outputs.Add(new ChannelOutput(this, "B", new FloatColor(0.0f, 0.0f, 1.0f, ignoreAlpha ? 1.0f : 0.0f)));
            outputs.Add(new ChannelOutput(this, "A", new FloatColor(0.0f, 0.0f, 0.0f, 1.0f)));

            Init();
        }

        public override FloatColor GetPixel(float x, float y, bool preview)
        {
            return input.GetPixel(x, y, preview);
        }
    }
}
