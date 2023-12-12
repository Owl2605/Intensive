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

namespace Arithmometer
{
    /// <summary>
    /// Логика взаимодействия для _3Dmodel.xaml
    /// </summary>
    public partial class _3Dmodel : Window
    {
        //Path to the model file
        private string MODEL_PATH = (Directory.GetCurrentDirectory() + @"\..\..\..\arith.obj").ToString();

        MainWindow? mw;
        public MainWindow? MW { get { return mw; } set { mw = value; } }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MW.Show();
            this.Close();
        }
        public _3Dmodel()
        {
            InitializeComponent();
            Start();
            ModelVisual3D device3D = new ModelVisual3D();
            Model3D model3D = Display3d(MODEL_PATH);
            ScaleTransform3D _zScaleTransform = new ScaleTransform3D();
            model3D.Transform = _zScaleTransform;
            Rect3D bounds = model3D.Bounds;
            Point3D lookAtPoint = new Point3D(bounds.X + bounds.SizeX / 2, bounds.Y + bounds.SizeY / 2, bounds.Z + bounds.SizeZ / 2);
            _zScaleTransform.CenterZ = lookAtPoint.Z;
            device3D.Content = model3D;
            // Add to view port
            viewPort3d.Children.Add(device3D);
        }

        /// <summary>
        /// Display 3D Model
        /// </summary>
        /// <param name="model">Path to the Model file</param>
        /// <returns>3D Model Content</returns>
        private Model3D Display3d(string model)
        {
            Model3D device = null;
            try
            {
                //Adding a gesture here
                viewPort3d.RotateGesture = new MouseGesture(MouseAction.LeftClick);

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
                client = server.AcceptTcpClient();
                Console.WriteLine("Клиент подключился к серверу");
                running = true;
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

        void Connection()
        {
            NetworkStream nwStream = client.GetStream();
            byte[] buffer = new byte[client.ReceiveBufferSize];
            int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
            string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            if (dataReceived.Length != 0)
            {
                Console.WriteLine(dataReceived);
                doIt(dataReceived);
            }
            //string s = "true";
            //byte[] bytes = Encoding.UTF8.GetBytes(s);
            //Console.WriteLine("true");
            //nwStream.Write(bytes, 0, bytes.Length);

        }
        void doIt(string str)
        {
            if (str.Length == 2) // Если длина строки равна двум, значит мы передали на сервер данные по рычажку
                                 //первая цифра - номер рычажка, вторая цифра - цифра на рычажке
            {
                //тут должен быть код, связанный с выставлением цифр на рычажках 
                //на рычажке str[0] выставить значение str[1]
            }
            if (str == "вращаем ручку") // выполнить код для вращения ручки
            {

            }
            if (str.Length == 1) //выставить каретку на значение str[0]
            {

            }
        }
    }
}
