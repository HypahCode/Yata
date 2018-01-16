using System;
using System.Collections.Generic;
using System.Drawing;
using static Yata.CoreNode.PropertyBundle;

namespace Yata.CoreNode.DataTypes
{
    public class DataTypeSerializerFactory
    {
        private Dictionary<string, DataTypeSerializer> serializers = new Dictionary<string, DataTypeSerializer>();

        private DataTypeSerializerFactory()
        {
            RegisterSerializer(typeof(bool), new BoolSerializer());
            RegisterSerializer(typeof(int), new IntSerializer());
            RegisterSerializer(typeof(float), new FloatSerializer());
            RegisterSerializer(typeof(string), new StringSerializer());
            RegisterSerializer(typeof(Color), new ColorSerializer());
        }

        private static DataTypeSerializerFactory instance;
        public static DataTypeSerializerFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataTypeSerializerFactory();
                }
                return instance;
            }
        }

        public void RegisterSerializer(Type type, DataTypeSerializer ser)
        {
            serializers[type.FullName] = ser;
        }

        public string Serialize(SerializeValue value)
        {
            return serializers[value.value.GetType().FullName].Serialize(value.value);
        }

        public object Deserialize(string type, string value)
        {
            return serializers[type].Deserialize(value);
        }
    }
}
