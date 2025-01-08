using Modem.Geo.VideoRelayerDesktop.Core.Classes;
using Modem.Geo.VideoRelayerDesktop.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
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
        private CameraCollection _collection = CameraCollection.GetInstance();
        private BatExecutionService _batExecutionService = BatExecutionService.GetInstance();
        private ProcessDictionaryService _processDictionaryService = ProcessDictionaryService.GetInstanse();
        public MainWindow()
        {
            InitializeComponent();
            PageFrame.Source = new Uri("ProcessInfoPage.xaml", UriKind.Relative);

            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(DispatcherTimer_Tick);
            timer.Interval = new TimeSpan(0, 0, 10);
            timer.Start();

            Task<Response<string>> resp = _collection.AddCameraFromApi();
            
            
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            //PortalPing.Content = DateTime.Now.ToString();
            if(PingHost("show.innerica.ru"))
            {
                PortalPing.Content = "online";
                PortalPing.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            }
            else
            {
                PortalPing.Content = "offline";
                PortalPing.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
        }

        private bool PingHost(string nameOrAddress)
        {
            try
            {
                using (Ping pinger = new Ping())
                {
                    PingReply reply = pinger.Send(nameOrAddress);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch (PingException)
            {
                return false;
            }
        }

        public void AddCamera(string name, string streamKey)
        {
            Response<string> resp;
            resp = _collection.AddCamera(new Camera());
            if(resp.Status == Core.Enums.Status.Error)
            {
                InvokeErrorWindow(resp.Message);
            }
        }

        public void CreateButton(Camera camera)
        {
            Button button = new Button();
            button.Content = camera.Name;
            button.Name = $"N{camera.Id}";
            button.Click += ButtonClick;
            CameraButtons.Children.Add(button);
            Response<byte> resp = _batExecutionService.CreateBat(camera);
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
                p.SetCameraName(b.Content.ToString());
                p.CameraIp = p.GetCameraIp();
                p.Launch.Visibility = Visibility.Visible;
                p.CameraPingLabel.Visibility = Visibility.Visible;
                p.CameraPing.Visibility = Visibility.Visible;   
                p.CameraStatus.Visibility = Visibility.Visible;
                p.Stop.Visibility = Visibility.Visible;
            }
            catch (Exception ex) 
            { 
                InvokeErrorWindow(ex.Message);
            }
        }

        public void CreateButtonsFromList()
        {
            RemoveButtons();
            List<Camera> list = _collection.GetCameraCollection();
            foreach (Camera c in list)
            {
                CreateButton(c);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreateButtonsFromList();
        }

        private void RemoveButtons()
        {
            CameraButtons.Children.Clear();
        }
    }
}
