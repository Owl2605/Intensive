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
using System.Windows.Shapes;

namespace Arithmometer
{
    /// <summary>
    /// Логика взаимодействия для WindowFact3.xaml
    /// </summary>
    
    public partial class WindowFact3 : Window
    {

        MainWindow? mw;
        public MainWindow? MW { get { return mw; } set { mw = value; } }
        public WindowFact3()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MW.Show();
            this.Close();
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            WindowFact2 fact2 = new WindowFact2();
            fact2.MW = this.MW;
            fact2.Show();
            this.Close();
        }
    }
}
