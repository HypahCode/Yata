using System;
using Yata.CoreNode;
using Yata.CoreNode.PropertiesUi;

namespace Yata.ImageNodes.Nodes.Operations
{
    [NodeUsage(@"Image.Operations", nodeName)]
    public class BlendNode : ImageNodeBase
    {
        private const string nodeName = @"Blend";

        private enum BlendMode { Mix, Add, Mul, Sub, ColorBurn, LinearBurn, Screen, ColorDodge, Inversion, Exclusion };

        private FloatColorInput inputA;
        private FloatColorInput inputB;

        [DataTypeUIEnum("Blend mode")]
        private BlendMode blendMode = BlendMode.Add;
        [DataTypeUISlider("Blend ammount", 0, 1000, 10)]
        private int blendAmount = 1000;

        public BlendNode()
            : base(nodeName)
        {
            inputA = AddInput("Input A");
            inputB = AddInput("Input B");

            outputs.Add(new FloatColorOutput(this, "Output"));

            Init();
        }

        public override FloatColor GetPixel(float x, float y, bool preview)
        {
            FloatColor a = inputA.GetPixel(x, y, preview);
            FloatColor b = inputB.GetPixel(x, y, preview);
            FloatColor result = new FloatColor(1.0f);
            switch (blendMode)
            {
                case BlendMode.Mix: result = b; break;
                case BlendMode.Add: result = a + b; break;
                case BlendMode.Mul: result = a * b; break;
                case BlendMode.Sub: result = a - b; break;
                case BlendMode.ColorBurn: result = (new FloatColor(1.0f) - (new FloatColor(1.0f) - a) / b); break;
                case BlendMode.LinearBurn: result = (a + b) - new FloatColor(1.0f); break;
                case BlendMode.Screen: result = (new FloatColor(1.0f) - a) * (new FloatColor(1.0f) - b); break;
                case BlendMode.ColorDodge: result = b / (new FloatColor(1.0f) - a); break;
                case BlendMode.Inversion:
                    {
                        FloatColor col = a - b;
                        col.r = Math.Abs(col.r);
                        col.g = Math.Abs(col.g);
                        col.b = Math.Abs(col.b);
                        col.a = Math.Abs(col.a);
                        result = col;
                    }
                    break;
                case BlendMode.Exclusion: result = a * b - new FloatColor(2.0f) * a * b; break;
                default:
                    result = a + b;
                    break;
            }

            return FloatColor.Lerp(a, result, (blendAmount / 1000.0f) * a.a);
        }

        public override bool ShowPropertiesDialog()
        {
            base.ShowPropertiesDialog();
            PropertiesFormWrapper form = new PropertiesFormWrapper(this, nodeName);
            return form.Show();
        }
    }
}
