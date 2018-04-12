using System;

namespace Inventory.Services
{
    public class ServiceManager : IServiceManager
    {
        public ServiceManager(INavigationService navigationService, IMessageService messageService, IDialogService dialogService, ILogService logService)
        {
            NavigationService = navigationService;
            MessageService = messageService;
            DialogService = dialogService;
            LogService = logService;
        }

        public INavigationService NavigationService { get; }

        public IMessageService MessageService { get; }

        public IDialogService DialogService { get; }

        public ILogService LogService { get; }
    }
}
