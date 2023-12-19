global using System;
global using System.IO;
global using System.Windows;

global using ThSpellCardRecordViewer.Extensions;
global using ThSpellCardRecordViewer.Score;
global using ThSpellCardRecordViewer.Settings;

using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace ThSpellCardRecordViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string? _appName = VersionInfo.AppName;
        private string? _gameId;

        public string GameId 
        {
            get
            {
                return _gameId;
            }

            set
            {
                _gameId = value;
                if (!string.IsNullOrEmpty(value))
                {
                    ScoreFilePathBox.Text = ScoreFilePath.GetScoreFilePath(value);
                    ViewSpellCardRecord();
                }
            }
        }

        private readonly Dictionary<string, int> GameDictionary =
            new()
            {
                { "Th06", 0 },
                { "Th07", 1 },
                { "Th08", 2 },
                { "Th09", 3 },
                { "Th10", 4 },
                { "Th11", 5 },
                { "Th12", 6 },
                { "Th13", 7 },
                { "Th14", 8 },
                { "Th15", 9 },
                { "Th16", 10 },
                { "Th17", 11 },
                { "Th18", 12 }
            };


        public MainWindow()
        {
            InitializeComponent();

            this.Title = _appName;

            this.GameId = string.Empty;

            try
            {
                SettingsConfiguration.ConfigureScoreFilePathSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"スコアファイルパス設定の構成に失敗\n{ex.Message}", "エラー",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            try
            {
                ConfigureMainWindowSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"メインウィンドウ設定の構成に失敗\n{ex.Message}", "エラー",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ViewSpellCardRecord()
        {
            bool displayNotChallengedCardName = DisplayNotChallengedCardMenuItem.IsChecked;
            if (GameComboBox.SelectedIndex > -1)
            {
                string gameId = this.GameId;

                try
                {
                    EnableLimitationMode(true);
                    await Task.Run(()
                        => SpellCardRecord.GetSpellCardRecord(gameId, displayNotChallengedCardName)
                    );
                    if (SpellCardRecord.SpellCardRecordDataLists.Count >= 0)
                    {
                        SpellCardRecordDataGrid.DataContext = SpellCardRecord.SpellCardRecordDataLists;
                    }


                    EnableLimitationMode(false);
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show(this, ex.Message, "エラー",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EnableLimitationMode(bool enabled)
        {
            GameComboBox.IsEnabled = !enabled;
            OpenScoreFileMenuItem.IsEnabled = !enabled;
            OpenScoreFileButton.IsEnabled = !enabled;
        }

        private void ConfigureMainWindowSettings()
        {
            MainWindowSettings mainWindowSettings = SettingsConfiguration.ConfigureMainWindowSettings();
            this.Width = mainWindowSettings.MainWindowWidth;
            this.Height = mainWindowSettings.MainWindowHeight;

            DisplayNotChallengedCardMenuItem.IsChecked = mainWindowSettings.DisplayUnchallengedCardName;

            string? selectedGameId = mainWindowSettings.SelectedGameId;
            if (!string.IsNullOrEmpty(selectedGameId))
            {
                GameComboBox.SelectedIndex = GameDictionary[selectedGameId];
            }
            else
            {
                GameComboBox.SelectedIndex = 0;
            }
        }

        private void SaveMainWindowSettings()
        {
            MainWindowSettings mainWindowSettings = new()
            {
                MainWindowWidth = this.Width,
                MainWindowHeight = this.Height,
                SelectedGameId = this.GameId,
                DisplayUnchallengedCardName = DisplayNotChallengedCardMenuItem.IsChecked
            };

            SettingsConfiguration.SaveMainWindowSettings(mainWindowSettings);
        }

        private void ExitMenuItemClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OpenScoreFileButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            { 
                Filter = "スコアデータファイル|score*.dat",
                Title = "スコアファイルを指定してください"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ScoreFilePathBox.Text = openFileDialog.FileName;

                ScoreFilePath.SetScoreFilePath(this.GameId, openFileDialog.FileName);

                ViewSpellCardRecord();
            }
        }

        private void GameComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem gameItem = (ComboBoxItem)GameComboBox.SelectedItem;
            string gameId = gameItem.Uid;
            if (gameId != null) 
            {
                this.GameId = gameId;
            }
            else
            {
                this.GameId = string.Empty;
            }
        }

        private void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                SettingsConfiguration.SaveScoreFilePathSettings();
                SaveMainWindowSettings();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
