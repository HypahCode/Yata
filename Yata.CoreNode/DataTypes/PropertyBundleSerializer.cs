using System;
using System.Reflection;

namespace Yata.CoreNode.DataTypes
{
    public class PropertyBundleSerializer
    {
        private PropertyBundleSerializer() { }

        public static void Deserialize(PropertyBundle bundle, object instance)
        {
            Type type = instance.GetType();
            MemberInfo[] members = type.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

            for (int i = 0; i < members.Length; i++)
            {
                MemberInfo member = members[i];
                object[] attribs = member.GetCustomAttributes(typeof(DataTypeAttribute), true);

                for (int j = 0; j < attribs.Length; j++)
                {
                    if (attribs[j] is DataTypeAttribute modelAttrib)
                    {
                        FillField(instance, member, modelAttrib, bundle);
                    }
                }
            }
        }

        private static void FillField(object instance, MemberInfo member, DataTypeAttribute attrib, PropertyBundle bundle)
        {
            MemberValue mi = new MemberValue(member, instance);
            if (bundle.TryGetObject(mi.GetName(), out PropertyBundle.SerializeValue value))
                mi.SetValue(value.value);
        }


        public static PropertyBundle Serialize(object instance)
        {
            PropertyBundle bundle = new PropertyBundle();
            Serialize(bundle, instance);
            return bundle;
        }

        public static void Serialize(PropertyBundle bundle, object instance)
        {
            Type type = instance.GetType();
            MemberInfo[] members = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            for (int i = 0; i < members.Length; i++)
            {
                MemberInfo member = members[i];
                object[] attribs = member.GetCustomAttributes(typeof(DataTypeAttribute), true);

                for (int j = 0; j < attribs.Length; j++)
                {
                    if (attribs[j] is DataTypeAttribute attrib)
                    {
                        MemberValue mi = new MemberValue(member, instance);
                        object val = mi.GetValue();
                        if (val != null)
                        {
                            bundle.SetValue(member.Name, val);
                        }
                    }
                }
            }
        }
    }
}
