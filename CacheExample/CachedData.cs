using System;
using System.Collections.Concurrent;

namespace CacheExample
{
    public class CachedData
    {
        private readonly ConcurrentDictionary<CacheKey, IData> _data = new ConcurrentDictionary<CacheKey, IData>();
    }

    public struct CacheKey
    {
        public CacheKey(Type dataType)
        {
            DataType = dataType;
        }

        // def of key, now it can be DataType?
        public Type DataType { get; }  
    }
}