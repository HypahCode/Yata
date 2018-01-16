using Yata.CoreNode.DataTypes;

namespace Yata.CoreNode.PropertiesUi
{
    public class DataTypeUIFloat : UiPropertyAttribute
    {
        private float min;
        private float max;

        public DataTypeUIFloat(string friendlyName, float min, float max) : base(friendlyName)
        {
            this.min = min;
            this.max = max;
        }

        public override PropertyControlValue AddUIComponent(PropertiesForm form, MemberValue value)
        {
            base.AddUIComponent(form, value);
            return form.AddNumericFloatControl(FriendlyName, (float)value.GetValue(), min, max);
        }
    }
}
