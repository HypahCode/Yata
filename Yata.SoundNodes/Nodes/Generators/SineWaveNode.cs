using System;
using Yata.CoreNode;

namespace Yata.SoundNodes.Nodes.Generators
{
    [NodeUsage(@"Sound.Generator", nodeName)]
    public class SineWaveNode : GeneratorNodeBase
    {
        private const string nodeName = @"Sine wave";

        public SineWaveNode()
            : base(nodeName)
        {
            outputs.Add(new SoundWaveOutput(this, "Sin"));

            Init();
        }

        public override float GetWave(float samplePosition)
        {
            return (float)Math.Sin(samplePosition * (Math.PI * 2.0) * frequency);
        }

        public override float GetLenght()
        {
            return 1.0f / frequency;
        }
    }
}
