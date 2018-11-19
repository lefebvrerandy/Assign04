using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace Server
{

    class Utility
    {
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
                Console.WriteLine(errorMessage.ToString());
            }

            return encodedString;

        }//GetClientInput


        /*  
        *  METHOD        : SetConsoleAttributes
        *  DESCRIPTION   : This method is used to set the console windows attributes once the program starts up.
        *                  It sets the text/background colour, and window dimensions
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
