using System;
using System.Drawing;
using Yata.CoreNode;
using Yata.ImageNodes;
using System.Windows.Forms;

namespace Yata.ImageNodes.Nodes
{
    public class ExampleNode : ImageNodeBase
    {
        private class ExampleOutput : FloatColorOutput
        {
            public ExampleOutput(Node parent, string name)
                : base(parent, name)
            {
            }

            public override FloatColor GetPixel(float x, float y, bool preview)
            {
                return ((ImageNodeBase)GetParent()).GetPixel(x, y, preview);
            }
        }

        private class RandomOutput : FloatColorOutput
        {
            private Random random;

            public RandomOutput(Node parent, string name)
                : base(parent, name)
            {
                random = new Random();
            }

            public override FloatColor GetPixel(float x, float y, bool preview)
            {
                return new FloatColor((float)random.Next(255) / 255.0f) + ((ImageNodeBase)GetParent()).GetPixel(x, y, preview);
            }
        }

        private FloatColorInput input;

        private Envelope envelope;

        public ExampleNode()
            : base(FriendlyName)
        {
            input = AddInput("input");

            outputs.Add(new ExampleOutput(this, "Output"));
            outputs.Add(new RandomOutput(this, "Random"));

            envelope = new Envelope();
            Envelope.EnvelopeLine line = envelope.AddLine("Test 1");
            line.AddPoint(new Envelope.EnvelopePoint(0.0f, 0.0f));
            line.AddPoint(new Envelope.EnvelopePoint(0.25f, 1.0f));
            line.AddPoint(new Envelope.EnvelopePoint(0.5f, 0.0f));
            line.AddPoint(new Envelope.EnvelopePoint(0.75f, 1.0f));
            line.AddPoint(new Envelope.EnvelopePoint(1.0f, 0.0f));

            Init();
        }

        public override FloatColor GetPixel(float x, float y, bool preview)
        {
            return input.GetPixel(x, y, preview);
        }

        public static string FriendlyName
        {
            get { return "Example node"; }
        }

        public static string SubMenuPath
        {
            get { return "Image"; }
        }

        public static Bitmap Icon
        {
            get {

                System.Reflection.Assembly thisExe;
                thisExe = System.Reflection.Assembly.GetExecutingAssembly();
                string[] resources = thisExe.GetManifestResourceNames();

                return new Bitmap(typeof(ExampleNode), "Icons.Example.png");
            }
        }

        public override bool ShowPropertiesDialog()
        {
            base.ShowPropertiesDialog();
            PropertiesForm form = new PropertiesForm(FriendlyName, this);
            //form.AddLabel("BADMUTS!");
            //form.AddSliderControl("Slider bitch", 5, 0, 10, 1);
            //form.AddEditControl("Edit bitch", "Mah bitch!");
            //form.AddNumericControl("Number bat bitch", (decimal)0.0, (decimal)-1.0, (decimal)1.0);
            //form.AddEnvenlopeControl("Test", envelope);

            return (form.ShowDialog() == DialogResult.OK);
        }

        public override void Save(PropertyBundle bundle)
        {
            base.Save(bundle);
            //bundle.PutBool("Bool", true);
            //bundle.PutInt("Int", 5);
            //bundle.PutFloat("Float", 5.0f);
            //bundle.PutString("String", "Some text");
        }
    }
}
