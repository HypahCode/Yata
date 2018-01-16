using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Yata.CoreNode;

namespace Yata.ImageNodes.Nodes.Generators
{
    public class ImageBatchLoader : ImageNodeBase
    {
        private class ImageBatchFileOutput : ImageBatchOutput
        {
            private ImageBatchLoader loader;
            public ImageBatchFileOutput(Node parent, string name)
                :base(parent, name)
            {
                loader = parent as ImageBatchLoader;
            }

            public override int GetImageCount() { return loader.GetImageCount(); }
            public override bool LoadImage(int index) { return loader.LoadImage(index); }
            public override string GetImageFileName(int index) { return loader.GetImageFileName(index); }
            public override Size GetImageSize() { return loader.GetImageSize(); }
        }


        private string directory = "";
        private Bitmap activeImage = null;
        private string[] files;

        public ImageBatchLoader()
            : base(FriendlyName)
        {
            outputs.Add(new FloatColorOutput(this, "Color"));
            outputs.Add(new ImageBatchFileOutput(this, "File array"));

            Init();
        }

        public override FloatColor GetPixel(float x, float y, bool preview)
        {
            if (activeImage != null)
            {
                Color c;
                lock (activeImage)
                {
                    int xx = (int)(x * (float)activeImage.Width) % activeImage.Width;
                    int yy = (int)(y * (float)activeImage.Height) % activeImage.Height;
                    if (xx < 0.0f) xx += activeImage.Width;
                    if (yy < 0.0f) yy += activeImage.Height;
                    c = activeImage.GetPixel(xx, yy);
                }
                return new FloatColor(c);
            }
            else
            {
                return base.GetPixel(x, y, preview);
            }
        }

        public int GetImageCount()
        {
            files = Directory.GetFiles(directory);
            return files.Length;
        }
        
        public bool LoadImage(int index)
        {
            string filename = files[index];
            if (File.Exists(filename))
            {
                activeImage = new Bitmap(filename);
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetImageFileName(int index)
        {
            return files[index];
        }

        public Size GetImageSize()
        {
            if (activeImage == null)
            {
                return new Size(0, 0);
            }
            return new Size(activeImage.Width, activeImage.Height);
        }

        public override bool ShowPropertiesDialog()
        {
            base.ShowPropertiesDialog();
            PropertiesForm form = new PropertiesForm(FriendlyName, this);
            Button browseButton;
            TextBox textBox = form.AddEditWithButtonControl("Input directory", directory, "Browse", out browseButton);
            textBox.TextChanged += new System.EventHandler(DirectoryTextChanged);
            browseButton.Click += delegate(object sender, EventArgs args) 
                {
                    FolderBrowserDialog  dialog = new FolderBrowserDialog ();
                    dialog.SelectedPath = directory;
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        textBox.Text = dialog.SelectedPath;
                    }
                };
            return (form.ShowDialog() == DialogResult.OK);
        }

        private void DirectoryTextChanged(object sender, EventArgs e)
        {
            directory = ((TextBox)sender).Text;
        }

        public override void Save(PropertyBundle bundle)
        {
            base.Save(bundle);
            //bundle.PutString("Dir", directory);
        }

        public override void Load(PropertyBundle bundle)
        {
            base.Load(bundle);
            //directory = bundle.GetString("Dir", "");
        }

        public static string FriendlyName
        {
            get { return "Batch loader"; }
        }

        public static string SubMenuPath
        {
            get { return "Image.Batch"; }
        }
    }
}
