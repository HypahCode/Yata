using System;

namespace Yata.CoreNode
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NodeUsage : Attribute
    {
        public string name;
        public string directory;
        public NodeUsage(string directory, string name)
        {
            this.name = name;
            this.directory = directory;
        }
    }
}
