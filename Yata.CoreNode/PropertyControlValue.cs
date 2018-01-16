using System;

namespace Yata.CoreNode
{
    public class PropertyControlValue
    {
        public class ValueAgrs : EventArgs
        {
            private object val;
            public object Value { get { return val; } }
            public ValueAgrs(object v)
            {
                val = v;
            }
        }
        
        public EventHandler<ValueAgrs> ValueChanged;

        private object propValue;

        public object Value
        {
            get { return propValue; }
            set
            {
                propValue = value;
                ValueChanged?.Invoke(this, new ValueAgrs(propValue));
            }
        }

        public PropertyControlValue(object val)
        {
            propValue = val;
        }
    }
}
