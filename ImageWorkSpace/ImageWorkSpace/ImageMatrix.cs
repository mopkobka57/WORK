using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ImageWorkSpace
{
    class ImageMatrix
    {
        private Bitmap Image, Gist;
        private int[,] Img;
        private double[,] Bright, Dx, Dy, Value, Direction;
        private int Height, Width;
        public ImageMatrix(string s)
        {
            Image = new Bitmap(s);
            Height = Image.Width;
            Width = Image.Height;
            Img = new int[Height, Width];
            Bright = new double[Height, Width];
            Color C;
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                {
                    C = Image.GetPixel(j, i);
                    Img[i, j] = (C.R * 16 * 16 + C.G) * 16 * 16 + C.B;
                    Bright[i, j] = (C.R + C.G + C.B) / 3;
                    //Bright[i, j] = C.GetBrightness();
                }
        }
        private void SetDx()
        {
            Dx = new double[Height, Width];
            for (int i = 0; i < Height; i++)
                for (int j = 1; j < Width - 1; j++)
                    Dx[i, j] = Bright[i, j - 1] + Bright[i, j + 1];
            for (int i = 0; i < Height; i++)
            {
                Dx[i, 0] = Bright[i, 0] - Bright[i, 1];
                Dx[i, Width-1] = Bright[i, Width-1] - Bright[i, Width - 2];
            }
        }
        private void SetDy()
        {
            Dy = new double[Height, Width];
            for (int i = 1; i < Height - 1; i++)
                for (int j = 0; j < Width; j++)
                    Dy[i, j] = Bright[i - 1, j] - Bright[i + 1, j];
            for (int j = 0; j < Width; j++)
            {
                Dy[0, j] = Bright[0, j] - Bright[1, j];
                Dy[Height - 1, j] = Bright[Height - 1, j] - Bright[Height - 2, j];
            }
        }
        private void SetDxDy()
        {
            SetDx();
            SetDy();
        }
        private void Gradien()
        {
            Value = new double[Height, Width];
            Direction = new double[Height, Width];
            for (int i=0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                {
                    Value[i, j] = Math.Sqrt(Dx[i, j] * Dx[i, j] + Dy[i, j] * Dy[i, j]);
                    Direction[i, j] = Math.Atan(Dy[i, j] / Dx[i, j]);
                }
        }
        public Bitmap Gistogram()
        {
            Bitmap Gis = new Bitmap(256, 256);
            double[] Brightness = new double[256];
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    Brightness[(int) Math.Round(Bright[i, j])]++;
            double div = Height * Width / 10000;
            for (int i = 0; i < 256; i++)
                Brightness[i] /= div;
            for (int i = 0; i < 256; i++)
                for (int j = 0; j < ((Brightness[i]<=256) ? Math.Round(Brightness[i]) : 256); j++)
                    Gis.SetPixel(255-i, 255-j, Color.Black);
            return Gis;
        }
        public Bitmap Shadows()
        {
            SetDxDy();
            Bitmap Shadow = new Bitmap(Height, Width);
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                {
                    int col = Math.Abs((int)Math.Round((Dx[i, j] + Dy[i, j])) / 2) >> 1;
                    Shadow.SetPixel(j, i, Color.FromArgb(col,col,col));
                }
            return Shadow;
        }
    }
}
