using System;
using HelixToolkit.Wpf;
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
using System.Windows.Media.Media3D;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Reflection;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Reflection.Metadata;
using System.Threading.Channels;
using System.Windows.Threading;
using Microsoft.VisualBasic;
using System.Security.Cryptography.X509Certificates;

namespace Arithmometer
{

    public partial class _3Dmodel : Window
    {
        ModelVisual3D machine3D;
        private string MODEL_PATH = (Directory.GetCurrentDirectory() + @"\..\..\..\Resources\MachinElem\machine.obj").ToString(); //получаем полный путь к 3D модели корпуса арифмометра
        ModelVisual3D lc3D;
        Model3D lc3DModel3D;
        private string LC_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\Resources\MachinElem\leftСircle.obj").ToString(); //получаем полный путь к 3D модели левого барашка
        ModelVisual3D rc3D;
        Model3D rc3DModel3D;
        private string RC_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\Resources\MachinElem\rightСircle.obj").ToString(); //получаем полный путь к 3D модели правого барашка
        ModelVisual3D handle3D;
        Model3D handelModel3D;
        private string HANDLE_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\Resources\MachinElem\handle.obj").ToString(); //получаем полный путь к 3D модели рукоятки
        ModelVisual3D bogie3D;
        private string BOGIE_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\Resources\MachinElem\bogie.obj").ToString(); //получаем полный путь к 3D модели каретки

        string[] leversSTR = new string[] //массив путей к 3D моделям рычежков
        {
        (Directory.GetCurrentDirectory() + @"\..\..\..\Resources\Levers\lever1.obj").ToString(),
        (Directory.GetCurrentDirectory() + @"\..\..\..\Resources\Levers\lever2.obj").ToString(),
        (Directory.GetCurrentDirectory() + @"\..\..\..\Resources\Levers\lever3.obj").ToString(),
        (Directory.GetCurrentDirectory() + @"\..\..\..\Resources\Levers\lever4.obj").ToString(),
        (Directory.GetCurrentDirectory() + @"\..\..\..\Resources\Levers\lever5.obj").ToString(),
        (Directory.GetCurrentDirectory() + @"\..\..\..\Resources\Levers\lever6.obj").ToString(),
        (Directory.GetCurrentDirectory() + @"\..\..\..\Resources\Levers\lever7.obj").ToString(),
        (Directory.GetCurrentDirectory() + @"\..\..\..\Resources\Levers\lever8.obj").ToString(),
        (Directory.GetCurrentDirectory() + @"\..\..\..\Resources\Levers\lever9.obj").ToString()
        };

        ModelVisual3D[] levers3D = new ModelVisual3D[9]; //
        int[] levers = new int[9]; //массив значений рычажков
        Label[] results, iterations; //массив лейблов результата и количества операций
        int[] iterationsValue = new int[8]; //массив количества операций


        MainWindow? mw; //переменная для главного окна
        public MainWindow? MW { get { return mw; } set { mw = value; } } //свойство для переменной 
        private void Back_Click(object sender, RoutedEventArgs e) //обработчик кнопки "назад"
        {
            if (nwStream != null && client != null) //проверяем наличие соединения с клиентом
            {
                nwStream = client.GetStream();
                string s = "stop";
                byte[] bytes = Encoding.UTF8.GetBytes(s); //отправляем stop на клиент
                nwStream.Write(bytes, 0, bytes.Length); 
                client.Close();
            }
            server.Stop(); //останавливаем сервер
            thread.Interrupt(); //завершаем поток
            thread.Join();
            MW.Show(); //показывает главное окно
            this.Close(); //закрывает текущее окно
        }

        Point3D loAtPoint; //центр вращения рукоятки
        Point3D RCloAtPoint; //центр вращения правого барашка
        Point3D LCloAtPoint; //центр вращения левого барашка

