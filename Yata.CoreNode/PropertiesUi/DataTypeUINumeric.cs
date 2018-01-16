using Yata.CoreNode.DataTypes;

namespace Yata.CoreNode.PropertiesUi
{
    public class DataTypeUINumeric : UiPropertyAttribute
    {
        private int min;
        private int max;

        public DataTypeUINumeric(string friendlyName, int min, int max) : base(friendlyName)
        {
            this.min = min;
            this.max = max;
        }

        public override PropertyControlValue AddUIComponent(PropertiesForm form, MemberValue value)
        {
            base.AddUIComponent(form, value);
            return form.AddNumericIntControl(FriendlyName, (int)value.GetValue(), min, max);
        }
    }
}
