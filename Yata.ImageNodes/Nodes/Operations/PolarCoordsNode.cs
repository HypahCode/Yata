using System;
using Yata.CoreNode.PropertiesUi;

namespace Yata.ImageNodes.Nodes.Operations
{
    public class PolarCoordsNode : ImageNodeBase
    {
        private FloatColorInput input;

        [DataTypeUIBool("To polar")]
        private bool toPolar;

        public PolarCoordsNode()
            : base(FriendlyName)
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

        public static string FriendlyName
        {
            get { return "Polar coords"; }
        }

        public static string SubMenuPath
        {
            get { return "Image.Operations"; }
        }

        public override bool ShowPropertiesDialog()
        {
            base.ShowPropertiesDialog();
            PropertiesFormWrapper form = new PropertiesFormWrapper(this, FriendlyName);
            return form.Show();
        }
    }
}
