using Modem.Geo.VideoRelayerDesktop.Core.Classes;
using Modem.Geo.VideoRelayerDesktop.Helpers;
using Modem.Geo.VideoRelayerDesktop.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using System.Windows.Threading;

namespace Modem.Geo.VideoRelayerDesktop
{
    /// <summary>
    /// Логика взаимодействия для ProcessInfoPage.xaml
    /// </summary>
    public partial class ProcessInfoPage : Page
    {
        private BatExecutionService _batExecutionService = BatExecutionService.GetInstance();
        private CameraCollection _cameraCollection = CameraCollection.GetInstance();
        private ProcessDictionaryService _processDictionaryService = ProcessDictionaryService.GetInstanse();
        public string CameraIp = null;
        public ProcessInfoPage()
        {
            InitializeComponent();

            DispatcherTimer pingTimer = new DispatcherTimer();
            pingTimer.Tick += new EventHandler(PingTimer_Tick);
            pingTimer.Interval = new TimeSpan(0, 0, 10);
            pingTimer.Start();

            DispatcherTimer refreshTimer = new DispatcherTimer();
            refreshTimer.Tick += RefreshTimer_Tick;
            refreshTimer.Interval = new TimeSpan(0, 0, 2);
            refreshTimer.Start();
        }

        public void SetCameraName(string name)
        {
            cameraNameLabel.Content = name;
        }

        private void PingTimer_Tick(object sender, EventArgs e)
        {
            PingRemote();
        }

        public void PingRemote()
        {
            if (CameraIp != null)
            {
                ThreadPool.QueueUserWorkItem(CheckCameraStatus);
            }
        }

        private void CheckCameraStatus(Object stateInfo)
        {
            PingHelper.AdressPing(CameraIp);
        }

        public void RefreshTimer_Tick(object sender, EventArgs e)
        {
            RefreshStatus();
        }

        public void RefreshStatus()
        {
            if (PingHelper.CurrentCameraStatus)
            {
                CameraPing.Content = "онлайн";
                CameraPing.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            }
            else
            {
                CameraPing.Content = "оффлайн";
                CameraPing.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }

            if (_processDictionaryService.IsRunning(cameraNameLabel.Content.ToString()).Data)
            {
                CameraStatus.Content = "Камера запущена";
                CameraStatus.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            }
            else
            {
                CameraStatus.Content = "Камера остановлена";
                CameraStatus.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
        }

        public string GetCameraIp()
        {
            Camera cam = _cameraCollection.GetCameraCollection().FirstOrDefault(x => x.Name == cameraNameLabel.Content.ToString());
            string cameraIp = null;
            if (cam != null)
            {
                string pattern = "[0-9]+.[0-9]+.[0-9]+.[0-9]+";
                MatchCollection match = Regex.Matches(cam.InputUrl, pattern);
                if (match.Count > 0)
                {
                    foreach (Match m in match)
                    {
                        cameraIp = m.Groups[0].Value;
                    }
                }
            }
            return cameraIp;
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Button b = (Button)sender;
                
                //Camera cam = _cameraCollection.GetCameraCollection().FirstOrDefault(x => x.Name == cameraNameLabel.Content.ToString());

                Response<byte> resp = _batExecutionService.ExecuteBat(cameraNameLabel.Content.ToString());
                if (resp.Status == Core.Enums.Status.Error)
                {
                    InvokeErrorWindow(resp.Message);
                }
            }
            catch (Exception ex)
            {
                InvokeErrorWindow(ex.Message);
            }
        }

        public void InvokeErrorWindow(string message)
        {
            MessageBox.Show(message, "Ошибка");
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            _processDictionaryService.StopProcess(cameraNameLabel.Content.ToString());
        }
    }
}
