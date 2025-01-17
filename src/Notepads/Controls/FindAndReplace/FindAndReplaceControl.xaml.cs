﻿namespace Notepads.Controls.FindAndReplace
{
    using System;
    using System.Collections.Generic;
    using Notepads.Commands;
    using Notepads.Extensions;
    using Notepads.Services;
    using Windows.System;
    using Windows.UI;
    using Windows.UI.Core;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Media;

    public sealed partial class FindAndReplaceControl : UserControl
    {
        public event EventHandler<RoutedEventArgs> OnDismissKeyDown;
        public event EventHandler<FindAndReplaceEventArgs> OnFindAndReplaceButtonClicked;
        public event EventHandler<bool> OnToggleReplaceModeButtonClicked;
        public event EventHandler<KeyRoutedEventArgs> OnFindReplaceControlKeyDown;

        private readonly IList<KeyboardCommand<bool>> _nativeKeyboardCommands = new List<KeyboardCommand<bool>>
        {
            new KeyboardCommand<bool>(VirtualKey.F3, null),
            new KeyboardCommand<bool>(false, false, true, VirtualKey.F3, null),
            new KeyboardCommand<bool>(false, true, false, VirtualKey.E, null),
            new KeyboardCommand<bool>(false, true, false, VirtualKey.R, null),
            new KeyboardCommand<bool>(false, true, false, VirtualKey.W, null),
            new KeyboardCommand<bool>(true, true, false, VirtualKey.Enter, null)
        };

        //When enter key is pressed focus is returned to control
        //This variable is used to remove flicker in text selection
        private bool _enterPressed = false;

        private bool _shouldUpdateSearchString = true;

        private double _cursorOffset = 0.0;

        private CoreCursor _sizingCursor = new CoreCursor(CoreCursorType.SizeWestEast, 0);
        private CoreCursor _arrowCursor = new CoreCursor(CoreCursorType.Arrow, 0);

        public FindAndReplaceControl()
        {
            InitializeComponent();

            SetSelectionHighlightColor(ThemeSettingsService.AppAccentColor);
            ThemeSettingsService.OnAccentColorChanged += ThemeSettingsService_OnAccentColorChanged;

            Loaded += FindAndReplaceControl_Loaded;
        }

        public void Dispose()
        {
            Loaded -= FindAndReplaceControl_Loaded;
            ThemeSettingsService.OnAccentColorChanged -= ThemeSettingsService_OnAccentColorChanged;
        }

        public SearchContext GetSearchContext()
        {
            bool matchCase = MatchCaseToggle.IsChecked != null && (bool)MatchCaseToggle.IsChecked;
            bool matchWholeWord = MatchWholeWordToggle.IsChecked != null && (bool)MatchWholeWordToggle.IsChecked;
            bool matchRegex = UseRegexToggle.IsChecked != null && (bool)UseRegexToggle.IsChecked;

            return new SearchContext(FindBar.Text, matchCase, matchWholeWord, matchRegex);
        }

        private void FindAndReplaceControl_Loaded(object sender, RoutedEventArgs e)
        {
            Focus(string.Empty, FindAndReplaceMode.FindOnly);
        }

        private async void ThemeSettingsService_OnAccentColorChanged(object sender, Color color)
        {
            await Dispatcher.CallOnUIThreadAsync(() =>
            {
                SetSelectionHighlightColor(color);
            });
        }

        public double GetHeight(bool showReplaceBar)
        {
            if (showReplaceBar)
            {
                return FindBarPlaceHolder.Height + ReplaceBarPlaceHolder.Height;
            }
            else
            {
                return FindBarPlaceHolder.Height;
            }
        }

        private void SetSelectionHighlightColor(Color color)
        {
            FindBar.SelectionHighlightColor = new SolidColorBrush(color);
            FindBar.SelectionHighlightColorWhenNotFocused = new SolidColorBrush(color);
            ReplaceBar.SelectionHighlightColor = new SolidColorBrush(color);
            ReplaceBar.SelectionHighlightColorWhenNotFocused = new SolidColorBrush(color);
        }

        public void Focus(string searchString, FindAndReplaceMode mode)
        {
            if (_shouldUpdateSearchString && !string.IsNullOrEmpty(searchString)) FindBar.Text = searchString;

            if (mode == FindAndReplaceMode.FindOnly)
            {
                FindBar.Focus(FocusState.Programmatic);
            }
            else
            {
                ReplaceBar.Focus(FocusState.Programmatic);
            }

            FindBar_OnTextChanged(null, null);
        }

        public void ShowReplaceBar(bool showReplaceBar)
        {
            if (showReplaceBar)
            {
                ToggleReplaceModeButtonGrid.SetValue(Grid.RowSpanProperty, 2);
                ToggleReplaceModeButton.Content = new FontIcon { Glyph = "\xE011", FontSize = 12 };
                ReplaceBarPlaceHolder.Visibility = Visibility.Visible;
                if (!string.IsNullOrEmpty(FindBar.Text))
                {
                    ReplaceButton.IsEnabled = true;
                    ReplaceAllButton.IsEnabled = true;
                }
            }
            else
            {
                ToggleReplaceModeButtonGrid.SetValue(Grid.RowSpanProperty, 1);
                ToggleReplaceModeButton.Content = new FontIcon { Glyph = "\xE00F", FontSize = 12 };
                ReplaceBarPlaceHolder.Visibility = Visibility.Collapsed;
                ReplaceButton.IsEnabled = false;
                ReplaceAllButton.IsEnabled = false;
            }
        }

        private void DismissButton_OnClick(object sender, RoutedEventArgs e)
        {
            OnDismissKeyDown?.Invoke(sender, e);
        }

        private void FindBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(FindBar.Text))
            {
                SearchForwardButton.IsEnabled = true;
                SearchBackwardButton.IsEnabled = true;
                if (ReplaceBarPlaceHolder.Visibility == Visibility.Visible)
                {
                    ReplaceButton.IsEnabled = true;
                    ReplaceAllButton.IsEnabled = true;
                }
            }
            else
            {
                SearchForwardButton.IsEnabled = false;
                SearchBackwardButton.IsEnabled = false;
                ReplaceButton.IsEnabled = false;
                ReplaceAllButton.IsEnabled = false;
            }
        }

        private void SearchForwardButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyout) return;

            OnFindAndReplaceButtonClicked?.Invoke(sender, new FindAndReplaceEventArgs(GetSearchContext(), null, FindAndReplaceMode.FindOnly, SearchDirection.Next));
        }

        private void SearchBackwardButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyout) return;

            OnFindAndReplaceButtonClicked?.Invoke(sender, new FindAndReplaceEventArgs(GetSearchContext(), null, FindAndReplaceMode.FindOnly, SearchDirection.Previous));
        }

        private void FindBar_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            var shiftDown = Window.Current.CoreWindow.GetKeyState(VirtualKey.Shift).HasFlag(CoreVirtualKeyStates.Down);

            if (e.Key == VirtualKey.Enter && !string.IsNullOrEmpty(FindBar.Text))
            {
                _enterPressed = true;
                if (shiftDown)
                {
                    SearchBackwardButton_OnClick(sender, e);
                }
                else
                {
                    SearchForwardButton_OnClick(sender, e);
                }
            }
            else if (e.Key == VirtualKey.Tab)
            {
                e.Handled = true;
                if (ReplaceBarPlaceHolder.Visibility == Visibility.Visible) ReplaceBar.Focus(FocusState.Programmatic);
            }
        }

        private void FindBar_GotFocus(object sender, RoutedEventArgs e)
        {
            _enterPressed = false;
            _shouldUpdateSearchString = false;
            FindBar.SelectionStart = 0;
            FindBar.SelectionLength = FindBar.Text.Length;
        }

        private void FindBar_LostFocus(object sender, RoutedEventArgs e)
        {
            _shouldUpdateSearchString = true;
            if (_enterPressed) return;
            FindBar.SelectionStart = FindBar.Text.Length;
        }

        private void ReplaceBar_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            var shiftDown = Window.Current.CoreWindow.GetKeyState(VirtualKey.Shift).HasFlag(CoreVirtualKeyStates.Down);

            if (e.Key == VirtualKey.Enter && !string.IsNullOrEmpty(FindBar.Text))
            {
                _enterPressed = true;
                if (shiftDown)
                {
                    OnFindAndReplaceButtonClicked?.Invoke(sender,
                        new FindAndReplaceEventArgs(GetSearchContext(), ReplaceBar.Text, FindAndReplaceMode.Replace, SearchDirection.Previous));
                }
                else
                {
                    ReplaceButton_OnClick(sender, e);
                }
            }
            else if (e.Key == VirtualKey.Tab)
            {
                e.Handled = true;
                if (ReplaceBarPlaceHolder.Visibility == Visibility.Visible) FindBar.Focus(FocusState.Programmatic);
            }
        }

        private void ReplaceBar_GotFocus(object sender, RoutedEventArgs e)
        {
            _enterPressed = false;
            _shouldUpdateSearchString = false;
            ReplaceBar.SelectionStart = 0;
            ReplaceBar.SelectionLength = ReplaceBar.Text.Length;
        }

        private void ReplaceBar_LostFocus(object sender, RoutedEventArgs e)
        {
            _shouldUpdateSearchString = true;
            if (_enterPressed) return;
            ReplaceBar.SelectionStart = ReplaceBar.Text.Length;
        }

        private void ReplaceBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void ReplaceButton_OnClick(object sender, RoutedEventArgs e)
        {
            OnFindAndReplaceButtonClicked?.Invoke(sender, new FindAndReplaceEventArgs(GetSearchContext(), ReplaceBar.Text, FindAndReplaceMode.Replace, SearchDirection.Next));
        }

        private void ReplaceAllButton_OnClick(object sender, RoutedEventArgs e)
        {
            OnFindAndReplaceButtonClicked?.Invoke(sender, new FindAndReplaceEventArgs(GetSearchContext(), ReplaceBar.Text, FindAndReplaceMode.ReplaceAll));
        }

        // private void OptionButtonFlyoutItem_OnClick(object sender, RoutedEventArgs e)
        // {
        //     MatchWholeWordToggle.IsEnabled = !UseRegexToggle.IsChecked;
        //     UseRegexToggle.IsEnabled = !MatchWholeWordToggle.IsChecked;

        //     if (MatchCaseToggle.IsChecked || MatchWholeWordToggle.IsChecked || UseRegexToggle.IsChecked)
        //     {
        //         OptionButtonSelectionIndicator.Visibility = Visibility.Visible;
        //     }
        //     else
        //     {
        //         OptionButtonSelectionIndicator.Visibility = Visibility.Collapsed;
        //     }
        // }

        private void FindAndReplaceRootGrid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            var ctrlDown = Window.Current.CoreWindow.GetKeyState(VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down);
            var altDown = Window.Current.CoreWindow.GetKeyState(VirtualKey.Menu).HasFlag(CoreVirtualKeyStates.Down);
            var shiftDown = Window.Current.CoreWindow.GetKeyState(VirtualKey.Shift).HasFlag(CoreVirtualKeyStates.Down);

            var isNativeKeyboardCommand = false;

            foreach (var KeyboardCommand in _nativeKeyboardCommands)
            {
                if (KeyboardCommand.Hit(ctrlDown, altDown, shiftDown, e.Key))
                {
                    isNativeKeyboardCommand = true;
                    break;
                }
            }

            if (!isNativeKeyboardCommand && !e.Handled)
            {
                OnFindReplaceControlKeyDown?.Invoke(sender, e);
            }
        }

        private void ToggleReplaceModeButton_OnClick(object sender, RoutedEventArgs e)
        {
            _shouldUpdateSearchString = false;
            OnToggleReplaceModeButtonClicked?.Invoke(sender, ReplaceBarPlaceHolder.Visibility == Visibility.Collapsed ? true : false);
        }

        private void ResizeGripper_DragDelta(object sender, Windows.UI.Xaml.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (Window.Current.CoreWindow.PointerCursor.Type != CoreCursorType.SizeWestEast)
            {
                Window.Current.CoreWindow.PointerCursor = _sizingCursor;
            }

            var minWidth = 250;
            var maxWidth = Window.Current.Content.ActualSize.X - 50;

            var delta = e.HorizontalChange;
            var newWidth = Width - delta;
            if (newWidth < minWidth || newWidth > maxWidth || Math.Abs(_cursorOffset) >= 1.0)
            {
                _cursorOffset -= delta;
            }

            if (Math.Abs(_cursorOffset) < 1.0)
            {
                _cursorOffset = 0.0;
                Width = Math.Clamp(newWidth, minWidth, maxWidth);
            }
        }

        private void ResizeGripper_DragStarted(object sender, Windows.UI.Xaml.Controls.Primitives.DragStartedEventArgs e)
        {
            _cursorOffset = 0;
            Window.Current.CoreWindow.PointerCursor = _sizingCursor;
        }

        private void ResizeGripper_DragCompleted(object sender, Windows.UI.Xaml.Controls.Primitives.DragCompletedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = _arrowCursor;
        }
    }
}
