using Yata.CoreNode;
using Yata.CoreNode.PropertiesUi;

namespace Yata.ImageNodes.Nodes.Operations
{
    [NodeUsage(@"Image.Operations", nodeName)]
    public class Rotate90Node : ImageNodeBase
    {
        private const string nodeName = @"Rotate 90";

        private enum Rotation { Rotate0, Rotate90, Rotate180, Rotate270 }
        private FloatColorInput input;

        [DataTypeUIEnum("Rotation")]
        private Rotation rotation = Rotation.Rotate0;

        public Rotate90Node()
            : base(nodeName)
        {
            input = AddInput("Input");
            outputs.Add(new FloatColorOutput(this, "Output"));
            Init();
        }

        public override FloatColor GetPixel(float x, float y, bool preview)
        {
            float xx = x;
            float yy = y;
            switch (rotation)
            {
                case Rotation.Rotate90:
                    xx = y;
                    yy = 1.0f - x;
                    break;
                case Rotation.Rotate180:
                    xx = 1.0f - x;
                    yy = 1.0f - y;
                    break;
                case Rotation.Rotate270:
                    xx = 1.0f - y;
                    yy = x;
                    break;
            }

            return input.GetPixel(xx, yy, preview);
        }

        public override bool ShowPropertiesDialog()
        {
            base.ShowPropertiesDialog();
            PropertiesFormWrapper form = new PropertiesFormWrapper(this, nodeName);
            return form.Show();
        }
    }
}
