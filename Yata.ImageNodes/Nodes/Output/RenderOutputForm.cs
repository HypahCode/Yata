using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Yata.ImageNodes.Nodes.Output
{
    public partial class RenderOutputForm : Form
    {
        private class FlickerFreePanel : Panel
        {
            public FlickerFreePanel()
            {
                SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            }
        }

        private const int JobSize = 32;
        private ImageNodeBase outputNode;
        private bool rendering = false;
        private List<Rectangle> jobs = new List<Rectangle>();

        private Bitmap renderOuput = null;
        private FlickerFreePanel renderPanel;

        public RenderOutputForm(ImageNodeBase node)
        {
            outputNode = node;
            InitializeComponent();
            renderPanel = new FlickerFreePanel();
            renderPanel.Paint += new PaintEventHandler(panel1_Paint);
            renderPanel.Dock = DockStyle.Fill;
            renderPanel.Parent = panel1;

            FormClosed += new FormClosedEventHandler(RenderOutputForm_FormClosed);

            Size renderSize = new Size(renderPanel.Width, renderPanel.Height);

            SetupJobs(renderSize.Width, renderSize.Height);
            StartJobProcessing(renderSize.Width, renderSize.Height);
        }

        private void RenderOutputForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            rendering = false;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (renderOuput != null)
            {
                lock (renderOuput)
                {
                    e.Graphics.DrawImage(renderOuput, new Rectangle(0, 0, renderPanel.Width, renderPanel.Height));
                }
            }
        }

        private void SetupJobs(int w, int h)
        {
            renderOuput = new Bitmap(w, h);

            lock (jobs)
            {
                jobs.Clear();
                int x = 0;
                while (x < w)
                {
                    int y = 0;
                    while (y < h)
                    {
                        Rectangle r = ClipRect(new Rectangle(x, y, JobSize, JobSize), w, h);
                        jobs.Add(r);
                        y += JobSize;
                    }
                    x += JobSize;
                }
            }
        }

        private Rectangle ClipRect(Rectangle rectangle, int w, int h)
        {
            if (rectangle.Left > w) rectangle.Width = rectangle.Width - (w - (rectangle.X + rectangle.Width));
            if (rectangle.Bottom > h) rectangle.Height = rectangle.Height - (h - (rectangle.Y + rectangle.Height));
            return rectangle;
        }

        private void StartJobProcessing(int w, int h)
        {
            outputNode.PreRenderSetup(w, h);
            rendering = true;

            int jobCount = Environment.ProcessorCount;
            if (jobs.Count < jobCount) jobCount = jobs.Count;
            for (int i = 0; i < jobCount; i++)
            {
                DoNextJob(w, h);
            }
        }

        private void DoNextJob(int w, int h)
        {
            lock (jobs)
            {
                if ((jobs.Count > 0) && (rendering))
                {
                    StartRenderJob(jobs[0], w, h);
                    jobs.RemoveAt(0);
                }
            }
        }

        private void StartRenderJob(Rectangle rect, int w, int h)
        {
            if (rendering)
            {
                Thread thread = new Thread(() => RenderBitmap(rect, w, h));
                rendering = true;
                thread.Start();
            }
        }

        private void RenderBitmap(Rectangle rect, int w, int h)
        {
            Bitmap bitmap = new Bitmap(rect.Width, rect.Height);
            for (int y = rect.Y; y < rect.Bottom; y++)
            {
                for (int x = rect.X; x < rect.Right; x++)
                {
                    // Normalize coords
                    float xx = (float)x / (float)w;
                    float yy = (float)y / (float)h;

                    FloatColor color = outputNode.GetPixel(xx, yy, false);

                    bitmap.SetPixel(x - rect.X, y - rect.Y, color.ToColor());
                }
            }

           
            lock (renderOuput)
            {
                Graphics g = Graphics.FromImage(renderOuput);
                g.DrawImage(bitmap, rect);
            }
            renderPanel.Invalidate();

            DoNextJob(w, h);
        }
    }
}
