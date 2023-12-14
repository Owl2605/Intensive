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

namespace Arithmometer
{

    public partial class _3Dmodel : Window
    {
        //Path to the model file
        private string MODEL_PATH = (Directory.GetCurrentDirectory() + @"\..\..\..\Arith.obj").ToString();

        MainWindow? mw;
        public MainWindow? MW { get { return mw; } set { mw = value; } }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            nwStream = client.GetStream();
            string s = "stop";
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            Console.WriteLine("true");
            nwStream.Write(bytes, 0, bytes.Length);
            Console.WriteLine("отправил стоп");
            client.Close();
            server.Stop();
            thread.Interrupt();
            thread.Join(); 
            MW.Show();
            this.Close();
        }
        public _3Dmodel()
        {
            InitializeComponent();
            Start();
            ModelVisual3D device3D = new ModelVisual3D();
            Model3D model3D = Display3d(MODEL_PATH);
            device3D.Content = model3D;
            viewPort3d.Children.Add(device3D);
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
        char numberOfLever = '0';
        char numberOfSlide = '0';
        int countOfRound = 0;
        int Result = 0;
        void doIt(string str)
        {
            this.Dispatcher.Invoke(() =>
            {
                Console.WriteLine(str + " длина" + str.Length);
                if (str.Length == 2) // Если длина строки равна двум, значит мы передали на сервер данные по рычажку
                                     //первая цифра - номер рычажка, вторая цифра - цифра на рычажке
                {
                    //тут должен быть код, связанный с выставлением цифр на рычажках 
                    //на рычажке str[0] выставить значение str[1]
                    numberOfLever = str[0];
                    switch (numberOfLever)
                    {
                        case '1': label1.Content = str[1]; break;
                        case '2': label2.Content = str[1]; break;
                        case '3': label3.Content = str[1]; break;
                        case '4': label4.Content = str[1]; break;
                        case '5': label5.Content = str[1]; break;
                        case '6': label6.Content = str[1]; break;
                        case '7': label7.Content = str[1]; break;
                        case '8': label8.Content = str[1]; break;
                        case '9': label9.Content = str[1]; break;
                    }
                }
                else if (str == "Прокрутили ручку вперед") // выполнить код для вращения ручки вперед
                {
                    countOfRound += 1;
                    labelRounds.Content = countOfRound;
                }
                else if (str == "Прокрутили ручку назад") // выполнить код для вращения ручки назад
                {
                    countOfRound += 1;
                    labelRounds.Content = countOfRound;
                }
                else if (str.Length == 3) //выставить каретку на значение str[2]
                {
                    numberOfSlide = str[2];
                    clearLabel();
                    switch (numberOfSlide)
                    {
                        case '1': label11.Content = 1; break;
                        case '2': label21.Content = 1; break;
                        case '3': label31.Content = 1; break;
                        case '4': label41.Content = 1; break;
                        case '5': label51.Content = 1; break;
                        case '6': label61.Content = 1; break;
                        case '7': label71.Content = 1; break;
                        case '8': label81.Content = 1; break;
                        case '9': label91.Content = 1; break;
                    }
                }
                else if (str == "сбросил результаты") //сбросить результаты
                {
                    labelResult.Content = "0";
                }
                else if (str == "сбросил обороты") //сбросил обороты
                {
                    labelRounds.Content = "0";
                }
                void clearLabel()
                {
                    label11.Content = "0";
                    label21.Content = "0";
                    label31.Content = "0";
                    label41.Content = "0";
                    label51.Content = "0";
                    label61.Content = "0";
                    label71.Content = "0";
                    label81.Content = "0";
                    label91.Content = "0";
                }
            });
        }

    }
}
