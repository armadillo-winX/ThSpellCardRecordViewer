global using System;
global using System.IO;
global using System.Windows;

global using ThSpellCardRecordViewer.Extensions;
global using ThSpellCardRecordViewer.Score;

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
    }
}
