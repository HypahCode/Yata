
namespace Yata.CoreNode
{
    public interface INodeContainer
    {
        int Count { get; }
        Node GetNode(int i);
        Node GetNodeById(int id);
        int Add(Node node);
    }
}
