using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using static AutoIt.AutoItX;

namespace HardcoreDDLC
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public NotifyIcon Notifier;
        public DispatcherTimer OnlyTopTimer = new DispatcherTimer();
        public DispatcherTimer MouseCaptureTimer = new DispatcherTimer();
        public DispatcherTimer MonikaMover = new DispatcherTimer();

        public bool IsDragging = false;
        public Point ClickedPosition = default;

        public MainWindow()
        {
            InitializeComponent();

            WindowStyle = WindowStyle.None;

            MouseSetup();
        }

        private void MonikaMove()
        {
            MonikaMover.Interval = new TimeSpan(0, 0, 0, 0, 2);
            MonikaMover.Tick += (sender, arg) => Monika.SetValue(Canvas.LeftProperty, (double)(Monika.GetValue(Canvas.LeftProperty)) - 1);
            MonikaMover.Start();
        }

        private void MouseSetup()
        {
            MouseCaptureTimer.Tick += MouseCaptureTimer_Tick;
            MouseCaptureTimer.Interval = new TimeSpan(0, 0, 0, 0, 5);
        }

        private void WindowsNotify()
        {
            Notifier = new NotifyIcon();
            Notifier.Icon = new System.Drawing.Icon(@"D:\Download\RPA Extractor for Windows\RPA Extractor for Windows\images\gui\mouse\s_head2.ico");
            Notifier.Text = "CC";
            Notifier.Visible = true;
            Notifier.BalloonTipText = "JUST MONIKA";
            Notifier.ShowBalloonTip(1000);
        }

        private void WindowsOff()
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();

            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C shutdown -s -t 10";

            process.StartInfo = startInfo;
            process.Start();

            Task.Delay(9000).ContinueWith(_ =>
            {
                startInfo.Arguments = "/C shutdown -a";

                process.Start();
            });
        }

        private void ExecuteYoutube()
        {
            Process.Start(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe");

            Thread.Sleep(1000);
            var r = new Random();
            @"https://music.youtube.com/watch?v=zX4rQyvBR70".ToList().ForEach((x) =>
            {
                Send($"{x}");
                Thread.Sleep(40 + r.Next(3) * 30);
                if (r.Next(13) < 1)
                    Thread.Sleep(400);
            });
            Send("{enter}");
            Thread.Sleep(1000);
            Activate();
            VirtualWindow.SetValue(Canvas.LeftProperty, (double)(Screen.PrimaryScreen.Bounds.Width / 2 - 640));
            VirtualWindow.SetValue(Canvas.TopProperty, (double)(Screen.PrimaryScreen.Bounds.Height / 2 - 375));
        }

        private void OnlyTopSetup()
        {
            OnlyTopTimer.Tick += (e, arg) =>
            {
                if (!IsActive)
                {
                    Activate();
                    System.Windows.MessageBox.Show("야임마 ㅎㅎ..");
                    Debug.WriteLine(11);
                };
            };

            OnlyTopTimer.Interval = new TimeSpan(0, 0, 1);
            OnlyTopTimer.Start();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsDragging = true;
            ClickedPosition = e.GetPosition(VirtualWindow);
            MouseCaptureTimer.Start();
        }

        private void Rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsDragging = false;
            ClickedPosition = default;
            MouseCaptureTimer.Stop();
            Debug.WriteLine("off");
        }
        
        private void MouseCaptureTimer_Tick(object sender, EventArgs e)
        {
            if (IsDragging)
            {
                double X = 0;
                double Y = 0;

                X = MouseGetPos().X - ClickedPosition.X;
                Y = MouseGetPos().Y - ClickedPosition.Y;

                VirtualWindow.SetValue(Canvas.LeftProperty, X);
                VirtualWindow.SetValue(Canvas.TopProperty, Y);
                Debug.WriteLine(Mouse.GetPosition(null));

                if (Mouse.LeftButton == MouseButtonState.Released)
                    Rectangle_MouseLeftButtonUp(null, null);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

    }
}
