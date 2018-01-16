using Yata.CoreNode.DataTypes;

namespace Yata.CoreNode.PropertiesUi
{
    public class UiPropertyAttribute : DataTypeAttribute
    {
        public UiPropertyAttribute(string friendlyName) : base(friendlyName)
        {
        }

        public virtual PropertyControlValue AddUIComponent(PropertiesForm form, MemberValue value) { return null; }
    }
}
