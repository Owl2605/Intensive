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
    public partial class WindowFact2 : Window
    {
        public WindowFact2()
        {
            InitializeComponent();
        }
        MainWindow? mw;
        public MainWindow? MW { get { return mw; } set { mw = value; } }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MW.Show();
            this.Close();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            WindowFact3 fact3 = new WindowFact3();
            fact3.MW = this.MW;
            fact3.Show();
            this.Close();
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            WindowFact1 fact1 = new WindowFact1();
            fact1.MW = this.MW;
            fact1.Show();
            this.Close();
        }
    }
}
