using System;
using Yata.CoreNode.PropertiesUi;

namespace Yata.ImageNodes.Nodes.Generators
{
    public class FlareGeneratorNodee : ImageNodeBase
    {
        private enum CircleFunc { normal, sin, cos }

        [DataTypeUIEnum("Circle function")]
        private CircleFunc circleFunction = CircleFunc.normal;
        [DataTypeUINumeric("Circle count", 0, 9999999)]
        public int circleCount = 1;

        public FlareGeneratorNodee()
            : base(FriendlyName)
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

        public static string FriendlyName
        {
            get { return "Flare"; }
        }

        public static string SubMenuPath
        {
            get { return "Image.Generator"; }
        }

        public override bool ShowPropertiesDialog()
        {
            base.ShowPropertiesDialog();
            PropertiesFormWrapper form = new PropertiesFormWrapper(this, FriendlyName);
            return form.Show();
        }
    }
}
