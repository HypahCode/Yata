using System.Drawing;

namespace Yata.CoreNode
{
    public class NodeIO
    {
        private string name;
        public Rectangle visualRect;
        private Node parent;
        protected Color drawColor;

        public NodeIO(Node _parent, string _name)
        {
            parent = _parent;
            name = _name;
        }

        public string Name
        {
            get { return name; }
        }

        internal Point GetCenterPoint()
        {
            return Utils.GetRectCenterPoint(visualRect);
        }

        public Node GetParent()
        {
            return parent;
        }

        public Color DrawColor { get { return drawColor; } }
    }
}
