using System;
using System.Drawing;
using System.Windows.Forms;

namespace Yata.CoreNode
{
    public partial class PropertiesForm : Form
    {
        private const int PROPERTY_TEXT_WIDTH = 150;
        private const int PROPERTY_WIDTH = 300;
        private const int PROPERTY_HEIGHT = 32;
        private const int SPACING = 16;
        private int propertiesHeight = 16;
        private Node node;

        public PropertiesForm(string name, Node node)
        {
            InitializeComponent();
            this.Text = name;
            this.node = node;
        }
        
        public void AddControl(string name, Control c)
        {
            Label label = new Label();
            label.Text = name;
            label.Parent = this;
            label.Size = new Size(PROPERTY_TEXT_WIDTH, PROPERTY_HEIGHT);
            label.Location = new Point(SPACING, propertiesHeight);

            int controlHeight = PROPERTY_HEIGHT;

            if (c != null)
            {
                c.Parent = this;
                c.Location = new Point(PROPERTY_TEXT_WIDTH + SPACING * 2, propertiesHeight);
                c.Width = PROPERTY_WIDTH;

                if (c.Height > controlHeight)
                {
                    controlHeight = c.Height;
                }
            }

            propertiesHeight += controlHeight;
            Size s = ClientSize;
            s.Height = propertiesHeight + panel1.Height + SPACING;
            ClientSize = s;
        }
        
        public void AddLabel(string text)
        {
            AddControl(text, null);
        }

        public PropertyControlValue AddSliderControl(string name, int currentValue, int min, int max, int steps)
        {
            TrackBar trackBar = new TrackBar();
            trackBar.Tag = name;
            trackBar.Minimum = min;
            trackBar.Maximum = max;
            //trackBar.TickFrequency = steps;
            trackBar.Value = currentValue;
            AddControl(name, trackBar);

            PropertyControlValue value = new PropertyControlValue(currentValue);
            trackBar.ValueChanged += (x, y) => value.Value = trackBar.Value;

            return value;
        }

        public TextBox AddEditControl(string name, string currentValue)
        {
            TextBox textBox = new TextBox();
            textBox.Tag = name;
            textBox.Text = currentValue;
            AddControl(name, textBox);
            return textBox;
        }

        public TextBox AddEditWithButtonControl(string name, string currentValue, string buttonText, out Button button)
        {
            Panel panel = new Panel()
            {
                Width = PROPERTY_WIDTH,
                Height = PROPERTY_HEIGHT
            };
            TextBox textBox = new TextBox()
            {
                Parent = panel,
                Width = PROPERTY_WIDTH / 3 * 2,
                Text = currentValue
            };
            button = new Button()
            {
                Text = buttonText,
                Location = new Point(textBox.Width, 0),
                Width = panel.Width - textBox.Width,
                Parent = panel
            };
            AddControl(name, panel);
            return textBox;
        }

        public PropertyControlValue AddNumericFloatControl(string name, float currentValue, float min, float max)
        {
            NumericUpDown number = new NumericUpDown()
            {
                Tag = name,
                Minimum = Convert.ToDecimal(min),
                Maximum = Convert.ToDecimal(max),
                Value = Convert.ToDecimal(currentValue),
                
                DecimalPlaces = 6
            };
            AddControl(name, number);

            PropertyControlValue value = new PropertyControlValue(currentValue);
            number.ValueChanged += (x, y) => value.Value = (float)(number.Value);

            return value;
        }

        public PropertyControlValue AddNumericIntControl(string name, int currentValue, int min, int max)
        {
            NumericUpDown number = new NumericUpDown()
            {
                Tag = name,
                Minimum = min,
                Maximum = max,
                Value = currentValue,
                DecimalPlaces = 0
            };
            AddControl(name, number);

            PropertyControlValue value = new PropertyControlValue((int)currentValue);
            number.ValueChanged += (x, y) => value.Value = (int)(number.Value);

            return value;
        }

        public PropertyControlValue AddColorControl(string name, Color currentValue)
        {
            ColorPanel panel = new ColorPanel()
            {
                Tag = name,
                Color = currentValue,
                Height = SPACING,
                Width = PROPERTY_WIDTH
            };
            AddControl(name, panel);

            PropertyControlValue value = new PropertyControlValue(currentValue);
            panel.ColorChanged += (c) => value.Value = c;

            return value;
        }

        public PropertyControlValue AddComboBoxControl(string name, string currentValue, string[] options)
        {
            ComboBox combo = new ComboBox()
            {
                Tag = name
            };
            combo.Items.AddRange(options);
            combo.SelectedItem = currentValue;
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            AddControl(name, combo);

            PropertyControlValue value = new PropertyControlValue(currentValue);
            combo.SelectedValueChanged += (x, y) => value.Value = combo.SelectedItem;
            
            return value;
        }

        public PropertyControlValue AddCheckbox(string name, bool currentValue)
        {
            CheckBox cb = new CheckBox()
            {
                Tag = name,
                Checked = currentValue
            };
            AddControl(name, cb);

            PropertyControlValue value = new PropertyControlValue(currentValue);
            cb.CheckedChanged += (x, y) => value.Value = cb.Checked;

            return value;
        }

        public void AddGradientControl(string name)
        {
        }

        public void AddEnvenlopeControl(string name, Envelope env)
        {
            EnvenlopeControl envCtr = new EnvenlopeControl(env);
            envCtr.Tag = name;
            envCtr.Height = 64;
            envCtr.Click += new System.EventHandler(envCtr_Click);
            AddControl(name, envCtr);
        }

        public Button AddButton(string name, string buttonText)
        {
            Button button = new Button();
            button.Tag = name;
            button.Text = buttonText;
            AddControl(name, button);
            return button;
        }

        private void envCtr_Click(object sender, System.EventArgs e)
        {
            EnvelopeEditorForm form = new EnvelopeEditorForm(((EnvenlopeControl)sender).Env);
            form.ShowDialog();
        }

        private void okBtn_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelBtn_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
