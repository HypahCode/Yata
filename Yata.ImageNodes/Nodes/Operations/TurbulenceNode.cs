using Yata.CoreNode;
using Yata.CoreNode.PropertiesUi;

namespace Yata.ImageNodes.Nodes.Operations
{
    [NodeUsage(@"Image.Operations", nodeName)]
    public class TurbulenceNode : ImageNodeBase
    {
        private const string nodeName = @"Turbulence";
        private const int maxValue = 100;

        private FloatColorInput input;
        private FloatColorInput turbulence;

        [DataTypeUISlider("amount", 1, maxValue, 1)]
        private int amount = 20;

        public TurbulenceNode()
            : base(nodeName)
        {
            input = AddInput("Input");
            turbulence = AddInput("Turbulence");
            outputs.Add(new FloatColorOutput(this, "Output"));
            Init();
        }

        public override FloatColor GetPixel(float x, float y, bool preview)
        {
            float size = (float)amount / maxValue;

            FloatColor terb = turbulence.GetPixel(x, y, preview);
            return input.GetPixel(x + (terb.r * size), y + (terb.g * size), preview);
        }

        public override bool ShowPropertiesDialog()
        {
            base.ShowPropertiesDialog();
            PropertiesFormWrapper form = new PropertiesFormWrapper(this, nodeName);
            return form.Show();
        }
    }
}
