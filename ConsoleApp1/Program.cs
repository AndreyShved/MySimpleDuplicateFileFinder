using System;
using System.IO;

using Newtonsoft.Json;

namespace MySimpleDuplicateFileFinder
{
    class Program
    {

        static void Main(string[] args)
        {
            var scanResult = FileDuplicateFinder.Scan(@"C:\Users");
            var json = JsonConvert.SerializeObject(scanResult);
            var resultFilePath = @"result.json";
            File.WriteAllText(resultFilePath , json);
            Console.WriteLine("Result saved in " + resultFilePath);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        
    }
}
