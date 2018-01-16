using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using Yata.CoreNode;

namespace Yata
{
    internal class NodeExtractor
    {
        private List<Yata.NodeFactory.NodeInfo> nodes = new List<Yata.NodeFactory.NodeInfo>();

        internal void Extract(Type[] types)
        {
            foreach (Type type in types)
                ExtractType(type, type);
        }

        internal List<Yata.NodeFactory.NodeInfo> GetNodes()
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
            if (isTypeOfNode(type))
            {
                Yata.NodeFactory.NodeInfo info = new Yata.NodeFactory.NodeInfo();
                FillNodeProperties(ref info, detailedType);
                nodes.Add(info);
            }
        }

        private void FillNodeProperties(ref Yata.NodeFactory.NodeInfo info, Type type)
        {
            info.type = type;
            info.className = type.FullName;
            info.friendlyName = GetFriendlyNameProperty(type);
            info.subMenuPath = GetSubMenuPathProperty(type);
        }

        private string GetFriendlyNameProperty(Type type)
        {
            PropertyInfo nameProp = type.GetProperty("FriendlyName");
            if ((nameProp != null) && (nameProp.CanRead))
                return (string)nameProp.GetValue(null, null);
            return "";
        }
                
        private string GetSubMenuPathProperty(Type type)
        {
            PropertyInfo subMenuPathProp = type.GetProperty("SubMenuPath");
            if ((subMenuPathProp != null) && (subMenuPathProp.CanRead))
                return (string)subMenuPathProp.GetValue(null, null);
            return "";
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

        private bool isTypeOfNode(Type type)
        {
            return type.Equals(typeof(Node));
        }
    }
}
