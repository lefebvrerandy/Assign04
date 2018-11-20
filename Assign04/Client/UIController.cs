/* 
*  FILE          : UIController.cs
*  PROJECT       : PROG 2120 - Assignment 4
*  PROGRAMMER    : Bence Karner & Randy Lefebvre
*  DESCRIPTION   : This file contains the Logger class, and is used for recording data related to 
*                  errors, or application events to a log file. 
*/


using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
namespace Client
{

    /* 
    *   NAME    : UIController
    *   PURPOSE : This class was designed as a light weight solution to printing messages to the main display outside of the chat application.
    *   The class contains a single method used to print message boxes to the screen. It's primary purpose is to report errors caught in 
    *   catch blocks
    */
    class UIController
    {

        /*  
        *  METHOD        : PrintErrorToMessageBox
        *  DESCRIPTION   : This method is used to display a message box when an exception is thrown
        *  PARAMETERS    : The methods parameters are as follows,
        *  string errorCategory: The string used for the message box's title
        *  string messageToPrint: The string containing the actual error message printed to the message box
        *  RETURNS       : void : The method has no return value
        */
        public static void PrintErrorToMessageBox(string errorCategory, string messageToPrint)
        {

            //Configure the message box and print the error to the screen
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Error;
            MessageBox.Show(messageToPrint, errorCategory, button, icon);

        }//PrintErrorToMessageBox
    }//class
}//namespace
