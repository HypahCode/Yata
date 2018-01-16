using System.Drawing;
using System.Collections.Generic;
using Yata.CoreNode.DataTypes;

namespace Yata.CoreNode
{
    public class Node
    {
        private string name;
        private int id = -1;

        protected List<NodeInput> inputs = new List<NodeInput>();
        protected List<NodeOutput> outputs = new List<NodeOutput>();

        private NodeDrawer nodeDrawer;

        public Node(string name)
        {
            this.name = name;
        }

        public int Id { get { return id; } set { id = value; } }
        public RectangleF Rectangle 
        { 
            get { return nodeDrawer.Rectangle; }
            set { nodeDrawer.Rectangle = value; }
        }

        public bool ShowDialog()
        {
            if (ShowPropertiesDialog())
            {
                Setup();
                return true;
            }
            return false;
        }
        public virtual bool ShowPropertiesDialog() { return false; }
        public virtual void Save(PropertyBundle bundle) { PropertyBundleSerializer.Serialize(bundle, this); }
        public virtual void Load(PropertyBundle bundle) { PropertyBundleSerializer.Deserialize(bundle, this); Setup(); }
        protected virtual void Update(Bitmap preview) { }

        protected string GetName()
        {
            return name;
        }

        protected void Init()
        {
            Setup();

            nodeDrawer = new NodeDrawer();
            nodeDrawer.SetInputs(inputs);
            nodeDrawer.SetOutputs(outputs);
            Update(nodeDrawer.GetPreviewBitmap());
        }

        protected virtual void Setup() { }

        public void Update()
        {
            Update(nodeDrawer.GetPreviewBitmap());
        }

        public void Draw(IRenderContext context)
        {
            nodeDrawer.Draw(context, name);
        }

        public void DrawConnectionLines(IRenderContext context)
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                if (inputs[i].outputChannel != null)
                {
                    Point p1 = Utils.GetRectCenterPoint(inputs[i].visualRect);
                    Point p2 = Utils.GetRectCenterPoint(inputs[i].outputChannel.visualRect);
                    context.DrawLine(Colors.ConnectorLines, p1, p2);
                }
            }
        }

        public NodeIO GetIOFromLocation(Point location)
        {
            NodeIO io = GetOutputFromLocation(location);
            if (io == null)
            {
                io = GetInputFromLocation(location);
            }
            return io;
        }

        public NodeOutput GetOutputFromLocation(Point location)
        {
            for (int i = 0; i < outputs.Count; i++)
            {
                if (Utils.PointInsideRect(location, outputs[i].visualRect))
                {
                    return outputs[i];
                }
            }
            return null;
        }

        public NodeInput GetInputFromLocation(Point location)
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                if (Utils.PointInsideRect(location, inputs[i].visualRect))
                {
                    return inputs[i];
                }
            }
            return null;
        }

        public List<NodeInput> GetInputs()
        {
            return inputs;
        }

        public NodeInput GetInputByName(string name)
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                if (inputs[i].Name == name)
                {
                    return inputs[i];
                }
            }
            return null;
        }

        public NodeInput GetInputFromIndex(int index)
        {
            return inputs[index];
        }

        public NodeOutput GetOutputByName(string name)
        {
            for (int i = 0; i < outputs.Count; i++)
            {
                if (outputs[i].Name == name)
                {
                    return outputs[i];
                }
            }
            return null;
        }

        public NodeOutput GetOutputFromIndex(int index)
        {
            return outputs[index];
        }

        public void Unbind(NodeIO io)
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                if (inputs[i] == io)
                {
                    inputs[i].SetOutputChannel(null);
                }
                if (inputs[i].outputChannel == io)
                {
                    inputs[i].SetOutputChannel(null);
                }
            }
        }

        public void Unbind(Node node)
        {
            for (int i = 0; i < node.inputs.Count; i++)
            {
                Unbind(node.inputs[i]);
            }
            for (int i = 0; i < node.outputs.Count; i++)
            {
                Unbind(node.outputs[i]);
            }
        }
    }
}