        public _3Dmodel()
        {
            InitializeComponent();
            Start();
            Add3DModels(); //добавление 3D моделей на форму
            BogLRX(-8); //переводим каретку в крайнее левое положение
            Rect3D bounds = handelModel3D.Bounds; //получение границ 3D модели рычажка
            loAtPoint = new Point3D(bounds.X + bounds.SizeX / 2, bounds.Y + bounds.SizeY / 2 + 6.5, bounds.Z + bounds.SizeZ / 2); //вычисление центра вращения рукоятки
            bounds = lc3DModel3D.Bounds; //получение границ 3D модели левого барашка
            LCloAtPoint = new Point3D(bounds.X + bounds.SizeX / 2, bounds.Y + bounds.SizeY / 2, bounds.Z + bounds.SizeZ / 2); //вычисление центра вращения левого барашка
            bounds = rc3DModel3D.Bounds; //получение границ 3D модели правого барашка
            RCloAtPoint = new Point3D(bounds.X + bounds.SizeX / 2, bounds.Y + bounds.SizeY / 2, bounds.Z + bounds.SizeZ / 2); //вычисление центра вращения правого барашка
            results = new Label[] { LB1, LB2, LB3, LB4, LB5, LB6, LB7, LB8, LB9, LB10, LB11, LB12, LB13 }; //заплнение масссива лейблов результата
            iterations = new Label[] { IT1, IT2, IT3, IT4, IT5, IT6, IT7, IT8 }; //заплнение масссива лейблов количества операций
            for (int i = 0; i < iterationsValue.Length; i++) //запонения массива количества операций
            {
                iterationsValue[i] = 0;
            }
        }

        void BogLRX(double x) //сдвигаем части каретки по X
        {
            LRX(bogie3D, x); 
            LRX(lc3D, x);
            LRX(rc3D, x);
        }

        private Model3D Display3d(string model)
        {
            Model3D device = null;
            try
            {
                //Adding a gesture here
                viewPort3d.RotateGesture = new MouseGesture(MouseAction.LeftClick);
                viewPort3d.Camera.UpDirection = new Vector3D(0, 1, 0);
                viewPort3d.Camera.LookDirection = new Vector3D(-1.4, -60, -60);
                viewPort3d.Camera.Position = new Point3D(40, 86, 20);

                //Import 3D model file
                ModelImporter import = new ModelImporter();
                //Load the 3D model file
                device = import.Load(model);
            }
            catch (Exception e)
            {
                // Handle exception in case can not file 3D model
                MessageBox.Show("Exception Error : " + e.StackTrace);
            }
            return device;
        }

        Thread thread;
        public int connectionPort = 25001;
        TcpListener server;
        TcpClient client;
        bool running;

        /// <summary>
        /// Метод для запуска в отдельном потоке сервера
        /// </summary>
        void Start()
        {
            ThreadStart ts = new ThreadStart(GetData);
            thread = new Thread(ts);
            thread.Start();
        }

        /// <summary>
        /// Метод для получения данных с сервера
        /// </summary>
        void GetData()
        {
            try
            {
                server = new TcpListener(IPAddress.Any, connectionPort); 
                server.Start(); //Запускаем сервер
                Console.WriteLine("Сервер готов к получению клиента");
                running = true;
                client = server.AcceptTcpClient(); //Устанавливаем соединение
                Console.WriteLine("Клиент подключился к серверу");
                while (running) //Пока работает сервер читаем и выполняем команды клиента
                {
                    Connection(); 
                }
                server.Stop();
            }
            catch { }
        }
        NetworkStream nwStream;
        /// <summary>
        /// Пока установлено соединение читаем команды клиента
        /// </summary>
        void Connection()
        {
            nwStream = client.GetStream();
            //создаём буффер в который будем записывать строку
            byte[] buffer = new byte[client.ReceiveBufferSize];
            int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
            string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            if (dataReceived.Length != 0) //если полученная строка содержит информацию
            {
                Console.WriteLine(dataReceived);
                doIt(dataReceived); //выполняем команду клиента
            }

        }
        void LR(ModelVisual3D device, double z, double y) //сдвиг по Y, Z
        {
            Matrix3D matrix;
            matrix = device.Content.Transform.Value; //получаем матрицу 3D модели
            matrix.OffsetZ = z; //смещаем по оси Z
            matrix.OffsetY = y; //смещаем по оси Y
            device.Content.Transform = new MatrixTransform3D(matrix); //присваиваем 3D модели новую матрицу
        }
        void LRX(ModelVisual3D device, double x) //сдвиг по X
        {
            Matrix3D matrix;
            matrix = device.Content.Transform.Value; //получаем матрицу 3D модели
            matrix.OffsetX = x; //смещаем по оси X
            device.Content.Transform = new MatrixTransform3D(matrix); //присваиваем 3D модели новую матрицу
        }

