using System;

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
        { }

        public void Add(string item)
        {
            AddString(item);
        }

    }
}
