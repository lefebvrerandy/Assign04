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
using System.Threading.Tasks;

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


            //Open the LoginWindow, and get the user's userName
            LoginWindow loginScreen = new LoginWindow();
            loginScreen.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            loginScreen.ShowDialog();


            //Check if the user entered a valid userName, if not, close the chat window and exit the program
            if (User.ClientID == null)
            {
                Application.Current.Shutdown();
            }

            //Create a thread for managing incoming messages, and printing to the output textbox
            Thread incomingMessageManager = new Thread(ThreadedListener);
            incomingMessageManager.Name = "ThreadedListener";
            incomingMessageManager.Start();


            //Create a therad ofr managing outgoing messages
            Thread outgoingMessageManager = new Thread(ThreadedSender);
            outgoingMessageManager.Name = "ThreadedSender";
            outgoingMessageManager.Start();


            userNameLabel.Content = "Username: " + User.ClientID;
        }//MainWindow



        /*  
        *  METHOD        : 
        *  DESCRIPTION   : 
        *  PARAMETERS    : 
        *  RETURNS       : 
        */
        public void ThreadedListener()
        {

            //Open the necessary connections for reading from the server
            ClientStreamPipe pipeManager = new ClientStreamPipe();

            FileIO fileManager = new FileIO();
            string pipeName = fileManager.ReadXMLDocument("pipeName-incoming");      //string indicator of the element to search in the XML doc
            NamedPipeClientStream incomingMessagePipe = pipeManager.OpenIncomingPipe(pipeName);
            StreamReader inputStream = new StreamReader(incomingMessagePipe);
            Utility messageFormatter = new Utility();



            //Check to ensure the client hasn't signaled that they wish to shutdown the application
            while (User.ClientID != null)
            {

                //Read the incoming data from the stream, format the message, and add it to the output window
                string formattedMessage = inputStream.ReadLine();
                if (formattedMessage != "")
                {
                    formattedMessage = messageFormatter.BuildDisplayString(formattedMessage);
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        OutputTextBox.Text = OutputTextBox.Text + Environment.NewLine + formattedMessage;
                    }));

                }

                Thread.Sleep(100);
            }
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

            FileIO fileManager = new FileIO();
            string pipeName = fileManager.ReadXMLDocument("pipeName-outgoing");      //string indicator of the element to search in the XML doc
            NamedPipeClientStream outgoingMessagePipe = pipeManager.OpenOutgoingPipe(pipeName);
            StreamWriter outputStream = new StreamWriter(outgoingMessagePipe);
            Utility messageFormatter = new Utility();



            //Check to ensure the client hasn't signaled that they wish to shutdown the application
            while (User.ClientID != null)
            {

                //Check the user has a message ready to send
                if (User.Message != null)
                {
                    //ASCII encode the string, and build the output message as: clientID, clientCommand, clientString/textbox input
                    string outboundMessage = messageFormatter.ASCIIEncodeMessage(User.Message);
                    outboundMessage = messageFormatter.BuildOutboundString(User.ClientID, User.Command, User.Message);
                    outputStream.WriteLine(outboundMessage);
                    outputStream.Flush();


                    //Reset the client's message components before the next cycle
                    User.Command = null;
                    User.Message = null;
                }
                Thread.Sleep(100);
            }
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
            FileIO fileManager = new FileIO();


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


                        //Split the text into the string array, and write each line to the file
                        OutputTextBox.SelectAll();
                        string textboxContents = OutputTextBox.SelectedText;
                        string[] textArray = Utility.StringSplitter(textboxContents);
                        fileManager.WriteToFile(filepath, textArray);
                    }
                }
            }//try


            catch (Exception errorMessage)
            {
                string filepath = fileManager.ReadXMLDocument("logFilePath");   //Indicator of the element to search in the XML doc
                Logger.LogApplicationEvents(filepath, errorMessage.ToString());
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
        *  METHOD        : 
        *  DESCRIPTION   : 
        *  PARAMETERS    : 
        *   object menuUIEvent : 
        *   RoutedEventArgs eventTrigger : 
        *  RETURNS       : 
        */
        private void Automate_Messages(object sender, RoutedEventArgs e)
        {
            bool flag = Automate.IsChecked;
            if (Automate.IsEnabled)
            {
                Task task1 = Task.Factory.StartNew(new Action(AutomatePlease));
                //task1.Wait();
                ////Generate random string
                //Utility generatedString = new Utility();
                //Thread.Sleep(generatedString.AutomateGenerateSleep());                  // Generate the sleep timer
                //string stringRnd = generatedString.AutomateGenerateString(); // Generate the random string
                //InputTextBox.SelectedText = stringRnd;              // Put the contents into the textbox
            } 


        }//Automate_Messages



        /*  
        *  METHOD        : 
        *  DESCRIPTION   : 
        *  PARAMETERS    : 
        *  RETURNS       : 
        */
        private void AutomatePlease()
        {
            //Generate random string
            Utility generatedString = new Utility();
            while (true)
            {
                Thread.Sleep(generatedString.AutomateGenerateSleep());                  // Generate the sleep timer
                string stringRnd = generatedString.AutomateGenerateString(); // Generate the random string
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    InputTextBox.SelectedText = stringRnd;
                }));
            }

        }



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
            User.Command = "-1";                //clientCommand of -1 means clientShutdown; informs the server to remove the pipes used by the client
            Application.Current.Shutdown();     //Close the application

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


            catch (Exception errorMessage)
            {
                FileIO fileManager = new FileIO();
                string filepath = fileManager.ReadXMLDocument("logFilePath");   //Indicator of the element to search in the XML doc
                Logger.LogApplicationEvents(filepath, errorMessage.ToString());
            }

        }//WindowExit



        /*  
        *  METHOD        : TextScanner
        *  DESCRIPTION   : This event is used to scan the text entered by the user, and update the 
        *   the char counter with each new character. Additionally, the method 
        *   checks each cahracter to see if the '\n' char (ENTER) was entered; if true, then
        *   it saves the users string and clears the input textbox
        *  PARAMETERS    : parameters are as follows,
        *  object sender:   The UI element which raised the event (Input textbox in this case)  
        *  RoutedEventArgs e: Contains the event data, in this case it's data related to the textual input in the chat window
        *  RETURNS       : void : This method has no return
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
                        User.Command = "1";                 //clientCommand of 1 marks the message as ordinary; informs the server to  redirect the message to all clients
                        User.Message = textboxContents;
                        InputTextBox.Text = "";
                    }
                }

                UpdateCharCount(sender, e);

            }//textbox is in focus
            else if (Automate.IsChecked)
            {
                if (InputTextBox.Text.Length <= 2000)
                {

                    //Dump the contents of the textbox into a string and scan for the newline char (signals a send command)
                    string textboxContents = InputTextBox.Text;
                    if (textboxContents.IndexOf("\n") > -1)
                    {

                        //Save the message, and clear the textbox
                        User.Command = "1";                 //clientCommand of 1 marks the message as ordinary; informs the server to  redirect the message to all clients
                        User.Message = textboxContents;
                        InputTextBox.Text = "";
                    }
                }
            }
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
        }

    }//class
}//namespace
