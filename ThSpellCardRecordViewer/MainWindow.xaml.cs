﻿global using System;
global using System.IO;
global using System.Windows;

global using ThSpellCardRecordViewer.Extensions;
global using ThSpellCardRecordViewer.Score;
global using ThSpellCardRecordViewer.Settings;

using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ThSpellCardRecordViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SpellCardRecordDetailDialog? _spellCardRecordDetailDialog = null;

        private string? _appName = VersionInfo.AppName;
        private string? _gameId;
        private string? _enemyFilter;

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
                    SetEnemyFilter();
                    ScoreFilePathBox.Text = ScoreFilePath.GetScoreFilePath(value);
                    ViewSpellCardRecord();
                }
            }
        }

        public string EnemyFilter
        {
            get
            {
                return _enemyFilter;
            }

            set
            {
                _enemyFilter = value;
                if (value != "ALL")
                {
                    FilteredEnemyBlock.Text = value;
                }
                else
                {
                    FilteredEnemyBlock.Text = string.Empty;
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

        private readonly Dictionary<int, string> ThemeDictionary =
            new()
            {
                { 0, "Light" },
                { 1, "Dark"  },
                { 2, "Black" },
                { 3, "NormalColor" }
            };

        public MainWindow()
        {
            InitializeComponent();

            this.Title = _appName;

            this.GameId = string.Empty;
            this.EnemyFilter = "ALL";

            ApplicationTheme.ThemeName = "Light";

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
                        if (this.EnemyFilter == "ALL")
                        {
                            SpellCardRecordDataGrid.DataContext = SpellCardRecord.SpellCardRecordDataLists;
                        }
                        else
                        {
                            FilterSpellCardRecordByEnemy();
                        }
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
            SpellCardRecordDataGrid.IsEnabled = !enabled;
            OpenScoreFileMenuItem.IsEnabled = !enabled;
            OpenScoreFileButton.IsEnabled = !enabled;
            ReloadMenuItem.IsEnabled = !enabled;
            SpellCardRecordDataStaticsMenuItem.IsEnabled = !enabled;
        }

        private void ConfigureMainWindowSettings()
        {
            MainWindowSettings mainWindowSettings = SettingsConfiguration.ConfigureMainWindowSettings();
            this.Width = mainWindowSettings.MainWindowWidth;
            this.Height = mainWindowSettings.MainWindowHeight;

            DisplayNotChallengedCardMenuItem.IsChecked = mainWindowSettings.DisplayUnchallengedCardName;
            ThemeSettingsComboBox.SelectedIndex = mainWindowSettings.ThemeIndex;

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
                DisplayUnchallengedCardName = DisplayNotChallengedCardMenuItem.IsChecked,
                ThemeIndex = ThemeSettingsComboBox.SelectedIndex
            };

            SettingsConfiguration.SaveMainWindowSettings(mainWindowSettings);
        }

        private void SetEnemyFilter()
        {
            EnemyFilterContextMenu.Items.Clear();

            MenuItem allItem = new()
            {
                Header = "ALL"
            };
            allItem.Click += new RoutedEventHandler(EnemyFilterMenuItemClick);
            EnemyFilterContextMenu.Items.Add(allItem);

            Separator separator = new();
            EnemyFilterContextMenu.Items.Add(separator);

            string gameId = this.GameId;
            string[] gameEnemies = GameEnemies.GetGameEnemies(gameId);
            if (gameEnemies != null)
            {
                foreach (string gameEnemy in gameEnemies)
                {
                    MenuItem item = new()
                    {
                        Header = gameEnemy
                    };
                    item.Click += new RoutedEventHandler(EnemyFilterMenuItemClick);
                    EnemyFilterContextMenu.Items.Add(item);
                }
            }
        }

        private void EnemyFilterMenuItemClick(object sender, RoutedEventArgs e)
        {
            string enemyName = ((MenuItem)sender).Header.ToString();
            if (enemyName != null)
            {
                this.EnemyFilter = enemyName;
            }
            else
            {
                this.EnemyFilter = "ALL";
            }

            if (enemyName == "ALL")
            {
                SpellCardRecordDataGrid.DataContext = SpellCardRecord.SpellCardRecordDataLists;
            }
            else
            {
                FilterSpellCardRecordByEnemy();
            }
        }

        private void FilterSpellCardRecordByEnemy()
        {
            ObservableCollection<SpellCardRecordData> spellCardRecordDataLists
                    = SpellCardRecord.SpellCardRecordDataLists;
            if (spellCardRecordDataLists != null && spellCardRecordDataLists.Count > 0)
            {
                ObservableCollection<SpellCardRecordData> filteredSpellCardRecordDataLists = new();
                foreach (SpellCardRecordData spellCardRecordData in spellCardRecordDataLists)
                {
                    if (spellCardRecordData.Enemy == this.EnemyFilter)
                    {
                        filteredSpellCardRecordDataLists.Add(spellCardRecordData);
                    }
                }

                if (filteredSpellCardRecordDataLists != null && filteredSpellCardRecordDataLists.Count > 0)
                {
                    SpellCardRecordDataGrid.DataContext = filteredSpellCardRecordDataLists;
                }
            }
            else
            {
                MessageBox.Show(this, "御札戦歴データが空です。", _appName,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                this.EnemyFilter = "ALL";
            }
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

            this.EnemyFilter = "ALL";
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

        private void DisplayNotChallengedCardMenuItemClick(object sender, RoutedEventArgs e)
        {
            ViewSpellCardRecord();
        }

        private void AboutMenuItemClick(object sender, RoutedEventArgs e)
        {
            AboutDialog aboutDialog = new()
            {
                Owner = this
            };
            aboutDialog.ShowDialog();
        }

        private void EnemyFilterButtonClick(object sender, RoutedEventArgs e)
        {
            if (!EnemyFilterContextMenu.IsOpen)
            {
                EnemyFilterContextMenu.PlacementTarget = EnemyFilterButton;
                EnemyFilterContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                EnemyFilterContextMenu.IsOpen = true;
            }
            else
            {
                EnemyFilterContextMenu.IsOpen = false;
            }
        }

        private void ReloadMenuItemClick(object sender, RoutedEventArgs e)
        {
            ViewSpellCardRecord();
        }

        private void ThemeSettingsComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ThemeSettingsComboBox.SelectedIndex > -1)
            {
                string themeName = ThemeDictionary[ThemeSettingsComboBox.SelectedIndex];
                ApplicationTheme.ThemeName = themeName;
            }
            else
            {
                ApplicationTheme.ThemeName = "Light";
            }
        }

        private void ViewSpellCardRecordDetailMenuItemClick(object sender, RoutedEventArgs e)
        {
            if (SpellCardRecord.SpellCardRecordDataLists != null 
                && SpellCardRecord.SpellCardRecordDataLists.Count > 0
                && SpellCardRecordDataGrid.SelectedIndex >= 0
                && SpellCardRecordDataGrid.SelectedIndex < SpellCardRecord.SpellCardRecordDataLists.Count)
            {
                int cardId = int.Parse(((SpellCardRecordData)SpellCardRecordDataGrid.SelectedItem).CardId);

                SpellCardRecordData spellCardRecordData
                    = SpellCardRecord.SpellCardRecordDataLists[cardId - 1];

                if (_spellCardRecordDetailDialog == null ||
                !_spellCardRecordDetailDialog.IsLoaded)
                {
                    _spellCardRecordDetailDialog = new SpellCardRecordDetailDialog
                    {
                        Owner = this,
                        SpellCardRecordData = spellCardRecordData
                    };
                    _spellCardRecordDetailDialog.Show();
                }
                else
                {
                    _spellCardRecordDetailDialog.SpellCardRecordData = spellCardRecordData;
                    _spellCardRecordDetailDialog.WindowState = WindowState.Normal;
                    _spellCardRecordDetailDialog.Activate();
                }
            }
        }

        private void SpellCardRecordDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SpellCardRecord.SpellCardRecordDataLists != null
                && SpellCardRecord.SpellCardRecordDataLists.Count > 0
                && SpellCardRecordDataGrid.SelectedIndex >= 0
                && SpellCardRecordDataGrid.SelectedIndex < SpellCardRecord.SpellCardRecordDataLists.Count)
            {
                int cardId = int.Parse(((SpellCardRecordData)SpellCardRecordDataGrid.SelectedItem).CardId);

                SpellCardRecordData spellCardRecordData 
                    = SpellCardRecord.SpellCardRecordDataLists[cardId - 1];

                if (_spellCardRecordDetailDialog != null &&
                _spellCardRecordDetailDialog.IsLoaded)
                {
                    _spellCardRecordDetailDialog.SpellCardRecordData = spellCardRecordData;
                    _spellCardRecordDetailDialog.WindowState = WindowState.Normal;
                }
            }
        }

        private void SpellCardRecordDataStaticsMenuItemClick(object sender, RoutedEventArgs e)
        {
            if (SpellCardRecord.SpellCardRecordDataLists != null &&
                SpellCardRecord.SpellCardRecordDataLists.Count > 0)
            {
                SpellCardRecordDataStaticsDialog spellCardRecordDataStaticsDialog = new()
                {
                    Owner = this
                };
                spellCardRecordDataStaticsDialog.ShowDialog();
            }
            else
            {
                MessageBox.Show(this, "御札戦歴データが空です。", _appName,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void CopySpellCardRecordMenuItemClick(object sender, RoutedEventArgs e)
        {
            if (SpellCardRecordDataGrid.SelectedIndex > -1
                && SpellCardRecordDataGrid.SelectedIndex < SpellCardRecord.SpellCardRecordDataLists.Count)
            {
                SpellCardRecordData spellCardRecordData = (SpellCardRecordData)SpellCardRecordDataGrid.SelectedItem;

                string info =
                    $"ID:{spellCardRecordData.CardId}\r\n" +
                    $"{spellCardRecordData.CardName}\r\n" +
                    $"取得数:{spellCardRecordData.Get}\r\n" +
                    $"挑戦数:{spellCardRecordData.Challenge}\r\n" +
                    $"取得率:{spellCardRecordData.GetRate}\r\n" +
                    $"発動場所:{spellCardRecordData.Place}\r\n" +
                    $"敵機:{spellCardRecordData.Enemy}\r\n";

                Clipboard.SetText(info);
            }
        }

        private void CopySpellCardNameMenuItemClick(object sender, RoutedEventArgs e)
        {
            if (SpellCardRecordDataGrid.SelectedIndex > -1)
            {
                SpellCardRecordData spellCardRecordData = (SpellCardRecordData)SpellCardRecordDataGrid.SelectedItem;

                Clipboard.SetText(spellCardRecordData.CardName);
            }
        }
    }
}
