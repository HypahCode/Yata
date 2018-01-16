using System.Drawing;
using Yata.CoreNode;

namespace Yata.Operations
{
    internal class MoveOperation : UndoOperation
    {
        private Point moveDelta;
        private int nodeId;

        internal MoveOperation(Point start, Point end, Node node)
        {
            moveDelta = new Point(end.X - start.X, end.Y - start.Y);
            nodeId = node.Id;
        }

        internal override bool Undo(NodeContainer container)
        {
            Node node = container.GetNodeById(nodeId);
            if (node != null)
            {
                Rectangle r = Utils.RectFToRect(node.Rectangle);
                r.X -= moveDelta.X;
                r.Y -= moveDelta.Y;
                node.Rectangle = r;
            }
            return node != null;
        }

        internal override bool Redo(NodeContainer container)
        {
            Node node = container.GetNodeById(nodeId);
            if (node != null)
            {
                Rectangle r = Utils.RectFToRect(node.Rectangle);
                r.X += moveDelta.X;
                r.Y += moveDelta.Y;
                node.Rectangle = r;
            }
            return node != null;
        }
    }
}
