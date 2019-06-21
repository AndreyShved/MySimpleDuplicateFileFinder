using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MySimpleDuplicateFileFinder
{

    public static class IterationSearchLogic
    {
        public static volatile bool searchEnable = true;
        public delegate void SearchResultDelegate(string path);
        public delegate Task SearchResultDelegateAsync(string path);

        public static void Search(SearchResultDelegate searchResult, List<string> exceptionsList, bool includeDirectories = true, string targetDir = "*")
        {
            if (!searchEnable) return;
            try
            {
                
                string[] dirs;
                if (targetDir == "*")
                {
                    var drives = System.IO.DriveInfo.GetDrives();
                    var driveNames = new List<string>();

                    foreach (DriveInfo drive in drives)
                    {
                        driveNames.Add(drive.Name);
                    }
                    dirs = driveNames.ToArray();
                }
                else
                {
                    dirs = Directory.GetDirectories(targetDir);
                }

                var dirsStack = new Stack<string>();

                foreach (var dir in dirs)
                {
                    dirsStack.Push(dir);
                }

                while (dirsStack.Any())
                {
                    var currentDir = dirsStack.Pop();

                    if (exceptionsList.Contains(currentDir)) continue;

                    if (includeDirectories) searchResult(currentDir);

                    try
                    {
                        foreach (var dir in Directory.GetDirectories(currentDir))
                        {
                            dirsStack.Push(dir);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        continue;
                    }

                    foreach (string f in Directory.GetFiles(currentDir))
                    {
                        try
                        {
                            searchResult(f);
                        }
                        catch (System.Exception excpt)
                        {
                            Console.WriteLine(excpt.Message);
                        }
                    }
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }

    }
}
