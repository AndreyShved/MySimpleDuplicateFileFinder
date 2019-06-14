using System;
using System.IO;

using Newtonsoft.Json;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var json = JsonConvert.SerializeObject(FileDuplicateFinder.Scan(@"C:\Users"));
            var resultFilePath = @"result.json";
            File.WriteAllText(resultFilePath , json);
            Console.WriteLine("Result saved in " + resultFilePath);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        
    }
}
