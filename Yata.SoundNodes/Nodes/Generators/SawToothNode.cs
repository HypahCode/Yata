using Yata.CoreNode;

namespace Yata.SoundNodes.Nodes.Generators
{
    [NodeUsage(@"Sound.Generator", nodeName)]
    public class SawToothNode : GeneratorNodeBase
    {
        private const string nodeName = @"Saw wave";

        public SawToothNode()
            : base(nodeName)
        {
            outputs.Add(new SoundWaveOutput(this, "Saw"));

            Init();
        }

        public override float GetWave(float samplePosition)
        {
            float tooth = (samplePosition % GetLenght()) / GetLenght();
            return tooth;
        }

        public override float GetLenght()
        {
            return 1.0f / frequency;
        }
    }
}
