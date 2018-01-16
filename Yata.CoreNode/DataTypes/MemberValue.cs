using System;
using System.Reflection;

namespace Yata.CoreNode.DataTypes
{
    public class MemberValue
    {
        private MemberInfo member;
        private object instance;

        public MemberValue(MemberInfo member, object instance)
        {
            this.member = member;
            this.instance = instance;
        }

        public void SetValue(object value)
        {
            if (member is FieldInfo fi)
                SetFieldValue(fi, value);
            else if (member is PropertyInfo pi)
                SetPropertyValue(pi, value);
        }

        public Type GetMemberType()
        {
            if (member is FieldInfo fi)
                return fi.FieldType;
            else if (member is PropertyInfo pi)
                return pi.PropertyType;
            throw new Exception("FUUUUUUUCK");
        }

        public object GetValue()
        {
            object value = null;
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    value = ((FieldInfo)member).GetValue(instance);
                    break;
                case MemberTypes.Property:
                    value = ((PropertyInfo)member).GetValue(instance, null);
                    break;
            }
            return value;
        }

        public string GetName()
        {
            return member.Name;
        }

        private void SetFieldValue(FieldInfo field, object value)
        {
            if (field.FieldType.IsEnum)
            {
                if (value is string s)
                    field.SetValue(instance, Enum.Parse(field.FieldType, s));
                else
                    field.SetValue(instance, value);
            }
            else
                field.SetValue(instance, value);
        }

        private void SetPropertyValue(PropertyInfo prop, object value)
        {
            if (prop.PropertyType.IsEnum)
            {
                if (value is string s)
                    prop.SetValue(instance, Enum.Parse(prop.PropertyType, s), null);
                else
                    prop.SetValue(instance, value, null);
            }
            else
                prop.SetValue(instance, value, null);
        }
    }
}
