/* 
*  FILE          : Utility.cs
*  PROJECT       : PROG 2120 - Assignment 4
*  PROGRAMMER    : Bence Karner & Randy Lefebvre
*  DESCRIPTION   : This file acts as a repository for helper methods that don't logically fit into 
*                  any of the other modules. The file contains methods used primarily for user input 
*                  validation, and message formating
*/



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace Server
{

    /* 
    *   NAME    : Utility
    *   PURPOSE : Utility class designed to house helper methods that do not logically fit into the other classes. Methods are included for processing incoming,
    *             and outgoing messages, by performing validation checks, and ensuring a common encoding standard is enforced. Addditionally, methods are included
    *             for enforcing rules related to userName and message character usage.
    *   
    */
    class Utility
    {

        /*  
        *  METHOD        : ASCIIEncodeMessage
        *  DESCRIPTION   : This method is used to encode the clients message to ASCII from the assumed UTF-8/16
        *  PARAMETERS    : string outboundString : The outbound string that will be converted to ASCII encoding
        *  RETURNS       : string : ASCII encoded string
        */
        public string ASCIIEncodeMessage(string outboundString)
        {
            //Get input from the user through the console window
            string encodedString = string.Empty;
            encodedString = outboundString;

            try
            {
                //Create two different encodings.
                Encoding unicode = Encoding.Unicode;
                Encoding ascii = Encoding.ASCII;


                //Convert the unicode string, UT16 encoded input, into a byte[]
                byte[] unicodeBytes = unicode.GetBytes(encodedString);


                //Perform the conversion from UT16 to ASCII
                byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);
                encodedString = Encoding.ASCII.GetString(asciiBytes);
            }


            //Catch for non specific errors
            catch (Exception errorMessage)
            {
                //Console.WriteLine(errorMessage.ToString());
                Logger.LogApplicationEvents("Not Necessary", errorMessage.ToString());

            }

            return encodedString;

        }//GetClientInput


        /*  
        *  METHOD        : SetConsoleAttributes
        *  DESCRIPTION   : This method is used to set the console windows attributes once the program starts up.
        *                  It sets the text/background color, and window dimensions
        *  PARAMETERS    : void : The method takes no arguments
        *  RETURNS       : void : The method has no return
        */
        public void SetConsoleAttributes()
        {

            Console.Title = "PROG 2120 - A04 Server";
            Console.WindowWidth = 139;          //Set the window width to 139 units
            Console.WindowHeight = 37;          //Set the windows height to 37 lines
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkMagenta;

        }//SetConsoleAttributes
    }
}
