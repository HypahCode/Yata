
namespace Yata.Operations
{
    internal class UndoOperation
    {
        internal virtual bool Undo(NodeContainer container) { return false; }
        internal virtual bool Redo(NodeContainer container) { return false; }
    }
}
