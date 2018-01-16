using System;
using System.Windows.Forms;
using Yata.CoreNode;

namespace Yata.SoundNodes.Nodes.Generators
{
    public class BlockWaveNode : GeneratorNodeBase
    {
        public BlockWaveNode()
            : base(FriendlyName)
        {
            outputs.Add(new SoundWaveOutput(this, "Block"));

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

        public static string FriendlyName
        {
            get { return "Block wave"; }
        }

        public static string SubMenuPath
        {
            get { return "Sound.Generator"; }
        }
    }
}
