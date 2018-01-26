using System;
using System.Collections.Generic;
using Yata.CoreNode;
using Yata.CoreNode.PropertiesUi;

namespace Yata.ImageNodes.Nodes.Operations
{
    [NodeUsage(@"Image.Operations", nodeName)]
    public class SeamlessBordersNode : ImageNodeBase
    {
        private const string nodeName = @"Seamless borders";

        [DataTypeUISlider("Border size", 1, 99, 1)]
        private int borderSize = 20;

        private FloatColorInput inputImage;
        
        public SeamlessBordersNode()
            : base(nodeName)
        {
            inputImage = AddInput("Input");
            
            outputs.Add(new FloatColorOutput(this, "Output"));

            Init();
        }

        private class PixelBender
        {
            public FloatColor color;
            public float borderDistance;
            public PixelBender(FloatColor c, float dist)
            {
                color = c;
                borderDistance = dist;
            }
        }

        private float Mirror(float value)
        {
            float sizeDiv2 = 0.5f;
            if (value > sizeDiv2)
                return sizeDiv2 - (value - sizeDiv2);
            return value;
        }

        public override FloatColor GetPixel(float x, float y, bool preview)
        {
            float size = 1.0f;
            float border = borderSize / 200.0f;

            FloatColor mainPixel = inputImage.GetPixel(x, y, preview);

            List<PixelBender> colors = new List<PixelBender>();
            if (x < border) colors.Add(new PixelBender(inputImage.GetPixel(x, Mirror(y), preview), x));
            if (y < border) colors.Add(new PixelBender(inputImage.GetPixel(y, Mirror(x), preview), y));

            if (x > size - border) colors.Add(new PixelBender(inputImage.GetPixel(size - x, Mirror(size - y), preview), size - x));
            if (y > size - border) colors.Add(new PixelBender(inputImage.GetPixel(size - y, Mirror(x), preview), size - y));

            if (colors.Count == 0)
                return mainPixel;

            PixelBender edgePixel = colors[0];

            if (colors.Count == 2)
            {
                float lerp = (colors[0].borderDistance - colors[1].borderDistance) + (border / 2.0f);
                FloatColor borderColor = FloatColor.Lerp(colors[0].color, colors[1].color, (lerp / (float)border));
                edgePixel = new PixelBender(borderColor, Math.Max(colors[0].borderDistance, colors[1].borderDistance));
            }
            return FloatColor.Lerp(edgePixel.color, mainPixel, edgePixel.borderDistance / border);
            
        }

        public override bool ShowPropertiesDialog()
        {
            base.ShowPropertiesDialog();
            PropertiesFormWrapper form = new PropertiesFormWrapper(this, nodeName);
            return form.Show();
        }
    }
}
