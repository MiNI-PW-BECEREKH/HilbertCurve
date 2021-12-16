using HilbertCurve;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace HilberCurve
{
    public partial class MainWindow : Form
    {

        Bitmap image = null;
         Bitmap pictureBoxAboveLayer =  new Bitmap(700,700);
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            
        }



        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "files (*.bmp;*.png;*.jpg)|*.bmp;*.png;*.jpg";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    //PictureBox PictureBox1 = new PictureBox();

                    // Create a new Bitmap object from the picture file on disk,
                    // and assign that to the PictureBox.Image property
                    image = helper.fillPictureBox(pictureBox1, new Bitmap(dlg.FileName));

                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            pictureBoxAboveLayer = new Bitmap(pictureBox1.Width,pictureBox1.Height);
            using (Graphics g = pictureBox1.CreateGraphics())
            {
                var gg = Graphics.FromImage(pictureBoxAboveLayer);
                gg.Clear(Color.Black);
                g.Clear(Color.Black);
                //image = new Bitmap(image, pictureBox1.ClientSize);
                Pen pen = new System.Drawing.Pen(Color.Red);
                pen.Width = 1;

                var N = helper.Pow(2, trackBar1.Value);

                int[] prev = { 0, 0 };
                int[] curr;

                var blockSize = pictureBox1.Width / N;
                var offset = blockSize / 2;



                for (var i = 0; i < N * N; i++)
                {
                    curr = helper.hindex2xy(i, N);

                    if (image != null)
                    {
                        var pixel =  image.GetPixel(curr[0] * blockSize + offset, curr[1] * blockSize + offset);
                        pen.Color = Color.FromArgb(pixel.R, pixel.G, pixel.B);
                    }
                    else
                        pen.Color = helper.HsvToRgb(i * 360 / (N * N), 1, 1);


                    //g.DrawEllipse(pen, curr[0] * blockSize + offset - 2, curr[1] * blockSize + offset - 2, 2, 2);
                    g.DrawLine(pen, prev[0] * blockSize + offset, prev[1] * blockSize + offset, curr[0] * blockSize + offset, curr[1] * blockSize + offset);

                    //gg.DrawEllipse(pen, curr[0] * blockSize + offset - 2, curr[1] * blockSize + offset - 2, 2, 2);
                    gg.DrawLine(pen, prev[0] * blockSize + offset, prev[1] * blockSize + offset, curr[0] * blockSize + offset, curr[1] * blockSize + offset);

                    prev = curr;
                    
                }

            }

            System.GC.Collect();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            using(SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Title = "Save Image";
                dlg.Filter = "files (*.bmp;*.png;*.jpg)|*.bmp;*.png;*.jpg";

                if(dlg.ShowDialog() == DialogResult.OK)
                {
                    pictureBoxAboveLayer.Save(dlg.FileName);
                }

            }
        }
    }
}
