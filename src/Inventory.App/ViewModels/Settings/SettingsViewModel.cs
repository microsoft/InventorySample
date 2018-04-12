using System;
using System.Threading.Tasks;

namespace Inventory.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
        }

        public string Version => $"v{AppSettings.Current.Version}";

        public SettingsViewState ViewState { get; private set; }

        public Task LoadAsync(SettingsViewState viewState)
        {
            ViewState = viewState;
            return Task.CompletedTask;
        }
    }
}
