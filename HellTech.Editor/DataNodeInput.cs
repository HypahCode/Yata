using Yata.CoreNode;
using System.Drawing;

namespace HellTech.Editor
{
    public class DataNodeInput : NodeInput
    {
        public DataNodeInput(Node parent, string name)
            : base(parent, name)
        {
            drawColor = Color.FromArgb(50, 200, 50);
        }

        public override bool SetOutputChannel(NodeOutput output)
        {
            if (output is DataNodeOutput)
            {
                return base.SetOutputChannel(output);
            }
            return false;
        }
    }
}
