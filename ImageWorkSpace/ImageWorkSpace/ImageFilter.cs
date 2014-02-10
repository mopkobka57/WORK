using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Drawing.Imaging;

namespace ImageWorkSpace
{
    class ImageFilter
    {
        private string path="";
        private const int MedN = 3, MedN2 = MedN * MedN, KerN = 3, KerN2 = KerN * KerN, BLUE = 255, GREEN = 256 * BLUE, RED = 256 * GREEN;
        public Bitmap Image;
        private int[,] img;
        private double[,] kernel;
        private int Height, Width, offset;
        public double div;
        public ImageFilter(string s)
        {
            path = s;
            Image = new Bitmap(s);
            Height = Image.Width;
            Width = Image.Height;
            img = new int[Height + 2, Width + 2];
            Color C;
            for (int i = 1; i < Height + 1; i++)
                for (int j = 1; j < Width + 1; j++)
                {
                    C = Image.GetPixel(j - 1, i - 1);
                    img[i, j] = (C.R * 16 * 16 + C.G) * 16 * 16 + C.B;
                }
            img[0, 0] = img[1, 1];
            img[0, Width + 1] = img[1, Width];
            img[Height + 1, 0] = img[Height, 1];
            img[Height + 1, Width + 1] = img[Height, Width];
            for (int i = 1; i < Width + 1; i++)
            {
                img[0, i] = img[1, i];
                img[Height + 1, i] = img[Height, i];
            }
            for (int i = 1; i < Height + 1; i++)
            {
                img[i, 0] = img[i, 1];
                img[i, Width + 1] = img[i, Width];
            }
            kernel = new double[KerN, KerN];
        }
        public void Blur()
        {
            kernel[0, 0] = 1;
            kernel[0, 1] = 1;
            kernel[0, 2] = 1;
            kernel[1, 0] = 1;
            kernel[1, 1] = 1;
            kernel[1, 2] = 1;
            kernel[2, 0] = 1;
            kernel[2, 1] = 1;
            kernel[2, 2] = 1;
            Image = Convolution.Apply(this.Image, kernel);
            return;
            Div();
            Multiple();
        }
        public void Negative()
        {
            kernel[0, 0] = 0;
            kernel[1, 0] = 0;
            kernel[2, 0] = 0;
            kernel[0, 1] = 0;
            kernel[1, 1] = -1;
            kernel[2, 1] = 0;
            kernel[0, 2] = 0;
            kernel[1, 2] = 0;
            kernel[2, 2] = 0;
            Image=Convolution.Apply(this.Image, kernel);
            return;
            Div();
            Multiple();
        }
        public void Clarity()
        {
            kernel[0, 0] = -1;
            kernel[1, 0] = -1;
            kernel[2, 0] = -1;
            kernel[0, 1] = -1;
            kernel[1, 1] = 9;
            kernel[2, 1] = -1;
            kernel[0, 2] = -1;
            kernel[1, 2] = -1;
            kernel[2, 2] = -1;
            Image = Convolution.Apply(this.Image, kernel);
            return;
            Div();
            Multiple();
        }
        private void Div()
        {
            div = 0;
            offset = 0;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    //div += ((kernel[i, j]>0) ? kernel[i,j] : -kernel[i,j]);
                    div += kernel[i, j];
                    if (kernel[i, j] < 0) offset = 255;
                }
            if (div < 0)
            {
                div = -div;
                //offset = 255;
            }
            if (div == 0) div = 1;
            //div = -1;
            //div = 3;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    kernel[i, j] /= div;
            /*offset = 0;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    div += kernel[i, j];
                    if (kernel[i, j] < 0) offset = 255;
                }*/
        }
        private void Multiple()
        {
            for (int i = 1; i < Height + 1; i++)
                for (int j = 1; j < Width + 1; j++)
                {
                    double rdf = 0, grf = 0, blf = 0;
                    for (int a = 0; a < 3; a++)
                        for (int b = 0; b < 3; b++)
                        {
                            rdf += ((img[i - 1 + a, j - 1 + b] & RED)>>16) * kernel[a, b];
                            grf += ((img[i - 1 + a, j - 1 + b] & GREEN)>>8) * kernel[a, b];
                            blf += (img[i - 1 + a, j - 1 + b] & BLUE) * kernel[a, b];
                        }
                    int rd = (int) Math.Round(rdf), gr = (int) Math.Round(grf), bl = (int) Math.Round(blf);
                    if (rd <= 0)
                        rd += offset;
                    if (gr <= 0)
                        gr += offset;
                    if (bl <= 0)
                        bl += offset;
                    /*if (rd > 255) rd = 255;
                    if (gr > 255) gr = 255;
                    if (bl > 255) bl = 255;*/
                    rd <<= 16;
                    gr <<= 8;
                    /*rd &= RED;
                    gr &= GREEN;
                    bl &= BLUE;*/
                    img[i, j] = (rd + gr + bl);
                }
            for (int i = 1; i < Height + 1; i++)
                for (int j = 1; j < Width + 1; j++)
                {
                    Image.SetPixel(j - 1, i - 1, Color.FromArgb(img[i, j]));
                }

        }