        void LeversRotation(ModelVisual3D device, int n) //сдвигаем рычажок
        {
            switch (n) //0 - крйний левый рычажок, 8 - крайний правый рычажок
            {
                case 0: LR(device, 0, 0); break;
                case 1: LR(device, 3, 1); break;
                case 2: LR(device, 6, 2); break;
                case 3: LR(device, 8.5, 2); break;
                case 4: LR(device, 11, 2); break;
                case 5: LR(device, 13.5, 1); break;
                case 6: LR(device, 16, 0); break;
                case 7: LR(device, 18.5, -2); break;
                case 8: LR(device, 19.7, -5); break;
                case 9: LR(device, 20.7, -7); break;
            }

        }

        int bogie = 0; //значение каретки 
        long number;
        long result;


        async void doIt(string str)
        {
            await this.Dispatcher.Invoke(async () =>
            {   
                if (str.Length == 2) // Если длина строки равна двум, значит мы передали на сервер данные по рычажку
                                     //первая цифра - номер рычажка, вторая цифра - цифра на рычажке
                {
                    int tempL = int.Parse(str.Substring(0, 1)) - 1; //получаем номер рычажка, поправка на элемент массива
                    int tempN = int.Parse(str.Substring(1, 1)); //получаем цифру выставленную на рычажке
                    levers[tempL] = tempN; //присваиваем выставленное число рычажку в массиве 
                    LeversRotation(levers3D[tempL], tempN); //сдвигаем рычажок (какой рычажок, на какую цифру)
                }
                else if (str == "Прокрутили ручку вперед") //выполнить код для вращения ручки вперед
                {
                    if (bogie == 5 && levers[8] != 0 || //проверка, что рычажки с выставленными числами находятся не над ячейками
                    bogie == 6 && levers[8] != 0 && levers[7] != 0 ||
                    bogie == 7 && levers[8] != 0 && levers[7] != 0 && levers[6] != 0 ||
                    iterations[bogie].Content.ToString() == "9") //проверка, что счетчик операций не выйдет за пределы
                    {
                        MessageBox.Show("Звонок!", "Ошибка!");
                        return;

                    }
                    RoundUp(); //анимация крутим ручку вперед
                    number = 0;
                    for (int i = 8; i >= 0; i--) //собираем данные с рычажков в одно число начиная с последнего
                    {
                        number += levers[i] * (long)Math.Pow(10, 8 - i); //собираем данные для рычажков в единое число
                    }
                    result += (number * (int)Math.Pow(10, bogie)); //подсчет резльтата, bogie - поправка на каретку
                    long temp = result;
                    for (int i = 0; i < 13; i++) //запись результата в ячейки
                    {
                        if ((temp / (long)Math.Pow(10, 13)) % 10 > 0) //проверка на выход числа за пределы ячеек
                        {
                            MessageBox.Show("Звонок!", "Ошибка!");
                            break;
                        }
                        results[i].Content = (temp / (long)Math.Pow(10, i)) % 10; //отделяем часть числа и записываем в соответствующий лейбл
                    }
                    iterationsValue[bogie] += 1; //увеличиваем число операций в массиве
                    iterations[bogie].Content = Math.Abs(iterationsValue[bogie]); //передаем количество операций в лейбл
                }
                else if (str == "Прокрутили ручку назад") // выполнить код для вращения ручки назад
                {
                    if (bogie == 5 && levers[8] != 0 || //проверка, что рычажки с выставленными числами находятся не над ячейками
                    bogie == 6 && levers[8] != 0 && levers[7] != 0 ||
                    bogie == 7 && levers[8] != 0 && levers[7] != 0 && levers[6] != 0 ||
                    iterations[bogie].Content.ToString() == "9") //проверка, что счетчик операций не выйдет за пределы
                    {
                        MessageBox.Show("Звонок!", "Ошибка!");
                        return;

                    }
                    RoundDown(); //анимация крутим ручку назад 
                    number = 0;
                    for (int i = 8; i >= 0; i--) //собираем данные с рычажков в одно число начиная с последнего
                    {
                        number += levers[i] * (long)Math.Pow(10, 8 - i); //собираем данные для рычажков в единое число
                    }
                    if (result - (number * (int)Math.Pow(10, bogie)) < 0) //проверка на отрицательное число
                    {
                        MessageBox.Show("Звонок!", "Ошибка!");
                        return;
                    }
                    result -= (number * (int)Math.Pow(10, bogie)); //подсчет резльтата, bogie - поправка на каретку
                    long temp = result;
                    for (int i = 0; i < 13; i++) //запись результата в ячейки
                    {
                        if ((temp / (long)Math.Pow(10, 13)) % 10 > 0) //проверка на выход числа за пределы ячеек
                        {
                            MessageBox.Show("Звонок!", "Ошибка!");
                            break;
                        }
                        results[i].Content = (temp / (long)Math.Pow(10, i)) % 10; //отделяем часть числа и записываем в соответствующий лейбл
                    }
                    iterationsValue[bogie] -= 1; //уменьшаем число операций в массиве
                    iterations[bogie].Content = Math.Abs(iterationsValue[bogie]); //передаем количество операций в лейбл
                }
                else if (str.Length == 3) //выставить каретку на значение str[2]
                {
                    bogie = int.Parse(str.Substring(2, 1)) - 1; //получаем на какое значение выставлена каретка
                    switch (bogie) //0 - крайнее левое положение, 7 - крайнее правое
                    {
                        case 0: BogLRX(-8); break;
                        case 1: BogLRX(-6); break;
                        case 2: BogLRX(-4); break;
                        case 3: BogLRX(-2); break;
                        case 4: BogLRX(0); break;
                        case 5: BogLRX(2); break;
                        case 6: BogLRX(4); break;
                        case 7: BogLRX(6); break;
                    }
                }
                else if (str == "сбросил результаты") //сбросить результаты
                {
                    for (int i = 0; i < results.Length; i++) //обнуляем результат вычислений
                    {
                        results[i].Content = 0;
                        result = 0;
                    }
                    for (int j = 0; j < 18; j++) //анимация правого барашка
                    {
                        Vector3D axis = new Vector3D(1, 0, 0); //вращение по X
                        Matrix3D matrix = rc3D.Content.Transform.Value; //получаем матрицу 3D модели
                        matrix.RotateAt(new Quaternion(axis, -20), RCloAtPoint); //вращаем матрицу ((вектор, радианы), центр вращения)
                        rc3D.Content.Transform = new MatrixTransform3D(matrix); //присваиваем 3D модели новую матрицу
                        await Task.Delay(75);
                    }
                }
                else if (str == "сбросил обороты") //сбросить количество операций
                {
                    for (int i = 0; i < iterations.Length; i++) //обнуляем количество операций
                    {
                        iterations[i].Content = 0;
                        iterationsValue[i] = 0;
                    }
                    for (int j = 0; j < 18; j++) //анимация левого барашка
                    {
                        Vector3D axis = new Vector3D(1, 0, 0);
                        Matrix3D matrix = lc3D.Content.Transform.Value; //получаем матрицу 3D модели
                        matrix.RotateAt(new Quaternion(axis, -20), LCloAtPoint); //вращаем матрицу ((вектор, радианы), центр вращения)
                        lc3D.Content.Transform = new MatrixTransform3D(matrix); //присваиваем 3D модели новую матрицу
                        await Task.Delay(75);
                    }
                }
                else if (str == "Справка") //вывести справку на экран
                {
                    popUp.IsOpen = !popUp.IsOpen;
                }
                else if (str == "Сброс") //закрыть справку
                {
                    popUp.IsOpen = !popUp.IsOpen;
                }
                if (str == "Рука обнаружена") //меняем цвет датчика на зелёный цвет
                    circleHandIndicator.Fill = new SolidColorBrush(Colors.Green);
                if (str == "Рука не обнаружена") //меняем цвет датчика на красный цвет
                    circleHandIndicator.Fill = new SolidColorBrush(Colors.Red);

            });
        }

