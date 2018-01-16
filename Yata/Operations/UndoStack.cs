using System.Collections.Generic;

namespace Yata.Operations
{
    internal class UndoStack
    {
        private int undoIndex = -1;
        private List<UndoOperation> undoStack = new List<UndoOperation>();

        public UndoStack()
        {
        }

        internal void Undo(NodeContainer container)
        {
            if (undoIndex >= 0)
            {
                UndoOperation operation = undoStack[undoIndex];
                undoIndex--;
                operation.Undo(container);
            }
        }

        internal void Redo(NodeContainer container)
        {
            if (undoIndex < undoStack.Count - 1)
            {
                undoIndex++;
                UndoOperation operation = undoStack[undoIndex];
                operation.Redo(container);
            }
        }

        internal void Push(UndoOperation operation)
        {
            DeleteEverythingAboveIndex();
            
            undoStack.Add(operation);
            undoIndex = undoStack.Count - 1;
        }

        private void DeleteEverythingAboveIndex()
        {
            int idx = undoIndex + 1;
            if (idx == undoStack.Count)
            {
                return;
            }

            undoStack.RemoveRange(idx, undoStack.Count - idx);
        }
    }
}
