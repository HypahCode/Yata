using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Yata.CoreNode;

namespace HellTech.Editor
{
    public class EngineNodeBase : Node
    {
        private DataNode dataNode;
        private List<EngineProperty> props = new List<EngineProperty>();
        private string exportFile = "";

        public EngineNodeBase(DataNode data)
            : base(data.Name)
        {
            dataNode = data;

            for (int i = 0; i < dataNode.Keys.Count; i++)
            {
                KeyValue kv = dataNode.Keys[i];

                if ((kv.Key != "PARENT") &&
                    (kv.Key != "UNUSED"))
                {
                    EngineProperty ep = new EngineProperty(kv.Key);

                    string[] str = kv.Value.Split(';');
                    if (str.Length > 0)
                    {
                        ep.type = str[0];
                        string t = ep.type.ToLower();
                        if (t == "bool") ep.value = "false";
                        if (t == "int") ep.value = "0";
                        if (t == "float") ep.value = "0";
                        if (str.Length > 1)
                        {
                            ep.extra = str[1];
                        }
                    }
                    props.Add(ep);
                }
            }

            if (!((data.Name == "root") || (data.Name == "vars") || (data.Name == "meta")))
            {
                inputs.Add(new DataNodeInput(this, "input"));
            }
            outputs.Add(new DataNodeOutput(this, "output"));

            Init();
        }

        protected override void Update(Bitmap preview)
        {
            Render(preview);
        }

        public void Render(Bitmap preview)
        {
            Graphics g = Graphics.FromImage(preview);
            int w = preview.Width;
            int h = preview.Height;

            g.FillRectangle(Brushes.Black, new Rectangle(0, 0, w, h));
        }

        public override bool ShowPropertiesDialog()
        {
            base.ShowPropertiesDialog();
            if (dataNode.Name == "root")
            {
                return RecursiveSave();
            }
            else
            {
                return ShowProperties();
            }
        }

        public void ExportRecursive(DataNode node)
        {
            DataNode n = node.AddChild(dataNode.Name, "");
            for (int i = 0; i < props.Count; i++)
            {
                n.AddValue(props[i].name, props[i].value);
            }
            for (int i = 0; i < outputs.Count; i++)
            {
                if (outputs[i].GetParent() != null)
                {
                    ((EngineNodeBase)outputs[i].GetParent()).ExportRecursive(n);
                }
            }
        }

        private void ExportRecursive()
        {
            DataFile dataFile = new DataFile();
            for (int i = 0; i < outputs.Count; i++)
            {
                if (outputs[i].GetParent() != null)
                {
                    ((EngineNodeBase)outputs[i].GetParent()).ExportRecursive(dataFile.Root);
                }
            }
            dataFile.SaveToFile(exportFile);
        }

        private bool RecursiveSave()
        {
            PropertiesForm form = new PropertiesForm(dataNode.Name, this);
            Button btn;
            TextBox textBox = form.AddEditWithButtonControl("Save file", exportFile, "browse", out btn);
            textBox.TextChanged += new System.EventHandler(FileTextChanged);
            btn.Click += delegate(object sender, System.EventArgs args)
            {
                SaveFileDialog od = new SaveFileDialog();
                od.Title = "Export data file";
                if (exportFile != "")
                {
                    od.InitialDirectory = Path.GetPathRoot(od.FileName);
                }
                if (od.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    textBox.Text = od.FileName;
                }
            };
            form.AddButton("Export", "Save file!").Click += delegate(object sender, System.EventArgs args) { ExportRecursive(); };
            return (form.ShowDialog() == DialogResult.OK);
        }

        private void FileTextChanged(object sender, System.EventArgs e)
        {
            exportFile = ((TextBox)sender).Text;
        }

        private bool ShowProperties()
        {
            PropertiesForm form = new PropertiesForm(dataNode.Name, this);

            for (int i = 0; i < props.Count; i++)
            {
                EngineProperty p = props[i];
                switch (p.type.ToLower())
                {
                    case "bool":
                        form.AddCheckbox(p.name, bool.Parse(p.value)).CheckedChanged += new System.EventHandler(CheckBoxChanged);
                        break;
                    case "int":
                        NumericUpDown numberInt = form.AddNumericIntControl(p.name, (decimal)(int.Parse(p.value)), (decimal)(int.MinValue), (decimal)(int.MaxValue));
                        numberInt.ValueChanged += new System.EventHandler(NumberIntValueChanged);
                        break;
                    case "float":
                        NumericUpDown number = form.AddNumericControl(p.name, (decimal)(double.Parse(p.value)), (decimal)(-1000000000.0), (decimal)(1000000000.0));
                        number.ValueChanged += new System.EventHandler(NumberValueChanged);
                        break;
                    default:
                        if (p.extra != "")
                        {
                            string[] str = p.extra.Split(',');
                            if (str.Length > 0)
                            {
                                form.AddComboBoxControl(p.name, p.value, str).SelectedValueChanged += new System.EventHandler(ComboBoxValueChanged);
                            }
                        }
                        else
                        {
                            form.AddEditControl(p.name, p.value).TextChanged += new System.EventHandler(TextValueChanged);
                        }
                        break;
                }
            }

            return (form.ShowDialog() == DialogResult.OK);
        }

        private EngineProperty FindProp(string name)
        {
            for (int i = 0; i < props.Count; i++)
            {
                if (props[i].name == name)
                {
                    return props[i];
                }
            }
            return null;
        }

        private void CheckBoxChanged(object sender, System.EventArgs e)
        {
            EngineProperty prop = FindProp((string)((Control)sender).Tag);
            if (prop != null)
            {
                prop.value = ((CheckBox)sender).Checked.ToString();
            }
        }

        private void NumberIntValueChanged(object sender, System.EventArgs e)
        {
            EngineProperty prop = FindProp((string)((Control)sender).Tag);
            if (prop != null)
            {
                int val = (int)((NumericUpDown)sender).Value;
                prop.value = val.ToString();
            }
        }
        private void NumberValueChanged(object sender, System.EventArgs e)
        {
            EngineProperty prop = FindProp((string)((Control)sender).Tag);
            if (prop != null)
            {
                float val = (float)((NumericUpDown)sender).Value;
                prop.value = val.ToString();
            }
        }
        private void ComboBoxValueChanged(object sender, System.EventArgs e)
        {
            EngineProperty prop = FindProp((string)((Control)sender).Tag);
            if (prop != null)
            {
                string val = (string)((ComboBox)sender).SelectedItem;
                prop.value = val;
            }
        }
        private void TextValueChanged(object sender, System.EventArgs e)
        {
            EngineProperty prop = FindProp((string)((Control)sender).Tag);
            if (prop != null)
            {
                string val = (string)((TextBox)sender).Text;
                prop.value = val;
            }
        }

        public override void Save(PropertyBundle bundle)
        {
            base.Save(bundle);
            for (int i = 0; i < props.Count; i++)
            {
                bundle.PutString(props[i].name, props[i].value);
            }
        }

        public override void Load(PropertyBundle bundle)
        {
            base.Load(bundle);
            for (int i = 0; i < props.Count; i++)
            {
                props[i].value = bundle.GetString(props[i].name, props[i].value);
            }
        }
    }
}
