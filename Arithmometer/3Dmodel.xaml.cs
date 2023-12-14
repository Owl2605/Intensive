﻿using System;
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

namespace Arithmometer
{

    public partial class _3Dmodel : Window
    {
        //Path to the model file
        ModelVisual3D machine3D;
        private string MODEL_PATH = (Directory.GetCurrentDirectory() + @"\..\..\..\machine.obj").ToString();
        ModelVisual3D lc3D;
        Model3D lc3DModel3D;
        private string LC_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\leftСircle.obj").ToString();
        ModelVisual3D rc3D;
        Model3D rc3DModel3D;
        private string RC_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\rightСircle.obj").ToString();
        ModelVisual3D handle3D;
        Model3D handelModel3D;
        private string HANDLE_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\handle.obj").ToString();
        ModelVisual3D bogie3D;
        private string BOGIE_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\bogie.obj").ToString();

        private string LEVER9_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\lever1.obj").ToString();
        string[] leversSTR = new string[]
        {
        (Directory.GetCurrentDirectory() + @"\..\..\..\lever9.obj").ToString(),
        (Directory.GetCurrentDirectory() + @"\..\..\..\lever8.obj").ToString(),
        (Directory.GetCurrentDirectory() + @"\..\..\..\lever7.obj").ToString(),
        (Directory.GetCurrentDirectory() + @"\..\..\..\lever6.obj").ToString(),
        (Directory.GetCurrentDirectory() + @"\..\..\..\lever5.obj").ToString(),
        (Directory.GetCurrentDirectory() + @"\..\..\..\lever4.obj").ToString(),
        (Directory.GetCurrentDirectory() + @"\..\..\..\lever3.obj").ToString(),
        (Directory.GetCurrentDirectory() + @"\..\..\..\lever2.obj").ToString(),
        (Directory.GetCurrentDirectory() + @"\..\..\..\lever1.obj").ToString()
        };

        private string RESULT1_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\.obj").ToString();
        private string RESULT2_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\.obj").ToString();
        private string RESULT3_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\.obj").ToString();
        private string RESULT4_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\.obj").ToString();
        private string RESULT5_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\.obj").ToString();
        private string RESULT6_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\.obj").ToString();
        private string RESULT7_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\.obj").ToString();
        private string RESULT8_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\.obj").ToString();
        private string RESULT9_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\.obj").ToString();
        private string RESULT10_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\.obj").ToString();
        private string RESULT11_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\.obj").ToString();
        private string RESULT12_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\.obj").ToString();
        private string RESULT13_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\.obj").ToString();

        private string COUNT1_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\.obj").ToString();
        private string COUNT2_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\.obj").ToString();
        private string COUNT3_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\.obj").ToString();
        private string COUNT4_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\.obj").ToString();
        private string COUNT5_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\.obj").ToString();
        private string COUNT6_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\.obj").ToString();
        private string COUNT7_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\.obj").ToString();
        private string COUNT8_MODEL = (Directory.GetCurrentDirectory() + @"\..\..\..\.obj").ToString();

        ModelVisual3D[] levers3D = new ModelVisual3D[9];
        int[] levers = new int[9];//значения рычажков
        ModelVisual3D[] results3D = new ModelVisual3D[13];
        Label[] results, iterations;
        ModelVisual3D[] counts3D = new ModelVisual3D[8];
        //int[] counts = new int[8];
        Point3D lookAtPointLever;
        int[] iterationsValue = new int[8];


        MainWindow? mw;
        public MainWindow? MW { get { return mw; } set { mw = value; } }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (nwStream != null && client != null)
            {
                nwStream = client.GetStream();
                string s = "stop";
                byte[] bytes = Encoding.UTF8.GetBytes(s);
                Console.WriteLine("true");
                nwStream.Write(bytes, 0, bytes.Length);
                Console.WriteLine("отправил стоп");
                client.Close();
            }
            server.Stop();
            thread.Interrupt();
            thread.Join();
            MW.Show();
            this.Close();
        }
        ModelVisual3D device3D;
        Model3D model3D;
        Point3D loAtPoint;
        Point3D RCloAtPoint;
        Point3D LCloAtPoint;
        public _3Dmodel()
        {
            InitializeComponent();
            Start();

            Add3DModels();
            BogLRX(-8);
            Rect3D bounds = handelModel3D.Bounds;
            loAtPoint = new Point3D(bounds.X + bounds.SizeX / 2, bounds.Y + bounds.SizeY / 2 + 6.5, bounds.Z + bounds.SizeZ / 2);
            bounds = lc3DModel3D.Bounds;
            LCloAtPoint = new Point3D(bounds.X + bounds.SizeX / 2, bounds.Y + bounds.SizeY / 2, bounds.Z + bounds.SizeZ / 2);
            bounds = rc3DModel3D.Bounds;
            RCloAtPoint = new Point3D(bounds.X + bounds.SizeX / 2, bounds.Y + bounds.SizeY / 2, bounds.Z + bounds.SizeZ / 2);
            results = new Label[] { LB1, LB2, LB3, LB4, LB5, LB6, LB7, LB8, LB9, LB10, LB11, LB12, LB13 };
            iterations = new Label[] { IT1, IT2, IT3, IT4, IT5, IT6, IT7, IT8 };
            for (int i = 0; i < iterationsValue.Length; i++)
            {
                iterationsValue[i] = 0;
            }
        }