        /*private void Multiple()
        {
            for (int i = 1; i < Height + 1; i++)
                for (int j = 1; j < Width + 1; j++)
                {
                    float rdf = 0, grf = 0, blf = 0;
                    for (int a = 0; a < 3; a++)
                        for (int b = 0; b < 3; b++)
                        {
                            rdf += ((img[i - 1 + a, j - 1 + b] & RED)>>16) * kernel[a, b];
                            grf += ((img[i - 1 + a, j - 1 + b] & GREEN)>>8) * kernel[a, b];
                            blf += (img[i - 1 + a, j - 1 + b] & BLUE) * kernel[a, b];
                        }
                    int rd = Round(rdf), gr = Round(grf), bl = Round(blf);
                    if (rd <= 0) 
                        rd += 255;
                    if (gr <= 0) 
                        gr += 255;
                    if (bl <= 0) 
                        bl += BLUE;
                    rd <<= 16;
                    gr <<= 8;
                    rd &= RED;
                    gr &= GREEN;
                    bl &= BLUE;
                    img[i, j] = (rd + gr + bl);
                }
            for (int i = 1; i < Height + 1; i++)
                for (int j = 1; j < Width + 1; j++)
                {
                    Image.SetPixel(j - 1, i - 1, Color.FromArgb(img[i, j]));
                }

        }*/
        private int Round(float a)
        {
            return (int) Math.Round(a);
        }
        public void Median()
        {
            int[] mas = new int[MedN2];
            for (int i = 1; i < Height + 1; i++)
                for (int j = 1; j < Width + 1; j++)
                {
                    for (int k = 0; k < MedN2; k++)
                        mas[k] = img[i + k / MedN - 1, j + k % MedN - 1];

                    Array.Sort(mas);
                    img[i, j] = mas[MedN2 / 2];
                }
            for (int i = 1; i < Width + 1; i++)
                for (int j = 1; j < Height + 1; j++)
                {
                    Image.SetPixel(j - 1, i - 1, Color.FromArgb(img[i, j]));
                }
        }
        public void BinarBorders()
        {
            int[, ,] mas = new int[24, Height + 2, Width + 2];
        }
        public void Save(string s)
        {
            try
            {
                Image.Save(s);
            }
            catch
            {
                MessageBox.Show("Выберите другое имя");
            }
        }
    }
    public class BitmapBytes
    {
        public static byte[] GetBytes(Bitmap Image)
        {
            int height = Image.Height, width = Image.Width;
            int length = height * width * 3;
            byte[] mas = new byte[length];
            for (int i = 0; i < length; i += 3)
            {
                Color C = Image.GetPixel((i / 3) / height, (i % height) / 3);
                mas[i] = C.R;
                mas[i + 1] = C.G;
                mas[i + 2] = C.B;
            }
            return mas;
        }
        public static Bitmap GetBitmap(byte[] mas, int width, int height)
        {
            Bitmap Image = new Bitmap(width, height);
            int length = 3 * height * width;
            for (int i = 0; i < length; i += 3)
            {
                Color C = Color.FromArgb(mas[i], mas[i + 1], mas[i + 2]);
                Image.SetPixel((i / 3) / height, (i % width) / 3, C);
            }
            return Image;
        }
    }
    public static class Convolution
    {
        public static Bitmap Apply(Bitmap input, double[,] kernel)
        {
            //Получаем байты изображения
            byte[] inputBytes = BitmapBytes.GetBytes(input);
            byte[] outputBytes = new byte[inputBytes.Length];

            int width = input.Width;
            int height = input.Height;
            int kernelWidth = kernel.GetLength(0);
            int kernelHeight = kernel.GetLength(1);

            //Производим вычисления
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    double rSum = 0, gSum = 0, bSum = 0, kSum = 0;

                    for (int i = 0; i < kernelWidth; i++)
                    {
                        for (int j = 0; j < kernelHeight; j++)
                        {
                            int pixelPosX = x + (i - (kernelWidth / 2));
                            int pixelPosY = y + (j - (kernelHeight / 2));
                            if ((pixelPosX < 0) ||
                              (pixelPosX >= width) ||
                              (pixelPosY < 0) ||
                              (pixelPosY >= height)) continue;

                            byte r = inputBytes[3 * (width * pixelPosY + pixelPosX) + 0];
                            byte g = inputBytes[3 * (width * pixelPosY + pixelPosX) + 1];
                            byte b = inputBytes[3 * (width * pixelPosY + pixelPosX) + 2];

                            double kernelVal = kernel[i, j];

                            rSum += r * kernelVal;
                            gSum += g * kernelVal;
                            bSum += b * kernelVal;

                            kSum += kernelVal;
                        }
                    }

                    if (kSum <= 0) kSum = 1;

                    //Контролируем переполнения переменных
                    rSum /= kSum;
                    if (rSum < 0) rSum = 0;
                    if (rSum > 255) rSum = 255;

                    gSum /= kSum;
                    if (gSum < 0) gSum = 0;
                    if (gSum > 255) gSum = 255;

                    bSum /= kSum;
                    if (bSum < 0) bSum = 0;
                    if (bSum > 255) bSum = 255;

                    //Записываем значения в результирующее изображение
                    outputBytes[3 * (width * y + x) + 0] = (byte)rSum;
                    outputBytes[3 * (width * y + x) + 1] = (byte)gSum;
                    outputBytes[3 * (width * y + x) + 2] = (byte)bSum;
                }
            }
            //Возвращаем отфильтрованное изображение
            return BitmapBytes.GetBitmap(outputBytes, width, height);
        }
    }
}

