namespace ThSpellCardRecordViewer
{
    /// <summary>
    /// AboutDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class AboutDialog : Window
    {
        public AboutDialog()
        {
            InitializeComponent();

            AppNameBlock.Text = VersionInfo.AppName;
            AppVersionBlock.Text = $"Version.{VersionInfo.AppVersion}";
            DeveloperBlock.Text = $"by {VersionInfo.Developer}";
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
