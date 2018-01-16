using System.Drawing;
using Yata.CoreNode;

namespace Yata.Operations
{
    internal class RemoveNodeOperation : UndoOperation
    {
        private Point location;
        private int nodeId;
        private string undoClassType;
        private PropertyBundle bundle = new PropertyBundle();

        internal RemoveNodeOperation(Node node)
        {
            location = new Point((int)node.Rectangle.X, (int)node.Rectangle.Y);
            nodeId = node.Id;
            node.Save(bundle);
            undoClassType = node.GetType().FullName;
        }

        internal override bool Undo(NodeContainer container)
        {
            Node node = container.Factory.CreateNodeFromName(undoClassType);
            if (node != null)
            {
                node.Id = nodeId;
                Rectangle r = Utils.RectFToRect(node.Rectangle);
                r.X = location.X;
                r.Y = location.Y;
                node.Rectangle = r;
                node.Load(bundle);
                container.Add(node);
            }
            return node != null;
        }

        internal override bool Redo(NodeContainer container)
        {
            Node node = container.GetNodeById(nodeId);
            if (node != null)
            {
                container.Remove(node);
            }
            return node != null;
        }
    }
}
