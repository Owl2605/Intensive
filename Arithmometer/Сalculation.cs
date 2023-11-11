using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arithmometer
{
    internal class Сalculation
    {
        int n, value; // эти считываем числа с 3д модели
        int number_of_revolutions; //количество оборотов
        public void Turnovers(int number_of_revolutions) //
        {
            if (number_of_revolutions == 1)
                Summa(n, value);
            if (number_of_revolutions == 2)
                Subtraction(n, value);
            if (number_of_revolutions == -1)
                Multiplication(n, value);
            if (number_of_revolutions == -2)
                Division(n, value);
        }
        public void Summa(int n, int value) // метод сложения
        {
            //Label.Content = n + value; 
        }

        public void Subtraction(int n, int value) //метод вычитания
        {
            //Label.Content = n + value; 
        }
        public void Multiplication(int n, int value) //метод умножения
        {
            //Label.Content = n * value; 
        }
        public void Division(int n, int value) //метод деления
        {
            //Label.Content = n / value; 
        }
    
    }
}
