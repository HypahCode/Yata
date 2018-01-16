using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Yata.CoreNode;

namespace HellTech.Editor
{
    public class NodePlugin : INodePlugin
    {
        private struct NodeItem
        {
            public DataNode dataNode;
            public string plugin;
        }

        private List<NodeItem> nodes = new List<NodeItem>();

        public NodePlugin()
        {
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            string[] files = Directory.GetFiles(appPath, "*.definition");

            for (int i = 0; i < files.Length; i++)
            {
                DataFile dataFile = DataFile.CreateFromDisk(files[i]);
                for (int j = 0; j < dataFile.Root.Childeren.Count; j++)
                {
                    NodeItem item = new NodeItem();
                    item.dataNode = dataFile.Root.Childeren[j];
                    //item.plugin = Path.GetFileName(files[i]);
                    item.plugin = Path.GetFileName(files[i]).Split('.')[0];
                    nodes.Add(item);
                }
            }
        }

        public int GetNodeCount()
        {
            return nodes.Count;
        }

        public NodeDescr GetNodeDesc(int index)
        {
            NodeDescr descr = new NodeDescr();
            descr.name = nodes[index].dataNode.Name;
            descr.subMenuPath = "HellTech." + nodes[index].plugin;

            return descr;
        }

        public Node CreateNode(NodeDescr descr)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].dataNode.Name == descr.name)
                {
                    return new EngineNodeBase(nodes[i].dataNode);
                }
            }
            return null;
        }

        public PluginFileExt[] GetFileExt()
        {
            PluginFileExt[] fileExt = new PluginFileExt[3];

            fileExt[0] = new PluginFileExt(".material", "HellTech Material");
            fileExt[1] = new PluginFileExt(".model", "HellTech model");
            fileExt[2] = new PluginFileExt(".flow", "HellTech camera flow");

            return fileExt;
        }

        public void LoadFile(string fileName, INodeContainer nodeContainer, INodeFactory factory)
        {
            string fileExt = Path.GetExtension(fileName).ToLower().Substring(1);
            string pluginName = "";
            for (int i = 0; i < nodes.Count; i++)
            {
                if (fileExt == nodes[i].plugin.ToLower())
                {
                    // Don't care which one, just need the name (case sensitive...)
                    pluginName = nodes[i].plugin;
                    break;
                }
            }

            string classTypeName = "@HellTech.Editor.NodePlugin|HellTech." + pluginName;
            new FileLoader(fileName, classTypeName, nodeContainer, factory);
        }
    }
}
