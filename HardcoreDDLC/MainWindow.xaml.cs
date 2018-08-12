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
using System.Windows.Threading;

namespace HardcoreDDLC
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        int n = 0;
        public MainWindow()
        {
            InitializeComponent();

            HH.Scripts.Add("Hello World and JUUUUUST Monika!");
            HH.Scripts.Add("This is My test scripts");
            HH.Scripts.Add("사실 나츠키 체고다 시발여신 낮추키~~~");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HH.Input();
        }
    }
}
