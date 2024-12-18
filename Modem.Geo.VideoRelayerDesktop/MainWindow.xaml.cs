using Modem.Geo.VideoRelayerDesktop.Core.Classes;
using Modem.Geo.VideoRelayerDesktop.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private BatExecutionService _batExecutionService = BatExecutionService.GetInstance();
        private ProcessDictionaryService _processDictionaryService = ProcessDictionaryService.GetInstanse();
        public MainWindow()
        {
            InitializeComponent();
            _collection = new CameraCollection();
            AddCamera("c1", "1");
            AddCamera("c2", "2");
            AddCamera("c3", "3");
            CreateButtonsFromList();
            PageFrame.Source = new Uri("ProcessInfoPage.xaml", UriKind.Relative);
        }

        public void AddCamera(string name, string streamKey)
        {
            Response<string> resp;
            resp = _collection.AddCamera(new Camera(streamKey, name));
            if(resp.Status == Core.Enums.Status.Error)
            {
                InvokeErrorWindow(resp.Message);
            }
        }

        public void CreateButton(string name, string streamKey)
        {
            Button button = new Button();
            button.Content = name;
            button.Name = $"N{streamKey}";
            button.Click += ButtonClick;
            CameraButtons.Children.Add(button);
            Response<string> resp = _batExecutionService.CreateBat(name, streamKey);
            if(resp.Status == Core.Enums.Status.Error)
            {
                InvokeErrorWindow(resp.Message);
            }
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Button b = (Button)sender;
                ProcessInfoPage p = (ProcessInfoPage)PageFrame.Content;
                p.SetCameraName(b.Name);
                Response<Process> resp = _batExecutionService.ExecuteBat(b.Content.ToString());
                if(resp.Status == Core.Enums.Status.Error)
                {
                    InvokeErrorWindow(resp.Message);
                }
            }
            catch (Exception ex) 
            { 
                InvokeErrorWindow(ex.Message);
            }
        }

        public void CreateButtonsFromList()
        {
            List<Camera> list = _collection.GetCameraCollection();
            foreach (Camera c in list)
            {
                CreateButton(c.GetName(), c.GetStreamKey());
            }
        }

        public void InvokeErrorWindow(string message)
        {
            MessageBox.Show(message, "Ошибка");
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            _processDictionaryService.CoseAll();
            base.OnClosing(e);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow sw = new SettingsWindow();
            sw.ShowDialog();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
