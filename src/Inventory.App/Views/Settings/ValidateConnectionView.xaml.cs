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

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Inventory.ViewModels;

namespace Inventory.Views
{
    public sealed partial class ValidateConnectionView : ContentDialog
    {
        private string _connectionString = null;

        public ValidateConnectionView(string connectionString)
        {
            _connectionString = connectionString;
            ViewModel = ServiceLocator.Current.GetService<ValidateConnectionViewModel>();
            InitializeComponent();
            Loaded += OnLoaded;
        }

        public ValidateConnectionViewModel ViewModel { get; }

        public Result Result { get; private set; }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.ExecuteAsync(_connectionString);
            Result = ViewModel.Result;
        }

        private void OnOkClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void OnCancelClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Result = Result.Ok("Operation cancelled by user");
        }
    }
}
