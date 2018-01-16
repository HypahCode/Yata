using System;
using Yata.CoreNode;
using Yata.CoreNode.PropertiesUi;

namespace Yata.ImageNodes.Nodes.Generators
{
    public class PerlinNoiseGeneratorNode : ImageNodeBase
    {
        [DataTypeUINumeric("Random seed", 0, 9999999)]
        private int seed = 0;
        [DataTypeUINumeric("Octaves", 0, 9)]
        private int octaves = 4;

        private float amplitudeFalloff = 2.0f;

        public PerlinNoiseGeneratorNode()
            : base(FriendlyName)
        {
            outputs.Add(new FloatColorOutput(this, "Noise"));
            Init();
        }

        public override FloatColor GetPixel(float x, float y, bool preview)
        {
            float n = Noise(x, y);
            return new FloatColor(n, 1.0f);
        }

        float Noise(float _x, float _y)
        {
            float noise = 0;
            float multiplyFactor = 1.0f;

            int oct = (int)Math.Pow(2, octaves + 2);
            for (int i = 2; i < oct; i *= 2)
            {
                float x = _x * i;
                float y = _y * i;

                float c000 = Rand(Round(x, i), Round(y, i));
                float c001 = Rand(Round(x, i), Round(y + 1, i));
                float c010 = Rand(Round(x + 1, i), Round(y, i));
                float c011 = Rand(Round(x + 1, i), Round(y + 1, i));

                float i00 = Utils.Lerp(c000, c001, Frac(y));
                float i01 = Utils.Lerp(c010, c011, Frac(y));
                float iFinal = Utils.Lerp(i00, i01, Frac(x));

                multiplyFactor /= amplitudeFalloff;
                noise += iFinal * multiplyFactor;
            }
            return noise;
        }

        private int Round(float val, int warp)
        {
            return (int)Math.Floor(val) % warp;
        }

        private float Rand(int x, int y)
        {
            int val = (int)(Frac( (float)(Math.Sin(x) + Math.Cos(y)) ) * 1000000.0f);
            Random r = new Random(seed + val);
            return (float)r.NextDouble();
        }

        private float Frac(float value)
        {
            return value - (float)Math.Floor(value);
        }

        public override bool ShowPropertiesDialog()
        {
            base.ShowPropertiesDialog();
            PropertiesFormWrapper form = new PropertiesFormWrapper(this, FriendlyName);
            return form.Show();
        }

        public static string FriendlyName
        {
            get { return "Perlin noise"; }
        }

        public static string SubMenuPath
        {
            get { return "Image.Generator"; }
        }
    }
}
