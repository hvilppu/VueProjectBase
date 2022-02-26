using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.Web
{
    public enum MemoryCacheKeys { UserConnectionId }

    public class ProjectMemoryCache : IMemoryCache
    {
        private Dictionary<string, int> userConnectionIds = new Dictionary<string, int>();
        public byte[] buffer = new byte[1000];
        public double dbSaveFrequency = 100;
        private object threadLock = new object();

        public ICacheEntry CreateEntry(object key)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Remove(object key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(object key, out object value)
        {
            throw new NotImplementedException();
        }

        public bool SetCacheValue(MemoryCacheKeys key, object id, object value)
        {
            lock (threadLock)
            {
                if (key == MemoryCacheKeys.UserConnectionId)
                {
                    var cacheValue = Convert.ToInt32(value);


                    if (!userConnectionIds.ContainsKey(id.ToString()))
                    {
                        userConnectionIds.Add(id.ToString(), cacheValue);
                        return true;
                    }

                    userConnectionIds[id.ToString()] = cacheValue;
                    return true;
                }

                return false;
            }
        }

        public bool TryGetCacheValue(MemoryCacheKeys key, object id, out object value)
        {
            lock (threadLock)
            {
                value = null;

                if (key == MemoryCacheKeys.UserConnectionId)
                {
                    int cacheValue;
                    if (userConnectionIds.TryGetValue(id.ToString(), out cacheValue))
                    {
                        value = cacheValue;
                        return true;
                    }
                }

                return false;
            }
        }

        public List<string> TryGetKeysByValue(MemoryCacheKeys key, object value)
        {
            lock (threadLock)
            {
                if (key == MemoryCacheKeys.UserConnectionId)
                {

                    return userConnectionIds.Where(x => x.Value == Convert.ToInt32(value)).Select(x => x.Key).ToList();

                }

                return null;
            }
        }

        public bool TryRemoveCacheValue(MemoryCacheKeys key, object id)
        {
            lock (threadLock)
            {

                if (key == MemoryCacheKeys.UserConnectionId)
                {
                    if (userConnectionIds.ContainsKey(id.ToString()))
                    {
                        userConnectionIds.Remove(id.ToString());
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
