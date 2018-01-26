using Yata.CoreNode;

namespace Yata.ImageNodes.Nodes.Output
{
    [NodeUsage(@"Image.Output", nodeName)]
    public class RenderOutputNode : ImageNodeBase
    {
        private const string nodeName = @"Render output";

        private FloatColorInput input;

        public RenderOutputNode()
            : base(nodeName)
        {
            input = AddInput("Input");

            Init();
        }

        public override FloatColor GetPixel(float x, float y, bool preview)
        {
            return input.GetPixel(x, y, preview);
        }

        public override bool ShowPropertiesDialog()
        {
            base.ShowPropertiesDialog();

            RenderOutputForm output = new RenderOutputForm(this);
            output.ShowDialog();

            return false;
        }
    }
}
