using System;
using Yata.CoreNode;
using Yata.CoreNode.PropertiesUi;

namespace Yata.ImageNodes.Nodes.Operations
{
    [NodeUsage(@"Image.Operations", nodeName)]
    public class PolarCoordsNode : ImageNodeBase
    {
        private const string nodeName = @"Polar coords";
        
        private FloatColorInput input;

        [DataTypeUIBool("To polar")]
        private bool toPolar;

        public PolarCoordsNode()
            : base(nodeName)
        {
            input = AddInput("Input");

            outputs.Add(new FloatColorOutput(this, "Output"));

            Init();
        }

        public override FloatColor GetPixel(float x, float y, bool preview)
        {
            x -= 0.5f;
            y -= 0.5f;
            if (toPolar)
            {
                float r = (float)Math.Sqrt((x * x) + (y * y));
                float q = (float)Math.Atan2(y, x);

                x = r;
                y = q;

            }
            else
            {
                float r = x;
                float q = y;
                x = r * (float)Math.Cos(q);
                y = r * (float)Math.Sin(q);
            }
            x += 0.5f;
            y += 0.5f;

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
