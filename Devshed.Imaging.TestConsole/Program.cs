using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devshed.Shared;
using Devshed.Imaging;


namespace Devshed.Imaging.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var file = File.OpenRead("E:\\Framed mat black.1.jpg"))
            {
                using (var output = new MemoryStream())
                {
                    Imaging.SaveImageTo(file, output, 200, 200, SizeMode.FitRatio);
                }
            }

            Console.WriteLine("DONE");
            Console.ReadLine();
        }
    }
}
