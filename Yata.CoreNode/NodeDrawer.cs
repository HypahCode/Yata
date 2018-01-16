using System;
using System.Collections.Generic;
using System.Drawing;

namespace Yata.CoreNode
{
    internal class NodeDrawer
    {
        private const int CIRCLE_SIZE = 16;
        private const int CAPTION_SIZE = 16;
        private const int PREVIEW_SIZE = 64;
        private const int PREVIEW_BORDER = 8;

        private Bitmap previewBitmap = new Bitmap(PREVIEW_SIZE, PREVIEW_SIZE);
        private Rectangle previewRect;
        private RectangleF boundingRect = new RectangleF(0, 0, 32, 32);

        private List<NodeInput> inputs;
        private List<NodeOutput> outputs;

        private void BeforeRender()
        {
            int inputSize = (inputs.Count * CIRCLE_SIZE) + CAPTION_SIZE;
            int h = inputSize + (outputs.Count * CIRCLE_SIZE) + PREVIEW_SIZE + (PREVIEW_BORDER * 2);
            int w = PREVIEW_SIZE + (PREVIEW_BORDER * 2);

            boundingRect.Width = w;
            boundingRect.Height = h;

            previewRect = new Rectangle(PREVIEW_BORDER, inputSize + PREVIEW_BORDER, PREVIEW_SIZE, PREVIEW_SIZE);
        }

        internal Bitmap GetPreviewBitmap()
        {
            return previewBitmap;
        }

        internal RectangleF Rectangle
        {
            get { return boundingRect; }
            set { boundingRect = value; }
        }
        
        internal void SetInputs(List<NodeInput> inputList)
        {
            inputs = inputList;
        }

        internal void SetOutputs(List<NodeOutput> outputList)
        {
            outputs = outputList;
        }

        internal void Draw(IRenderContext context, string nodeName)
        {
            BeforeRender();

            DrawBoundingRectangle(context);
            DrawPreviewBitmap(context);
            DrawNodeName(context, nodeName);
            DrawInputs(context);
            DrawOutputs(context);
        }

        private void DrawBoundingRectangle(IRenderContext context)
        {
            Brush b = GetBrushAccoringIO();
            context.FillRectangle(b, Utils.RectFToRect(boundingRect));
        }

        private Brush GetBrushAccoringIO()
        {
            Brush brush;
            if ((inputs.Count > 0) && (outputs.Count > 0))
            {
                brush = new SolidBrush(Colors.Node);
            }
            else if (inputs.Count > 0)
            {
                brush = new SolidBrush(Colors.NodeOnlyInputs);
            }
            else if (outputs.Count > 0)
            {
                brush = new SolidBrush(Colors.NodeOnlyInputs);
            }
            else
            {
                brush = new SolidBrush(Colors.NodeNoIO);
            }
            return brush;
        }

        private void DrawPreviewBitmap(IRenderContext context)
        {
            // Draw preview
            if (previewBitmap != null)
            {
                RectangleF previewPoint = new RectangleF(previewRect.X + boundingRect.X,
                                                       previewRect.Y + boundingRect.Y,
                                                       previewRect.Width,
                                                       previewRect.Height);
                context.DrawImage(previewBitmap, Utils.RectFToRect(previewPoint));
            }
            else
            {
                Brush imagePreview = new SolidBrush(Colors.ImagePreviewEmpty);
                Rectangle imagePreviewRect = previewRect;
                imagePreviewRect.X = previewRect.X + (int)boundingRect.X;
                imagePreviewRect.Y = previewRect.Y + (int)boundingRect.Y;
                context.FillRectangle(imagePreview, imagePreviewRect);
            }
        }

        private void DrawNodeName(IRenderContext context, string name)
        {
            DrawString(context, name, new Point((int)boundingRect.Location.X, (int)boundingRect.Location.Y));
        }

        private void DrawInputs(IRenderContext context)
        {
            int x = (int)boundingRect.X - (CIRCLE_SIZE / 2);
            int y = (int)boundingRect.Y + CAPTION_SIZE;
            for (int i = 0; i < inputs.Count; i++)
            {
                Rectangle rectangle = new Rectangle(x, y + (i * CIRCLE_SIZE), CIRCLE_SIZE, CIRCLE_SIZE);
                inputs[i].visualRect = new Rectangle((int)boundingRect.X - (CIRCLE_SIZE / 2), (int)boundingRect.Y + CAPTION_SIZE + (i * CIRCLE_SIZE),
                    CIRCLE_SIZE, CIRCLE_SIZE);

                DrawCircle(context, rectangle, inputs[i].DrawColor);
                DrawString(context, inputs[i].Name, new Point(rectangle.Right + 4, rectangle.Y));
            }
        }

        private void DrawOutputs(IRenderContext context)
        {
            int x = (int)boundingRect.Right - (CIRCLE_SIZE / 2);
            int y = (int)boundingRect.Bottom - outputs.Count * CIRCLE_SIZE;
            for (int i = 0; i < outputs.Count; i++)
            {
                Rectangle rectangle = new Rectangle(x, y + (i * CIRCLE_SIZE), CIRCLE_SIZE, CIRCLE_SIZE);
                outputs[i].visualRect = new Rectangle((int)boundingRect.Right - (CIRCLE_SIZE / 2), (int)boundingRect.Bottom - outputs.Count * CIRCLE_SIZE + (i * CIRCLE_SIZE),
                    CIRCLE_SIZE, CIRCLE_SIZE);

                DrawCircle(context, rectangle, outputs[i].DrawColor);
                DrawStringInverted(context, outputs[i].Name, rectangle.Location);
            }
        }

        private void DrawCircle(IRenderContext context, Rectangle bounds, Color color)
        {
            Brush circleBrush = new SolidBrush(color);
            context.FillEllipse(circleBrush, bounds);
        }

        private void DrawStringInverted(IRenderContext context, string text, Point location)
        {
            int textWidth = (int)context.MeasureString(text);
            DrawString(context, text, new Point(location.X - 4 - textWidth, location.Y));
        }

        private void DrawString(IRenderContext context, string text, Point location)
        {
            SolidBrush textBrush = new SolidBrush(Colors.Font);
            context.DrawString(text, textBrush, location);
        }
    }
}
