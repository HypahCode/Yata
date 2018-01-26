using System;
using Yata.CoreNode;
using Yata.CoreNode.PropertiesUi;

namespace Yata.ImageNodes.Nodes.Operations
{
    [NodeUsage(@"Image.Operations", nodeName)]
    public class TwirlNode : ImageNodeBase
    {
        private const string nodeName = @"Twirl";
        private const int maxValue = 1000;

        private FloatColorInput input;

        [DataTypeUISlider("amount", -maxValue, maxValue, 1)]
        private int amount = 20;

        public TwirlNode()
            : base(nodeName)
        {
            input = AddInput("Input");
            outputs.Add(new FloatColorOutput(this, "Output"));
            Init();
        }

        public override FloatColor GetPixel(float x, float y, bool preview)
        {
            float twirlValue = (float)amount / (maxValue / 100);

            x -= 0.5f;
            y -= 0.5f;
            float dist = (float)Math.Sqrt(x * x + y * y);

            float s = (float)Math.Sin(amount * dist);
            float c = (float)Math.Cos(amount * dist);

            float xnew = x * c - y * s;
            float ynew = x * s + y * c;

            x = xnew + 0.5f;
            y = ynew + 0.5f;

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
