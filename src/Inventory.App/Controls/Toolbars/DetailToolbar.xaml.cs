using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Inventory.Controls
{
    public sealed partial class DetailToolbar : UserControl
    {
        public event ToolbarButtonClickEventHandler ButtonClick;

        public DetailToolbar()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            UpdateControl();
        }

        #region ToolbarMode
        public DetailToolbarMode ToolbarMode
        {
            get { return (DetailToolbarMode)GetValue(ToolbarModeProperty); }
            set { SetValue(ToolbarModeProperty, value); }
        }

        private static void ToolbarModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DetailToolbar;
            control.UpdateControl();
        }

        public static readonly DependencyProperty ToolbarModeProperty = DependencyProperty.Register("ToolbarMode", typeof(DetailToolbarMode), typeof(DetailToolbar), new PropertyMetadata(DetailToolbarMode.Default, ToolbarModeChanged));
        #endregion

        private void UpdateControl()
        {
            switch (ToolbarMode)
            {
                default:
                case DetailToolbarMode.Default:
                    ShowCategory("default");
                    break;
                case DetailToolbarMode.BackEditdDelete:
                    ShowCategory("back", "edit", "delete");
                    break;
                case DetailToolbarMode.CancelSave:
                    ShowCategory("cancel", "save");
                    break;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ElementSet.Children<AppBarButton>(commandBar.PrimaryCommands).Click += OnButtonClick;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            ElementSet.Children<AppBarButton>(commandBar.PrimaryCommands).Click -= OnButtonClick;
        }

        private void ShowCategory(params string[] categories)
        {
            ElementSet.Children<AppBarButton>(commandBar.PrimaryCommands)
                .ForEach(v => v.Show(v.IsCategory(categories)));
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is AppBarButton button)
            {
                switch (button.Name)
                {
                    case "buttonBack":
                        RaiseButtonClick(ToolbarButton.Back);
                        break;
                    case "buttonEdit":
                        RaiseButtonClick(ToolbarButton.Edit);
                        break;
                    case "buttonDelete":
                        RaiseButtonClick(ToolbarButton.Delete);
                        break;
                    case "buttonCancel":
                        RaiseButtonClick(ToolbarButton.Cancel);
                        break;
                    case "buttonSave":
                        RaiseButtonClick(ToolbarButton.Save);
                        break;
                }
            }
        }

        private void RaiseButtonClick(ToolbarButton toolbarButton)
        {
            ButtonClick?.Invoke(this, new ToolbarButtonClickEventArgs(toolbarButton));
        }
    }
}
