using System.Collections.Generic;
using System.Drawing;
using Yata.CoreNode;
using Yata.Rendering;

namespace Yata
{
    internal sealed class NodeContainer : INodeContainer
    {
        private List<Node> nodes = new List<Node>();
        private int uniqueNodeId = 1; // 0 is invalid.
        private NodeFactory nodeFactory;

        public NodeContainer(NodeFactory factory)
        {
            nodeFactory = factory;
        }

        public int Count
        {
            get { return nodes.Count; }
        }

        public NodeFactory Factory
        {
            get { return nodeFactory; }
        }

        public Node GetNode(int i)
        {
            return nodes[i];
        }

        public int Add(Node node)
        {
            if (node.Id == -1)
            {
                node.Id = uniqueNodeId;
                uniqueNodeId++;
            }
            else if (node.Id <= uniqueNodeId)
            {
                uniqueNodeId = node.Id + 1;
            }
            nodes.Add(node);
            return node.Id;
        }

        internal void Remove(Node node)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Unbind(node);
            }
            nodes.Remove(node);
        }

        public void UpdatePreviewRendering()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Update();
            }
        }

        public void Draw(RenderContext context)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Draw(context);
            }
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].DrawConnectionLines(context);
            }
        }

        public Node GetNodeFromLocation(Point location)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                Node node = nodes[i];
                if (Utils.PointInsideRect(location, node.Rectangle))
                {
                    return node;
                }
            }
            return null;
        }

        public NodeIO GetIOPoint(Point location)
        {
            NodeIO result = GetOutputPoint(location);
            if (result == null)
            {
                result = GetInputPoint(location);
            }
            return result;
        }

        public NodeOutput GetOutputPoint(Point location)
        {
            NodeOutput result;
            for (int i = 0; i < nodes.Count; i++)
            {
                Node node = nodes[i];
                result = node.GetOutputFromLocation(location);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        public NodeInput GetInputPoint(Point location)
        {
            NodeInput result;
            for (int i = 0; i < nodes.Count; i++)
            {
                Node node = nodes[i];
                result = node.GetInputFromLocation(location);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        internal void Unbind(NodeIO io)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Unbind(io);
            }
        }

        public Node GetNodeById(int id)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Id == id)
                {
                    return nodes[i];
                }
            }
            return null;
        }
    }
}
