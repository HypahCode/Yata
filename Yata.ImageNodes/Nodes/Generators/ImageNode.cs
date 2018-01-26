using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Yata.CoreNode;
using Yata.CoreNode.DataTypes;

namespace Yata.ImageNodes.Nodes.Generators
{
    [NodeUsage(@"Image.Generator", nodeName)]
    public class ImageNode : ImageNodeBase
    {
        private const string nodeName = @"Image";

        [DataType("Image from disk")]
        private string imageFromDisk = "";
        private Bitmap image;
        
        public ImageNode()
            : base(nodeName)
        {
            outputs.Add(new FloatColorOutput(this, "Output"));
            Init();
        }

        protected override void Setup()
        {
            base.Setup();
            Load();
        }

        public override FloatColor GetPixel(float x, float y, bool preview)
        {
            if (image != null)
            {
                Color c;
                lock (image)
                {
                    int xx = (int)(x * (float)image.Width) % image.Width;
                    int yy = (int)(y * (float)image.Height) % image.Height;
                    if (xx < 0.0f) xx += image.Width;
                    if (yy < 0.0f) yy += image.Height;
                    c = image.GetPixel(xx, yy);
                }
                return new FloatColor(c);
            }
            else
            {
                return base.GetPixel(x, y, preview);
            }
        }

        private void Load()
        {
            if ((!String.IsNullOrEmpty(imageFromDisk)) && (File.Exists(imageFromDisk)))
            {
                image = new Bitmap(imageFromDisk);
            }
        }

        public override bool ShowPropertiesDialog()
        {
            base.ShowPropertiesDialog();

            PropertiesForm form = new PropertiesForm(nodeName, this);
            Button btn;
            TextBox textBox = form.AddEditWithButtonControl("image file", imageFromDisk, "browse", out btn);
            textBox.TextChanged += new EventHandler(FileTextChanged);
            btn.Click += delegate (object sender, EventArgs args) 
                {
                    OpenFileDialog od = new OpenFileDialog()
                    {
                        Filter = "Image files|*.jpg;*.png;*.bmp",
                        Title = "Load a image file"
                    };
                    if (imageFromDisk != "")
                    {
                        od.InitialDirectory = Path.GetPathRoot(imageFromDisk);
                    }
                    if (od.ShowDialog() == DialogResult.OK)
                    {
                        textBox.Text = od.FileName;
                    }
                };

            return (form.ShowDialog() == DialogResult.OK);
        }

        private void FileTextChanged(object sender, EventArgs e)
        {
            imageFromDisk = ((TextBox)sender).Text;
            Load();
        }
    }
}
