using System.Drawing;
using Yata.CoreNode;

namespace Yata.ImageNodes
{
    public class ImageBatchOutput : NodeOutput
    {
        public ImageBatchOutput(Node parent, string name)
            : base(parent, name)
        {
            drawColor = Color.FromArgb(250, 250, 0);
        }

        public virtual int GetImageCount() { return 0; }
        public virtual bool LoadImage(int index) { return false; }
        public virtual string GetImageFileName(int index) { return ""; }
        public virtual Size GetImageSize() { return new Size(0, 0); }
    }
}
