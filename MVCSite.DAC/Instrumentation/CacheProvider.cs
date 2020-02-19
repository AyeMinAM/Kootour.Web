using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVCSite.DAC.Interfaces;
using System.Web;
using System.Web.Caching;

namespace MVCSite.DAC.Instrumentation
{
    public interface ICacheProvider
    {
        T Get<T>(string key, TimeSpan pheriod, object lockObject, Func<T> factoryMethod) where T : class;
    }

    public class HttpCacheProvider : ICacheProvider
    {
        public T Get<T>(string key, TimeSpan pheriod, object lockObject, Func<T> factoryMethod) where T : class
        {
            var data = HttpRuntime.Cache[key] as T;
            if (data == null)
            {
                lock (lockObject)
                {
                    data = HttpRuntime.Cache[key] as T;
                    if (data == null)
                    {
                        data = factoryMethod();
                        HttpRuntime.Cache.Insert(key, data, null, DateTime.Now + pheriod, Cache.NoSlidingExpiration);
                    }
                }
            }
            return data;
        }
    }
}
