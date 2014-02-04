using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ImageToInt
{
    class Massive1
    {
        Bitmap Image;
        string path;
        int[] mas;
        int Height, Width, Length;
        public Massive1(string s)
        {
            path = s;
            Image = new Bitmap(s);
            Height = Image.Height;
            Width = Image.Width;
            Length = Height * Width;
            mas = new int[Length];
            Color C;
            for (int i = 0; i < Length; i++)
            {
                C = Image.GetPixel(i / Height, i % Height);
                mas[i] = (C.R * 16 * 16 + C.G) * 16 * 16 + C.B;
            }
        }
        public int Pixel(int x, int y)
        {
            return mas[x * Height + y];
        }
        public void Pixel(int x, int y, int color)
        {
            mas[x * Height + y] = color;
        }
        public void Save(string s)
        {
            for (int i = 0; i < Length; i++)
            {
                Image.SetPixel(i / Height, i % Height, Color.FromArgb(255, Color.FromArgb(mas[i])));
            }
            Image.Save(s);
        }
    }
}
