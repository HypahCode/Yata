using Yata.CoreNode;
using System.Drawing;

namespace Yata.ImageNodes
{
    public class ImageBatchInput : NodeInput
    {

        public ImageBatchInput(Node parent, string name)
            : this(parent, name, new FloatColor(0.0f, 0.0f, 0.0f, 1.0f))
        {
        }

        public ImageBatchInput(Node parent, string name, FloatColor defaultColor)
            : base(parent, name)
        {
            drawColor = Color.FromArgb(250, 250, 0);
        }

        public int GetImageCount()
        {
            ImageBatchOutput output = outputChannel as ImageBatchOutput;
            if (outputChannel == null)
            {
                return 0;
            }
            return ((ImageBatchOutput)outputChannel).GetImageCount();
        }

        public bool LoadImage(int index)
        {
            ImageBatchOutput output = outputChannel as ImageBatchOutput;
            if (outputChannel == null)
            {
                return false;
            }
            return ((ImageBatchOutput)outputChannel).LoadImage(index);
        }

        public string GetImageFileName(int index)
        {
            ImageBatchOutput output = outputChannel as ImageBatchOutput;
            if (outputChannel == null)
            {
                return "";
            }
            return ((ImageBatchOutput)outputChannel).GetImageFileName(index);
        }

        public Size GetImageSize()
        {
            ImageBatchOutput output = outputChannel as ImageBatchOutput;
            if (outputChannel == null)
            {
                return new Size(0, 0);
            }
            return ((ImageBatchOutput)outputChannel).GetImageSize();
        }

        public override bool SetOutputChannel(NodeOutput output)
        {
            if (output is ImageBatchOutput)
            {
                return base.SetOutputChannel(output);
            }
            return false;
        }
    }
}
