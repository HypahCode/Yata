using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Yata.CoreNode.DataTypes;

namespace Yata.CoreNode.PropertiesUi
{
    public class PropertiesFormWrapper
    {
        private class UiProperty
        {
            public MemberValue mi;
            public DataTypeAttribute attr;
            public PropertyControlValue uiControlValue = null;
            public UiProperty(MemberValue m, DataTypeAttribute a)
            {
                mi = m;
                attr = a;
            }
        }


        private Node node;
        private string name;

        private List<UiProperty> nodeProperties = new List<UiProperty>();

        public PropertiesFormWrapper(Node node, string name)
        {
            this.node = node;
            this.name = name;
        }

        public bool Show()
        {
            PropertiesForm form = new PropertiesForm(name, node);
            LoadProperties();
            BuildPropertiesUi(form);

            if (form.ShowDialog() == DialogResult.OK)
            {
                SaveProperties();
                return true;
            }

            return false;
        }

        private void BuildPropertiesUi(PropertiesForm form)
        {
            foreach (UiProperty m in nodeProperties)
            {
                if (m.attr is UiPropertyAttribute uip)
                    m.uiControlValue = uip.AddUIComponent(form, m.mi);
                else
                    form.AddEditControl(m.attr.FriendlyName, m.mi.GetValue().ToString());
            }
        }

        private void LoadProperties()
        {
            Type type = node.GetType();
            MemberInfo[] members = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            for (int i = 0; i < members.Length; i++)
            {
                MemberInfo member = members[i];
                object[] attribs = member.GetCustomAttributes(typeof(DataTypeAttribute), true);

                for (int j = 0; j < attribs.Length; j++)
                {
                    if (attribs[j] is DataTypeAttribute attrib)
                    {
                        nodeProperties.Add(new UiProperty(new MemberValue(member, node), attrib));
                    }
                }
            }
        }

        private void SaveProperties()
        {
            foreach (UiProperty m in nodeProperties)
            {
                if (m.uiControlValue != null)
                {
                    m.mi.SetValue(m.uiControlValue.Value);
                }
            }
        }
    }
}
