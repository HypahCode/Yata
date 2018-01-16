
namespace Yata.CoreNode
{
    public struct NodeDescr
    {
        public string name;
        public string subMenuPath;
    }

    public struct PluginFileExt
    {
        public string fileExt;
        public string description;
        public PluginFileExt(string ext, string descr)
        {
            fileExt = ext;
            description = descr;
        }
    }

    public interface INodePlugin
    {
        int GetNodeCount();
        NodeDescr GetNodeDesc(int index);
        Node CreateNode(NodeDescr descr);

        PluginFileExt[] GetFileExt();
        void LoadFile(string fileName, INodeContainer nodeContainer, INodeFactory factory);
    }
}
