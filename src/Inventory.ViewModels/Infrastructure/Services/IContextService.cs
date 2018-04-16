using System;
using System.Threading.Tasks;

namespace Inventory.Services
{
    public interface IContextService
    {
        int ViewID { get; }

        bool IsMainView { get; }

        string Version { get; }

        void Initialize(object dispatcher, int viewID, bool isMainView);

        Task RunAsync(Action action);
    }
}
