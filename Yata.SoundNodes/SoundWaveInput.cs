using Yata.CoreNode;
using System.Drawing;

namespace Yata.SoundNodes
{
    public class SoundWaveInput : NodeInput
    {
        public float defaultValue;

        public SoundWaveInput(Node parent, string name)
            : this(parent, name, 0.0f)
        {
        }

        public SoundWaveInput(Node parent, string name, float defaultColor)
            : base(parent, name)
        {
            defaultValue = defaultColor;
            drawColor = Color.FromArgb(200, 100, 100);
        }

        public float GetLength()
        {
            SoundWaveOutput output = outputChannel as SoundWaveOutput;

            if (outputChannel == null)
            {
                return defaultValue;
            }
            return ((SoundWaveOutput)outputChannel).GetLength(); 
        }

        public float GetWave(float x)
        {
            SoundWaveOutput output = outputChannel as SoundWaveOutput;

            if (outputChannel == null)
            {
                return defaultValue;
            }
            return ((SoundWaveOutput)outputChannel).GetWave(x);
        }

        public override bool SetOutputChannel(NodeOutput output)
        {
            if (output is SoundWaveOutput)
            {
                return base.SetOutputChannel(output);
            }
            return false;
        }
    }
}