        void BogLRX(double x)
        {
            LRX(bogie3D, x);
            LRX(lc3D, x);
            LRX(rc3D, x);
        }
        void MyRotationAtX(ModelVisual3D device, double ang, Point3D lookAtPoint)
        {
            Vector3D axis = new Vector3D(1, 0, 0);
            double angle = ang;
            Matrix3D matrix = device.Content.Transform.Value;
            matrix.RotateAt(new Quaternion(axis, angle), lookAtPoint);
            device.Content.Transform = new MatrixTransform3D(matrix);
        }
        //void MyRotationAtZ(ModelVisual3D device, Point3D lookAtPoint)
        //{
        //    Vector3D axis = new Vector3D(1, 0, 0);
        //    double angle = ang;
        //    Matrix3D matrix = device.Content.Transform.Value;
        //    matrix.RotateAt(new Quaternion(axis, angle), lookAtPoint);
        //    device.Content.Transform = new MatrixTransform3D(matrix);
        //}

        void MyRotationZ(ModelVisual3D device, double angle)
        {
            Vector3D axis = new Vector3D(0, 0, 1);
            Matrix3D matrix = device.Content.Transform.Value;
            matrix.Rotate(new Quaternion(axis, angle));
            device.Content.Transform = new MatrixTransform3D(matrix);
        }



        bool stopServer = false;
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
        void Start()
        {
            ThreadStart ts = new ThreadStart(GetData);
            thread = new Thread(ts);
            thread.Start();
        }

        void GetData()
        {
            try
            {
                server = new TcpListener(IPAddress.Any, connectionPort);
                server.Start();
                Console.WriteLine("Сервер готов к получению клиента");
                running = true;
                client = server.AcceptTcpClient();
                Console.WriteLine("Клиент подключился к серверу");
                //NetworkStream nwStream = client.GetStream();
                //byte[] buffer = new byte[64];
                //int bytesRead = nwStream.Read(buffer, 0, buffer.Length);
                //Console.WriteLine("test1 " + bytesRead);
                while (running)
                {
                    Connection();
                }
                server.Stop();
            }
            catch { }
        }
        NetworkStream nwStream;
        void Connection()
        {
            nwStream = client.GetStream();
            byte[] buffer = new byte[client.ReceiveBufferSize];
            int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
            string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            if (dataReceived.Length != 0)
            {
                Console.WriteLine(dataReceived);
                doIt(dataReceived);
            }

        }
        void LR(ModelVisual3D device, double z, double y)
        {
            Matrix3D matrix;
            matrix = device.Content.Transform.Value;
            matrix.OffsetZ = z;
            matrix.OffsetY = y;
            device.Content.Transform = new MatrixTransform3D(matrix);
        }
        void LRX(ModelVisual3D device, double x)
        {
            Matrix3D matrix;
            matrix = device.Content.Transform.Value;
            matrix.OffsetX = x;
            device.Content.Transform = new MatrixTransform3D(matrix);
        }

