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
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Arithmometer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btn_3dmodel_Click(object sender, RoutedEventArgs e)
        {
            _3Dmodel _3Dmodel = new _3Dmodel();
            _3Dmodel.MW = this;
            _3Dmodel.Show();
            this.Hide();
        }

        private void btn_facts_Click(object sender, RoutedEventArgs e)
        {
            WindowFact1 fact = new WindowFact1();
            fact.MW = this;
            fact.Show();
            this.Hide();
        }

        private void btn_mechanism_Click(object sender, RoutedEventArgs e)
        {
            IN3dModel mechanism = new IN3dModel();
            mechanism.MW = this;
            mechanism.Show();
            this.Hide();
        }

        private void DocumentationBtn_Click(object sender, RoutedEventArgs e)
        {
            Documentation documentation = new Documentation();
            documentation.MW = this;
            documentation.Show();
            this.Hide();
        }

        private void AuthorsBtn_Click(object sender, RoutedEventArgs e)
        {
            Authors authors = new Authors();
            authors.MW = this;
            authors.Show();
            this.Hide();
        }
    }
}
