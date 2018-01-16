using System.Drawing;
using System.Windows.Forms;
using Yata.CoreNode;
using Yata.Operations;
using Yata.Rendering;

namespace Yata
{
    public partial class EditControl : FlickerFreePanel
    {
        private string fileName;
        private UndoStack undoStack = new UndoStack();
        private NodeFactory nodeFactory;
        private NodeContainer nodes;
        
        private Point lastMousePosition = new Point();
        private bool dragLock = false;
        private Point dragPos = new Point(0, 0);
        private Point startPos = new Point(0, 0);
        private Node dragNode = null;
        private NodeIO outputLine = null;

        private RenderContext renderContext = new RenderContext();

        public string FileName { get { return fileName; } set { fileName = value; } }

        public EditControl(NodeFactory factory)
        {
            nodeFactory = factory;
            nodes = new NodeContainer(nodeFactory);

            InitializeComponent();

            OnRender += new RenderEventHandler(DoRender);
            MouseDown += new MouseEventHandler(DoMouseDown);
            MouseUp += new MouseEventHandler(DoMouseUp);
            MouseMove += new MouseEventHandler(DoMouseMove);
            KeyDown += new KeyEventHandler(DoKeyDown);
            DragEnter += new DragEventHandler(EditControl_DragEnter);
            DragDrop += new DragEventHandler(DoDragDrop);
            MouseDoubleClick += new MouseEventHandler(EditControl_MouseDoubleClick);
            MouseWheel += DoMouseWheel;
            AllowDrop = true;
        }

        private void DoMouseWheel(object sender, MouseEventArgs e)
        {
            Point mouseLocation = new Point(e.X, e.Y);

            Point pointerPos = new Point(e.X, e.Y);
            PointF zoomPoint = new PointF((pointerPos.X - renderContext.Offset.X) / renderContext.Zoom, (pointerPos.Y - renderContext.Offset.Y) / renderContext.Zoom);

            float zoomDirection = e.Delta > 1 ? 1.0f : -1.0f;
            float zoomDelta = (renderContext.Zoom * 0.1f) * zoomDirection;
            renderContext.Offset = new PointF(renderContext.Offset.X - (zoomPoint.X * zoomDelta), renderContext.Offset.Y - (zoomPoint.Y * zoomDelta));
            renderContext.Zoom += zoomDelta;

            Render();
        }

