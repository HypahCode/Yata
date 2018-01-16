using Yata.CoreNode;
using System.Drawing;

namespace Yata.ImageNodes
{
    public class FloatColorInput : NodeInput
    {
        public FloatColor defaultOutColor;

        public FloatColorInput(Node parent, string name)
            : this(parent, name, new FloatColor(0.0f, 0.0f, 0.0f, 1.0f))
        {
        }

        public FloatColorInput(Node parent, string name, FloatColor defaultColor)
            : base(parent, name)
        {
            defaultOutColor = defaultColor;
            drawColor = Color.FromArgb(220, 50, 50);
        }

        public FloatColor GetPixel(float x, float y, bool preview)
        {
            FloatColorOutput output = outputChannel as FloatColorOutput;

            if (outputChannel == null)
            {
                return defaultOutColor;
            }
            return ((FloatColorOutput)outputChannel).GetPixel(x, y, preview);
        }

        public override bool SetOutputChannel(NodeOutput output)
        {
            if (output is FloatColorOutput)
            {
                return base.SetOutputChannel(output);
            }
            return false;
        }
    }
}
