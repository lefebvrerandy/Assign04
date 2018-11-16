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
using System.ComponentModel;
using Microsoft.Win32;
using System.Threading;
using FileManager;
using Client;


//===========================================================================
//TODO:

/*
 * ADD EVENT FOR SEND BUTTON? (trigger send command by ENTER or send button)
 * CHANGE THE INPUT AREA FROM RTB TO A TEXT BOX
 * ADD STATUS BAR FOR INPUT CHAR COUNT
 *      ADD EVENT CODE FOR CHAR COUNT
 * REARRANGE CLIENT APPLICATION TO LOOK PROPER
 * ADD EVENT FOR EXPORT MESSAGE OPTION (half done, save option is enabled but not functional)
 * FIX ABOUT WINDOW TO HAVE THE PROPER TEXT
 * FIX ALL COMMENT BLOCKS AND HEADERS 
 */
//===========================================================================


namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            MainWindow ApplicationWindow = new MainWindow();
            Logger.LogApplicationEvents("DEBUG INSERT FILEPATH TO LOG FILE", "APPLICATION START");



            //try
            //{
            //    ////Thread used for managing all incoming messages
            //    //Thread incomingMessageThread = new Thread();
            //    //incomingMessageThread.Start();

            //    ////Thread used for managing all outgoing messages
            //    //Thread outgoingMessageThread = new Thread();
            //    //outgoingMessageThread.Start();

            //    ////Thread used for signaling to the server that the client is still alive, and connected (DEBUG MAYBE REMOVE DEPENDING ON HOW RANDY STRUCTURES SERVER)
            //    //Thread clientStatusThread = new Thread();
            //    //clientStatusThread.Start();
            //}



            //catch (OutOfMemoryException exception)
            //{
            //    //DEBUG ADD THE LOGGING METHOD TO CAPTURE THE EVENT
            //    //There is not enough memory available to start the thread.
            //    throw new NotImplementedException();
            //}

            //Open stream for capturing input from the user, and writing to the server
            //StreamReader inputStream = new StreamReader(DEBUG INSERT SERVER PIPE NAME);
            //StreamWriter outputStream = new StreamWriter(client);

            //finally
            //{
            //    incomingMessageThread.join();
            //    outgoingMessageThread.join();
            //    clientStatusThread.join();
            //}


        }
    }



    /* 
    *   NAME    : MainWindow
    *   PURPOSE : The purpose of this class is to instantiate the elements of the main window, and to handle events originating from it. 
    *             Methods are provided for handeling events related to user interaction with the applications manin menu. This includes 
    *             event handlers for saving chat logs, clearing/closing the application, and reacting to changes made in the textbox.
    */
    public partial class MainWindow : Window
    {
        private bool ChangesMade { get; set; }     //Auto property for checking if changes have been made to the document

        /*  
        *  METHOD        : MainWindow
        *  DESCRIPTION   : This method is used to instantiate the elements of the window
        *  PARAMETERS    : void : The method takes no arguments
        *  RETURNS       : void : The method has no return value
        */
        public MainWindow()
        {
            InitializeComponent();
            ChangesMade = false;
        }//...MainWindow



        /*  
        *  METHOD        : MenuSaveClick
        *  DESCRIPTION   : This method allows the user to save the contents of the Incoming Textbox area, into a txt file
        *  PARAMETERS    : Parameters are as follows,
        *   object menuUIEvent : The object from which the even was triggered
        *   RoutedEventArgs eventTrigger : Identifier for the triggered event
        *  RETURNS       : void : The method has no return value
        *  
        *  NOTE: Much of the methods functionality was provided by Microsoft in their Dialog Boxes Overview Example;
        *        For more information, please see: Microsoft. (2017). Dialog Boxes Overview. Retrieved Octover 2, 2018, from
        *           https://docs.microsoft.com/en-us/dotnet/api/microsoft.win32.openfiledialog?view=netframework-4.7.2
        */
        private void MenuSaveClick(object menuUIEvent, RoutedEventArgs eventTrigger)
        {

            try
            {
                SaveFileDialog saveFileWindow = new SaveFileDialog();


                //Configure save file dialog box
                saveFileWindow.Title = "Save Chat Log: Save";                   //Set the window title
                saveFileWindow.FileName = "";                                   //Default file name
                saveFileWindow.DefaultExt = ".txt";                             //Default file extension
                saveFileWindow.Filter = "Text documents (.txt)|*.txt";          //Filter files by extension
                saveFileWindow.InitialDirectory = @"%userprofile%\desktop";     //Set the initial open directory to the users dektop


                //Display the file menu, and capture the users response if they select a file
                Nullable<bool> fileSelected = saveFileWindow.ShowDialog();
                if (fileSelected == true)
                {

                    //Check if the target filename exists; if true, save the path and open the file
                    if (saveFileWindow.CheckPathExists == true)
                    {

                        //Get the filepath from the diolog box, and select all the text in the document
                        string filepath = saveFileWindow.FileName;

                        //DEBUG HOW TO SELECT ALL THE TEXT IN THE INPUT WINDOW?
                        //string selectedText = InputTextBox.SelectAll();


                        //Split the text into the string array, and write each line to the file
                        FileIO fileManager = new FileIO();
                        string[] fileContents = new string[] { };
                        fileContents = selectedText.Split('\n');
                        fileManager.WriteToFile(filepath, fileContents);
                    }
                }
            }//...try


            catch (Exception errorMessage)
            {
                //DEBUG ADD LOGGING METHOD
                throw new NotImplementedException();
            }

        }//...MenuSaveClick



        /*  
        *  METHOD        : MenuAboutClick
        *  DESCRIPTION   : This method  is used to print the about statement for the program
        *  PARAMETERS    : Parameters are as follows,
        *   object menuUIEvent : The object from which the event was triggered
        *   RoutedEventArgs eventTrigger : Identifier for the triggered event
        *  RETURNS       : void : The method has no return value
        *  
        *  References: The source code, and C# image found in the About window were retrieved from the following sources,
        *  Bakalenyik, M. (ND). C#: Static vs Non-Static Classes and Static vs Instance Methods. Retrieved on Oct 5, 2018, from
        *       https://hackernoon.com/c-static-vs-instance-classes-and-methods-50fe8987b231 
        *  
        *  Mika, N. (2018). WpfApp1 [source code]. Retrieved on Oct 5, 2018, from Conestoga College, Doon Campus, K Drive
        */
        private void MenuAboutClick(object menuUIEvent, RoutedEventArgs eventTrigger)
        {
            //Configure the about window and display it
            About aboutMessageBox = new About();
            aboutMessageBox.Owner = Application.Current.MainWindow;
            aboutMessageBox.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            aboutMessageBox.ShowDialog();

        }//...MenuAboutClick



        /*  
        *  METHOD        : MenuExitClick
        *  DESCRIPTION   : This method is used to close the application when the user selects the Exit command from the File menu
        *  PARAMETERS    : Parameters are as follows,
        *   object menuUIEvent : The object from which the event was triggered
        *   RoutedEventArgs eventTrigger : Identifier for the triggered event
        *  RETURNS       : void : The method has no return value
        */
        private void MenuExitClick(object menuUIEvent, RoutedEventArgs eventTrigger)
        {
            Application.Current.Shutdown();    //Close the application

        }//...MenuExitClick



        /*  
        *  METHOD        : windowExit
        *  DESCRIPTION   : This method is used to check if the user wants to close the application, once the top right X button is pressed
        *  PARAMETERS    : Parameters are as follows,
        *   object menuUIEvent : The object from which the event was triggered
        *   CancelEventArgs eventTrigger : Identifier for the triggered event
        *  RETURNS       : void : The method has no return value
        */
        public void WindowExit(object menuUIEvent, CancelEventArgs eventTrigger)
        {
            try
            {

                //Check if changes were made to the document before confirming the user wants to exit
                if (ChangesMade == true)
                {
                    MessageBoxResult exitConfirmation = MessageBox.Show("Are you sure you wish to exit?", "Exit Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (exitConfirmation == MessageBoxResult.Yes)
                    {
                        //Exit confirmed, close the application
                        RoutedEventArgs newEventTrigger = new RoutedEventArgs();
                        MenuExitClick(menuUIEvent, newEventTrigger);
                    }


                    else
                    {
                        //The user indicated they don't want to close the application
                        eventTrigger.Cancel = true;
                    }
                }
            }


            catch (Exception errorMessage)
            {
                //DEBUG ADD LOGGING METHOD
                throw new NotImplementedException();
            }

        }//...WindowExit
        


        private void UpdateCharacterCount(object sender, TextChangedEventArgs e)
        {
            if (InputTextBox != null)
            {
                InputTextBox.Text = InputTextBox.Text.Length.ToString();
            }
        }
    
    }//...class
}//...namespace
