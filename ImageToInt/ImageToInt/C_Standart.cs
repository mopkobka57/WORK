using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ImageToInt
{
    class C_Standart
    {
        public Bitmap Image1;
        public C_Standart(string s)
        {
            this.Image1 = new Bitmap(s);
            /*Rectangle rect = new Rectangle(0, 0, Image1.Width, Image1.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                Image1.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                Image1.PixelFormat);
            Image1.UnlockBits(bmpData);*/
        }
        public int Pixel(int x, int y)
        {
            // return Convert.ToInt32(Image1.GetPixel(x, y));
            Color C = Image1.GetPixel(x, y);
            return (C.R * 16 * 16 + C.G) * 16 * 16 + C.B;
        }
        public void Pixel(int x, int y, int color)
        {
            Image1.SetPixel(x, y, Color.FromArgb(255, Color.FromArgb(color)));
        }
        public void Save(string s)
        {
            Image1.Save(s);
        }
    }
}
