/*
 * Created by SharpDevelop.
 * User: shved_as
 * Date: 09.02.2018
 * Time: 15:42
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    /// <summary>
    /// Description of RecursiveSearchLogic.
    /// </summary>
    public static class RecursiveSearchLogic
    {
        public static volatile bool searchEnable = true;
        public delegate void SearchResultDelegate(string path);
        
        public static async Task RecursiveSearchAsync(SearchResultDelegate searchResult, List<string> exceptionsList, bool includeDirectories = true, string targetDir = "*")
        {
            if (!searchEnable) return;

            try
            {
                string[] dirs;
                if (exceptionsList.Contains(targetDir)) return;
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
                    foreach (string f in Directory.GetFiles(targetDir))
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
                foreach (string dir in dirs)
                {
                    if (includeDirectories) searchResult(dir);
                    await RecursiveSearchAsync(searchResult, exceptionsList, includeDirectories, dir);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }
    }
}
