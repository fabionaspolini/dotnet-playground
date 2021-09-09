using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangFeatures_Sample
{
    static class ApplicationPath
    {
        public static void Execute()
        {
            Console.WriteLine($"Launched from               {Environment.CurrentDirectory}");
            Console.WriteLine($"Physical location           {AppDomain.CurrentDomain.BaseDirectory}");
            Console.WriteLine($"AppContext.BaseDirectory    {AppContext.BaseDirectory}");
            Console.WriteLine($"Runtime Call                {Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)}");
        }
    }
}
