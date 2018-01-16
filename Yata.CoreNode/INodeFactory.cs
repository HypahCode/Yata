
namespace Yata.CoreNode
{
    public interface INodeFactory
    {
        Node CreateNodeFromName(string className);
    }
}
