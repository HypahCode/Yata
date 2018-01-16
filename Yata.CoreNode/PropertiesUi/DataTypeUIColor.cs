using System.Drawing;
using Yata.CoreNode.DataTypes;

namespace Yata.CoreNode.PropertiesUi
{
    public class DataTypeUIColor : UiPropertyAttribute
    {
        public DataTypeUIColor(string friendlyName) : base(friendlyName)
        {
        }

        public override PropertyControlValue AddUIComponent(PropertiesForm form, MemberValue value)
        {
            base.AddUIComponent(form, value);
            return form.AddColorControl(FriendlyName, (Color)value.GetValue());
        }
    }
}
