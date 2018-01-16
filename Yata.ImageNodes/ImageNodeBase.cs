using System.Drawing;
using Yata.CoreNode;

namespace Yata.ImageNodes
{
    public class ImageNodeBase : Node
    {
        private FloatColor black = new FloatColor(0.0f, 0.0f, 0.0f, 1.0f);
        protected Point prepareForRenderSize = new Point(0, 0);

        public ImageNodeBase(string name)
            : base(name)
        {
        }

        public virtual void PrepareForRender(int w, int h) { }

        public virtual FloatColor GetPixel(float x, float y, bool isPreview)
        {
            return black;
        }

        protected override void Update(Bitmap preview)
        {
            Render(preview);
        }

        public void PreRenderSetup(int w, int h)
        {
            if ((prepareForRenderSize.X != w) || (prepareForRenderSize.Y != h))
            {
                prepareForRenderSize.X = w;
                prepareForRenderSize.Y = h;
                PrepareForRender(w, h);
            }
            for (int i = 0; i < inputs.Count; i++)
            {
                if (inputs[i].outputChannel != null)
                {
                    ImageNodeBase outChannelNode = inputs[i].outputChannel.GetParent() as ImageNodeBase;
                    if (outChannelNode != null)
                    {
                        outChannelNode.PreRenderSetup(w, h);
                    }
                }
            }
        }

        public void Render(Bitmap preview)
        {
            int w = preview.Width;
            int h = preview.Height;
            PreRenderSetup(w, h);

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    float xx = (float)x / (float)w;
                    float yy = (float)y / (float)h;

                    FloatColor color = GetPixel(xx, yy, true);
                    preview.SetPixel(x, y, color.ToColor());
                }
            }
        }

        protected float WrapCoord(float value)
        {
            return value % 1.0f;
        }

        public FloatColorInput AddInput(string name)
        {
            FloatColorInput input = new FloatColorInput(this, name);
            inputs.Add(input);
            return input;
        }
    }
}
