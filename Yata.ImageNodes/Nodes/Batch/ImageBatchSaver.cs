using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Yata.CoreNode;

namespace Yata.ImageNodes.Nodes.Generators
{
    public class ImageBatchSaver : ImageNodeBase
    {
        private FloatColorInput colorInput;
        private ImageBatchInput fileArrayInput;

        private string outputDirectory = "";
        private bool preserveImageSize = false;
        private Size imageSize = new Size(200, 200);

        public ImageBatchSaver()
            : base(FriendlyName)
        {
            colorInput = AddInput("Color");
            fileArrayInput = new ImageBatchInput(this, "File array");
            inputs.Add(fileArrayInput);

            Init();
        }

        public override FloatColor GetPixel(float x, float y, bool preview)
        {
            return colorInput.GetPixel(x, y, preview);
        }

        public bool RenderEverything()
        {
            base.ShowPropertiesDialog();

            int count = fileArrayInput.GetImageCount();
            for (int i = 0; i < count; i++)
            {
                if (fileArrayInput.LoadImage(i))
                {
                    string file = fileArrayInput.GetImageFileName(i);
                    string fileName;
                    if (outputDirectory[outputDirectory.Length-1] == Path.DirectorySeparatorChar)
                    {
                        fileName = outputDirectory + Path.GetFileName(file);
                    }
                    else
                    {
                        fileName = outputDirectory + Path.DirectorySeparatorChar + Path.GetFileName(file);
                    }

                    Size size;
                    if (preserveImageSize)
                    {
                        size = fileArrayInput.GetImageSize();
                        if (size.Width == 0 || size.Height == 0)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        size = new Size(imageSize.Width, imageSize.Height);
                    }

                    Bitmap bitmap = new Bitmap(size.Width, size.Height);
                    PreRenderSetup(bitmap.Width, bitmap.Height);

                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        for (int y = 0; y < bitmap.Height; y++)
                        {
                            bitmap.SetPixel(x, y, GetPixel((float)x / (float)bitmap.Width, (float)y / (float)bitmap.Height, false).ToColor());
                        }
                    }
                    bitmap.Save(fileName + ".png", ImageFormat.Png);
                    bitmap.Dispose();
                }
            }
            
            return true;
        }

        public override bool ShowPropertiesDialog()
        {
            base.ShowPropertiesDialog();
            PropertiesForm form = new PropertiesForm(FriendlyName, this);
            Button browseButton;
            TextBox textBox = form.AddEditWithButtonControl("Input directory", outputDirectory, "Browse", out browseButton);
            textBox.TextChanged += new System.EventHandler(DirectoryTextChanged);
            browseButton.Click += delegate(object sender, EventArgs args)
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.SelectedPath = outputDirectory;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBox.Text = dialog.SelectedPath;
                }
            };
            //form.AddCheckbox("Preserve image size", preserveImageSize).CheckedChanged += delegate(object sender, EventArgs args) { preserveImageSize = ((CheckBox)sender).Checked; };
            //form.AddNumericIntControl("Width", imageSize.Width, 0, 1000000).ValueChanged += delegate(object sender, EventArgs args) { imageSize.Width = (int)((NumericUpDown)sender).Value; };
            //form.AddNumericIntControl("Height", imageSize.Height, 0, 1000000).ValueChanged += delegate(object sender, EventArgs args) { imageSize.Height = (int)((NumericUpDown)sender).Value; };
            //form.AddButton("Render", "Render everything!").Click += delegate(object sender, EventArgs args) { RenderEverything(); };
            return (form.ShowDialog() == DialogResult.OK);
        }

        private void DirectoryTextChanged(object sender, EventArgs e)
        {
            outputDirectory = ((TextBox)sender).Text;
        }

        public override void Save(PropertyBundle bundle)
        {
            base.Save(bundle);
            //bundle.PutString("Dir", outputDirectory);
            //bundle.PutBool("preserveImageSize", preserveImageSize);
            //bundle.PutInt("Width", imageSize.Width);
            //bundle.PutInt("Height", imageSize.Height);
        }

        public override void Load(PropertyBundle bundle)
        {
            base.Load(bundle);
            //outputDirectory = bundle.GetString("Dir", "");
            //preserveImageSize = bundle.GetBool("preserveImageSize", false);
            //imageSize.Width = bundle.GetInt("Width", 10);
            //imageSize.Height = bundle.GetInt("Height", 0);
        }

        public static string FriendlyName
        {
            get { return "Batch saver"; }
        }

        public static string SubMenuPath
        {
            get { return "Image.Batch"; }
        }
    }
}
