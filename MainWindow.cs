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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            
        }




        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            using (Graphics g = pictureBox1.CreateGraphics())
            {
                g.Clear(Color.Black);

                Pen pen = new System.Drawing.Pen(Color.Red);
                pen.Width = 1;

                var N = helper.Pow(2, trackBar1.Value);

                int[] prev = { 0, 0 };
                int[] curr;

                var blockSize = 512 / N;
                var offset = blockSize / 2;



                for (var i = 0; i < N * N; i++)
                {
                    pen.Color = helper.HsvToRgb(i * 360 / (N * N), 1, 1);

                    curr = helper.hindex2xy(i, N);

                    g.DrawEllipse(pen, curr[0] * blockSize + offset -2, curr[1] * blockSize + offset -2, 4, 4);
                    g.DrawLine(pen, prev[0] * blockSize + offset, prev[1] * blockSize + offset, curr[0] * blockSize + offset, curr[1] * blockSize + offset);

                    prev = curr;
                }

            }

            System.GC.Collect();

        }
    }
}
