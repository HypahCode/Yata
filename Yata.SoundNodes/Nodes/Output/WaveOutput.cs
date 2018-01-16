using System.Media;
using Yata.CoreNode;

namespace Yata.SoundNodes.Nodes.Output
{
    public class WaveOutput : SoundNodeBase
    {
        private SoundWaveInput input;
        private const int frequency = 44100;

        public WaveOutput()
            : base(FriendlyName)
        {
            input = new SoundWaveInput(this, "Input");
            inputs.Add(input);
            
            Init();
        }

        public override float GetLenght()
        {
            return input.GetLength();
        }

        public override float GetWave(float samplePosition)
        {
            return input.GetWave(samplePosition);
        }

        public override bool ShowPropertiesDialog()
        {
            Play();
            return false;
        }

        private void Play()
        {
            WaveStream stream = CreateSoundStream();
            SoundPlayer snd = new SoundPlayer(stream);
            
            snd.Play();
        }

        private WaveStream CreateSoundStream()
        {
            float len = GetLenght();
            short[] buffer = sampleSound(len, frequency);

            return new WaveStream(buffer, frequency);
        }

        private short[] sampleSound(float len, float freqeuncy)
        {
            float sampleStep = 1.0f / freqeuncy;
            int bufferSize = (int)(len / sampleStep);
            short[] buffer = new short[bufferSize];

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = WaveFloatSoundToShort(GetWave((float)i * sampleStep));
            }
            return buffer;
        }

        private short WaveFloatSoundToShort(float value)
        {
            return (short)(value * 32767.0f);
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
            get { return "Speaker output"; }
        }

        public static string SubMenuPath
        {
            get { return "Sound.Output"; }
        }
    }
}
