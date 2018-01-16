using System.Collections.Generic;
using Yata.CoreNode.PropertiesUi;

namespace Yata.ImageNodes.Nodes.Generators
{
    public class BrickGeneratorNode : ImageNodeBase
    {

        private class Brick
        {
            public float x, y, w, h;
            public Brick(float x, float y, float w, float h)
            {
                this.x = x;
                this.y = y;
                this.w = w;
                this.h = h;
            }
            public bool IsPointInside(float pX, float pY)
            {
                return ((pX >= x) && (pY >= y) && (pX < x + w) && (pY < y + h));
            }
            public Brick Clone()
            {
                return new Brick(x, y, w, h);
            }
        }

        private FloatColor white = new FloatColor(1.0f, 1.0f, 1.0f, 1.0f);
        private FloatColor black = new FloatColor(0.0f, 0.0f, 0.0f, 1.0f);
        private List<Brick> bricks = new List<Brick>();

        [DataTypeUINumeric("Density X", 1, 200)]
        private int brickDensityX = 5;
        [DataTypeUINumeric("Density Y", 1, 200)]
        private int brickDensityY = 8;
        [DataTypeUIFloat("Space X", 0.0f, 1.0f)]
        private float spaceX = 0.02f;
        [DataTypeUIFloat("Space Y", 0.0f, 1.0f)]
        private float spaceY = 0.02f;
        [DataTypeUIFloat("Row shift", 0.0f, 1.0f)]
        private float brickRowShift = 0.1f;

        public BrickGeneratorNode()
            : base(FriendlyName)
        {
            outputs.Add(new FloatColorOutput(this, "Bricks"));
            Init();
        }

        protected override void Setup()
        {
            base.Setup();
            GenerateBricks();
        }

        public override FloatColor GetPixel(float x, float y, bool preview)
        {
            x = WrapCoord(x);
            y = WrapCoord(y);
            for (int i = 0; i < bricks.Count; i++)
            {
                if (bricks[i].IsPointInside(x, y))
                {
                    return white;
                }
            }
            return black;
        }

        private void GenerateBricks()
        {
            bricks.Clear();

            float brickW = 1.0f / brickDensityX;
            float brickH = 1.0f / brickDensityY;

            float shift = 0.0f;
            for (int y = 0; y < brickDensityY; y++)
            {
                shift += brickRowShift;
                while (shift > brickW)
                    shift -= brickW;

                for (int x = 0; x < brickDensityX; x++)
                {
                    float xx = brickW * x;
                    float yy = brickH * y;
                    Brick b = CreateBrick(xx + shift, yy, (1.0f / brickDensityX), (1.0f / brickDensityY));
                    AddBrickInView(b);
                }
            }
        }

        private void AddBrickInView(Brick b)
        {
            if (IsBickFullView(b))
            {
                Brick b2 = b.Clone();
                b2.x -= 1.0f;
                bricks.Add(b2);
            }
            bricks.Add(b);
        }

        private Brick CreateBrick(float x, float y, float w, float h)
        {
            float sizeX = spaceX / 2.0f;
            float sizeY = spaceY / 2.0f;
            return new Brick(x + sizeX, y + sizeY, w - spaceX, h - spaceY);
        }

        private bool IsBickFullView(Brick b)
        {
            // Only shifted at the x-axis
            return (b.x + b.w) > 1.0;
        }

        public override bool ShowPropertiesDialog()
        {
            base.ShowPropertiesDialog();
            PropertiesFormWrapper form = new PropertiesFormWrapper(this, FriendlyName);
            return form.Show();
        }

        public static string FriendlyName
        {
            get { return "Bricks generator"; }
        }

        public static string SubMenuPath
        {
            get { return "Image.Generator"; }
        }
    }
}
