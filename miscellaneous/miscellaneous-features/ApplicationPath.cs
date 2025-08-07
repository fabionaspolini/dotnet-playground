using System;
using System.Diagnostics;
using System.IO;

namespace miscellaneous_features_playground;

static class ApplicationPath
{
    public static void Execute()
    {
        Console.WriteLine($"Launched from               {Environment.CurrentDirectory}");
        Console.WriteLine($"Physical location           {AppDomain.CurrentDomain.BaseDirectory}");
        Console.WriteLine($"AppContext.BaseDirectory    {AppContext.BaseDirectory}");
        Console.WriteLine($"Runtime Call                {Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)}");
        Console.WriteLine($"MainModule.FileName         {System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName}");
    }
}