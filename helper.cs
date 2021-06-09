using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HilbertCurve
{

    public static class helper
    {
        public static int[] hindex2xy(int hindex, int N)
        {
            int[][] positions = new int[][] {
                /* 0: */new int[] { 0, 0},
                /* 1: */new int[] { 0, 1 },
                /* 2: */new int[] { 1, 1},
                /* 3: */new int[] { 1, 0 }
                            };

            var tmp = positions[Last2Bits(hindex)];
            hindex = (hindex >> 2);

            var x = tmp[0];
            var y = tmp[1];

            for (var n = 4; n <= N; n *= 2)
            {
                int tmp2;
                var n2 = n / 2;

                switch (Last2Bits(hindex))
                {
                    case 0: /* left-bottom */
                        tmp2 = x; x = y; y = tmp2;
                        break;

                    case 1: /* left-upper */
                        x = x;
                        y = y + n2;
                        break;

                    case 2: /* right-upper */
                        x = x + n2;
                        y = y + n2;
                        break;

                    case 3: /* right-bottom */
                        tmp2 = y;
                        y = (n2 - 1) - x;
                        x = (n2 - 1) - tmp2;
                        x = x + n2;
                        break;
                }

                hindex = (hindex >> 2);
            }

            return new int[] { x, y };
        }


        private static int Last2Bits(int x) { return x & 3; }



        public static int Pow(this int bas, int exp)
        {
            return Enumerable
                  .Repeat(bas, exp)
                  .Aggregate(1, (a, b) => a * b);
        }


        public static Color HsvToRgb(double h, double S, double V)
        {
            double H = h;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            double R, G, B;
            if (V <= 0)
            { R = G = B = 0; }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i)
                {

                    // Red is the dominant color

                    case 0:
                        R = V;
                        G = tv;
                        B = pv;
                        break;

                    // Green is the dominant color

                    case 1:
                        R = qv;
                        G = V;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = V;
                        B = tv;
                        break;

                    // Blue is the dominant color

                    case 3:
                        R = pv;
                        G = qv;
                        B = V;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = V;
                        break;

                    // Red is the dominant color

                    case 5:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                    case 6:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // The color is not defined, we should throw an error.

                    default:
                        //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                        R = G = B = V; // Just pretend its black/white
                        break;
                }
            }
           var r = Clamp((int)(R * 255.0));
           var g = Clamp((int)(G * 255.0));
           var b = Clamp((int)(B * 255.0));

            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// Clamp a value to 0-255
        /// </summary>
        static int  Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }



        static public Bitmap fillPictureBox(PictureBox pbox, Bitmap bmp)
        {
            pbox.SizeMode = PictureBoxSizeMode.Normal;
            bool source_is_wider = (float)bmp.Width / bmp.Height > (float)pbox.Width / pbox.Height;

            var resized = new Bitmap(pbox.Width, pbox.Height);
            var g = Graphics.FromImage(resized);
            var dest_rect = new Rectangle(0, 0, pbox.Width, pbox.Height);
            Rectangle src_rect;

            if (source_is_wider)
            {
                float size_ratio = (float)pbox.Height / bmp.Height;
                int sample_width = (int)(pbox.Width / size_ratio);
                src_rect = new Rectangle((bmp.Width - sample_width) / 2, 0, sample_width, bmp.Height);
            }
            else
            {
                float size_ratio = (float)pbox.Width / bmp.Width;
                int sample_height = (int)(pbox.Height / size_ratio);
                src_rect = new Rectangle(0, (bmp.Height - sample_height) / 2, bmp.Width, sample_height);
            }

            g.DrawImage(bmp, dest_rect, src_rect, GraphicsUnit.Pixel);
            g.Dispose();

            pbox.Image = resized;
            return resized;
        }

    }
}
