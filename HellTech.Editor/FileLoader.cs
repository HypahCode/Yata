using System.Collections.Generic;
using System.Drawing;
using Yata.CoreNode;

namespace HellTech.Editor
{
    internal class FileLoader
    {
        private List<int> depthPosition = new List<int>();
        public INodeContainer nodeContainer;
        private INodeFactory factory;
        private string classTypeName;

        public FileLoader(string fileName, string classTypeName, INodeContainer nodeContainer, INodeFactory factory)
        {
            this.nodeContainer = nodeContainer;
            this.factory = factory;
            this.classTypeName = classTypeName;

            DataFile file = DataFile.CreateFromDisk(fileName);
            LoadNode(file.Root, null, 0);
        }

        private void LoadNode(DataNode dataNode, Node parent, int depth)
        {
            if (depthPosition.Count <= depth)
            {
                depthPosition.Add((int)0);
            }

            string className = classTypeName + "|" + dataNode.Name;
            Node node = factory.CreateNodeFromName(className);
            if (node != null)
            {
                Rectangle r = node.Rectangle;
                r.X = depth * 128;
                r.Y = depthPosition[depth];
                node.Rectangle = r;
                nodeContainer.Add(node);

                depthPosition[depth] = depthPosition[depth] + r.Height + 16;

                if (parent != null)
                {
                    node.GetInputFromIndex(0).SetOutputChannel(parent.GetOutputFromIndex(0));
                }

                for (int i = 0; i < dataNode.Childeren.Count; i++)
                {
                    LoadNode(dataNode.Childeren[i], node, depth + 1);
                }
            }
        }
    }
}
