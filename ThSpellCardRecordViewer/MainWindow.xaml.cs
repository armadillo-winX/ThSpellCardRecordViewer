global using System;
global using System.IO;
global using System.Windows;

global using ThSpellCardRecordViewer.Extensions;
global using ThSpellCardRecordViewer.Score;

using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
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
                    BrowseScoreFileButton.IsEnabled = false;
                    GameComboBox.IsEnabled = false;
                    await Task.Run(()
                        => SpellCardRecord.GetSpellCardRecord(gameId, displayNotChallengedCardName)
                    );
                    if (SpellCardRecord.SpellCardRecordDataLists.Count >= 0)
                    {
                        SpellCardRecordDataGrid.DataContext = SpellCardRecord.SpellCardRecordDataLists;
                    }
                    BrowseScoreFileButton.IsEnabled = true;
                    GameComboBox.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show(this, ex.Message, "エラー",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
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

        private void BrowseScoreFileButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            { 
                Filter = "スコアデータファイル|score*.dat",
                Title = "スコアファイルを指定してください"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ScoreFilePathBox.Text = openFileDialog.FileName;
                if (GetSeletedGameId() == GameIndex.Th06)
                {
                    ScoreFilePath.Th06ScoreFile = openFileDialog.FileName;
                }
                else if (GetSeletedGameId() == GameIndex.Th07)
                {
                    ScoreFilePath.Th07ScoreFile = openFileDialog.FileName;
                }

                ViewSpellCardRecord();
            }
        }
    }
}
