﻿namespace Notepads.Services
{
    using System;
    using System.Text;
    using Notepads.Settings;
    using Notepads.Utilities;
    using Windows.UI.Text;
    using Windows.UI.Xaml;

    public static class AppSettingsService
    {
        public static event EventHandler<bool> OnUseAltFontChanged;
        public static event EventHandler<string> OnFontFamilyChanged;
        public static event EventHandler<FontStyle> OnFontStyleChanged;
        public static event EventHandler<FontWeight> OnFontWeightChanged;
        public static event EventHandler<int> OnFontSizeChanged;

        //public static event EventHandler<string> OnDefaultFontFamilyChanged;
        //public static event EventHandler<FontStyle> OnDefaultFontStyleChanged;
        //public static event EventHandler<FontWeight> OnDefaultFontWeightChanged;
        //public static event EventHandler<int> OnDefaultFontSizeChanged;
        //public static event EventHandler<string> OnAltFontFamilyChanged;
        //public static event EventHandler<FontStyle> OnAltFontStyleChanged;
        //public static event EventHandler<FontWeight> OnAltFontWeightChanged;
        //public static event EventHandler<int> OnAltFontSizeChanged;

        public static event EventHandler<TextWrapping> OnDefaultTextWrappingChanged;
        public static event EventHandler<bool> OnDefaultLineHighlighterViewStateChanged;
        public static event EventHandler<bool> OnDefaultDisplayLineNumbersViewStateChanged;
        public static event EventHandler<LineEnding> OnDefaultLineEndingChanged;
        public static event EventHandler<Encoding> OnDefaultEncodingChanged;
        public static event EventHandler<int> OnDefaultTabIndentsChanged;
        public static event EventHandler<bool> OnStatusBarVisibilityChanged;
        public static event EventHandler<bool> OnSessionBackupAndRestoreOptionChanged;
        public static event EventHandler<bool> OnHighlightMisspelledWordsChanged;

        public static void NotiyFontChange(bool fontFamily=false, bool fontSize = false, bool fontStyle = false, bool fontWeight = false)
        {
            if (fontFamily)
                OnFontFamilyChanged?.Invoke(null, EditorFontFamily);
            if (fontSize)
                OnFontSizeChanged?.Invoke(null, EditorFontSize);
            if (fontStyle)
                OnFontStyleChanged?.Invoke(null, EditorFontStyle);
            if (fontWeight)
                OnFontWeightChanged?.Invoke(null, EditorFontWeight);
        }

        private static bool _editorUseAltFont;

        public static bool EditorUseAltFont
        {
            get => _editorUseAltFont;
            set
            {
                _editorUseAltFont = value;
                NotiyFontChange(true, true, true, true);
                OnUseAltFontChanged?.Invoke(null, value);
            }
        }

        public static string EditorFontFamily
        {
            get => _editorUseAltFont ? EditorAltFontFamily : EditorDefaultFontFamily;
        }

        public static int EditorFontSize
        {
            get => _editorUseAltFont ? EditorAltFontSize : EditorDefaultFontSize;
        }

        public static FontStyle EditorFontStyle
        {
            get => _editorUseAltFont ? EditorAltFontStyle : EditorDefaultFontStyle;
        }

        public static FontWeight EditorFontWeight
        {
            get => _editorUseAltFont ? EditorAltFontWeight : EditorDefaultFontWeight;
        }

        private static string _editorDefaultFontFamily;

        public static string EditorDefaultFontFamily
        {
            get => _editorDefaultFontFamily;
            set
            {
                _editorDefaultFontFamily = value;
                // OnDefaultFontFamilyChanged?.Invoke(null, value);
                NotiyFontChange(fontFamily: true);
                ApplicationSettingsStore.Write(SettingsKey.EditorFontFamilyStr, value);
            }
        }

        private static int _editorDefaultFontSize;

        public static int EditorDefaultFontSize
        {
            get => _editorDefaultFontSize;
            set
            {
                _editorDefaultFontSize = value;
                //OnDefaultFontSizeChanged?.Invoke(null, value);
                NotiyFontChange(fontSize: true);
                ApplicationSettingsStore.Write(SettingsKey.EditorFontSizeInt, value);
            }
        }

        private static FontStyle _editorDefaultFontStyle;

        public static FontStyle EditorDefaultFontStyle
        {
            get => _editorDefaultFontStyle;
            set
            {
                _editorDefaultFontStyle = value;
                //OnDefaultFontStyleChanged?.Invoke(null, value);
                NotiyFontChange(fontStyle: true);
                ApplicationSettingsStore.Write(SettingsKey.EditorFontStyleStr, value.ToString());
            }
        }

        private static FontWeight _editorDefaultFontWeight;

        public static FontWeight EditorDefaultFontWeight
        {
            get => _editorDefaultFontWeight;
            set
            {
                _editorDefaultFontWeight = value;
                //OnDefaultFontWeightChanged?.Invoke(null, value);
                NotiyFontChange(fontWeight: true);
                ApplicationSettingsStore.Write(SettingsKey.EditorFontWeightUshort, value.Weight);
            }
        }

        private static string _editorAltFontFamily;

        public static string EditorAltFontFamily
        {
            get => _editorAltFontFamily;
            set
            {
                _editorAltFontFamily = value;
                //OnAltFontFamilyChanged?.Invoke(null, value);
                NotiyFontChange(fontFamily: true);
                ApplicationSettingsStore.Write(SettingsKey.EditorAltFontFamilyStr, value);
            }
        }

        private static int _editorAltFontSize;

        public static int EditorAltFontSize
        {
            get => _editorAltFontSize;
            set
            {
                _editorAltFontSize = value;
                //OnAltFontSizeChanged?.Invoke(null, value);
                NotiyFontChange(fontSize: true);
                ApplicationSettingsStore.Write(SettingsKey.EditorAltFontSizeInt, value);
            }
        }

        private static FontStyle _editorAltFontStyle;

        public static FontStyle EditorAltFontStyle
        {
            get => _editorAltFontStyle;
            set
            {
                _editorAltFontStyle = value;
                //OnAltFontStyleChanged?.Invoke(null, value);
                NotiyFontChange(fontStyle: true);
                ApplicationSettingsStore.Write(SettingsKey.EditorAltFontStyleStr, value.ToString());
            }
        }

        private static FontWeight _editorAltFontWeight;

        public static FontWeight EditorAltFontWeight
        {
            get => _editorAltFontWeight;
            set
            {
                _editorAltFontWeight = value;
                //OnAltFontWeightChanged?.Invoke(null, value);
                NotiyFontChange(fontWeight: true);
                ApplicationSettingsStore.Write(SettingsKey.EditorAltFontWeightUshort, value.Weight);
            }
        }

        private static TextWrapping _editorDefaultTextWrapping;

        public static TextWrapping EditorDefaultTextWrapping
        {
            get => _editorDefaultTextWrapping;
            set
            {
                _editorDefaultTextWrapping = value;
                OnDefaultTextWrappingChanged?.Invoke(null, value);
                ApplicationSettingsStore.Write(SettingsKey.EditorDefaultTextWrappingStr, value.ToString());
            }
        }

        private static bool _editorDisplayLineHighlighter;

        public static bool EditorDisplayLineHighlighter
        {
            get => _editorDisplayLineHighlighter;
            set
            {
                _editorDisplayLineHighlighter = value;
                OnDefaultLineHighlighterViewStateChanged?.Invoke(null, value);
                ApplicationSettingsStore.Write(SettingsKey.EditorDefaultLineHighlighterViewStateBool, value);
            }
        }

        private static LineEnding _editorDefaultLineEnding;

        public static LineEnding EditorDefaultLineEnding
        {
            get => _editorDefaultLineEnding;
            set
            {
                _editorDefaultLineEnding = value;
                OnDefaultLineEndingChanged?.Invoke(null, value);
                ApplicationSettingsStore.Write(SettingsKey.EditorDefaultLineEndingStr, value.ToString());
            }
        }

        private static Encoding _editorDefaultEncoding;

        public static Encoding EditorDefaultEncoding
        {
            get => _editorDefaultEncoding;
            set
            {
                _editorDefaultEncoding = value;

                if (value is UTF8Encoding)
                {
                    ApplicationSettingsStore.Write(SettingsKey.EditorDefaultUtf8EncoderShouldEmitByteOrderMarkBool,
                        Equals(value, new UTF8Encoding(true)));
                }

                OnDefaultEncodingChanged?.Invoke(null, value);
                ApplicationSettingsStore.Write(SettingsKey.EditorDefaultEncodingCodePageInt, value.CodePage);
            }
        }

        private static Encoding _editorDefaultDecoding;

        public static Encoding EditorDefaultDecoding
        {
            get
            {
                if (_editorDefaultDecoding == null)
                {
                    return null;
                }
                // If it is not UTF-8 meaning user is using ANSI decoding,
                // We should always try get latest system ANSI code page.
                else if (!(_editorDefaultDecoding is UTF8Encoding))
                {
                    if (EncodingUtility.TryGetSystemDefaultANSIEncoding(out var systemDefaultANSIEncoding))
                    {
                        _editorDefaultDecoding = systemDefaultANSIEncoding;
                    }
                    else if (EncodingUtility.TryGetCurrentCultureANSIEncoding(out var currentCultureANSIEncoding))
                    {
                        _editorDefaultDecoding = currentCultureANSIEncoding;
                    }
                    else
                    {
                        _editorDefaultDecoding = new UTF8Encoding(false); // Fall back to UTF-8 (no BOM)
                    }
                }
                return _editorDefaultDecoding;
            }
            set
            {
                _editorDefaultDecoding = value;
                var codePage = value?.CodePage ?? -1;
                ApplicationSettingsStore.Write(SettingsKey.EditorDefaultDecodingCodePageInt, codePage);
            }
        }

        private static int _editorDefaultTabIndents;

        public static int EditorDefaultTabIndents
        {
            get => _editorDefaultTabIndents;
            set
            {
                _editorDefaultTabIndents = value;
                OnDefaultTabIndentsChanged?.Invoke(null, value);
                ApplicationSettingsStore.Write(SettingsKey.EditorDefaultTabIndentsInt, value);
            }
        }

        private static SearchEngine _editorDefaultSearchEngine;

        public static SearchEngine EditorDefaultSearchEngine
        {
            get => _editorDefaultSearchEngine;
            set
            {
                _editorDefaultSearchEngine = value;
                ApplicationSettingsStore.Write(SettingsKey.EditorDefaultSearchEngineStr, value.ToString());
            }
        }

        private static string _editorCustomMadeSearchUrl;

        public static string EditorCustomMadeSearchUrl
        {
            get => _editorCustomMadeSearchUrl;
            set
            {
                _editorCustomMadeSearchUrl = value;
                ApplicationSettingsStore.Write(SettingsKey.EditorCustomMadeSearchUrlStr, value);
            }
        }

        private static bool _showStatusBar;

        public static bool ShowStatusBar
        {
            get => _showStatusBar;
            set
            {
                _showStatusBar = value;
                OnStatusBarVisibilityChanged?.Invoke(null, value);
                ApplicationSettingsStore.Write(SettingsKey.EditorShowStatusBarBool, value);
            }
        }

        private static bool _isSessionSnapshotEnabled;

        public static bool IsSessionSnapshotEnabled
        {
            get => _isSessionSnapshotEnabled;
            set
            {
                _isSessionSnapshotEnabled = value;
                OnSessionBackupAndRestoreOptionChanged?.Invoke(null, value);
                ApplicationSettingsStore.Write(SettingsKey.EditorEnableSessionBackupAndRestoreBool, value);
            }
        }

        private static bool _isHighlightMisspelledWordsEnabled;

        public static bool IsHighlightMisspelledWordsEnabled
        {
            get => _isHighlightMisspelledWordsEnabled;
            set
            {
                _isHighlightMisspelledWordsEnabled = value;
                OnHighlightMisspelledWordsChanged?.Invoke(null, value);
                ApplicationSettingsStore.Write(SettingsKey.EditorHighlightMisspelledWordsBool, value);
            }
        }

        private static bool _alwaysOpenNewWindow;

        public static bool AlwaysOpenNewWindow
        {
            get => _alwaysOpenNewWindow;
            set
            {
                _alwaysOpenNewWindow = value;
                ApplicationSettingsStore.Write(SettingsKey.AlwaysOpenNewWindowBool, value);
            }
        }

        private static bool _displayLineNumbers;

        public static bool EditorDisplayLineNumbers
        {
            get => _displayLineNumbers;
            set
            {
                _displayLineNumbers = value;
                OnDefaultDisplayLineNumbersViewStateChanged?.Invoke(null, value);
                ApplicationSettingsStore.Write(SettingsKey.EditorDefaultDisplayLineNumbersBool, value);
            }
        }

        private static bool _isSmartCopyEnabled;

        public static bool IsSmartCopyEnabled
        {
            get => _isSmartCopyEnabled;
            set
            {
                _isSmartCopyEnabled = value;
                ApplicationSettingsStore.Write(SettingsKey.EditorEnableSmartCopyBool, value);
            }
        }

        public static void Initialize()
        {
            InitializeFontSettings();

            InitializeAltFontSettings();

            InitializeTextWrappingSettings();

            InitializeSpellingSettings();

            InitializeDisplaySettings();

            InitializeSmartCopySettings();

            InitializeLineEndingSettings();

            InitializeEncodingSettings();

            InitializeDecodingSettings();

            InitializeTabIndentsSettings();

            InitializeSearchEngineSettings();

            InitializeStatusBarSettings();

            InitializeSessionSnapshotSettings();

            InitializeAppOpeningPreferencesSettings();
        }

        private static void InitializeStatusBarSettings()
        {
            if (ApplicationSettingsStore.Read(SettingsKey.EditorShowStatusBarBool) is bool showStatusBar)
            {
                _showStatusBar = showStatusBar;
            }
            else
            {
                _showStatusBar = true;
            }
        }

        private static void InitializeSessionSnapshotSettings()
        {
            // We should disable session snapshot feature on multi instances
            if (!App.IsPrimaryInstance)
            {
                _isSessionSnapshotEnabled = false;
            }
            else if (App.IsGameBarWidget)
            {
                _isSessionSnapshotEnabled = true;
            }
            else
            {
                if (ApplicationSettingsStore.Read(SettingsKey.EditorEnableSessionBackupAndRestoreBool) is bool enableSessionBackupAndRestore)
                {
                    _isSessionSnapshotEnabled = enableSessionBackupAndRestore;
                }
                else
                {
                    _isSessionSnapshotEnabled = false;
                }
            }
        }

        private static void InitializeLineEndingSettings()
        {
            if (ApplicationSettingsStore.Read(SettingsKey.EditorDefaultLineEndingStr) is string lineEndingStr &&
                Enum.TryParse(typeof(LineEnding), lineEndingStr, out var lineEnding))
            {
                _editorDefaultLineEnding = (LineEnding)lineEnding;
            }
            else
            {
                _editorDefaultLineEnding = LineEnding.Crlf;
            }
        }

        private static void InitializeTextWrappingSettings()
        {
            if (ApplicationSettingsStore.Read(SettingsKey.EditorDefaultTextWrappingStr) is string textWrappingStr &&
                Enum.TryParse(typeof(TextWrapping), textWrappingStr, out var textWrapping))
            {
                _editorDefaultTextWrapping = (TextWrapping)textWrapping;
            }
            else
            {
                _editorDefaultTextWrapping = TextWrapping.NoWrap;
            }
        }

        private static void InitializeSpellingSettings()
        {
            if (ApplicationSettingsStore.Read(SettingsKey.EditorHighlightMisspelledWordsBool) is bool highlightMisspelledWords)
            {
                _isHighlightMisspelledWordsEnabled = highlightMisspelledWords;
            }
            else
            {
                _isHighlightMisspelledWordsEnabled = false;
            }
        }

        private static void InitializeDisplaySettings()
        {
            if (ApplicationSettingsStore.Read(SettingsKey.EditorDefaultLineHighlighterViewStateBool) is bool displayLineHighlighter)
            {
                _editorDisplayLineHighlighter = displayLineHighlighter;
            }
            else
            {
                _editorDisplayLineHighlighter = true;
            }

            if (ApplicationSettingsStore.Read(SettingsKey.EditorDefaultDisplayLineNumbersBool) is bool displayLineNumbers)
            {
                _displayLineNumbers = displayLineNumbers;
            }
            else
            {
                _displayLineNumbers = true;
            }
        }

        private static void InitializeSmartCopySettings()
        {
            if (ApplicationSettingsStore.Read(SettingsKey.EditorEnableSmartCopyBool) is bool enableSmartCopy)
            {
                _isSmartCopyEnabled = enableSmartCopy;
            }
            else
            {
                _isSmartCopyEnabled = false;
            }
        }

        private static void InitializeEncodingSettings()
        {
            Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            if (ApplicationSettingsStore.Read(SettingsKey.EditorDefaultEncodingCodePageInt) is int encodingCodePage)
            {
                var encoding = Encoding.GetEncoding(encodingCodePage);

                if (encoding is UTF8Encoding)
                {
                    if (ApplicationSettingsStore.Read(SettingsKey.EditorDefaultUtf8EncoderShouldEmitByteOrderMarkBool) is bool shouldEmitBom)
                    {
                        encoding = new UTF8Encoding(shouldEmitBom);
                    }
                    else
                    {
                        encoding = new UTF8Encoding(false);
                    }
                }

                _editorDefaultEncoding = encoding;
            }
            else
            {
                _editorDefaultEncoding = new UTF8Encoding(false);
            }
        }

        private static void InitializeDecodingSettings()
        {
            if (ApplicationSettingsStore.Read(SettingsKey.EditorDefaultDecodingCodePageInt) is int decodingCodePage)
            {
                try
                {
                    if (decodingCodePage == -1)
                    {
                        _editorDefaultDecoding = null; // Meaning we should guess encoding during runtime
                    }
                    else
                    {
                        Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                        _editorDefaultDecoding = Encoding.GetEncoding(decodingCodePage);
                        if (_editorDefaultDecoding is UTF8Encoding)
                        {
                            _editorDefaultDecoding = new UTF8Encoding(false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.LogError($"[{nameof(AppSettingsService)}] Failed to get encoding, code page: {decodingCodePage}, ex: {ex.Message}");
                    _editorDefaultDecoding = null;
                }
            }
            else
            {
                _editorDefaultDecoding = null; // Default to null
            }
        }

        private static void InitializeTabIndentsSettings()
        {
            if (ApplicationSettingsStore.Read(SettingsKey.EditorDefaultTabIndentsInt) is int tabIndents)
            {
                _editorDefaultTabIndents = tabIndents;
            }
            else
            {
                _editorDefaultTabIndents = -1;
            }
        }

        private static void InitializeSearchEngineSettings()
        {
            if (ApplicationSettingsStore.Read(SettingsKey.EditorDefaultSearchEngineStr) is string searchEngineStr &&
                Enum.TryParse(typeof(SearchEngine), searchEngineStr, out var searchEngine))
            {
                _editorDefaultSearchEngine = (SearchEngine)searchEngine;
            }
            else
            {
                _editorDefaultSearchEngine = SearchEngine.Bing;
            }

            if (ApplicationSettingsStore.Read(SettingsKey.EditorCustomMadeSearchUrlStr) is string customMadeSearchUrl)
            {
                _editorCustomMadeSearchUrl = customMadeSearchUrl;
            }
            else
            {
                _editorCustomMadeSearchUrl = string.Empty;
            }
        }

        private static void InitializeFontSettings()
        {
            if (ApplicationSettingsStore.Read(SettingsKey.EditorFontFamilyStr) is string fontFamily)
            {
                _editorDefaultFontFamily = fontFamily;
            }
            else
            {
                _editorDefaultFontFamily = "Consolas";
            }

            if (ApplicationSettingsStore.Read(SettingsKey.EditorFontSizeInt) is int fontSize)
            {
                _editorDefaultFontSize = fontSize;
            }
            else
            {
                _editorDefaultFontSize = 14;
            }

            if (ApplicationSettingsStore.Read(SettingsKey.EditorFontStyleStr) is string fontStyleStr &&
                Enum.TryParse(typeof(FontStyle), fontStyleStr, out var fontStyle))
            {
                _editorDefaultFontStyle = (FontStyle)fontStyle;
            }
            else
            {
                _editorDefaultFontStyle = FontStyle.Normal;
            }

            if (ApplicationSettingsStore.Read(SettingsKey.EditorFontWeightUshort) is ushort fontWeight)
            {
                _editorDefaultFontWeight = new FontWeight()
                {
                    Weight = fontWeight
                };
            }
            else
            {
                _editorDefaultFontWeight = FontWeights.Normal;
            }
        }

        private static void InitializeAltFontSettings()
        {
            if (ApplicationSettingsStore.Read(SettingsKey.EditorAltFontFamilyStr) is string fontFamily)
            {
                _editorAltFontFamily = fontFamily;
            }
            else
            {
                _editorAltFontFamily = "Arial";
            }

            if (ApplicationSettingsStore.Read(SettingsKey.EditorAltFontSizeInt) is int fontSize)
            {
                _editorAltFontSize = fontSize;
            }
            else
            {
                _editorAltFontSize = 14;
            }

            if (ApplicationSettingsStore.Read(SettingsKey.EditorAltFontStyleStr) is string fontStyleStr &&
                Enum.TryParse(typeof(FontStyle), fontStyleStr, out var fontStyle))
            {
                _editorAltFontStyle = (FontStyle)fontStyle;
            }
            else
            {
                _editorAltFontStyle = FontStyle.Normal;
            }

            if (ApplicationSettingsStore.Read(SettingsKey.EditorAltFontWeightUshort) is ushort fontWeight)
            {
                _editorAltFontWeight = new FontWeight()
                {
                    Weight = fontWeight
                };
            }
            else
            {
                _editorAltFontWeight = FontWeights.Normal;
            }
        }

        private static void InitializeAppOpeningPreferencesSettings()
        {
            if (ApplicationSettingsStore.Read(SettingsKey.AlwaysOpenNewWindowBool) is bool alwaysOpenNewWindow)
            {
                _alwaysOpenNewWindow = alwaysOpenNewWindow;
            }
            else
            {
                _alwaysOpenNewWindow = false;
            }
        }
    }
}