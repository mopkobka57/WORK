using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ImageToInt
{
    class FromJPG
    {
        byte[] mas;
        int Height;
        Image img;
        public FromJPG(string s)
        {
            img = Image.FromFile(s);
            Height = img.Height;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                mas = ms.ToArray();
            }
        }
        public int Pixel(int x, int y)
        {
            return mas[x * Height + y];
        }
        public void Pixel(int x, int y, int color)
        {
            mas[x * Height + y] = (byte) color;
        }
        public void Save(string s)
        {
            using (Image image = Image.FromStream(new MemoryStream(mas)))
            {
                image.Save(s, ImageFormat.Jpeg);  // Or Png
            }
        }
    }
}
