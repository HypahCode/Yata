using Yata.CoreNode;
using Yata.CoreNode.PropertiesUi;

namespace Yata.ImageNodes.Nodes.Operations
{
    [NodeUsage(@"Image.Operations", nodeName)]
    public class FlipNode : ImageNodeBase
    {
        private const string nodeName = @"Flip";

        private FloatColorInput input;

        [DataTypeUIBool("Flip X")]
        private bool flipX = true;
        [DataTypeUIBool("Flip Y")]
        private bool flipY = false;

        public FlipNode()
            : base(nodeName)
        {
            input = AddInput("Input");
            outputs.Add(new FloatColorOutput(this, "Output"));
            Init();
        }

        public override FloatColor GetPixel(float x, float y, bool preview)
        {
            if (flipX) x = 1.0f - x;
            if (flipY) y = 1.0f - y;
            return input.GetPixel(x, y, preview);
        }

        public override bool ShowPropertiesDialog()
        {
            base.ShowPropertiesDialog();
            PropertiesFormWrapper form = new PropertiesFormWrapper(this, nodeName);
            return form.Show();
        }
    }
}
