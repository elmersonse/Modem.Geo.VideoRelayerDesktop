using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CreateButton("b1", "b1");
            CreateButton("b2", "b2");
            CreateButton("b3", "b3");
        }

        public void CreateButton(string Name, string StreamKey)
        {
            Button button = new Button();
            button.Content = Name;
            button.Name = StreamKey;
            CameraButtons.Children.Add(button);
        }
    }
}
