using System;
using System.Collections.Generic;
using System.Linq;

namespace BWJ.Web.OTM.Internal.Caching
{
    internal partial class ResponseCache<T>
    {
        private readonly int _cacheLimit;

        public ResponseCache(int cacheLimit)
        {
            _cacheLimit = cacheLimit > 0 ? cacheLimit : 1;
        }

        public T this[string session]
        {
            get
            {
                lock(objLock)
                {
                    var cacheItem = Cache.FirstOrDefault(c => c.Session == session);
                    if (cacheItem is not null)
                    {
                        cacheItem.LastAccessed = DateTime.UtcNow;
                        return cacheItem.Item;
                    }
                    else
                    {
                        return default;
                    }
                }
            }
            set
            {
                if (value is not null)
                {
                    lock (objLock)
                    {
                        var cacheItem = Cache.FirstOrDefault(c => c.Session == session);
                        if (cacheItem is null)
                        {
                            Cache.Add(new CacheItem(session, value));
                            if (Cache.Count > _cacheLimit)
                            {
                                var remove = Cache.Min(c => c.LastAccessed);
                            }
                        }
                        else
                        {
                            cacheItem.Item = value;
                            cacheItem.LastAccessed = DateTime.UtcNow;
                        }
                    }
                }
            }
        }

        private List<CacheItem> Cache = new List<CacheItem>();
        private object objLock = new object();
    }
}
