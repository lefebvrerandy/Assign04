/* 
*  FILE          : About.xaml.cs
*  PROJECT       : PROG 2120 - Assignment 4
*  PROGRAMMER    : Bence Karner & Randy Lefebvre
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
            aboutWindowProgramAuthor.Text = "Author: Bence Karner & Randy Lefebvre";
            aboutWindowProgramVersion.Text = "Version: 1.0";
            aboutWindowProgramDescription.Text = "Description: The application allows users to send messages to one another. Additionally, the user is able to export all chat data received during the session, and save it to a text file. This application is provided as is, without warranty of any kind ";

        }//...About


        /*  
        *  METHOD        : aboutOKButtonClick
        *  DESCRIPTION   : This method is used to close the about window when the user clicks on the OK button
        *  PARAMETERS    : Parameters are as follows,
        *   object mneuUIEvent : The object from which the event was triggered
        *   RoutedEventArgs eventTrigger : Identifier for the triggered event
        *  RETURNS       : void : The method has no return value
        */
        private void AboutOKButtonClick(object mneuUIEvent, RoutedEventArgs eventTrigger)
        {
            this.Close();

        }//...aboutOKButtonClick

    }//...class
}//...namespace
