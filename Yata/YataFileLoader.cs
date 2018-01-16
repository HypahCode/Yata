using System.Drawing;
using System.Globalization;
using Yata.CoreNode;
using Yata.CoreNode.DataTypes;

namespace Yata
{
    internal class YataFileLoader
    {
        private YataFileLoader() { }

        internal static void SaveToFile(string filename, NodeContainer container)
        {
            DataFile data = new DataFile();

            for (int i = 0; i < container.Count; i++)
            {
                Node node = container.GetNode(i);
                DataNode dataNode = data.Root.AddChild("Node", "");
                dataNode.AddValue("ClassName", node.GetType().FullName);
                dataNode.AddValue("Id", node.Id.ToString());
                dataNode.AddValue("Location.x", node.Rectangle.X.ToString());
                dataNode.AddValue("Location.y", node.Rectangle.Y.ToString());

                // Links
                foreach (NodeInput input in node.GetInputs())
                {
                    if (input.outputChannel != null)
                    {
                        DataNode linkNode = dataNode.AddChild("Link", "");
                        linkNode.AddValue("Input", input.Name);
                        linkNode.AddValue("Node.Id", input.outputChannel.GetParent().Id.ToString());
                        linkNode.AddValue("Node.Output", input.outputChannel.Name);
                    }
                }

                PropertyBundle bundle = new PropertyBundle();
                node.Save(bundle);
                // Properties
                foreach (PropertyBundle.KeyValue kv in bundle.GetItems())
                {
                    DataNode PropertyNode = dataNode.AddChild("Property", "");

                    if (kv.value.value.GetType().IsEnum)
                    {
                        PropertyNode.AddValue("Type", "enum");
                        PropertyNode.AddValue("Key", kv.key);
                        PropertyNode.AddValue("Value", kv.value.value.ToString());
                    }
                    else
                    {
                        PropertyNode.AddValue("Type", kv.value.value.GetType().ToString());
                        PropertyNode.AddValue("Key", kv.key);
                        PropertyNode.AddValue("Value", DataTypeSerializerFactory.Instance.Serialize(kv.value));
                    }
                }
            }

            data.SaveToFile(filename);
        }

        internal static void LoadfromFile(string filename, NodeContainer nodeContainer, NodeFactory factory)
        {
            DataFile data = DataFile.CreateFromDisk(filename);

            // Read only node data
            foreach (DataNode node in data.Root.Childeren)
            {
                if (node.Name == "Node")
                {
                    string className = node.GetKeyByName("ClassName", "");
                    int id = int.Parse(node.GetKeyByName("Id", "-1"));
                    int x = int.Parse(node.GetKeyByName("Location.x", "0"));
                    int y = int.Parse(node.GetKeyByName("Location.y", "0"));
                    Node n = factory.CreateNodeFromName(className);
                    if (n != null)
                    {
                        n.Id = id;
                        Rectangle rect = Utils.RectFToRect(n.Rectangle);
                        rect.X = x;
                        rect.Y = y;
                        n.Rectangle = rect;

                        nodeContainer.Add(n);
                    }
                }
            }

            // Read links and properties
            foreach (DataNode node in data.Root.Childeren)
            {
                if (node.Name == "Node")
                {
                    int id = int.Parse(node.GetKeyByName("Id", "-1"));
                    Node n = nodeContainer.GetNodeById(id);
                    PropertyBundle bundle = new PropertyBundle();
                    
                    foreach (DataNode linkNode in node.Childeren)
                    {
                        if (linkNode.Name == "Link")
                        {
                            string input = linkNode.GetKeyByName("Input", "");
                            int linkToNodeId = int.Parse(linkNode.GetKeyByName("Node.Id", "-1"));
                            string linkToNodeOutput = linkNode.GetKeyByName("Node.Output", "");

                            Node curr = nodeContainer.GetNodeById(id);
                            Node link = nodeContainer.GetNodeById(linkToNodeId);

                            if ((curr != null) && (link != null))
                            {
                                NodeInput nodeIn = curr.GetInputByName(input);
                                NodeOutput nodeOut = link.GetOutputByName(linkToNodeOutput);
                                if (nodeIn != null && nodeOut != null)
                                {
                                    nodeIn.SetOutputChannel(nodeOut);
                                }
                            }
                        }
                        if (linkNode.Name == "Property")
                        {
                            string type = linkNode.GetKeyByName("Type", "");
                            string key = linkNode.GetKeyByName("Key", "");
                            string value = linkNode.GetKeyByName("Value", "");
                            if (type == "enum")
                                bundle.SetValue(key, value);
                            else
                                bundle.SetValue(key, DataTypeSerializerFactory.Instance.Deserialize(type, value));
                        }
                    }
                    n.Load(bundle);
                }
            }
        }
    }
}
