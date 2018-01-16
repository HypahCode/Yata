using System.Windows.Forms;
using Yata.CoreNode;

namespace Yata.SoundNodes.Nodes.Generators
{
    public class GeneratorNodeBase : SoundNodeBase
    {
        protected float frequency = 1000f;

        public GeneratorNodeBase(string name)
            : base(name)
        { }

        public override bool ShowPropertiesDialog()
        {
            base.ShowPropertiesDialog();
            PropertiesForm form = new PropertiesForm(GetName(), this);
            //form.AddNumericControl("Frequency", (s)frequency, (decimal)1.0, (decimal)100000.0);
            return (form.ShowDialog() == DialogResult.OK);
        }

        public override void Save(PropertyBundle bundle)
        {
            base.Save(bundle);
            //bundle.PutFloat("frequency", frequency);
        }

        public override void Load(PropertyBundle bundle)
        {
            base.Load(bundle);
            //frequency = bundle.GetFloat("frequency", 1000f);
        }
    }
}