        void HandleRot(int angle)
        {
            Vector3D axis = new Vector3D(1, 0, 0); //вращение по X
            Matrix3D matrix = handle3D.Content.Transform.Value; //получаем матрицу 3D модели
            matrix.RotateAt(new Quaternion(axis, angle), loAtPoint);  //вращаем матрицу ((вектор, радианы), центр вращения)
            handle3D.Content.Transform = new MatrixTransform3D(matrix); //присваиваем 3D модели новую матрицу

        }
        async void RoundUp() //анимация вращения ручки вперед 
        {
            for (int j = 0; j < 18; j++)
            {
                HandleRot(-20); //поврот на -20 радиан
                await Task.Delay(75);
            }
        }

		private void Help_Click(object sender, RoutedEventArgs e)
		{
            popUp.IsOpen = !popUp.IsOpen;
		}

		async void RoundDown() //анимация вращения ручки назад
        {
            for (int j = 0; j < 18; j++)
            {
                HandleRot(20); //поврот на 20 радиан
                await Task.Delay(75);
            }
        }

        void Add3DModels() //добовляем 3D модели на форму
        {
            Model3D model3D;
            machine3D = new ModelVisual3D();
            model3D = Display3d(MODEL_PATH); //загружаем 3D модель из файла
            machine3D.Content = model3D; //добавляем 3D модель в контейнер для отображения
            viewPort3d.Children.Add(machine3D); //добавляет объект в коллекцию дочерних элементов viewPort3d

            lc3D = new ModelVisual3D();
            lc3DModel3D = Display3d(LC_MODEL);
            lc3D.Content = lc3DModel3D;
            viewPort3d.Children.Add(lc3D);

            rc3D = new ModelVisual3D();
            rc3DModel3D = Display3d(RC_MODEL);
            rc3D.Content = rc3DModel3D;
            viewPort3d.Children.Add(rc3D);

            handle3D = new ModelVisual3D();
            handelModel3D = Display3d(HANDLE_MODEL);
            handle3D.Content = handelModel3D;
            viewPort3d.Children.Add(handle3D);

            bogie3D = new ModelVisual3D();
            model3D = Display3d(BOGIE_MODEL);
            bogie3D.Content = model3D;
            viewPort3d.Children.Add(bogie3D);

            for (int i = 0; i < levers3D.Length; i++) //цикл для добавления массива 3D моделей рычажков
            {
                levers3D[i] = new ModelVisual3D();
                model3D = Display3d(leversSTR[i]);
                levers3D[i].Content = model3D;
                viewPort3d.Children.Add(levers3D[i]);
            }
        }
    }
}
