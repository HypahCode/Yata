using System;

namespace Yata.CoreNode.DataTypes
{
    public class DataTypeAttribute : Attribute
    {
        public string FriendlyName { get; }

        public DataTypeAttribute(string friendlyName)
        {
            FriendlyName = friendlyName;
        }
    }
}
