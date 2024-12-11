using Modem.Geo.VideoRelayerDesktop.Core.Classes;
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
        private CameraCollection _collection;
        public MainWindow()
        {
            InitializeComponent();
            _collection = new CameraCollection();
            AddCamera("c1", "1");
            AddCamera("c1", "2");
            AddCamera("c3", "3");
            List<Camera> list = _collection.GetCameraCollection();
            foreach (Camera c in list) 
            {
                CreateButton(c.GetName(), c.GetStreamKey());
            }
        }

        public void AddCamera(string Name, string StreamKey)
        {
            Response resp;
            resp = _collection.AddCamera(new Camera(StreamKey, Name));
            if(resp.Status == Core.Enums.Status.Error)
            {
                InvokeErrorWindow(resp.Message);
            }
        }

        public void CreateButton(string Name, string StreamKey)
        {
            Button button = new Button();
            button.Content = Name;
            button.Name = Name;
            CameraButtons.Children.Add(button);
        }

        public void InvokeErrorWindow(string message)
        {
            MessageBox.Show(message, "Ошибка");
        }
    }
}
