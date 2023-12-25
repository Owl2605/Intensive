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
        MainWindow? mw; //переменная для главного окна
        public MainWindow? MW { get { return mw; } set { mw = value; } } //свойство для переменной 

        private void Back_Click(object sender, RoutedEventArgs e) //обработчик кнопки "назад"
        {
            MW.Show(); //показывает главное окно
            this.Close(); //закрывает текущее окно
        }

        private void Next_Click(object sender, RoutedEventArgs e) //обработчик кнопки "следующий"
        {
            WindowFact3 fact3 = new WindowFact3(); //создает новый экземпляр класса
            fact3.MW = this.MW; //передает главное окно в переменную
            fact3.Show(); //показывает экземпляр окна 
            this.Close(); //закрывает текущее окно
        }

        private void Previous_Click(object sender, RoutedEventArgs e) //обработчик кнопки "предыдущий"
        {
            WindowFact1 fact1 = new WindowFact1(); //создает новый экземпляр класса
            fact1.MW = this.MW; //передает главное окно в переменную
            fact1.Show(); //показывает экземпляр окна 
            this.Close(); //закрывает текущее окно
        }
    }
}
