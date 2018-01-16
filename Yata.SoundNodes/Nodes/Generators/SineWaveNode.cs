using System;

namespace Yata.SoundNodes.Nodes.Generators
{
    public class SineWaveNode : GeneratorNodeBase
    {
        public SineWaveNode()
            : base(FriendlyName)
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

        public static string FriendlyName
        {
            get { return "Sine wave"; }
        }

        public static string SubMenuPath
        {
            get { return "Sound.Generator"; }
        }
    }
}
