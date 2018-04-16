using System;
using System.Diagnostics;

using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    public class ViewModelBase : ModelBase
    {
        private Stopwatch _stopwatch = new Stopwatch();

        public ViewModelBase(ICommonServices commonServices)
        {
            ContextService = commonServices.ContextService;
            NavigationService = commonServices.NavigationService;
            MessageService = commonServices.MessageService;
            DialogService = commonServices.DialogService;
            LogService = commonServices.LogService;
        }

        public IContextService ContextService { get; }
        public INavigationService NavigationService { get; }
        public IMessageService MessageService { get; }
        public IDialogService DialogService { get; }
        public ILogService LogService { get; }

        public bool IsMainView => ContextService.IsMainView;

        virtual public string Title => String.Empty;

        public void StartStatusMessage(string message)
        {
            StatusMessage(message);
            _stopwatch.Reset();
            _stopwatch.Start();
        }

        public void EndStatusMessage(string message)
        {
            _stopwatch.Stop();
            StatusMessage($"{message} ({_stopwatch.Elapsed.TotalSeconds:#0.000} seconds)");
        }

        public void StatusReady()
        {
            MessageService.Send(this, "StatusMessage", "Ready");
        }

        public void StatusMessage(string message)
        {
            MessageService.Send(this, "StatusMessage", message);
        }

        public void StatusError(string message)
        {
            MessageService.Send(this, "StatusError", message);
        }
    }
}
