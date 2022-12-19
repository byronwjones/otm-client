using System;

namespace BWJ.Web.OTM.Internal.Caching
{
    internal partial class ResponseCache<T>
    {
        private class CacheItem
        {
            public CacheItem(string session, T item)
            {
                Session = session;
                Item = item;
                LastAccessed = DateTime.UtcNow;
            }

            public string Session { get; }
            public T Item { get; set; }
            public DateTime LastAccessed { get; set; }
        }
    }
}
