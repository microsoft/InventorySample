using System;
using System.Threading.Tasks;

namespace Inventory.Services
{
    public interface IContextService
    {
        int ContextID { get; }

        bool IsMainView { get; }

        void Initialize(object dispatcher, int contextID, bool isMainView);

        Task RunAsync(Action action);
    }
}
