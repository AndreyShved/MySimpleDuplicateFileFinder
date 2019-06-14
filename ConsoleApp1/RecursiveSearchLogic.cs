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
        
        public static async Task RecursiveSearchAsync(SearchResultDelegate searchResult, List<string> exceptionsList, bool includeDirectories = true, string sDir = "*")
        {
            string[] dirs;
            if (!searchEnable) return;
            try
            {
                if (exceptionsList.Contains(sDir)) return;
                if (sDir == "*")
                {
                    var drives = System.IO.DriveInfo.GetDrives();
                    var tmpList = new List<string>();

                    foreach (DriveInfo dro in drives)
                    {
                        tmpList.Add(dro.Name);
                    }
                    dirs = tmpList.ToArray();
                }
                else
                {
                    dirs = Directory.GetDirectories(sDir);
                    foreach (string f in Directory.GetFiles(sDir))
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
                foreach (string d in dirs)
                {
                    if (includeDirectories) searchResult(d);
                    await RecursiveSearchAsync(searchResult, exceptionsList, includeDirectories, d);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }
    }
}
