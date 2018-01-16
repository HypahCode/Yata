using System;

namespace Yata.SoundNodes.Nodes.Generators
{
    public class SawToothNode : GeneratorNodeBase
    {
        public SawToothNode()
            : base(FriendlyName)
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

        public static string FriendlyName
        {
            get { return "Saw tooth wave"; }
        }

        public static string SubMenuPath
        {
            get { return "Sound.Generator"; }
        }
    }
}
