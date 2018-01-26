using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using Yata.CoreNode;

namespace Yata
{
    internal class NodeExtractor
    {
        private List<NodeFactory.NodeInfo> nodes = new List<NodeFactory.NodeInfo>();

        internal void Extract(Type[] types)
        {
            foreach (Type type in types)
                ExtractType(type, type);
        }

        internal List<NodeFactory.NodeInfo> GetNodes()
        {
            return nodes;
        }

        private void ExtractType(Type type, Type detailedType)
        {
            if (type != null)
            {
                CreateNodeInfo(type, detailedType);
                ExtractType(type.BaseType, detailedType);
            }
        }

        private void CreateNodeInfo(Type type, Type detailedType)
        {
            if (IsTypeOfNode(type))
            {
                var attribute = GetNodeUsageAttribute(detailedType);
                if (attribute != null)
                {
                    NodeFactory.NodeInfo info = new NodeFactory.NodeInfo();
                    FillNodeProperties(ref info, detailedType, attribute);
                    nodes.Add(info);
                }
            }
        }

        private void FillNodeProperties(ref NodeFactory.NodeInfo info, Type type, NodeUsage nodeUsage)
        {
            info.type = type;
            info.className = type.FullName;
            info.friendlyName = nodeUsage.name;
            info.subMenuPath = nodeUsage.directory;
        }

        private Bitmap GetIconProperty(Type type)
        {
            PropertyInfo iconProp = type.GetProperty("Icon");
            if ((iconProp != null) && (iconProp.CanRead))
                return GetIconBitmap(iconProp);
            return null;
        }

        private Bitmap GetIconBitmap(PropertyInfo iconProp)
        {
            object obj = iconProp.GetValue(null, null);
            if ((obj != null) && (obj is Bitmap))
                return (Bitmap)obj;
            return null;
        }

        private bool IsTypeOfNode(Type type)
        {
            return type.Equals(typeof(Node));
        }

        private NodeUsage GetNodeUsageAttribute(Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(NodeUsage), false);
            if (attributes.Length > 0)
            {
                return attributes[0] as NodeUsage;
            }
            return null;
        }
    }
}
