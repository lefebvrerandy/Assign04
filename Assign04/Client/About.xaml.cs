/* 
*  FILE          : About.xaml.cs
*  PROJECT       : PROG 2120 - Assignment 2
*  PROGRAMMER    : Bence Karner
*  DESCRIPTION   : This file contains the About class, which is used to instantiate, and close the About window. The file is 
*                  part of the same project as MainWindow.Xaml.cs, and behaves as a respository for the main windows
*                  operation.
*/


using System.Windows;


namespace Client
{


    /* 
    *   NAME    : About
    *   PURPOSE : The purpose of this class is to instantiate the elements of the "About" window, and to handle the windows closure. 
    *             The window is called from the main window and relys on the user selecting the appropriate option from the main menu.
    */
    public partial class About : Window
    {


        /*  
        *  METHOD        : About
        *  DESCRIPTION   : This method is used to initialize the about window once the user selects the option from the Help option in the main menu
        *  PARAMETERS    : void : The method takes no arguments
        *  RETURNS       : void : The method has no return value
        */
        public About()
        {
            InitializeComponent();


            //Configure the window message and startup location 
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            aboutWindowProgramAuthor.Text = "Author: Bence Karner";
            aboutWindowProgramVersion.Text = "Version: 1.0";
            aboutWindowProgramDescription.Text = "Description: TextEdit# is a text editor written in C#. The application allows users to write, copy & paste, and format text with different font options. The user is also able to save and load .txt files. This application is provided as is, without warranty of any kind ";

        }//...About


        /*  
        *  METHOD        : aboutOKButtonClick
        *  DESCRIPTION   : This method is used to close the about window when the user clicks on the OK button
        *  PARAMETERS    : Parameters are as follows,
        *   object mneuUIEvent : The object from which the event was triggered
        *   RoutedEventArgs eventTrigger : Identifier for the triggered event
        *  RETURNS       : void : The method has no return value
        */
        private void aboutOKButtonClick(object mneuUIEvent, RoutedEventArgs eventTrigger)
        {
            this.Close();

        }//...aboutOKButtonClick

    }//...class
}//...namespace
