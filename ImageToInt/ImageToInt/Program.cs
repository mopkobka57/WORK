using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageToInt
{
    class MainClass
    {
        static void Main()
        {
            Console.WriteLine("{0}", System.Environment.ProcessorCount);
            int i = 5;
            while (i != 0)
            {
                vivod("Введите номер метода\n");
                string s=Console.ReadLine();
                i = Convert.ToInt32(s);
                switch (i)
                {
                    case 1: Standart(); break;
                    case 2: Massiv1(); break;
                    case 3: Massiv2(); break;
                    case 4: Spisok(); break;
                    default: break;
                }
            }

        }
        static void Standart()
        {
            /*C_Standart Image= new C_Standart(@"C:\Image3.PNG");
            int i = 3;
            int x, y, color;
            while (i != 0)
            {
                vivod("1 - посмотреть значение, 2 - изменить значение\n");
                string s = Console.ReadLine();
                i = Convert.ToInt32(s);
                if (i == 0) break;
                vivod("Введите координаты через Enter\n");
                x = Convert.ToInt32(Console.ReadLine());
                y = Convert.ToInt32(Console.ReadLine());
                if (i == 1)
                {
                    vivod((string) int.Parse(Image.Pixel(x, y).ToString()).ToString("X")+"\n");
                }
                if (i == 2)
                {
                    vivod("Введите цвет в десятичном виде\n");
                    color = Convert.ToInt32(Console.ReadLine());
                    Image.Pixel(x, y, color);
                }
            }*/
            DateTime one = DateTime.Now;
            Random rnd = new Random();
            C_Standart Image = new C_Standart("C:\\Image.PNG");
            for (int i = 0; i < 10000; i++)
                for (int j = 0; j < 1000; j++)
                    Image.Pixel(i % 789, j % 729, rnd.Next(10000000));
            Image.Save("C:\\Image1.PNG");
            vivod(DateTime.Now - one);
            vivod("\n");
        }
        static void Massiv1()
        {
            DateTime one = DateTime.Now;
            Random rnd = new Random();
            Massive1 Image = new Massive1("C:\\Image.PNG");
            for (int i = 0; i < 100000; i++)
                for (int j = 0; j < 10000; j++)
                    Image.Pixel(i % 789, j % 729, rnd.Next(10000000));
            Image.Save("C:\\Image2.PNG");
            vivod(DateTime.Now - one);
            vivod("\n");
        }
        static void Massiv2()
        {
            DateTime one = DateTime.Now;
            Random rnd = new Random();
            Massive2 Image = new Massive2("C:\\Image.PNG");
            for (int i = 0; i < 100000; i++)
                for (int j = 0; j < 10000; j++)
                    Image.Pixel(i % 789, j % 729, rnd.Next(10000000));
            Image.Save("C:\\Image3.PNG");
            vivod(DateTime.Now - one);
            vivod("\n");
        }
        static void Spisok()
        {
        }
        static void PrintPixel(C_Standart I, int x, int y)
        {
            Console.WriteLine("{0}", int.Parse(I.Pixel(x, y).ToString()).ToString("X"));
        }
        static void vivod(object s)
        {
            Console.Write("{0}", s);
        }
    }
}
