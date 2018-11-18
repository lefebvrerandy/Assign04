



using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Threading;
namespace Client
{


    public partial class App : Application
    {
        public static void Main()
        {

            //Instantiate the applications base WPF components
            App applicationMain = new App();
            applicationMain.InitializeComponent();


            //Open create a file used for logging errors, and log the application start time
            FileIO filemanager = new FileIO();
            filemanager.CreateFile("DEBUG GET FILEPATH FROM XML FILE");
            Logger.LogApplicationEvents("DEBUG GET FILEPATH FROM XML FILE", "APPLICATION START");


            //Open pipe used to signal to the server that the client is stil alive and functioning
            ClientStreamPipe pipeManager = new ClientStreamPipe();
            object outgoingStatusPipe = pipeManager.OpenOutgoingPipe();
            //Thread clientStatusThread = new Thread();


            //Begin the chat application
            Thread mainThread = Thread.CurrentThread;
            mainThread.IsBackground = true;
            applicationMain.Run();


            //Join the remaining threads, and exit the application
        }



        /*  
        *  METHOD        : 
        *  DESCRIPTION   : 
        *  PARAMETERS    : 
        *  RETURNS       : 
        */
        public void LaunchLoginWindow(object sender, StartupEventArgs args)
        {

            //Open the LoginWindow, and get the user's userName
            LoginWindow loginScreen = new LoginWindow();
            loginScreen.Owner = Application.Current.MainWindow;
            loginScreen.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            loginScreen.ShowDialog();
        }
    }//class
}//namespace
