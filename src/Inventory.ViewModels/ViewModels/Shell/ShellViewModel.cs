using System;
using System.Threading.Tasks;

using Inventory.Services;

namespace Inventory.ViewModels
{
    public class ShellArgs
    {
        public Type ViewModel { get; set; }
        public object Parameter { get; set; }
    }

    public class ShellViewModel : ViewModelBase
    {
        public ShellViewModel(ICommonServices commonServices) : base(commonServices)
        {
        }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set => Set(ref _isBusy, value);
        }

        private string _message = "Ready";
        public string Message
        {
            get => _message;
            set => Set(ref _message, value);
        }

        private bool _isError = false;
        public bool IsError
        {
            get => _isError;
            set => Set(ref _isError, value);
        }

        public ShellArgs ViewModelArgs { get; protected set; }

        virtual public Task LoadAsync(ShellArgs args)
        {
            ViewModelArgs = args ?? new ShellArgs();
            NavigationService.Navigate(ViewModelArgs.ViewModel, ViewModelArgs.Parameter);
            return Task.CompletedTask;
        }
        virtual public void Unload()
        {
        }

        virtual public void Subscribe()
        {
            MessageService.Subscribe<ViewModelBase, String>(this, OnMessage);
        }
        virtual public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        private void OnMessage(ViewModelBase viewModel, string message, string status)
        {
            if (viewModel.ContextService.ContextID != ContextService.ContextID)
                return;

            switch (message)
            {
                case "StatusMessage":
                case "StatusError":
                    IsError = message == "StatusError";
                    Message = status;
                    break;
            }
        }
    }
}
