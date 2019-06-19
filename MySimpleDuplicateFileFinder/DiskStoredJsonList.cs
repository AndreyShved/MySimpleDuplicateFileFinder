using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MySimpleDuplicateFileFinder
{
    public class DiskStoredJsonList<T> : DiskStoredStringList, IEnumerable<T>
    {
        protected override string GenerateFileName()
        {
            return "DiskStoredJsonList" + Guid.NewGuid() + ".txt";
        }

        public void Add(T item)
        {
            AddString(JsonConvert.SerializeObject(item));
        }
        public IEnumerator<T> GetEnumerator()
        {
            FlushList();
            return new DiskStoredJsonListEnumerator<T>(this.fileName);
        }
    }

    public class DiskStoredJsonListEnumerator<T> : IEnumerator<T>
    {
        public DiskStoredJsonListEnumerator(string fName)
        {
            var lines = File.ReadLines(fName);
            EnumeRator = lines.GetEnumerator();
        }

        object IEnumerator.Current
        {
            get
            {
                return JsonConvert.DeserializeObject<T>(EnumeRator.Current);
            }
        }

        public virtual T Current
        {
            get
            {
                return JsonConvert.DeserializeObject<T>(EnumeRator.Current);
            }
        }

        public void Dispose()
        {
            EnumeRator.Dispose();
        }

        public bool MoveNext()
        {
            return EnumeRator.MoveNext();
        }

        public void Reset()
        {
            EnumeRator.Reset();
        }

        private readonly IEnumerator<string> EnumeRator;
    }
}
