using Yata.CoreNode.PropertiesUi;

namespace Yata.ImageNodes.Nodes.Operations
{
    public class FlipNode : ImageNodeBase
    {
        private FloatColorInput input;

        [DataTypeUIBool("Flip X")]
        private bool flipX = true;
        [DataTypeUIBool("Flip Y")]
        private bool flipY = false;

        public FlipNode()
            : base(FriendlyName)
        {
            input = AddInput("Input");
            outputs.Add(new FloatColorOutput(this, "Output"));
            Init();
        }

        public override FloatColor GetPixel(float x, float y, bool preview)
        {
            if (flipX) x = 1.0f - x;
            if (flipY) y = 1.0f - y;
            return input.GetPixel(x, y, preview);
        }

        public static string FriendlyName
        {
            get { return "Flip"; }
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
