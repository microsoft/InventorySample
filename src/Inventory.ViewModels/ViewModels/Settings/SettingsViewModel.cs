using System;
using System.Threading.Tasks;

using Inventory.Services;

namespace Inventory.ViewModels
{
    #region SettingsArgs
    public class SettingsArgs
    {
    }
    #endregion

    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel(ICommonServices commonServices) : base(commonServices)
        {
        }

        public string Version => $"v{ContextService.Version}";

        public SettingsArgs ViewModelArgs { get; private set; }

        public Task LoadAsync(SettingsArgs args)
        {
            ViewModelArgs = args;
            StatusReady();
            return Task.CompletedTask;
        }
    }
}
