using Yata.CoreNode.DataTypes;

namespace Yata.CoreNode.PropertiesUi
{
    public class DataTypeUISlider : UiPropertyAttribute
    {
        private int min;
        private int max;
        private int step;

        public DataTypeUISlider(string friendlyName, int min, int max, int step) : base(friendlyName)
        {
            this.min = min;
            this.max = max;
            this.step = step;
        }

        public override PropertyControlValue AddUIComponent(PropertiesForm form, MemberValue value)
        {
            base.AddUIComponent(form, value);
            return form.AddSliderControl(FriendlyName, (int)value.GetValue(), min, max, step);
        }
    }
}
