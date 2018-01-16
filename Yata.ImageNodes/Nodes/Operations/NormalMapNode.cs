using System.Drawing;
using Yata.CoreNode.PropertiesUi;

namespace Yata.ImageNodes.Nodes.Operations
{
    public class NormalMapNode : ImageNodeBase
    {
        private FloatColorInput input;
        private PointF offset = new PointF(0.01f, 0.01f);

        [DataTypeUISlider("Red intensity", 0, 255, 1)]
        private int intensityR = (int)(255.0f * 0.8f);
        [DataTypeUISlider("Green intensity", 0, 255, 1)]
        private int intensityG = (int)(255.0f * 0.8f);
        [DataTypeUISlider("Blue intensity", 0, 255, 1)]
        private int intensityB = (int)(255.0f * 0.8f);

        private FloatColor intensityColor = new FloatColor();

        public NormalMapNode()
            : base(FriendlyName)
        {
            input = AddInput("Input");
            outputs.Add(new FloatColorOutput(this, "Output"));
            Init();
        }

        protected override void Setup()
        {
            base.Setup();
            intensityColor = new FloatColor(intensityR / 255.0f, intensityG / 255.0f, intensityB / 255.0f, 0.0f);
        }

        public override void PrepareForRender(int w, int h)
        {
            base.PrepareForRender(w, h);
            offset.X = 1.0f / w;
            offset.Y = 1.0f / h;
        }

        public override FloatColor GetPixel(float x, float y, bool preview)
        {
            FloatColor col00 = input.GetPixel(x - offset.X, y - offset.Y, preview); 
            FloatColor col01 = input.GetPixel(x, y - offset.Y, preview);
            FloatColor col02 = input.GetPixel(x + offset.X, y - offset.Y, preview);

            FloatColor col10 = input.GetPixel(x - offset.X, y, preview);
            FloatColor col11 = input.GetPixel(x, y, preview);
            FloatColor col12 = input.GetPixel(x + offset.X, y, preview);

            FloatColor col20 = input.GetPixel(x - offset.X, y + offset.Y, preview);
            FloatColor col21 = input.GetPixel(x, y + offset.Y, preview);
            FloatColor col22 = input.GetPixel(x + offset.X, y + offset.Y, preview);

            FloatColor c0 = col00 + 2f * col10 + col20 - col02 - 2f * col12 - col22;
            FloatColor c1 = col00 + 2f * col01 + col02 - col20 - 2f * col21 - col22;

            FloatColor c = new FloatColor(FloatColor.Dot(c0, intensityColor), FloatColor.Dot(c1, intensityColor), 1f, 0.0f);
            c.Normalize();
            c *= 0.5f;
            c += 0.5f;
            c.a = 1.0f;

            return c;
        }

        public override bool ShowPropertiesDialog()
        {
            base.ShowPropertiesDialog();
            PropertiesFormWrapper form = new PropertiesFormWrapper(this, FriendlyName);
            return form.Show();
        }

        public static string FriendlyName
        {
            get { return "Normal map"; }
        }

        public static string SubMenuPath
        {
            get { return "Image.Operations"; }
        }
    }
}
