using System.Drawing;

namespace Yata.CoreNode
{
    public class NodeOutput : NodeIO
    {
        public NodeOutput(Node parent, string name)
            : base(parent, name)
        {
        }

        public void PrepareForRender() { }
    }
}
