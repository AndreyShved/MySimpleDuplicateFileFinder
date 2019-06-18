using System;
using System.Collections;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class HashedFilesList : IDisposable, IEnumerable<HashedFilesList.HashedFilePathPair>
    {
        private SimpleDiskStoredStringList _list = new SimpleDiskStoredStringList();
        
        public void AddHashedFilePath(string item, string hash)
        {
            _list.Add(item);
            _list.Add(hash);
        }

        public void AddHashedFilePathPair(HashedFilePathPair hashedFilePathPair)
        {
            _list.Add(hashedFilePathPair.FullPath);
            _list.Add(hashedFilePathPair.Hash);
        }

        public class HashedFilePathPair
        {
            public string FullPath { get; set; }
            public string Hash { get; set; }
        }

        public IEnumerator<HashedFilePathPair> GetEnumerator()
        {
            return new HashedFilePathPairEnumerator(_list);
        }

        public class HashedFilePathPairEnumerator : System.Collections.Generic.IEnumerator<HashedFilePathPair>
        {
            private DiskStoredStringList _list;
            private IEnumerator<string> _listEnumerator;
            private string _fullPath;
            private string _hash;
            public HashedFilePathPairEnumerator(DiskStoredStringList list)
            {
                _list = list;
                _listEnumerator = _list.GetEnumerator();
            }

            public HashedFilePathPair Current
            {
                get
                {
                    return new HashedFilePathPair() { FullPath = _fullPath, Hash = _hash };
                }
            }

            object IEnumerator.Current {
                get
                {
                    return new HashedFilePathPair() { FullPath = _fullPath, Hash = _hash };
                }
            }

            public void Dispose()
            {
                _listEnumerator.Dispose();
            }

            public bool MoveNext()
            {
                var res = _listEnumerator.MoveNext();
                _fullPath = _listEnumerator.Current;
                res = _listEnumerator.MoveNext();
                _hash = _listEnumerator.Current;
                return res && (_hash!=null) && (_fullPath!=null);
            }

            public void Reset()
            {
                _listEnumerator.Reset();
            }
        }

        public void FlushList()
        {
            _list.FlushList();
        }

        public void Dispose()
        {
            _list.Dispose();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
