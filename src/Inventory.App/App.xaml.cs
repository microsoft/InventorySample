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
using System.Threading.Tasks;

using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;

using Inventory.Views;
using Inventory.ViewModels;
using Inventory.Services;

namespace Inventory
{
    sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.PreferredLaunchViewSize = new Size(1280, 840);

            this.Suspending += OnSuspending;
            this.UnhandledException += OnUnhandledException;
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            await ActivateAsync(e);
        }

        protected override async void OnActivated(IActivatedEventArgs e)
        {
            await ActivateAsync(e);
        }

        private async Task ActivateAsync(IActivatedEventArgs e)
        {
            var activationInfo = ActivationService.GetActivationInfo(e);

            var frame = Window.Current.Content as Frame;
            if (frame == null)
            {
                await Startup.ConfigureAsync();

                frame = new Frame();
                Window.Current.Content = frame;
                var shellArgs = new ShellArgs
                {
                    ViewModel = activationInfo.EntryViewModel,
                    Parameter = activationInfo.EntryArgs,
                    UserInfo = await TryGetUserInfoAsync(e as IActivatedEventArgsWithUser)
                };
#if SKIP_LOGIN
                frame.Navigate(typeof(MainShellView), shellArgs);
                var loginService = ServiceLocator.Current.GetService<ILoginService>();
                loginService.IsAuthenticated = true;
#else
                frame.Navigate(typeof(LoginView), shellArgs);
#endif
                Window.Current.Activate();
            }
            else
            {
                var navigationService = ServiceLocator.Current.GetService<INavigationService>();
                await navigationService.CreateNewViewAsync(activationInfo.EntryViewModel, activationInfo.EntryArgs);
            }
        }

        private async Task<UserInfo> TryGetUserInfoAsync(IActivatedEventArgsWithUser argsWithUser)
        {
            if (argsWithUser != null)
            {
                var user = argsWithUser.User;
                var userInfo = new UserInfo
                {
                    AccountName = await user.GetPropertyAsync(KnownUserProperties.AccountName) as String,
                    FirstName = await user.GetPropertyAsync(KnownUserProperties.FirstName) as String,
                    LastName = await user.GetPropertyAsync(KnownUserProperties.LastName) as String
                };
                if (!userInfo.IsEmpty)
                {
                    if (String.IsNullOrEmpty(userInfo.AccountName))
                    {
                        userInfo.AccountName = $"{userInfo.FirstName} {userInfo.LastName}";
                    }
                    var pictureStream = await user.GetPictureAsync(UserPictureSize.Size64x64);
                    if (pictureStream != null)
                    {
                        userInfo.PictureSource = await BitmapTools.LoadBitmapAsync(pictureStream);
                    }
                    return userInfo;
                }
            }
            return UserInfo.Default;
        }

        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var logService = ServiceLocator.Current.GetService<ILogService>();
            await logService.WriteAsync(Data.LogType.Information, "App", "Suspending", "Application End", $"Application ended by [User].");
        }

        private void OnUnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            var logService = ServiceLocator.Current.GetService<ILogService>();
            logService.WriteAsync(Data.LogType.Error, "App", "UnhandledException", e.Message, e.Exception.ToString());
        }
    }
}
