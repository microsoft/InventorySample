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
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;

using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Core;

using Inventory.Views;
using Inventory.ViewModels;

namespace Inventory.Services
{
    public partial class NavigationService : INavigationService
    {
        static private readonly ConcurrentDictionary<Type, Type> _viewModelMap = new ConcurrentDictionary<Type, Type>();

        static NavigationService()
        {
            MainViewId = ApplicationView.GetForCurrentView().Id;
        }

        static public int MainViewId { get; }

        static public void Register<TViewModel, TView>() where TView : Page
        {
            if (!_viewModelMap.TryAdd(typeof(TViewModel), typeof(TView)))
            {
                throw new InvalidOperationException($"ViewModel already registered '{typeof(TViewModel).FullName}'");
            }
        }

        static public Type GetView<TViewModel>()
        {
            return GetView(typeof(TViewModel));
        }
        static public Type GetView(Type viewModel)
        {
            if (_viewModelMap.TryGetValue(viewModel, out Type view))
            {
                return view;
            }
            throw new InvalidOperationException($"View not registered for ViewModel '{viewModel.FullName}'");
        }

        static public Type GetViewModel(Type view)
        {
            var type = _viewModelMap.Where(r => r.Value == view).Select(r => r.Key).FirstOrDefault();
            if (type == null)
            {
                throw new InvalidOperationException($"View not registered for ViewModel '{view.FullName}'");
            }
            return type;
        }

        public bool IsMainView => CoreApplication.GetCurrentView().IsMain;

        public Frame Frame { get; private set; }

        public bool CanGoBack => Frame.CanGoBack;

        public void GoBack() => Frame.GoBack();

        public void Initialize(object frame)
        {
            Frame = frame as Frame;
        }

        public bool Navigate<TViewModel>(object parameter = null)
        {
            return Navigate(typeof(TViewModel), parameter);
        }
        public bool Navigate(Type viewModelType, object parameter = null)
        {
            if (Frame == null)
            {
                throw new InvalidOperationException("Navigation frame not initialized.");
            }
            return Frame.Navigate(GetView(viewModelType), parameter);
        }

        public async Task<int> CreateNewViewAsync<TViewModel>(object parameter = null)
        {
            return await CreateNewViewAsync(typeof(TViewModel), parameter);
        }
        public async Task<int> CreateNewViewAsync(Type viewModelType, object parameter = null)
        {
            int viewId = 0;

            var newView = CoreApplication.CreateNewView();
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                viewId = ApplicationView.GetForCurrentView().Id;

                var frame = new Frame();
                var args = new ShellArgs
                {
                    ViewModel = viewModelType,
                    Parameter = parameter
                };
                frame.Navigate(typeof(ShellView), args);

                Window.Current.Content = frame;
                Window.Current.Activate();
            });

            if (await ApplicationViewSwitcher.TryShowAsStandaloneAsync(viewId))
            {
                return viewId;
            }

            return 0;
        }

        public async Task CloseViewAsync()
        {
            int currentId = ApplicationView.GetForCurrentView().Id;
            await ApplicationViewSwitcher.SwitchAsync(MainViewId, currentId, ApplicationViewSwitchingOptions.ConsolidateViews);
        }
    }
}
