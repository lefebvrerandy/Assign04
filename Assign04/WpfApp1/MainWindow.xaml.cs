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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using Server_Service;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Thread tServer = null;
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Start_Btn_Click(object sender, RoutedEventArgs e)
        {
            //Start the Thread
            txtbox.Text = "Server Starting";
            Server.Run = true;
            ThreadStart ts = new ThreadStart(Server.RunServer);
            tServer = new Thread(ts);
            tServer.Start();
            txtbox.Text = "Server Started";
        }

        private void Pause_Btn_Click(object sender, RoutedEventArgs e)
        {
            //Pause the Thread
            txtbox.Text = "Server Pausing";
            Server.Pause = true;
            txtbox.Text = "Server Paused";

        }

        private void Continue_Btn_Click(object sender, RoutedEventArgs e)
        {
            //Resume the Thread
            txtbox.Text = "Server Continuing";
            Server.Pause = false;
            txtbox.Text = "Server Continued";
        }

        private void Stop_Btn_Click(object sender, RoutedEventArgs e)
        {
            //Stop the Thread
            txtbox.Text = "Server Stopping";
            Server.Run = false;
            Server.toDisconnect = true;
            // Wait for the server thread to finish
            Thread.Sleep(100);
            tServer.Join();
            txtbox.Text = "Server Stopped";
        }
    }
}
