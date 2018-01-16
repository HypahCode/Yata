using System.Drawing;
using System;

namespace Yata.CoreNode
{
    public class NodeInput : NodeIO
    {
        public NodeOutput outputChannel;

        public NodeInput(Node parent, string name)
            : base(parent, name)
        {
            drawColor = Color.FromArgb(200, 100, 100);
        }

        public virtual bool SetOutputChannel(NodeOutput output)
        {
            outputChannel = output;
            return true;
        }
    }
}
