using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
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
        int n = 0;
        public DispatcherTimer OnlyTopTimer = new DispatcherTimer();
        public DispatcherTimer MouseCaptureTimer = new DispatcherTimer();
        public DispatcherTimer MonikaMover = new DispatcherTimer();

        public bool IsDragging = false;
        public Point ClickedPosition = default;

        public MainWindow()
        {
            InitializeComponent();

            WindowStyle = WindowStyle.None;

            OnlyTopSetup();
            MouseSetup();

            MonikaMover.Interval = new TimeSpan(0, 0, 0, 0, 2);
            MonikaMover.Tick += (sender, arg) => Monika.SetValue(Canvas.LeftProperty, (double)(Monika.GetValue(Canvas.LeftProperty)) - 1);
            MonikaMover.Start();
        }

        private void MouseSetup()
        {
            MouseCaptureTimer.Tick += MouseCaptureTimer_Tick;
            MouseCaptureTimer.Interval = new TimeSpan(0, 0, 0, 0, 5);
        }

        private void OnlyTopSetup()
        {
            OnlyTopTimer.Tick += (e, arg) =>
            {
                if (!IsActive)
                {
                    Activate();
                    MessageBox.Show("야임마 ㅎㅎ..");
                    Debug.WriteLine(11);
                };
            };

            OnlyTopTimer.Interval = new TimeSpan(0, 0, 1);
            //OnlyTopTimer.Start();
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
    }
}
