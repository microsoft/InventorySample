using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using System.Threading.Tasks;

namespace Inventory.Views
{
    public sealed partial class ChartPane : UserControl
    {
        WebView _webView;
        public ChartPane()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;

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
            StorageFile f = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///html/{name}"));
            return await FileIO.ReadTextAsync(f);
        }
    }
}
