using HardcoreDDLC.Actions;
using HardcoreDDLC.Functions;
using System;
using System.Diagnostics;
using System.IO;
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
        public static DependencyObject MonikaStatic;
        private readonly DispatcherTimer OnlyTopTimer = new DispatcherTimer();
        private readonly DispatcherTimer MouseCaptureTimer = new DispatcherTimer();
        private readonly DispatcherTimer MonikaMover = new DispatcherTimer();
        
        private NotifyIcon Notifier;
        
        private bool IsDragging = false;
        private Point ClickedPosition = default;

        public MainWindow()
        {
            InitializeComponent();

            WindowStyle = WindowStyle.None;

            MonikaStatic = Monika;

            MouseSetup();
            InitializeScript();

            var fd = new OpenFileDialog()
            {
                DefaultExt = "txt",
                Filter = "Text Files(*.txt)|*.txt"
            };
            fd.ShowDialog();
            if (fd.FileName.Length > 0)
            {
                _actions = ParseManager.ParseRawScript(File.ReadAllText(fd.FileName));
            }
            
            /*
            _actions.Add(new DDLCDelayAction(3000) { isSkiped = true });
            _actions.Add(new DDLCScriptAction("딜레이 3초!"));
            _actions.Add(new DDLCProcessAction(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe") { isSkiped = true });
            _actions.Add(new DDLCDelayAction(1500) { isSkiped = true });
            _actions.Add(new DDLCKeyinputAction("{alt down}{tab}{alt up}") { isSkiped = true });
            _actions.Add(new DDLCScriptAction(@"크롬 프로그램을 실행하였습니다.
경로 : C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"));
            _actions.Add(new DDLCScriptAction("스크립트 재생해보기"));
            */
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
            Notifier = new NotifyIcon
            {
                Icon = new System.Drawing.Icon(
                    @"D:\Download\RPA Extractor for Windows\RPA Extractor for Windows\images\gui\mouse\s_head2.ico"),
                Text = "CC",
                Visible = true,
                BalloonTipText = "JUST MONIKA"
            };
            Notifier.ShowBalloonTip(1000);
        }

        private void WindowsOff()
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/C shutdown -s -t 10"
            };


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

            OnlyTopTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
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
            if (!IsDragging) return;
            
            double X = 0;
            double Y = 0;

            X = MouseGetPos().X - ClickedPosition.X;
            Y = MouseGetPos().Y - ClickedPosition.Y;

            VirtualWindow.SetValue(Canvas.LeftProperty, X);
            VirtualWindow.SetValue(Canvas.TopProperty, Y);
            OverlayVirtualWindow.SetValue(Canvas.LeftProperty, X);
            OverlayVirtualWindow.SetValue(Canvas.TopProperty, Y+30);
            Debug.WriteLine(Mouse.GetPosition(null));

            if (Mouse.LeftButton == MouseButtonState.Released)
                Rectangle_MouseLeftButtonUp(null, null);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void MonikaEscape(object sender, RoutedEventArgs e)
        {
            MonikaMove();
        }
    }
}
