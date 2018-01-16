using System.Drawing;
using Yata.CoreNode.PropertiesUi;

namespace Yata.ImageNodes.Nodes.Generators
{
    public class ColorGeneratorNode : ImageNodeBase
    {
        [DataTypeUIColor("Color")]
        private Color color = Color.Red;

        private FloatColor floatColor = new FloatColor(1.0f, 0.0f, 0.0f, 1.0f);

        public ColorGeneratorNode()
            : base(FriendlyName)
        {
            outputs.Add(new FloatColorOutput(this, "Color"));
            Init();
        }

        protected override void Setup()
        {
            base.Setup();
            floatColor = new FloatColor(color);
        }

        public override FloatColor GetPixel(float x, float y, bool preview)
        {
            return floatColor;
        }

        public override bool ShowPropertiesDialog()
        {
            base.ShowPropertiesDialog();
            PropertiesFormWrapper form = new PropertiesFormWrapper(this, FriendlyName);
            return form.Show();
        }

        public static string FriendlyName
        {
            get { return "Color"; }
        }

        public static string SubMenuPath
        {
            get { return "Image.Generator"; }
        }
    }
}
