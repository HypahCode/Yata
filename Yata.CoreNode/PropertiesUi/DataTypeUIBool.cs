using Yata.CoreNode.DataTypes;

namespace Yata.CoreNode.PropertiesUi
{
    public class DataTypeUIBool : UiPropertyAttribute
    {
        public DataTypeUIBool(string friendlyName) : base(friendlyName)
        {
        }

        public override PropertyControlValue AddUIComponent(PropertiesForm form, MemberValue value)
        {
            base.AddUIComponent(form, value);
            return form.AddCheckbox(FriendlyName, (bool)value.GetValue());
        }
    }
}
