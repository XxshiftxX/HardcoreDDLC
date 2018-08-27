using HardcoreDDLC.Actions;
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

            MouseSetup();
            InitializeScript();

            actions.Add(new DDLCScriptAction("Hello World!"));
            actions.Add(new DDLCScriptAction("This is Test Scripts"));
            actions.Add(new DDLCScriptAction("이 문장은 테스트용 스크립트입니다."));
            actions.Add(new DDLCMoveAction(Monika, new Point(0, 100)));
            actions.Add(new DDLCScriptAction("놀랐어?"));
            actions.Add(new DDLCMoveAction(Monika, new Point(200, -100), 1));
            actions.Add(new DDLCScriptAction("천천히 움직일 수도 있어!"));
            actions.Add(new DDLCScriptAction("이번엔 인터넷?"));
            actions.Add(new DDLCProcessAction(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe") { isSkiped = true });
            actions.Add(new DDLCKeyinputAction("{alt down}|{tab}|{alt up}"));
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
