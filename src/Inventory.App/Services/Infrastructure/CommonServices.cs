#region copyright
// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************
#endregion

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
