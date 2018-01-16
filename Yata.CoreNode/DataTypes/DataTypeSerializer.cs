using System;

namespace Yata.CoreNode.DataTypes
{
    public abstract class DataTypeSerializer
    {
        public abstract string Serialize(object o);
        public abstract object Deserialize(string s);
    }
}
