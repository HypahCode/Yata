using System;
using System.Drawing;
using System.Windows.Forms;

namespace Yata
{
    public class FlickerFreePanel : UserControl
    {
        private bool initializationComplete;
        private bool isDisposing;

        private BufferedGraphicsContext backbufferContext;
        private BufferedGraphics backbufferGraphics;
        private Graphics drawingGraphics;

        public delegate void RenderEventHandler(object sender, Graphics graphics);
        public event RenderEventHandler OnRender;

        public FlickerFreePanel()
        {
            // Set the control style to double buffer.
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, false);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            // Assign our buffer context.
            backbufferContext = BufferedGraphicsManager.Current;
            initializationComplete = true;

            RecreateBuffers();

            Redraw();
        }

        protected override void Dispose(bool disposing)
        {
            isDisposing = true;
            if (disposing)
            {
                // We must dispose of backbufferGraphics before we dispose of backbufferContext or we will get an exception.
                if (backbufferGraphics != null)
                    backbufferGraphics.Dispose();
                if (backbufferContext != null)
                    backbufferContext.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            RecreateBuffers();
            Redraw();
        }

        private void RecreateBuffers()
        {
            if (!initializationComplete || isDisposing)
                return;

            backbufferContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);

            if (backbufferGraphics != null)
                backbufferGraphics.Dispose();

            backbufferGraphics = backbufferContext.Allocate(this.CreateGraphics(),
                new Rectangle(0, 0, Math.Max(this.Width, 1), Math.Max(this.Height, 1)));

            drawingGraphics = backbufferGraphics.Graphics;

            Invalidate();
        }

        public void Render()
        {
            Redraw();
        }

        private void Redraw()
        {
            if (drawingGraphics == null)
                return;

            if (OnRender != null)
            {
                OnRender(this, drawingGraphics);
            }

            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!isDisposing && backbufferGraphics != null)
                backbufferGraphics.Render(e.Graphics);
        }
    }

}
