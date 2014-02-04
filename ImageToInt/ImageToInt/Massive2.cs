using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ImageToInt
{
    class Massive2
    {
        Bitmap Image;
        int Height, Width;
        int[,] mas;
        string path;
        public Massive2(string s)
        {
            path = s;
            Image = new Bitmap(s);
            Height = Image.Height;
            Width = Image.Width;
            mas = new int[Width, Height];
            Color C;
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++ )
                {
                    C = Image.GetPixel(i, j);
                    mas[i, j] = (C.R * 16 * 16 + C.G) * 16 * 16 + C.B;
                }
        }
        public int Pixel(int x, int y)
        {
            return mas[x, y];
        }
        public void Pixel(int x, int y, int color)
        {
            mas[x, y] = color;
        }
        public void Save(string s)
        {
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    Image.SetPixel(i, j, Color.FromArgb(255, Color.FromArgb(mas[i,j])));
            Image.Save(s);
        }
    }
}
