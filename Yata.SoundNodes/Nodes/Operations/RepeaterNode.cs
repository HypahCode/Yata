using System.Windows.Forms;
using Yata.CoreNode;

namespace Yata.SoundNodes.Nodes.Generators
{
    public class RepeaterNode : SoundNodeBase
    {
        private float repeatCycles = 1000f;

        private SoundWaveInput input;
        private const int frequency = 44100;

        public RepeaterNode()
            : base(FriendlyName)
        {
            outputs.Add(new SoundWaveOutput(this, "Out"));

            input = new SoundWaveInput(this, "In");
            inputs.Add(input);

            Init();
        }

        public override float GetWave(float samplePosition)
        {
            return input.GetWave(samplePosition);
        }

        public override float GetLenght()
        {
            return input.GetLength() * repeatCycles;
        }

        public override bool ShowPropertiesDialog()
        {
            base.ShowPropertiesDialog();
            PropertiesForm form = new PropertiesForm(FriendlyName, this);

            return (form.ShowDialog() == DialogResult.OK);
        }

        public override void Save(PropertyBundle bundle)
        {
            base.Save(bundle);
        }

        public override void Load(PropertyBundle bundle)
        {
            base.Load(bundle);
        }

        public static string FriendlyName
        {
            get { return "Repeater"; }
        }

        public static string SubMenuPath
        {
            get { return "Sound.Operations"; }
        }

    }
}
