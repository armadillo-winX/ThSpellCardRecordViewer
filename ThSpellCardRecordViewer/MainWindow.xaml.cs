﻿global using System;
global using System.IO;
global using System.Windows;

global using ThSpellCardRecordViewer.Extensions;
global using ThSpellCardRecordViewer.Score;

using Microsoft.Win32;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();

            this.Title = _appName;
        }

        private async void ViewSpellCardRecord()
        {
            bool displayNotChallengedCardName = DisplayNotChallengedCardMenuItem.IsChecked;
            if (GameComboBox.SelectedIndex > -1)
            {
                string gameId = GetSeletedGameId();

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

        private string GetSeletedGameId()
        {
            ComboBoxItem gameItem = (ComboBoxItem)GameComboBox.SelectedItem;
            return gameItem.Uid;
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

                ScoreFilePath.SetScoreFilePath(GetSeletedGameId(), openFileDialog.FileName);

                ViewSpellCardRecord();
            }
        }

        private void GameComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string gameId = GetSeletedGameId();
            if (gameId != null) 
            {
                ScoreFilePathBox.Text = ScoreFilePath.GetScoreFilePath(gameId);
                ViewSpellCardRecord();
            }
        }
    }
}
