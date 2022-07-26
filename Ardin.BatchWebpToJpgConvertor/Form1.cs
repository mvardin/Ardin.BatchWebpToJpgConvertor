using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ardin.BatchWebpToJpgConvertor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {

        }

        private void listBox1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            Task.Run(() =>
            {
                string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
                listBox1.Invoke((MethodInvoker)delegate
                {
                    listBox1.Items.Insert(0, "Start for " + files.Length + " files");
                });
                if (files != null && files.Any())
                {
                    foreach (var file in files)
                    {
                        if (Path.GetExtension(file) != ".webp")
                        {
                            listBox1.Invoke((MethodInvoker)delegate
                            {
                                listBox1.Items.Insert(0, Path.GetFileName(file) + " is not webp");
                            });
                            continue;
                        }

                        string output = file.Replace(".webp", ".jpg");
                        byte[] rawWebP = File.ReadAllBytes(file);
                        using (WebP webp = new WebP())
                        {
                            var bitmap = webp.Decode(rawWebP);
                            bitmap.Save(output, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                        listBox1.Invoke((MethodInvoker)delegate
                        {
                            listBox1.Items.Insert(0, Path.GetFileNameWithoutExtension(file) + " converted");
                        });
                    }
                    listBox1.Invoke((MethodInvoker)delegate
                    {
                        listBox1.Items.Insert(0, "Finish");
                    });
                }
            });
        }
    }
}
