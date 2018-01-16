using Yata.CoreNode;
using System.Drawing;

namespace HellTech.Editor
{
    public class DataNodeOutput : NodeOutput
    {
        public DataNodeOutput(Node parent, string name)
            : base(parent, name)
        {
            drawColor = Color.FromArgb(50, 200, 50);
        }
    }
}
