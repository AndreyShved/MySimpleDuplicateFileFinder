/*
 * Created by SharpDevelop.
 * User: shved_as
 * Date: 26.02.2018
 * Time: 16:08
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp1
{
    /// <summary>
    /// Description of FileSearchStringList.
    /// </summary>
    public class FileSearchStringList : DiskStoredStringList
    {
        protected override string GenerateFileName()
        {
            return "filesearchStringListf" + Guid.NewGuid() + ".txt";
        }
        public FileSearchStringList() : base()
        {
        }
        
        public void Add(string item)
        {
            AddString(item);
        }

    }
}
