using Yata.CoreNode;

namespace Yata.Operations
{
    internal class BindOperation : UndoOperation
    {
        private struct IOBinding
        {
            public int nodeId;
            public string name;
            public IOBinding(int id, string n)
            {
                nodeId = id;
                name = n;
            }
        }

        private IOBinding input;    // Input node that receives the binding
        private IOBinding newOutput; // new input binding to output (needed for redo)
        private IOBinding prevOutput; // previous binding to output (needed for undo)

        internal BindOperation(NodeInput oldBinding)
        {
            input = new IOBinding(oldBinding.GetParent().Id, oldBinding.Name);
            if (oldBinding.outputChannel != null)
            {
                prevOutput = new IOBinding(oldBinding.outputChannel.GetParent().Id, oldBinding.outputChannel.Name);
            }
            else
            {
                prevOutput = new IOBinding(-1 , "");
            }
        }

        internal void SetNewBinding(NodeInput newBinding)
        {
            if (newBinding.outputChannel != null)
            {
                newOutput = new IOBinding(newBinding.outputChannel.GetParent().Id, newBinding.outputChannel.Name);
            }
            else
            {
                newOutput = new IOBinding(-1, "");
            }
        }

        internal override bool Undo(NodeContainer container)
        {
            Node node = container.GetNodeById(input.nodeId);
            if (node != null)
            {
                NodeInput inp = node.GetInputByName(input.name);
                if (inp != null)
                {
                    // Bind to NULL
                    if ((prevOutput.nodeId == -1) && (prevOutput.name == ""))
                    {
                        inp.SetOutputChannel(null);
                        return true;
                    }

                    // Bind to 'something'
                    Node prevNode = container.GetNodeById(prevOutput.nodeId);
                    if (prevNode != null)
                    {
                        NodeOutput prevOut = prevNode.GetOutputByName(prevOutput.name);
                        if (prevOut != null)
                        {
                            inp.SetOutputChannel(prevOut);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        internal override bool Redo(NodeContainer container)
        {
            Node node = container.GetNodeById(input.nodeId);
            if (node != null)
            {
                NodeInput inp = node.GetInputByName(input.name);
                if (inp != null)
                {
                    // Bind to NULL
                    if ((newOutput.nodeId == -1) && (newOutput.name == ""))
                    {
                        inp.SetOutputChannel(null);
                        return true;
                    }

                    // Bind to 'something'
                    Node newNode = container.GetNodeById(newOutput.nodeId);
                    if (newNode != null)
                    {
                        NodeOutput newOut = newNode.GetOutputByName(newOutput.name);
                        if (newOut != null)
                        {
                            inp.SetOutputChannel(newOut);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
