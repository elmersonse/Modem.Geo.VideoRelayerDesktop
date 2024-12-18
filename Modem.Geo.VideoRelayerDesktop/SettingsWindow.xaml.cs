using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Modem.Geo.VideoRelayerDesktop
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            BatPath.Text = Properties.Settings.Default.batPath;
            Api.Text = Properties.Settings.Default.api;
        }

        private void SavePath_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.batPath = BatPath.Text;
            Properties.Settings.Default.Save();
        }

        private void SaveApi_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.api = Api.Text;
            Properties.Settings.Default.Save();
        }
    }
}
