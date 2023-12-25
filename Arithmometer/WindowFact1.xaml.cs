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
    public partial class WindowFact1 : Window
    {
        public WindowFact1()
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
            WindowFact2 fact2 = new WindowFact2(); //создает новый экземпляр класса
            fact2.MW = this.MW; //передает главное окно в переменную
            fact2.Show(); //показывает экземпляр окна 
            this.Close(); //закрывает текущее окно
        }
    }
}
