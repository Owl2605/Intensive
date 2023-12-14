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
    /// Логика взаимодействия для Authors.xaml
    /// </summary>
    public partial class Authors : Window
    {
        public Authors()
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
    }
}
