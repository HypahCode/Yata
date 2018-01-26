using System;
using Yata.CoreNode;
using Yata.CoreNode.PropertiesUi;

namespace Yata.ImageNodes.Nodes.Generators
{
    [NodeUsage(@"Image.Generator", nodeName)]
    public class FlareGeneratorNodee : ImageNodeBase
    {
        private const string nodeName = @"Flare";

        private enum CircleFunc { normal, sin, cos }

        [DataTypeUIEnum("Circle function")]
        private CircleFunc circleFunction = CircleFunc.normal;
        [DataTypeUINumeric("Circle count", 0, 9999999)]
        public int circleCount = 1;

        public FlareGeneratorNodee()
            : base(nodeName)
        {
            outputs.Add(new FloatColorOutput(this, "Output"));
            Init();
        }

        public override FloatColor GetPixel(float x, float y, bool preview)
        {
            x = WrapCoord(x) - 0.5f;
            y = WrapCoord(y) - 0.5f;

            float grey = 1.0f - (float)Math.Sqrt(x * x + y * y) * 2.0f;

            if (circleFunction != CircleFunc.normal)
            {
                grey = FineTuneGrayScale(grey);
            }
            
            return new FloatColor(grey).Clip();
        }

        private float FineTuneGrayScale(float grey)
        {
            switch (circleFunction)
            {
                case CircleFunc.sin: return (float)Math.Sin(grey * (float)Math.PI * circleCount);
                case CircleFunc.cos: return (float)Math.Cos(grey * (float)Math.PI * circleCount);
            }
            return grey;
        }

        public override bool ShowPropertiesDialog()
        {
            base.ShowPropertiesDialog();
            PropertiesFormWrapper form = new PropertiesFormWrapper(this, nodeName);
            return form.Show();
        }
    }
}
