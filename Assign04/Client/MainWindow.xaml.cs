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
using System.IO;
using System.IO.Pipes;
namespace Client
{

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
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ChangesMade = false;


            Thread incomingMessageManager = new Thread(ThreadedListener);
            incomingMessageManager.Start();


            Thread outgoingMessageManager = new Thread(ThreadedSender);
            outgoingMessageManager.Start();
        }//MainWindow



        /*  
        *  METHOD        : 
        *  DESCRIPTION   : 
        *  PARAMETERS    : 
        *  RETURNS       : 
        */
        public void ThreadedListener()
        {
            ClientStreamPipe pipeManager = new ClientStreamPipe();
            NamedPipeClientStream incomingMessagePipe = pipeManager.OpenIncomingPipe();
            StreamReader inputStream = new StreamReader(incomingMessagePipe);




        }



        /*  
        *  METHOD        : 
        *  DESCRIPTION   : 
        *  PARAMETERS    : 
        *  RETURNS       : 
        */
        public void ThreadedSender()
        {


            //Open the necessary connections for writing to the server
            ClientStreamPipe pipeManager = new ClientStreamPipe();
            NamedPipeClientStream outgoingMessagePipe = pipeManager.OpenOutgoingPipe();
            StreamWriter outputStream = new StreamWriter(outgoingMessagePipe);


            //Format the outgoing message
            //DEBUG CHECK THE USERS MESSAGE IF ITS ACTUALLY CONTAINING SOMETHING
            //DEBUG run the string through the encoding method and return the byte array
            //

            //Write the users complete message to the pipe
            outputStream.WriteLine(User.Message);
            outputStream.Flush();
        }



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
                        OutputTextBox.SelectAll();
                        string textboxContents = OutputTextBox.SelectedText;
                        string[] textArray = Utility.StringSplitter(textboxContents);
                        fileManager.WriteToFile(filepath, textArray);
                    }
                }
            }//try


            catch (Exception errorMessage)
            {
                //DEBUG ADD LOGGING METHOD
                Logger.LogApplicationEvents("DEBUG INSERT FILEPATH FROM XML", errorMessage.ToString());
            }

        }//MenuSaveClick



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

        }//MenuAboutClick



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

        }//MenuExitClick



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

        }//WindowExit



        /*  
        *  METHOD        : 
        *  DESCRIPTION   : 
        *  PARAMETERS    : 
        *  RETURNS       : 
        */
        private void TextScanner(object sender, RoutedEventArgs e)
        {
            if (InputTextBox.IsFocused == true)
            {

                if (InputTextBox.Text.Length <= 2000)
                {
                    //Dump the contents of the textbox into a string and scan for the newline char (signals a send command)
                    string textboxContents = InputTextBox.Text;
                    if (textboxContents.IndexOf("\n") > -1)
                    {

                        //Save the message, and clear the textbox
                        User.Message = textboxContents;
                        InputTextBox.Text = "";
                    }
                }

                UpdateCharCount(sender, e);
            }//textbox is in focus
        }//TextScanner



        /*  
        *  METHOD        : 
        *  DESCRIPTION   : 
        *  PARAMETERS    : 
        *  RETURNS       : 
        */
        private void UpdateCharCount(object sender, RoutedEventArgs e)
        {
            int charCount = InputTextBox.Text.Length;
            charCountOutput.Text = charCount.ToString();


            if (charCount > 2000)
            {
                //DEBUG style the text to look red

            }
        }

    }//class
}//namespace
