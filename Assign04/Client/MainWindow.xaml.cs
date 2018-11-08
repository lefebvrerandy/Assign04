/* 
*  FILE          : MainWindow.xaml.cs
*  PROJECT       : PROG 2120 - Assignment 4
*  PROGRAMMER    : Bence Karner & Randy Lefebvre
*  DESCRIPTION   : This file houses the main entry point for the application, and contains all the logic 
*                  requried to execute each module. The program was created as per assigment requirments, 
*                  and is intended to function as part of a client-server chat system. The chat client 
*				   uses a duplex named pipe to communicate with the server. The program, allows the user to
*				   send a message using the text box input area, and display received messages from the server
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using FileManager;


namespace Client
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                //Thread used for managing all incoming messages
                Thread incomingMessageThread = new Thread();
                incomingMessageThread.Start();

                //Thread used for managing all outgoing messages
                Thread outgoingMessageThread = new Thread();
                outgoingMessageThread.Start();

                //Thread used for signaling to the server that the client is still alive, and connected (DEBUG MAYBE REMOVE DEPENDING ON HOW RANDY STRUCTURES SERVER)
                Thread clientStatusThread = new Thread();
                clientStatusThread.Start();
            }



            catch (OutOfMemoryException exception)
            {
                //DEBUG ADD THE LOGGING METHOD TO CAPTURE THE EVENT
                //There is not enough memory available to start the thread.
            }

            //Open stream for capturing input from the user, and writing to the server
            //StreamReader inputStream = new StreamReader(DEBUG INSERT SERVER PIPE NAME);
            //StreamWriter outputStream = new StreamWriter(client);

            //finally
            //{
            //    incomingMessageThread.join();
            //    outgoingMessageThread.join();
            //    clientStatusThread.join();
            //}

        }//...MainWindow


        //DEBUG ADD EVENT FOR SEND BUTTON
        //DEBUG ADD EVENT FOR CANCEL BUTTON
        //DEBUG ADD EVENT FOR CLOSE APPLICATION BUTTON
        //DEBUG ADD EVENT FOR ABOUT BUTTON
        //DEBUG ADD EVENT FOR EXPORT MESSAGE OPTION


    }//...class
}//...namespace
