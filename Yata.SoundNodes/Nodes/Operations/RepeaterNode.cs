using System.Windows.Forms;
using Yata.CoreNode;

namespace Yata.SoundNodes.Nodes.Generators
{
    [NodeUsage(@"Sound.Operations", nodeName)]
    public class RepeaterNode : SoundNodeBase
    {
        private const string nodeName = @"Repeater";

        private float repeatCycles = 1000f;

        private SoundWaveInput input;
        private const int frequency = 44100;

        public RepeaterNode()
            : base(nodeName)
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
            PropertiesForm form = new PropertiesForm(nodeName, this);

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
    }
}
