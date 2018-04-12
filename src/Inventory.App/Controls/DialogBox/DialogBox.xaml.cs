using System;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace VanArsdel.Inventory
{
    public sealed partial class DialogBox : ContentDialog
    {
        public DialogBox()
        {
            InitializeComponent();
            DataContext = this;
        }

        #region Message
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(DialogBox), new PropertyMetadata(null));
        #endregion

        static public async Task ShowAsync(string title, Exception ex, string ok = "Ok")
        {
            await ShowAsync(title, ex.Message, ok);
        }

        static public async Task ShowAsync(Result result, string ok = "Ok")
        {
            await ShowAsync(result.Message, result.Description, ok);
        }

        static public async Task<bool> ShowAsync(string title, string content, string ok = "Ok", string cancel = null)
        {
            var dialog = new DialogBox
            {
                Title = title,
                Message = content,
                PrimaryButtonText = ok
            };
            if (cancel != null)
            {
                dialog.SecondaryButtonText = cancel;
            }
            var result = await dialog.ShowAsync();
            return result == ContentDialogResult.Primary;
        }
    }
}
