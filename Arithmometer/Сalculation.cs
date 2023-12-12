using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Arithmometer
{
    internal class Сalculation
    {

        int number_of_revolutions; //количество оборотов
        int otvet;// то что должны записать в лейблы
        int cartca;//смещение картки(нужно для умножения)
        int chislo;//число которое мы умножаем(вводим его где считали итерации с помощью каретки)
        int num;// число которое мы получаем из питона
        string[] lines;//  получаем строки из питона
        public void Turnovers(int number_of_revolutions) //
        {
            if (number_of_revolutions == 1)
                Summa(num, otvet);
            if (number_of_revolutions == 2)
                Subtraction(num, otvet);
            if (number_of_revolutions == -1)
                Multiplication(num, otvet, chislo);//нада думать тк мы когда вводим первое число для умножения мы записываем его в итерациях
            if (number_of_revolutions == -2)
                Division(num, otvet, chislo);//тоже самое что и в умножении
        }
        public void perevod_stroki(int n, int value, int num, string[] lines)//переводим данные из питона в число которое нам нужно
        {

            //Label[] labels = { label1, label2, label3, label4, label5, label6, label7, label8, label9 , label10 , label11 , label12 , label13 };
            //int[] numbers = new int[labels.Length];

            //for (int i = 0; i < labels.Length; i++)//заполняем массив цыфрами из числа
            //{
            //    numbers[i] = int.Parse(labels[i].Text);
            //}
            //for (int i = 0; i<numbers.Length;i++)//создаем число с которым будем работать
            //{
            //    double res = Math.Pow(10, i);
            //    int result = (int)res;
            //    num = num+numbers[i]*result;
            //}

        }
        public void output_otveta_to_labels(int otvet) // метод вывода числа в лейблы по цыфре в каждый лейбл
        {
            //    label1.Text = otvet % 10;
            //    label2.Text = (otvet / 10) % 10;
            //    label3.Text = (otvet / 100) % 10;
            //    label4.Text = (otvet / 1000) % 10;
            //    label5.Text = (otvet / 10000) % 10;
            //    label6.Text = (otvet / 100000) % 10;
            //    label7.Text = (otvet / 1000000) % 10;
            //    label8.Text = (otvet / 10000000) % 10;
            //    label9.Text = (otvet / 100000000) % 10;
            //    label10.Text = (otvet / 1000000000) % 10;
            //    label11.Text = (otvet / 10000000000) % 10;
            //    label12.Text = (otvet / 100000000000) % 10;
            //    label13.Text = (otvet / 1000000000000) % 10;
        }
        public void output_iteration_to_labels(int otvet) // метод вывода числа в лейблы по цыфре в каждый лейбл
        {
            //    label1.Text = otvet % 10;
            //    label2.Text = (otvet / 10) % 10;
            //    label3.Text = (otvet / 100) % 10;
            //    label4.Text = (otvet / 1000) % 10;
            //    label5.Text = (otvet / 10000) % 10;
            //    label6.Text = (otvet / 100000) % 10;
            //    label7.Text = (otvet / 1000000) % 10;
            //    label8.Text = (otvet / 10000000) % 10;
            //    label9.Text = (otvet / 100000000) % 10;
        }

        public void Summa(int num, int otvet) // метод сложения
        {

            otvet = otvet + num;
            output_otveta_to_labels(otvet);

        }

        public void Subtraction(int num, int otvet) //метод вычитания
        {
            otvet = otvet - num;
            output_otveta_to_labels(otvet);
        }
        public void Multiplication(int num, int otvet, int chislo) //метод умножения
        {
            otvet = chislo * num;
            output_otveta_to_labels(otvet);
        }
        public void Division(int num, int otvet, int chislo) //метод деления
        {
            otvet = otvet / num;
            output_otveta_to_labels(otvet);
        }

    }
}
