
namespace Yata.ImageNodes.Nodes.Output
{
    public class RenderOutputNode : ImageNodeBase
    {
        private FloatColorInput input;

        public RenderOutputNode()
            : base(FriendlyName)
        {
            input = AddInput("Input");

            Init();
        }

        public override FloatColor GetPixel(float x, float y, bool preview)
        {
            return input.GetPixel(x, y, preview);
        }

        public static string FriendlyName
        {
            get { return "Render output"; }
        }

        public static string SubMenuPath
        {
            get { return "Image.Output"; }
        }

        public override bool ShowPropertiesDialog()
        {
            base.ShowPropertiesDialog();

            RenderOutputForm output = new RenderOutputForm(this);
            output.ShowDialog();

            return false;
        }
    }
}
