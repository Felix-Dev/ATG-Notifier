using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.UWP.Helpers
{
    public static class Singleton<T>
        where T : new()
    {
        private static ConcurrentDictionary<Type, T> instances = new ConcurrentDictionary<Type, T>();

        public static T Instance => instances.GetOrAdd(typeof(T), (t) => new T());
    }
}
