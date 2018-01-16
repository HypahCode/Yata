using Yata.CoreNode;

namespace Yata.Operations
{
    internal class ChangePropertiesOperation : UndoOperation
    {
        private int nodeId;
        private PropertyBundle oldBundle;
        private PropertyBundle newBundle;

        internal ChangePropertiesOperation(Node node, PropertyBundle oldB, PropertyBundle newB)
        {
            nodeId = node.Id;
            oldBundle = oldB;
            newBundle = newB;
        }

        internal override bool Undo(NodeContainer container)
        {
            Node node = container.GetNodeById(nodeId);
            if (node != null)
            {
                node.Load(oldBundle);
            }
            return node != null;
        }

        internal override bool Redo(NodeContainer container)
        {
            Node node = container.GetNodeById(nodeId);
            if (node != null)
            {
                node.Load(newBundle);
            }
            return node != null;
        }
    }
}
