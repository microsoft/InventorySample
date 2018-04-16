using System;

namespace Inventory.Services
{
    public class CommonServices : ICommonServices
    {
        public CommonServices(IContextService contextService, INavigationService navigationService, IMessageService messageService, IDialogService dialogService, ILogService logService)
        {
            ContextService = contextService;
            NavigationService = navigationService;
            MessageService = messageService;
            DialogService = dialogService;
            LogService = logService;
        }

        public IContextService ContextService { get; }

        public INavigationService NavigationService { get; }

        public IMessageService MessageService { get; }

        public IDialogService DialogService { get; }

        public ILogService LogService { get; }
    }
}
