
namespace HellTech.Editor
{
    internal class EngineProperty
    {
        public string name;
        public string type;
        public string value;
        public string extra;

        public EngineProperty(string _name)
        {
            name = _name;
            type = "";
            value = "";
            extra = "";
        }
    }
}
