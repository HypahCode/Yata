using System.Drawing;
using Yata.CoreNode;

namespace Yata.SoundNodes
{
    public class SoundWaveOutput : NodeOutput
    {
        public SoundWaveOutput(Node parent, string name)
            : base(parent, name)
        {
            drawColor = Color.FromArgb(100, 100, 200);
        }

        public virtual float GetLength()
        {
            return ((SoundNodeBase)GetParent()).GetLenght();
        }

        public virtual float GetWave(float x)
        {
            return ((SoundNodeBase)GetParent()).GetWave(x);
        }
    }
}