        void LeversRotation(ModelVisual3D device, int n)
        {
            switch (n)
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

        int numberOfLever = 1;
        int bogie = 0;
        int countOfRound = 0;
        int Result = 0;
        long number, result;


        async void doIt(string str)
        {
            if (str.Length == 2) // Если длина строки равна двум, значит мы передали на сервер данные по рычажку
                                 //первая цифра - номер рычажка, вторая цифра - цифра на рычажке
            {
                int tempL = int.Parse(str.Substring(0, 1)) - 1;
                int tempN = int.Parse(str.Substring(1, 1));
                levers[tempL] = tempN;
                LeversRotation(levers3D[tempL], tempN);
            }
            else if (str == "Прокрутили ручку вперед") // выполнить код для вращения ручки вперед
            {
                if (bogie == 5 && levers[8] != 0 || //рычажки с выбранными числами не находятся над ячейками
                bogie == 6 && levers[8] != 0 && levers[7] != 0 ||
                bogie == 7 && levers[8] != 0 && levers[7] != 0 && levers[6] != 0 ||
                iterations[bogie].Content.ToString() == "9")
                {
                    MessageBox.Show("Звонок!", "Ошибка!");
                    return;

                }
                RoundUp();
                number = 0;
                for (int i = 0; i < 9; i++) //собираем данные с рычажков в одно число
                {
                    number += levers[i] * (long)Math.Pow(10, i);
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
                    results[i].Content = (temp / (long)Math.Pow(10, i)) % 10;
                }
                iterationsValue[bogie] += 1;
                //string tmp = iterations[bogie].Content.ToString();
                iterations[bogie].Content = Math.Abs(iterationsValue[bogie]);
            }
            else if (str == "Прокрутили ручку назад") // выполнить код для вращения ручки назад
            {
                if (bogie == 5 && levers[8] != 0 || //рычажки с выбранными числами не находятся над ячейками
                bogie == 6 && levers[8] != 0 && levers[7] != 0 ||
                bogie == 7 && levers[8] != 0 && levers[7] != 0 && levers[6] != 0 ||
                iterations[bogie].Content.ToString() == "9")
                {
                    MessageBox.Show("Звонок!", "Ошибка!");
                    return;

                }
                RoundDown();
                number = 0;
                for (int i = 0; i < 9; i++) //собираем данные с рычажков в одно число
                {
                    number += levers[i] * (long)Math.Pow(10, i);
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
                    results[i].Content = (temp / (long)Math.Pow(10, i)) % 10;
                }
                iterationsValue[bogie] -= 1;
                //string tmp = iterations[bogie].Content.ToString();
                iterations[bogie].Content = Math.Abs(iterationsValue[bogie]);
            }
            else if (str.Length == 3) //выставить каретку на значение str[2]
            {
                bogie = (int)str[2];
                switch (bogie)
                {
                    case 1: BogLRX(-8); break;
                    case 2: BogLRX(-6); break;
                    case 3: BogLRX(-4); break;
                    case 4: BogLRX(-2); break;
                    case 5: BogLRX(0); break;
                    case 6: BogLRX(2); break;
                    case 7: BogLRX(4); break;
                    case 8: BogLRX(6); break;
                }
            }
            else if (str == "сбросил результаты") //сбросить результаты
            {
                for (int i = 0; i < results.Length; i++)
                {
                    results[i].Content = 0;
                    result = 0;
                }
                for (int j = 0; j < 18; j++)
                {
                    Vector3D axis = new Vector3D(1, 0, 0);
                    Matrix3D matrix = rc3D.Content.Transform.Value;
                    matrix.RotateAt(new Quaternion(axis, -20), RCloAtPoint);
                    rc3D.Content.Transform = new MatrixTransform3D(matrix);
                    await Task.Delay(75);
                }
            }
            else if (str == "сбросил обороты") //сбросил обороты
            {
                for (int i = 0; i < iterations.Length; i++)
                {
                    iterations[i].Content = 0;
                    iterationsValue[i] = 0;
                }
                for (int j = 0; j < 18; j++)
                {
                    Vector3D axis = new Vector3D(1, 0, 0);
                    Matrix3D matrix = lc3D.Content.Transform.Value;
                    matrix.RotateAt(new Quaternion(axis, -20), LCloAtPoint);
                    lc3D.Content.Transform = new MatrixTransform3D(matrix);
                    await Task.Delay(75);
                }
            }
        }

        void HandleRot(int angle)
        {
            Vector3D axis = new Vector3D(1, 0, 0);
            Matrix3D matrix = handle3D.Content.Transform.Value;
            matrix.RotateAt(new Quaternion(axis, angle), loAtPoint);
            handle3D.Content.Transform = new MatrixTransform3D(matrix);

        }
        async void RoundUp()
        {
            for (int j = 0; j < 18; j++)
            {
                HandleRot(-20);
                await Task.Delay(75);
            }
        }
        async void RoundDown()
        {
            for (int j = 0; j < 18; j++)
            {
                HandleRot(20);
                await Task.Delay(75);
            }
        }
    }

        //double t = -8;
        //private async void Button_Click(object sender, RoutedEventArgs e)
        //{

        //t += 2.3;
        //BogLRX(t);
        //for (int i = 0; i < 9; i++)
        //{
        //    string str = $"{9 - i}{i + 1}";
        //    doIt(str);
        //}
        //MyRotationX(levers3D[0]);//, 10, lookAtPointLever);
        //}



        void Add3DModels()
        {
            Model3D model3D;
            machine3D = new ModelVisual3D();
            model3D = Display3d(MODEL_PATH);
            machine3D.Content = model3D;
            viewPort3d.Children.Add(machine3D);


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

            //int temp = 0;
            for (int i = 0; i < levers3D.Length; i++)
            {
                levers3D[i] = new ModelVisual3D();
                model3D = Display3d(leversSTR[i]);
                levers3D[i].Content = model3D;
                viewPort3d.Children.Add(levers3D[i]);
            }
        }
    }
}
