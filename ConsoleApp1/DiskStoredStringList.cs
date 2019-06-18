using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp1
{
    public abstract class DiskStoredStringList : IDisposable, IEnumerable<string>
    {
        protected string fileName;
        private const int BUFFER_SIZE = 1000000;

        public MemoryStream ms = new MemoryStream();

        protected virtual string GenerateFileName()
        {
            return "DiskStoredStringListf" + Guid.NewGuid() + ".txt";
        } 

        public DiskStoredStringList()
        {
            fileName = GenerateFileName();
            fileName = (new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", ""))).Directory.FullName + "\\" + fileName;
            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write("");
                }
            }
        }

        public void Dispose()
        {
            try
            {
                File.Delete(fileName);
            }
            catch (System.IO.IOException e)
            {

            }
            finally
            {
                ms?.Dispose();
            }

        }

        private void SaveToDisk()
        {
            using (var stream = new FileStream(fileName, FileMode.Append))
            {
                ms.WriteTo(stream);
                stream.Flush();
            }
            ms.Dispose();
            ms = new MemoryStream();
        }
        
        protected void AddString(string item)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(item + System.Environment.NewLine);
            ms.Write(buffer, 0, buffer.Length);
            if (ms.Length > BUFFER_SIZE)
            {
                SaveToDisk();
            }
        }

        public void FlushList()
        {
            SaveToDisk();
        }

        public class DiskStoredStringListEnumerator : System.Collections.Generic.IEnumerator<string>
        {

            public bool MoveNext()
            {
                return EnumeRator.MoveNext();
            }
            public void Reset()
            {
                EnumeRator.Reset();
            }

            public void Dispose()
            {
                EnumeRator.Dispose();
            }

            public virtual string Current
            {
                get
                {
                    return EnumeRator.Current;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return EnumeRator.Current;
                }
            }

            public DiskStoredStringListEnumerator(string fName)
            {
                var lines = File.ReadLines(fName);
                EnumeRator = lines.GetEnumerator();
            }

            private readonly IEnumerator<string> EnumeRator;
        }


        public virtual IEnumerator<string> GetEnumerator()
        {
            FlushList();
            return new DiskStoredStringListEnumerator(this.fileName);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            FlushList();
            return new DiskStoredStringListEnumerator(this.fileName);
        }

    }
}
