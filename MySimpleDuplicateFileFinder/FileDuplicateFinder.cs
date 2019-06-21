using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace MySimpleDuplicateFileFinder
{
    public static class FileDuplicateFinder
    {

        static List<List<string>> FindDuplicateFilesInHashedFilesList(IEnumerable<HashedFilesList.HashedFilePathPair> list)
        {
            using (var hashesList = new SimpleDiskStoredStringList())
            {
                using (var duplicateHashesList = new SimpleDiskStoredStringList())
                {
                    foreach (var pair in list)
                    {
                        if (hashesList.Contains(pair.Hash))
                        {
                            if (!duplicateHashesList.Contains(pair.Hash)) duplicateHashesList.Add(pair.Hash);
                        }
                        else
                        {
                            hashesList.Add(pair.Hash);
                        }
                    }

                    var duplicateFilesLists = new List<List<string>>();
                    foreach (var duplicateHash in duplicateHashesList)
                    {
                        var duplicatePaths = new List<string>();
                        duplicatePaths.AddRange(list.Where((pair) => pair.Hash == duplicateHash).Select(pair => pair.FullPath));
                        duplicateFilesLists.Add(duplicatePaths);
                    }
                    return duplicateFilesLists;
                }
            }
        }

        static Dictionary<string,List<string>> FindDuplicateFilesInHashedFilesDictionary(IEnumerable<HashedFilesList.HashedFilePathPair> list)
        {
            using (var hashesList = new SimpleDiskStoredStringList())
            {
                using (var duplicateHashesList = new SimpleDiskStoredStringList())
                {
                    foreach (var pair in list)
                    {
                        if (hashesList.Contains(pair.Hash))
                        {
                            if (!duplicateHashesList.Contains(pair.Hash)) duplicateHashesList.Add(pair.Hash);
                        }
                        else
                        {
                            hashesList.Add(pair.Hash);
                        }
                    }

                    var duplicateFilesDictionary = new Dictionary<string,List<string>>();
                    foreach (var duplicateHash in duplicateHashesList)
                    {
                        var duplicatePaths = new List<string>();
                        duplicatePaths.AddRange(list.Where((pair) => pair.Hash == duplicateHash).Select(pair => pair.FullPath));
                        duplicateFilesDictionary.Add(duplicateHash, duplicatePaths);
                    }
                    return duplicateFilesDictionary;
                }
            }
        }

        public static Dictionary<string, List<string>> ScanWithHashes(string directoryPath)
        {
            using (var list = new HashedFilesList())
            {
                RecursiveSearchLogic.RecursiveSearch((path) => list.AddHashedFilePath(path, HashesCalculator.CalculateMD5(path)), new List<string>(), false, directoryPath);
                return FindDuplicateFilesInHashedFilesDictionary(list);
            }
        }

        public static List<List<string>> Scan(string directoryPath)
        {
            using (var list = new HashedFilesList())
            {
                RecursiveSearchLogic.RecursiveSearch((path) => list.AddHashedFilePath(path, HashesCalculator.CalculateMD5(path)), new List<string>(), false, directoryPath);
                return FindDuplicateFilesInHashedFilesList(list);
            }
        }
        
        public static Dictionary<string, List<string>> FastScanWithHashes(string directoryPath)
        {
            var list = new List<HashedFileInfo>();
            var calculator = new HashesCalculator();
            IterationSearchLogic.Search((path) => calculator.AddCalculateTask(path), new List<string>(), false, directoryPath);
            var calculationResults = calculator.GetCalculationResults();
            foreach(var path in calculationResults.Keys)
            {
                list.Add(new HashedFileInfo { Path = path, Hash = calculationResults[path], Size = (new System.IO.FileInfo(path)).Length });
            }
            var hashedFilesGroups = list.GroupBy(hashFileInfo => hashFileInfo.Size).Where(group => group.Count() > 1).SelectMany(group => group.ToList()).GroupBy(hashedElement => hashedElement.Hash).Where(group => group.Count() > 1);
            var result = new Dictionary<string, List<string>>();
            foreach (var group in hashedFilesGroups)
            {
                result[group.Key] = group.Select(element => element.Path).ToList();
            }
            return result;
        }
    }
}
