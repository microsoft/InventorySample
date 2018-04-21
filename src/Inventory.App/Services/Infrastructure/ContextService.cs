using System;
using System.Threading.Tasks;

using Windows.UI.Core;

namespace Inventory.Services
{
    public class ContextService : IContextService
    {
        private CoreDispatcher _dispatcher = null;

        public int ContextID { get; private set; }

        public bool IsMainView { get; private set; }

        public void Initialize(object dispatcher, int contextID, bool isMainView)
        {
            _dispatcher = dispatcher as CoreDispatcher;
            ContextID = contextID;
            IsMainView = isMainView;
        }

        public async Task RunAsync(Action action)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
        }
    }
}
