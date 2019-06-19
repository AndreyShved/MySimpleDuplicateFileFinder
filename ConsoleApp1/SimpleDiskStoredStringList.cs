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

namespace MySimpleDuplicateFileFinder
{
    /// <summary>
    /// Description of SimpleDiskStoredStringList.
    /// </summary>
    public class SimpleDiskStoredStringList : DiskStoredStringList
    {
        protected override string GenerateFileName()
        {
            return "SimpleDiskStoredStringListf" + Guid.NewGuid() + ".txt";
        }
        public SimpleDiskStoredStringList() : base()
        {
        }
        
        public void Add(string item)
        {
            base.AddString(item);
        }

    }
}
