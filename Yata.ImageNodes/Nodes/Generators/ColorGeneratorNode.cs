using System.Drawing;
using Yata.CoreNode;
using Yata.CoreNode.PropertiesUi;

namespace Yata.ImageNodes.Nodes.Generators
{
    [NodeUsage(@"Image.Generator", nodeName)]
    public class ColorGeneratorNode : ImageNodeBase
    {
        private const string nodeName = @"Color";

        [DataTypeUIColor("Color")]
        private Color color = Color.Red;

        private FloatColor floatColor = new FloatColor(1.0f, 0.0f, 0.0f, 1.0f);

        public ColorGeneratorNode()
            : base(nodeName)
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
            PropertiesFormWrapper form = new PropertiesFormWrapper(this, nodeName);
            return form.Show();
        }
    }
}
