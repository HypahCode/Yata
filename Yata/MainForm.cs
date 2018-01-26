using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Yata.CoreNode;

namespace Yata
{
    public partial class MainForm : Form
    {
        private NodeFactory factory = new NodeFactory();
        private EditControl activeEditForm = null;

        public class NodeTreeViewItem : TreeNode
        {
            public NodeFactory.NodeInfo nodeInfo;
            public NodeTreeViewItem(NodeFactory.NodeInfo info)
                :base()
            {
                nodeInfo = info;
                Text = info.friendlyName;
            }
        }



        public MainForm()
        {
            InitializeComponent();
            SetColorSceme();

            string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            string[] files = Directory.GetFiles(appPath, "*.dll");
            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    factory.Load(Assembly.LoadFile(files[i]));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "ERROR", MessageBoxButtons.OK);
                }
            }
            treeView1.Dock = DockStyle.Fill;
            splitContainer1.Dock = DockStyle.Fill;
            AddNodesToListView();

            treeView1.ItemDrag += new ItemDragEventHandler(ItemDrag);

            SetActiveEditForm(new EditControl(factory));
        }

        private void SetActiveEditForm(EditControl editForm)
        {
            if (activeEditForm != null)
            {
                activeEditForm.Visible = false;
            }
            activeEditForm = editForm;
            activeEditForm.Parent = splitContainer1.Panel2;
            activeEditForm.Visible = true;
            activeEditForm.Dock = DockStyle.Fill;
        }

        private void ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Item is NodeTreeViewItem item)
            {
                treeView1.DoDragDrop(item.nodeInfo.className, DragDropEffects.Copy);
            }
        }


        private void AddNodesToListView()
        {
            treeView1.BeginUpdate();
            for (int i = 0; i < factory.nodes.Count; i++)
            {
                if (factory.nodes[i].subMenuPath != "")
                {
                    AddNode(factory.nodes[i]);
                }
            }
            treeView1.EndUpdate();
        }

        private void AddNode(NodeFactory.NodeInfo nodeInfo)
        {
            TreeNode item = new NodeTreeViewItem(nodeInfo);

            if (nodeInfo.image != null)
            {
                treeView1.ImageList.Images.Add(nodeInfo.image);
                item.ImageIndex = treeView1.ImageList.Images.Count - 1;
                item.SelectedImageIndex = item.ImageIndex;
            }

            // Find parent
            string[] splitStr = nodeInfo.subMenuPath.Split('.');
            AddTreeViewNode(splitStr, 0, treeView1.Nodes, item);
        }

        private void AddTreeViewNode(string[] path, int depth, TreeNodeCollection parent, TreeNode newNode)
        {
            if (depth == path.Length)
            {
                parent.Add(newNode);
                return;
            }

            for (int i = 0; i < parent.Count; i++)
            {
                if (parent[i].Text == path[depth])
                {
                    AddTreeViewNode(path, depth + 1, parent[i].Nodes, newNode);
                    return;
                }
            }

            // Could not find path, so add path node
            TreeNode pathFolder = new TreeNode(path[depth])
            {
                ImageIndex = 1,
                SelectedImageIndex = 1
            };
            parent.Add(pathFolder);
            AddTreeViewNode(path, depth + 1, pathFolder.Nodes, newNode);
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog od = new OpenFileDialog
            {
                Filter = BuildFileFilter(),
                Title = "Load a YATA file"
            };
            if (od.ShowDialog() == DialogResult.OK)
            {
                string filename = od.FileName;
                if (filename != "")
                {
                    EditControl edit = new EditControl(factory);

                    if (filename.ToLower().EndsWith(".yata"))
                    {
                        // Yata-file
                        YataFileLoader.LoadfromFile(filename, edit.GetNodeContainer(), factory);
                    }
                    else
                    {
                        // Other file...
                        for (int i = 0; i < factory.fileExtensions.Count; i++)
                        {
                            if (filename.ToLower().EndsWith( factory.fileExtensions[i].ext.ToLower() ))
                            {
                                factory.fileExtensions[i].plugin.LoadFile(filename, edit.GetNodeContainer(), factory);
                                break;
                            }
                        }
                    }

                    SetActiveEditForm(edit);
                    edit.FileName = filename;
                    edit.GetNodeContainer().UpdatePreviewRendering();
                }
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sd = new SaveFileDialog()
            {
                Filter = "Yata|*.yata",
                Title = "Save a YATA file"
            };
            if (sd.ShowDialog() == DialogResult.OK)
            {
                string filename = sd.FileName;
                if (filename != "")
                {
                    if (!filename.ToLower().EndsWith(".yata"))
                    {
                        filename = filename + ".Yata";
                    }
                    YataFileLoader.SaveToFile(filename, activeEditForm.GetNodeContainer());
                    activeEditForm.FileName = filename;
                }
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (activeEditForm.FileName == "")
            {
                SaveAsToolStripMenuItem_Click(sender, e);
            }
            else
            {
                YataFileLoader.SaveToFile(activeEditForm.FileName, activeEditForm.GetNodeContainer());
            }
        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            activeEditForm.Undo();
        }

        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            activeEditForm.Redo();
        }

        private string BuildFileFilter()
        {
            string filter = "Yata|*.yata|";
            for (int i = 0; i < factory.fileExtensions.Count; i++)
            {
                filter += factory.fileExtensions[i].descr + "|*" + factory.fileExtensions[i].ext + "|";
            }
            filter += "All files|*.*";

            return filter;
        }

        private void SetColorSceme()
        {
            treeView1.BorderStyle = BorderStyle.None;
            treeView1.BackColor = Colors.WindowBackground;
            treeView1.ForeColor = Colors.WindowFont;
            treeView1.LineColor = Colors.ConnectorLines;

            menuStrip1.Renderer = new ToolStripProfessionalRenderer(new MenuStripColorTable());

            RecursiveColor(menuStrip1.Items);
        }

        private void RecursiveColor(ToolStripItemCollection items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].ForeColor = Colors.WindowFont;
                if (items[i] is ToolStripMenuItem item)
                {
                    RecursiveColor(item.DropDownItems);
                }
            }
        }
    }
}
