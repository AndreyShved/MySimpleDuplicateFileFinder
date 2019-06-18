using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;


namespace ConsoleApp1
{
    public static class FileDuplicateFinder
    {

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

        static Dictionary<string,List<string>> FindDuplicateFilesInHashedFilesDictionary(HashedFilesList list)
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

                    Dictionary<string, List<string>> duplicateFilesDictionary = new Dictionary<string,List<string>>();
                    foreach (var duplicate in duplicateHashesList)
                    {
                        var duplicatePaths = new List<string>();
                        duplicatePaths.AddRange(list.Where((pair) => pair.Hash == duplicate).Select(pair => pair.FullPath));
                        duplicateFilesDictionary.Add(duplicate, duplicatePaths);
                    }
                    return duplicateFilesDictionary;
                }
            }
        }

        public static Dictionary<string, List<string>> ScanWithHashes(string directoryPath)
        {
            using (var list = new HashedFilesList())
            {
                var task = Task.Run(async () => { await RecursiveSearchLogic.RecursiveSearchAsync((path) => list.AddHashedFilePath(path, CalculateMD5(path)), new List<string>(), false, directoryPath); });
                task.Wait();
                list.FlushList();
                return FindDuplicateFilesInHashedFilesDictionary(list);
            }
        }

        public static List<List<string>> Scan(string directoryPath)
        {
            using (var list = new HashedFilesList())
            {
                var task = Task.Run(async () => { await RecursiveSearchLogic.RecursiveSearchAsync((path) => list.AddHashedFilePath(path, CalculateMD5(path)), new List<string>(), false, directoryPath); });
                task.Wait();
                list.FlushList();
                return FindDuplicateFilesInHashedFilesList(list);
            }
        }
    }
}
