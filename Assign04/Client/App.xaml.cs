/* 
*  FILE          : App.xaml.cs
*  PROJECT       : PROG 2120 - Assignment 4
*  PROGRAMMER    : Bence Karner & Randy Lefebvre
*  DESCRIPTION   : DEBUG
*				   
*
* References: The source code, and C# image found in the About window were retrieved from the following sources,
* Bakalenyik, M. (ND). C#: Static vs Non-Static Classes and Static vs Instance Methods. Retrieved on Oct 5, 2018, from
* https://hackernoon.com/c-static-vs-instance-classes-and-methods-50fe8987b231 
*
*/





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
            User.ClientID = null;


            //Open create a file used for logging errors, and log the application start time
            FileIO fileManager = new FileIO();
            string filepath = fileManager.ReadXMLDocument("logFilePath");
            fileManager.CreateFile(filepath);
            Logger.LogApplicationEvents(filepath, "APPLICATION START");


            //Open pipe used to signal to the server that the client is stil alive and functioning
            ClientStreamPipe pipeManager = new ClientStreamPipe();


            string pipeName = fileManager.ReadXMLDocument("pipeName-outgoing");      //string indicator of the element to search in the XML doc
            object outgoingStatusPipe = pipeManager.OpenOutgoingPipe(pipeName);
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


            //Check if the user entered a valid userName, if not, close the chat window and exit the program
            if (User.ClientID == null)
            {
                Application.Current.Shutdown();
            }
        }
    }//class
}//namespace
