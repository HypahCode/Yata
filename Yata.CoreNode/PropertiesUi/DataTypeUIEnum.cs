using System;
using Yata.CoreNode.DataTypes;

namespace Yata.CoreNode.PropertiesUi
{
    public class DataTypeUIEnum : UiPropertyAttribute
    {
        public DataTypeUIEnum(string friendlyName) : base(friendlyName)
        {
        }

        public override PropertyControlValue AddUIComponent(PropertiesForm form, MemberValue value)
        {
            base.AddUIComponent(form, value);
            return form.AddComboBoxControl(FriendlyName, value.GetValue().ToString(), Enum.GetNames(value.GetMemberType()));
        }
    }
}
