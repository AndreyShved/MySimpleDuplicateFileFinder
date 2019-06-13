using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var json = JsonConvert.SerializeObject(FindDuplicateFiles(@"C:\Users\ashved"));
            var resultFilePath = @"result.json";
            File.WriteAllText(resultFilePath , json);
            Console.WriteLine("Result saved in " + resultFilePath);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static string GetHashOfFile(string fullPath)
        {
            return CalculateMD5(fullPath);
        }

        static string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        static List<List<string>> FindDuplicateFilesInHashedFilesList(HashedFilesList list)
        {
            using (var hashesList = new DiskStoredStringList())
            {
                using (var duplicateHashesList = new DiskStoredStringList())
                {
                    foreach (var pair in list)
                    {
                        if (hashesList.Contains(pair.Hash))
                        {
                            if (!duplicateHashesList.Contains(pair.Hash)) duplicateHashesList.AddString(pair.Hash);
                        }
                        else
                        {
                            hashesList.AddString(pair.Hash);
                        }
                    }

                    List<List<string>> duplicateFilesLists = new List<List<string>>();
                    foreach (var duplicate in duplicateHashesList)
                    {
                        var duplicatePaths = new List<string>();
                        duplicatePaths.AddRange(list.Where((pair) => pair.Hash == duplicate).Select(pair => pair.FullPath));
                        duplicateFilesLists.Add(duplicatePaths);
                    }
                    return duplicateFilesLists;
                }
            }
        }

        static List<List<string>> FindDuplicateFiles (string directoryPath)
        {
            using (var list = new HashedFilesList())
            {
                var task = Task.Run(async () => { await RecursiveSearchLogic.RecursiveSearchAsync((path) => list.AddHashedFilePath(path, GetHashOfFile(path)), new List<string>(), false, directoryPath); });
                task.Wait();
                list.FlushList();
                return FindDuplicateFilesInHashedFilesList(list);
            }
        }
    }
}
