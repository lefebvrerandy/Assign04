/* 
*  FILE          : App.xaml.cs
*  PROJECT       : PROG 2120 - Assignment 4
*  PROGRAMMER    : Bence Karner & Randy Lefebvre
*  DESCRIPTION   : This file contains the the starting point of the client applications. The program 
*                  creates a file, and logs the start of the client application, before opening into the login window, and 
*				   starting the chat applciation.
*
* References: The source code, and C# image found in the About window were retrieved from the following sources,
*   Bakalenyik, M. (ND). C#: Static vs Non-Static Classes and Static vs Instance Methods. Retrieved on Oct 5, 2018, from
*   https://hackernoon.com/c-static-vs-instance-classes-and-methods-50fe8987b231 
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

        [STAThread]
        public static void Main()
        {

            //Instantiate the applications base WPF components
            var applicationMain = new App();
            applicationMain.InitializeComponent();


            //Name the main thread for identification once the chat window is up
            Thread mainThread = Thread.CurrentThread;
            mainThread.IsBackground = true;
            User.ClientID = null;


            //Open create a file used for logging errors, and log the application start time
            FileIO fileManager = new FileIO();
            string filepath = fileManager.ReadXMLDocument("logFilePath");
            fileManager.CreateFile(filepath);
            Logger.LogApplicationEvents(filepath, "APPLICATION START");


            //Begin the chat application
            applicationMain.Run();
        }
    }//class
}//namespace
