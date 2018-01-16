using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using Yata.CoreNode;

namespace Yata
{
    public class NodeFactory : INodeFactory
    {
        public struct NodeInfo
        {
            public string className;
            public string friendlyName;
            public string subMenuPath;
            public Type type;
            public Bitmap image;
            public INodePlugin plugin;
        }

        public struct FileExtensions
        {
            public string ext;
            public string descr;
            public INodePlugin plugin;
        }

        public List<NodeInfo> nodes = new List<NodeInfo>();
        public List<FileExtensions> fileExtensions = new List<FileExtensions>();

        public Node CreateNodeFromName(string className)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].className == className)
                {
                    if (nodes[i].plugin != null)
                    {
                        NodeDescr descr = new NodeDescr();
                        descr.name = nodes[i].friendlyName;
                        descr.subMenuPath = nodes[i].subMenuPath;
                        return nodes[i].plugin.CreateNode(descr);
                    }
                    else
                    {
                        Type t = nodes[i].type;
                        return (Node)Activator.CreateInstance(t);
                    }
                }
            }
            return null;
        }

        public void Load(Assembly asm)
        {
            INodePlugin plugin = GetPlugin(asm);
            if (plugin != null)
            {
                LoadNodesFromPlugin(plugin);
                LoadNodeFileExt(plugin);
            }
            else
            {
                LoadNodesFromAsm(asm);
            }
        }

        private void LoadNodesFromPlugin(INodePlugin plugin)
        {
            int count = plugin.GetNodeCount();
            for (int i = 0; i < count; i++)
            {
                NodeDescr descr = plugin.GetNodeDesc(i);
                NodeInfo info = MakeNodeInfo(plugin, descr);
                nodes.Add(info);
            }
        }

        private NodeInfo MakeNodeInfo(INodePlugin plugin, NodeDescr descr)
        {
            NodeInfo info = new NodeInfo();
            info.className = "@" + plugin.ToString() + '|' + descr.subMenuPath + "|" + descr.name;
            info.friendlyName = descr.name;
            info.subMenuPath = descr.subMenuPath;
            info.plugin = plugin;
            return info;
        }

        private void LoadNodesFromAsm(Assembly asm)
        {
            NodeExtractor extractor = new NodeExtractor();
            extractor.Extract(asm.GetTypes());
            nodes.AddRange(extractor.GetNodes());
        }

        private INodePlugin GetPlugin(Assembly asm)
        {
            Type[] types = asm.GetTypes();
            foreach (Type type in types)
            {
                Type interfaceType = type.GetInterface("Yata.CoreNode.INodePlugin");
                if (interfaceType != null)
                {
                    return (INodePlugin)Activator.CreateInstance(type);

                }
            }
            return null;
        }

        private void LoadNodeFileExt(INodePlugin plugin)
        {
            PluginFileExt[] ext = plugin.GetFileExt();
            for (int i = 0; i < ext.Length; i++)
            {
                FileExtensions fileExt = new FileExtensions();
                fileExt.plugin = plugin;
                fileExt.ext = ext[i].fileExt;
                fileExt.descr = ext[i].description;

                fileExtensions.Add(fileExt);                
            }
        }
    }
}
