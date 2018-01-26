using Yata.CoreNode;
using Yata.CoreNode.PropertiesUi;

namespace Yata.ImageNodes.Nodes.Generators
{
    [NodeUsage(@"Image.Generator", nodeName)]
    public class GradientGeneratorNode : ImageNodeBase
    {
        private const string nodeName = @"Gradient";

        private enum Direction { LeftToRight, RightToLeft, TopToBottom, BottomToTop, CenterHorizontal, CenterVertical}

        [DataTypeUIEnum("Direction")]
        private Direction direction = Direction.LeftToRight;

        public GradientGeneratorNode()
            : base(nodeName)
        {
            outputs.Add(new FloatColorOutput(this, "Output"));
            Init();
        }

        public override FloatColor GetPixel(float x, float y, bool preview)
        {
            FloatColor color = new FloatColor(0.0f);
            switch (direction)
            {
                case Direction.LeftToRight: color = new FloatColor(x); break;
                case Direction.RightToLeft: color = new FloatColor(1.0f - x); break;
                case Direction.TopToBottom: color = new FloatColor(y); break;
                case Direction.BottomToTop: color = new FloatColor(1.0f - y); break;
                case Direction.CenterHorizontal: color = new FloatColor(Mirror(y)); break;
                case Direction.CenterVertical: color = new FloatColor(Mirror(x)); break;
            }
            color.a = 1.0f;
            return color;
        }

        private float Mirror(float v)
        {
            v *= 2.0f;
            return (v > 1.0f) ? 2.0f - v : v;
        }

        public override bool ShowPropertiesDialog()
        {
            base.ShowPropertiesDialog();
            PropertiesFormWrapper form = new PropertiesFormWrapper(this, nodeName);
            return form.Show();
        }
    }
}
