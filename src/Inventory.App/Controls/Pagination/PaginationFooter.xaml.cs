using System;
using System.Linq;
using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace VanArsdel.Inventory.Controls
{
    public sealed partial class PaginationFooter : UserControl
    {
        public PaginationFooter()
        {
            InitializeComponent();
            this.SizeChanged += OnSizeChanged;
        }

        #region PageButtons
        private Button[] _pageButtons = null;
        public Button[] PageButtons => _pageButtons ?? (_pageButtons = GetPageButtons().ToArray());

        private IEnumerable<Button> GetPageButtons()
        {
            yield return pageButton1;
            yield return pageButton2;
            yield return pageButton3;
            yield return pageButton4;
            yield return pageButton5;
        }
        #endregion


        #region ItemsCount
        public int ItemsCount
        {
            get { return (int)GetValue(ItemsCountProperty); }
            set { SetValue(ItemsCountProperty, value); }
        }

        public static readonly DependencyProperty ItemsCountProperty = DependencyProperty.Register("ItemsCount", typeof(int), typeof(PaginationFooter), new PropertyMetadata(0, UpdateControl));
        #endregion

        #region PageIndex
        public int PageIndex
        {
            get { return (int)GetValue(PageIndexProperty); }
            set { SetValue(PageIndexProperty, value); }
        }

        private static void UpdateControl(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as PaginationFooter;
            control.UpdateControl();
        }

        public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register("PageIndex", typeof(int), typeof(PaginationFooter), new PropertyMetadata(0, UpdateControl));
        #endregion

        #region PageSizeIndex
        public int PageSizeIndex
        {
            get { return (int)GetValue(PageSizeIndexProperty); }
            set { SetValue(PageSizeIndexProperty, value); }
        }

        private static void PageSizeIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as PaginationFooter;
            control.UpdatePageSizeIndex((int)e.OldValue);
        }

        public static readonly DependencyProperty PageSizeIndexProperty = DependencyProperty.Register("PageSizeIndex", typeof(int), typeof(PaginationFooter), new PropertyMetadata(1, PageSizeIndexChanged));
        #endregion

        #region ButtonsCount
        private int _buttonsCount = 5;
        public int ButtonsCount
        {
            get => _buttonsCount;
            set
            {
                if (value != _buttonsCount)
                {
                    _buttonsCount = value;
                    UpdateControl();
                }
            }
        }
        #endregion

        public int PageSize => GetPageSize(PageSizeIndex);
        public int PagesCount => (int)Math.Ceiling(ItemsCount / (double)PageSize);
        public int StartIndex => PagesCount > ButtonsCount ? Math.Min(Math.Max(0, PageIndex - (ButtonsCount / 2)), PagesCount - ButtonsCount) : 0;

        public string CountLabel => GetCountLabel();
        public bool ComboVisibility => ItemsCount > 10;

        public bool IsVisibleButtonLT => PagesCount > ButtonsCount && PageIndex > (ButtonsCount - 3);
        public double ButtonGTOpacity => (PagesCount > ButtonsCount && PageIndex < PagesCount - (ButtonsCount - 2)) ? 1.0 : 0.0;

        private void UpdatePageSizeIndex(int oldIndex)
        {
            int itemIndex = GetPageSize(oldIndex) * PageIndex;
            PageIndex = itemIndex / PageSize;
            UpdateControl();
        }

        private void UpdateControl()
        {
            for (int n = 0; n < 5; n++)
            {
                PageButtons[n].Visibility = Visibility.Collapsed;
            }

            int startIndex = StartIndex;
            for (int n = 0; n < ButtonsCount; n++)
            {
                int buttonPage = startIndex++;
                var button = PageButtons[n];
                button.Content = buttonPage + 1;
                if (PagesCount > 1)
                {
                    if (buttonPage == PageIndex)
                    {
                        button.Style = Resources["SelectedPageButtonStyle"] as Style;
                        button.Visibility = Visibility.Visible;
                    }
                    else if (buttonPage < PagesCount)
                    {
                        button.Style = Resources["PageButtonStyle"] as Style;
                        button.Visibility = Visibility.Visible;
                    }
                }
            }

            Bindings.Update();
        }

        private string GetCountLabel()
        {
            if (ItemsCount > 10)
            {
                if (ButtonsCount > 3)
                {
                    return $"Page {PageIndex + 1} of {PagesCount} Showing ";
                }
                if (PagesCount > 1)
                {
                    return $"{PagesCount} x";
                }
                return $"Total {ItemsCount} items | Page size ";
            }
            return $"Total {ItemsCount} items";
        }

        private int GetPageSize(int index)
        {
            switch (index)
            {
                default:
                    return 10;
                case 1:
                    return 20;
                case 2:
                    return 50;
            }
        }

        private void LabelTapped(object sender, TappedRoutedEventArgs e)
        {
            if (ItemsCount > 10)
            {
                combo.IsDropDownOpen = true;
            }
        }

        private void OnPageButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement elem)
            {
                if (elem.Opacity > 0.0)
                {
                    if ("LT".Equals(elem.Tag))
                    {
                        PageIndex = Math.Max(0, PageIndex - ButtonsCount);
                        return;
                    }

                    if ("GT".Equals(elem.Tag))
                    {
                        PageIndex = Math.Min(PagesCount - 1, PageIndex + ButtonsCount);
                        return;
                    }

                    int index = Int32.Parse(elem.Tag as String);
                    PageIndex = StartIndex + index - 1;
                }
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width > 568)
            {
                ButtonsCount = 5;
            }
            else
            {
                ButtonsCount = 3;
            }
        }
    }
}
