using System.Drawing;
using Yata.CoreNode;

namespace Yata.SoundNodes
{
    public class SoundNodeBase : Node
    {
        public SoundNodeBase(string name)
            : base(name)
        {
        }

        public virtual float GetLenght()
        {
            return 0.0f;
        }

        public virtual float GetWave(float samplePosition)
        {
            return 0.0f;
        }

        protected override void Update(Bitmap preview)
        {
            Render(preview);
        }
        
        public void Render(Bitmap preview)
        {
            Graphics g = Graphics.FromImage(preview);
            Rectangle r = new Rectangle(0, 0, preview.Width, preview.Height);
            
            DrawBlackQuad(g, r);
            Point[] wave = CreateWaveForm(r);
            g.DrawLines(Pens.Lime, wave);
        }

        private void DrawBlackQuad(Graphics g, Rectangle r)
        {
            g.FillRectangle(Brushes.Black, r);
        }

        private Point[] CreateWaveForm(Rectangle r)
        {
            float sampleStep = GetLenght() / r.Width;

            Point[] points = new Point[r.Width];
            for (int x = 0; x < r.Width; x++)
            {
                float samplePos = sampleStep * (float)x;

                float scaledCenteredWave = (GetWave(samplePos) * 0.5f) + 0.5f;
                points[x] = new Point(x, (int)(scaledCenteredWave * (float)r.Height));
            }

            return points;
        }

        protected float WrapCoord(float value)
        {
            return value % 1.0f;
        }

        public SoundWaveInput AddInput(string name)
        {
            SoundWaveInput input = new SoundWaveInput(this, name);
            inputs.Add(input);
            return input;
        }
    }
}
