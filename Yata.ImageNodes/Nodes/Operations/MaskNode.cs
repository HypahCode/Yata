using Yata.CoreNode;
using Yata.CoreNode.PropertiesUi;

namespace Yata.ImageNodes.Nodes.Operations
{
    [NodeUsage(@"Image.Operations", nodeName)]
    public class MaskNode : ImageNodeBase
    {
        private const string nodeName = @"Mask";

        [DataTypeUIEnum("Mask channel")]
        private FloatColor.Channel channel = FloatColor.Channel.Red;

        [DataTypeUIBool("Invert")]
        private bool invert = false;

        private FloatColorInput inputImage;
        private FloatColorInput inputMask;
        
        public MaskNode()
            : base(nodeName)
        {
            inputImage = AddInput("Image");
            inputMask = AddInput("Mask");

            outputs.Add(new FloatColorOutput(this, "Output"));

            Init();
        }

        public override FloatColor GetPixel(float x, float y, bool preview)
        {
            FloatColor image = inputImage.GetPixel(x, y, preview);
            FloatColor mask = inputMask.GetPixel(x, y, preview);

            float maskValue = mask.RGBA(channel);
            image.a = invert ? 1.0f - maskValue : maskValue;
            return image;
        }

        public override bool ShowPropertiesDialog()
        {
            base.ShowPropertiesDialog();
            PropertiesFormWrapper form = new PropertiesFormWrapper(this, nodeName);
            return form.Show();
        }
    }
}
