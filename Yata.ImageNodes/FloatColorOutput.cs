using Yata.CoreNode;
using System.Drawing;

namespace Yata.ImageNodes
{
    public class FloatColorOutput : NodeOutput
    {
        public FloatColorOutput(Node parent, string name)
            : base(parent, name)
        {
            drawColor = Color.FromArgb(220, 50, 50);
        }

        public virtual FloatColor GetPixel(float x, float y, bool preview)
        {
            return ((ImageNodeBase)GetParent()).GetPixel(x, y, preview);
        }
    }
}
