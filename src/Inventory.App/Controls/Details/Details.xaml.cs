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
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Inventory.Controls
{
    public sealed partial class Details : UserControl, INotifyExpressionChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Details()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            DependencyExpressions.Initialize(this);
        }

        static private readonly DependencyExpressions DependencyExpressions = new DependencyExpressions();

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            foreach (var input in GetEditableControls())
            {
                input.EnterFocus += OnInputGotFocus;
            }
            UpdateEditMode();
        }

        #region CanGoBack*
        public bool CanGoBack
        {
            get { return (bool)GetValue(CanGoBackProperty); }
            set { SetValue(CanGoBackProperty, value); }
        }

        private static void CanGoBackChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Details;
            DependencyExpressions.UpdateDependencies(control, nameof(CanGoBack));
        }

        public static readonly DependencyProperty CanGoBackProperty = DependencyProperty.Register(nameof(CanGoBack), typeof(bool), typeof(Details), new PropertyMetadata(false, CanGoBackChanged));
        #endregion

        #region IsEditMode*
        public bool IsEditMode
        {
            get { return (bool)GetValue(IsEditModeProperty); }
            set { SetValue(IsEditModeProperty, value); }
        }

        private static void IsEditModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Details;
            DependencyExpressions.UpdateDependencies(control, nameof(IsEditMode));
            control.UpdateEditMode();
        }

        public static readonly DependencyProperty IsEditModeProperty = DependencyProperty.Register(nameof(IsEditMode), typeof(bool), typeof(Details), new PropertyMetadata(false, IsEditModeChanged));
        #endregion

        #region DetailsContent
        public object DetailsContent
        {
            get { return (object)GetValue(DetailsContentProperty); }
            set { SetValue(DetailsContentProperty, value); }
        }

        public static readonly DependencyProperty DetailsContentProperty = DependencyProperty.Register(nameof(DetailsContent), typeof(object), typeof(Details), new PropertyMetadata(null));
        #endregion

        #region DetailsTemplate
        public DataTemplate DetailsTemplate
        {
            get { return (DataTemplate)GetValue(DetailsTemplateProperty); }
            set { SetValue(DetailsTemplateProperty, value); }
        }

        public static readonly DependencyProperty DetailsTemplateProperty = DependencyProperty.Register(nameof(DetailsTemplate), typeof(DataTemplate), typeof(Details), new PropertyMetadata(null));
        #endregion

        #region DefaultCommands
        public string DefaultCommands
        {
            get { return (string)GetValue(DefaultCommandsProperty); }
            set { SetValue(DefaultCommandsProperty, value); }
        }

        public static readonly DependencyProperty DefaultCommandsProperty = DependencyProperty.Register(nameof(DefaultCommands), typeof(string), typeof(Details), new PropertyMetadata("edit,delete"));
        #endregion


        #region BackCommand
        public ICommand BackCommand
        {
            get { return (ICommand)GetValue(BackCommandProperty); }
            set { SetValue(BackCommandProperty, value); }
        }

        public static readonly DependencyProperty BackCommandProperty = DependencyProperty.Register(nameof(BackCommand), typeof(ICommand), typeof(Details), new PropertyMetadata(null));
        #endregion

        #region EditCommand
        public ICommand EditCommand
        {
            get { return (ICommand)GetValue(EditCommandProperty); }
            set { SetValue(EditCommandProperty, value); }
        }

        public static readonly DependencyProperty EditCommandProperty = DependencyProperty.Register(nameof(EditCommand), typeof(ICommand), typeof(Details), new PropertyMetadata(null));
        #endregion

        #region DeleteCommand
        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }

        public static readonly DependencyProperty DeleteCommandProperty = DependencyProperty.Register(nameof(DeleteCommand), typeof(ICommand), typeof(Details), new PropertyMetadata(null));
        #endregion

        #region SaveCommand
        public ICommand SaveCommand
        {
            get { return (ICommand)GetValue(SaveCommandProperty); }
            set { SetValue(SaveCommandProperty, value); }
        }

        public static readonly DependencyProperty SaveCommandProperty = DependencyProperty.Register(nameof(SaveCommand), typeof(ICommand), typeof(Details), new PropertyMetadata(null));
        #endregion

        #region CancelCommand
        public ICommand CancelCommand
        {
            get { return (ICommand)GetValue(CancelCommandProperty); }
            set { SetValue(CancelCommandProperty, value); }
        }

        public static readonly DependencyProperty CancelCommandProperty = DependencyProperty.Register(nameof(CancelCommand), typeof(ICommand), typeof(Details), new PropertyMetadata(null));
        #endregion

        public DetailToolbarMode ToolbarMode
        {
            get
            {
                if (IsEditMode)
                {
                    return DetailToolbarMode.CancelSave;
                }
                return CanGoBack ? DetailToolbarMode.BackEditdDelete : DetailToolbarMode.Default;
            }
        }
        static DependencyExpression ToolbarModeExpression = DependencyExpressions.Register(nameof(ToolbarMode), nameof(IsEditMode), nameof(CanGoBack));

        public void SetFocus()
        {
            GetEditableControls().FirstOrDefault()?.SetFocus();
        }

        private void OnToolbarClick(object sender, ToolbarButtonClickEventArgs e)
        {
            switch (e.ClickedButton)
            {
                case ToolbarButton.Back:
                    BackCommand?.TryExecute();
                    break;
                case ToolbarButton.Edit:
                    EditCommand?.TryExecute();
                    break;
                case ToolbarButton.Delete:
                    DeleteCommand?.TryExecute();
                    break;
                case ToolbarButton.Save:
                    SaveCommand?.TryExecute();
                    break;
                case ToolbarButton.Cancel:
                    CancelCommand?.TryExecute();
                    break;
            }
        }

        private void OnInputGotFocus(object sender, RoutedEventArgs e)
        {
            if (!IsEditMode)
            {
                EditCommand?.TryExecute();
            }
        }

        private void UpdateEditMode()
        {
            foreach (var input in GetEditableControls())
            {
                input.Mode = IsEditMode ? TextEditMode.ReadWrite : TextEditMode.Auto;
            }
            if (!IsEditMode)
            {
                Focus(FocusState.Programmatic);
            }
        }

        private IEnumerable<IInputControl> GetEditableControls()
        {
            return ElementSet.Children<Control>(container)
                .Where(r =>
                {
                    if (r is IInputControl input)
                    {
                        return input.Mode != TextEditMode.ReadOnly;
                    }
                    return false;
                })
                .Cast<IInputControl>();
        }

        #region NotifyPropertyChanged
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
