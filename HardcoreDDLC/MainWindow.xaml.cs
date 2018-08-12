using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;

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

        public bool IsDragging = false;
        public Point ClickedPosition = default;

        public MainWindow()
        {
            InitializeComponent();

            WindowStyle = WindowStyle.None;

            OnlyTopSetup();
            MouseSetup();
        }

        private void MouseSetup()
        {
            MouseCaptureTimer.Tick += MouseCaptureTimer_Tick;
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
        }
        
        private void MouseCaptureTimer_Tick(object sender, EventArgs e)
        {
            if (IsDragging)
            {
                double X = 0;
                double Y = 0;

                X = (Mouse.GetPosition(null) - ClickedPosition).X;
                Y = (Mouse.GetPosition(null) - ClickedPosition).Y;

                VirtualWindow.SetValue(Canvas.LeftProperty, X);
                VirtualWindow.SetValue(Canvas.TopProperty, Y);
                Debug.WriteLine(Mouse.GetPosition(null));
            }
        }
    }
}
