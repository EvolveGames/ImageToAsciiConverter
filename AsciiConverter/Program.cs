using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                try
                {
                    char[] chars = { '#', '#', '@', '%', '=', '+', '*', ':', '-', ' ', ' ' };

                    Console.Write("Image Directory: ");
                    string imageDirectory = Console.ReadLine();

                    Console.Write("Name: ");
                    string nameDirectory = Console.ReadLine();
                    Console.Write("Scale in (%): ");
                    float imageScale = float.Parse(Console.ReadLine());

                    var bm = new Bitmap(imageDirectory);

                    float prw = bm.Width / 100;
                    float prh = bm.Height / 100;
                    Bitmap image = resizeImage(bm, new Size((int)(prw * imageScale), (int)(prh * (imageScale / 2))));

                    StringBuilder sb;

                    string fileDirectory = Path.Combine(Directory.GetCurrentDirectory(), $"{nameDirectory}.ascii");

                    if (File.Exists(fileDirectory)) File.Delete(fileDirectory);
                    FileStream stream = new FileStream(fileDirectory, FileMode.CreateNew);
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        sb = new StringBuilder();

                        for (int h = 0; h < image.Height; h++)
                        {
                            for (int w = 0; w < image.Width; w++)
                            {
                                Color cl = ((Bitmap)image).GetPixel(w, h);
                                int gray = (cl.R + cl.G + cl.B) / 3;
                                int index = (gray * (chars.Length - 1)) / 255;

                                sb.Append(chars[index]);
                            }
                            sb.Append('\n');
                        }
                        writer.WriteLine(sb.ToString());
                        Console.WriteLine(sb.ToString());
                    }
                    Console.ReadKey(true);
                }
                catch
                { }
            }
        }
        private static Bitmap resizeImage(Bitmap imgToResize, Size size)
        {
            Bitmap b = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgToResize, 0, 0, size.Width, size.Height);
            g.Dispose();
            return (Bitmap)b;
        }
    }
    
}
