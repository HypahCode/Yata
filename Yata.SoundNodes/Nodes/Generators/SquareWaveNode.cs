using System;
using Yata.CoreNode;

namespace Yata.SoundNodes.Nodes.Generators
{
    [NodeUsage(@"Sound.Generator", nodeName)]
    public class SquareWaveNode : GeneratorNodeBase
    {
        private const string nodeName = @"Square wave";

        public SquareWaveNode()
            : base(nodeName)
        {
            outputs.Add(new SoundWaveOutput(this, "Square"));

            Init();
        }

        public override float GetWave(float samplePosition)
        {
            float wave = (float)Math.Sin(samplePosition * (Math.PI * 2.0) * frequency);
            return (wave > 0.0f) ? 1.0f : -1.0f;
        }

        public override float GetLenght()
        {
            return 1.0f / frequency;
        }
    }
}