        public void EditControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point p = renderContext.ScreenToPoint(new Point(e.X, e.Y));
                Node node = nodes.GetNodeFromLocation(p);
                if (node != null)
                {
                    PropertyBundle oldBundle = new PropertyBundle();
                    node.Save(oldBundle);
                    if (node.ShowDialog())
                    {
                        PropertyBundle newBundle = new PropertyBundle();
                        node.Save(newBundle);
                        // Add undo item (old, new)
                        undoStack.Push(new ChangePropertiesOperation(node, oldBundle, newBundle));
                    }
                    else
                    {
                        // Settings where not saved, restore old bundle
                        node.Load(oldBundle);
                    }
                }
                nodes.UpdatePreviewRendering();
            }
        }

        internal void Undo()
        {
            undoStack.Undo(nodes);
            nodes.UpdatePreviewRendering();
            Render();
        }

        internal void Redo()
        {
            undoStack.Redo(nodes);
            nodes.UpdatePreviewRendering();
            Render();
        }

        internal NodeContainer GetNodeContainer()
        {
            return nodes;
        }

        private void DoRender(object sender, Graphics graphics)
        {
            DoPaint(sender, graphics);
        }

        private void DoKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                NodeIO io = nodes.GetIOPoint(lastMousePosition);
                if (io != null)
                {
                    nodes.Unbind(io);
                    e.Handled = true;
                }
                else
                {
                    Node node = nodes.GetNodeFromLocation(lastMousePosition);
                    if (node != null)
                    {
                        undoStack.Push(new RemoveNodeOperation(node));
                        nodes.Remove(node);
                        e.Handled = true;
                    }
                }
                Render();
            }
        }

        private void DoDragDrop(object sender, DragEventArgs e)
        {
            string text = (string)e.Data.GetData(DataFormats.Text);
            Node node = nodeFactory.CreateNodeFromName(text);
            Rectangle r = Utils.RectFToRect(node.Rectangle);
            Point clientPoint = PointToClient(new Point(e.X, e.Y));
            Point p = renderContext.ScreenToPoint(clientPoint);
            r.X = p.X;
            r.Y = p.Y;
            node.Rectangle = r;
            nodes.Add(node);
            undoStack.Push(new AddNodeOperation(node));
            Render();
        }

        private void EditControl_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void DoMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragLock = true;
                Point position = new Point(e.X, e.Y);
                startPos = position;

                position.X -= (int)renderContext.Offset.X;
                position.Y -= (int)renderContext.Offset.Y;
                dragPos = position;

                Point p = renderContext.ScreenToPoint(new Point(e.X, e.Y));

                outputLine = nodes.GetIOPoint(p);
                if (outputLine == null)
                {
                    dragNode = nodes.GetNodeFromLocation(p);
                    if (dragNode == null)
                    {
                        dragPos.X += (int)renderContext.Offset.X;
                        dragPos.Y += (int)renderContext.Offset.Y;
                    }
                }
            }
        }

        private void DoMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point position = renderContext.ScreenToPoint(new Point(e.X, e.Y));

                if (outputLine != null)
                {
                    NodeIO other = nodes.GetIOPoint(position);
                    if ((other != null) && (outputLine.GetParent() != other.GetParent()))
                    {
                        NodeInput input = null;
                        NodeOutput output = null;
                        // Bind connector
                        if ((other is NodeInput) && (outputLine is NodeOutput))
                        {
                            input = other as NodeInput;
                            output = outputLine as NodeOutput;
                        }
                        else if ((outputLine is NodeInput) && (other is NodeOutput))
                        {
                            input = outputLine as NodeInput;
                            output = other as NodeOutput;
                        }
                        
                        if ((input != null) && (output != null))
                        {
                            BindOperation bindOp = new BindOperation(input);
                            ((NodeInput)input).SetOutputChannel(output as NodeOutput);
                            bindOp.SetNewBinding(input);
                            undoStack.Push(bindOp);
                        }
                    }
                    else
                    {
                        // Unbind connector
                        if (outputLine is NodeInput)
                        {
                            BindOperation bindOp = new BindOperation((NodeInput)outputLine);
                            ((NodeInput)outputLine).SetOutputChannel(null);
                            bindOp.SetNewBinding((NodeInput)outputLine);
                            undoStack.Push(bindOp);
                        }
                        else
                        {
                            if (outputLine is NodeOutput)
                            {
                                nodes.Unbind(outputLine);
                            }
                        }
                    }
                    nodes.UpdatePreviewRendering();
                }
                else
                {
                    // Move operation done
                    if (dragNode != null)
                    {
                        undoStack.Push(new MoveOperation(startPos, lastMousePosition, dragNode));
                    }
                }


                dragLock = false;
                dragNode = null;
                outputLine = null;

                Render();
            }
        }

        private void DoMouseMove(object sender, MouseEventArgs e)
        {
            lastMousePosition.X = e.X - (int)renderContext.Offset.X;
            lastMousePosition.Y = e.Y - (int)renderContext.Offset.Y;
            if (dragLock)
            {
                Point newPos = new Point(e.X - (int)renderContext.Offset.X, e.Y - (int)renderContext.Offset.Y);
                if (dragNode != null)
                {
                    RectangleF r = dragNode.Rectangle;
                    r.X += (newPos.X - dragPos.X) / renderContext.Zoom;
                    r.Y += (newPos.Y - dragPos.Y) / renderContext.Zoom;
                    dragNode.Rectangle = r;
                }
                else if (outputLine != null)
                {
                    // Do nothing, DoPaint will draw a line
                }
                else
                {
                    newPos = new Point(e.X, e.Y);
                    // Move screen
                    Point delta = new Point(newPos.X - dragPos.X, newPos.Y - dragPos.Y);
                    renderContext.Offset = new PointF(renderContext.Offset.X + delta.X, renderContext.Offset.Y + delta.Y);
                }
                dragPos = newPos;

                Render();
            }
        }

        private void DoPaint(object sender, Graphics graphics)
        {
            renderContext.SetGraphics(graphics);
            ClearScreen(graphics);

            nodes.Draw(renderContext);
            if (outputLine != null)
            {
                Point p = Utils.GetRectCenterPoint(outputLine.visualRect);
                Point start = renderContext.PointToScreen(p);
                graphics.DrawLine(new Pen(Colors.DragPreviewLine, 3), start.X, start.Y, dragPos.X + renderContext.Offset.X, dragPos.Y + renderContext.Offset.Y);
            }
        }

        private void ClearScreen(Graphics g)
        {
            Brush b = new SolidBrush(Colors.Background);
            g.FillRectangle(b, new Rectangle(0, 0, Width, Height));
        }

        private Point TranslateMouse(int x, int y)
        {
            return new Point(x - (int)renderContext.Offset.X, y - (int)renderContext.Offset.Y);
        }
    }
}
