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
using Windows.Storage;

namespace Inventory.Views
{
    public sealed partial class ChartPane : UserControl
    {
        private WebView _webView;

        public ChartPane()
        {
            InitializeComponent();
            Loaded += OnLoaded;

            //Initialze the WebView in a separate thread and Add it to the grid
            _webView = new WebView(WebViewExecutionMode.SeparateThread);
            _webView.SetValue(Grid.RowProperty, 1);
            _webView.NavigationCompleted += OnNavigationCompleted;
        }

        private void OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            RootGridLayout.Children.Add(_webView);
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            string text = await LoadStringFromPackageFileAsync("ChartHtmlControl.html");
            _webView.NavigateToString(text);
        }

        public static async Task<string> LoadStringFromPackageFileAsync(string name)
        {
            // Using the storage classes to read the content from a file as a string.
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/Html/{name}"));
            return await FileIO.ReadTextAsync(file);
        }
    }
}
