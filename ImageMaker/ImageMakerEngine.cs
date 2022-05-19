using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Drawing;


namespace ImageMaker
{
    class ImageMakerEngine
    {
        private string path;
        private static string mode;
        private int contrast;
        private int width;
        private int height;
        private IProgress<int> progress;
        public string image;
        
        public ImageMakerEngine(string spath, string smode, int scontrast, int swidth, int sheight, IProgress<int> sprogress)
        {
            path = spath;
            mode = smode;
            contrast = scontrast;
            width = swidth;
            height = sheight;
            progress = sprogress;
        }

        // Метод получает картикну в формате Bitmap и возвращает черно-белую картинку в виде массива
        private static byte[] ConvertToGrayImage(Bitmap img, int i)
        {
            byte[] mas = new byte[img.Width];
            for (int j = 0; j < img.Width; j++)
            {
                System.Drawing.Color pixel = img.GetPixel(j, i);
                if (pixel.A > 20)
                    mas[j] = (byte)((pixel.R + pixel.G + pixel.B) / 3);
                else
                    mas[j] = 255;
            }

            return mas;
        }


        // Картинка в формате массива преобразуется в однобитовую
        private static byte[] ConvertToOneBitImage(byte[][] grayImage, int cont)
        {
            // Вспомогательная функция
            byte MP(int newNum)
            {
                if (newNum > 255) { newNum = 255; }
                else if (newNum < 0) { newNum = 0; }
                return (byte)newNum;
            }

            // Высота и ширина картинки
            int width = grayImage[1].Length;

            // Преобразование картинки
            byte oldPixel;
            byte newPixel;
            int error;
            int newNumber;
            for (int y = 0; y < width; y++)
            {
                oldPixel = grayImage[1][y];
                if (oldPixel > 128) { newPixel = 255; }
                else { newPixel = 0; }
                grayImage[1][y] = newPixel;
                error = oldPixel - newPixel + cont - 256;

                if (y < width - 1)
                {
                    newNumber = grayImage[1][y + 1] + error * 7 / 16;
                    grayImage[1][y + 1] = MP(newNumber);
                }

                if (y > 0)
                {
                    newNumber = grayImage[2][y - 1] + error * 3 / 16;
                    grayImage[2][y - 1] = MP(newNumber);
                }


                newNumber = grayImage[2][y] + error * 5 / 16;
                grayImage[2][y] = MP(newNumber);


                if (y < width - 1)
                {
                    newNumber = grayImage[2][y + 1] + error * 1 / 16;
                    grayImage[2][y + 1] = MP(newNumber);
                }
            }
            return grayImage[1];
        }

        // Преобразовать однобитовую картинку в виде массива в строку
        private static string ConvertImageToText(byte[][] grayImage, string mode)
        {
            // Определение инверсии цветов
            byte mode1;
            byte mode2;
            if (mode == "white")
            {
                mode1 = 1;
                mode2 = 0;
            }
            else if (mode == "black")
            {
                mode1 = 0;
                mode2 = 1;
            }
            else
            {
                return "";
            }

            // Основная программа
            int l = 0;
            string st = "";
            byte[] mas = new byte[8];
            for (int i = 0; i < grayImage[1].Length; i += 2)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (col < grayImage.GetLength(0))
                    {
                        for (int k = i; k < i + 2; k++)
                        {
                            if (k < grayImage[1].Length)
                            {
                                if (grayImage[col][k] <= 100)
                                {
                                    mas[k - i + (col - l) * 2] = mode1;
                                }
                                else
                                {
                                    mas[k - i + (col - l) * 2] = mode2;
                                }
                            }
                            else
                            {
                                mas[k - i + (col - l) * 2] = 0;
                            }
                        }
                    }
                }
                st += (char)(10240 + mas[0] * Math.Pow(2, 0) + mas[2] * Math.Pow(2, 1) + mas[4] * Math.Pow(2, 2) + mas[1] * Math.Pow(2, 3) + mas[3] * Math.Pow(2, 4) + mas[5] * Math.Pow(2, 5) + mas[6] * Math.Pow(2, 6) + mas[7] * Math.Pow(2, 7));
            }
            return st + "\n";
        }

        public void Run()
        {
            Bitmap img = new Bitmap(System.Drawing.Image.FromFile(path), new System.Drawing.Size(width, height));
            byte[][] f = new byte[][] { new byte[img.Width], ConvertToGrayImage(img, 0), ConvertToGrayImage(img, 1) };
            byte[][] s = new byte[4][];
            string str = "";
            int timer = 0;
            for (int i = 0; i < img.Height; i++)
            {
                s[i % 4] = ConvertToOneBitImage(f, contrast);
                if (i + 1 == img.Height)
                {
                    while (i % 4 - 3 != 0)
                    {

                        i++;
                        for (int q = 0; q < img.Width; q++)
                        {
                            s[i % 4][q] = 255;
                        }
                    }
                }
                if (i % 4 - 3 == 0)
                {
                    str += ConvertImageToText(s, mode);
                }
                f[0] = f[1];
                f[1] = f[2];
                if (i + 2 < img.Height)
                    f[2] = ConvertToGrayImage(img, i + 2);
                else
                    f[2] = new byte[img.Width];
                if (100 * i / img.Height > timer)
                {
                    timer += 100 * i / img.Height - timer;
                    if (timer > 100)
                    {
                        timer = 100;
                    }
                    Console.WriteLine(timer);
                }
                progress.Report(timer);
            }
            image = str;
        }
    }
}