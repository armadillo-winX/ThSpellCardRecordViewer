﻿using DynamicAero2;

namespace ThSpellCardRecordViewer
{
    internal class ApplicationTheme
    {
        private static string? _themeName;

        public static string? ThemeName
        {
            get
            {
                return _themeName;
            }

            set
            {
                _themeName = value;
                SetApplicationTheme(_themeName);
            }
        }

        private static void SetApplicationTheme(string? themeName)
        {
            Theme theme = Application.Current.Resources.MergedDictionaries[0] as Theme;
            if (theme != null)
            {
                if (themeName == "Light")
                {
                    theme.Color = ThemeColor.Light;
                }
                else if (themeName == "Dark")
                {
                    theme.Color = ThemeColor.Dark;
                }
                else if (themeName == "Black")
                {
                    theme.Color = ThemeColor.Black;
                }
                else
                {
                    theme.Color = ThemeColor.NormalColor;
                }
            }
        }
    }
}
