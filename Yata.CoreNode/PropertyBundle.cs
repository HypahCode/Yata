using System;
using System.Collections.Generic;

namespace Yata.CoreNode
{
    public class PropertyBundle
    {
        public class SerializeValue
        {
            public object value;
            public SerializeValue(object v) { value = v; }
            public bool AsBool() { return (bool)value; }
            public int AsInt() { return (int)value; }
            public float AsFloat() { return (float)value; }
            public string AsString() { return (string)value; }
        }

        public class KeyValue
        {
            public string key;
            public SerializeValue value;
            public KeyValue(string k, SerializeValue v)
            {
                key = k;
                value = v;
            }
        }

        private List<KeyValue> items = new List<KeyValue>();
        public List<KeyValue> GetItems() { return items; }

        public PropertyBundle() { }

        private KeyValue GetKeyByName(string key)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].key == key)
                {
                    return items[i];
                }
            }
            return null;
        }

        public void SetValue(string key, object value)
        {
            KeyValue kv = GetKeyByName(key);
            if (kv != null)
            {
                kv.value.value = value;
            }
            else
            {
                items.Add(new KeyValue(key, new SerializeValue(value)));
            }
        }

        public bool TryGetObject(string key, out SerializeValue value)
        {
            KeyValue kv = GetKeyByName(key);
            value = kv?.value;
            return value != null;
        }

        public object GetObject(string key, object defaultValue)
        {
            KeyValue kv = GetKeyByName(key);
            return kv == null ? defaultValue : kv.value;
        }
    }
}
