using System;
using System.Collections.Concurrent;

using Windows.UI.ViewManagement;

namespace Inventory
{
    static public class ThreadSafeSingleton<T> where T : new()
    {
        static private ConcurrentDictionary<int, ConcurrentDictionary<Type, T>> _threadInstances = new ConcurrentDictionary<int, ConcurrentDictionary<Type, T>>();

        static public T Instance
        {
            get
            {
                int currentId = ApplicationView.GetForCurrentView().Id;
                var instances = _threadInstances.GetOrAdd(currentId, (id) => new ConcurrentDictionary<Type, T>());
                return instances.GetOrAdd(typeof(T), (t) => new T());
            }
        }
    }
}
