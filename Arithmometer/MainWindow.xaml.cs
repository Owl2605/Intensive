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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e) //обработчик кнопки выхода
        {
            Application.Current.Shutdown(); //завершает работу приложения
        }

        private void btn_3dmodel_Click(object sender, RoutedEventArgs e) //обработчик кнопки "Виртуальный арифмометр"
        {
            _3Dmodel _3Dmodel = new _3Dmodel(); //создает новый экземпляр класса
            _3Dmodel.MW = this; //передает текущее окно в переменную
            _3Dmodel.Show(); //показывает экземпляр окна 
            this.Hide(); //скрывает текущее окно
        }

        private void btn_facts_Click(object sender, RoutedEventArgs e) //обработчик кнопки "Интересные факты"
        {
            WindowFact1 fact = new WindowFact1(); //создает новый экземпляр класса
            fact.MW = this; //передает текущее окно в переменную
            fact.Show(); //показывает экземпляр окна 
            this.Hide(); //скрывает текущее окно
        }

        private void btn_mechanism_Click(object sender, RoutedEventArgs e) //обработчик кнопки "Механизм арифмометра"
        {
            IN3dModel mechanism = new IN3dModel(); //создает новый экземпляр класса
            mechanism.MW = this; //передает текущее окно в переменную
            mechanism.Show(); //показывает экземпляр окна 
            this.Hide(); //скрывает текущее окно
        }

        private void DocumentationBtn_Click(object sender, RoutedEventArgs e) //обработчик кнопки "Документация"
        {
            Documentation documentation = new Documentation(); //создает новый экземпляр класса
            documentation.MW = this; //передает текущее окно в переменную
            documentation.Show(); //показывает экземпляр окна 
            this.Hide(); //скрывает текущее окно
        }

        private void AuthorsBtn_Click(object sender, RoutedEventArgs e) //обработчик кнопки "Авторы"
        {
            Authors authors = new Authors(); //создает новый экземпляр класса
            authors.MW = this; //передает текущее окно в переменную
            authors.Show(); //показывает экземпляр окна 
            this.Hide(); //скрывает текущее окно
        }
    }
}
