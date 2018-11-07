/* 
*  FILE          : UtilityMethods.cs
*  PROJECT       : PROG 2120 - Assignment 1
*  PROGRAMMER    : Bence Karner & Randy Lefebvre
*  DESCRIPTION   : This file acts as a repository for helper methods that don't logically fit into 
*                  any of the other modules. The file contains methods used primarily for user input 
*                  validation, and message formating
*/


using System;
using System.Text;


namespace UtilityMethods
{


    /* 
    *   NAME    : Utility
    *   PURPOSE : Utility class designed to house helper methods that do not logically fit into the other classes 
    */
    class Utility
    {

        //Rebuild the clients message into a single string
        //Format of the outboundString is clientID, commandID, message
        public static string BuildOutboundString(string clientMessageComponents)
        {
            string completeOutboundMessage = null;
            return completeOutboundMessage;

        }//...BuildClientOutboundString


        /*
        *  REFERENCES    :  The following method was heavily based on the examples provided by Microsoft in there
        *                   char encoding documentation. For a full reference, please see
        *                   Microsoft. (ND). Encoding.Convert Method (Encoding, Encoding, Byte[]). Retrieved September 10, 2018, 
        *                   from https://msdn.microsoft.com/en-us/library/windows/apps/kdcak6ye(v=vs.105).aspx
        */
        public static string GetClientInput()
        {
            //Get input from the user through the console window
            string newString = string.Empty;
            newString = Console.ReadLine();

            try
            {
                ////Create two different encodings.
                //Encoding unicode = Encoding.Unicode;
                //Encoding ascii = Encoding.ASCII;


                ////Convert the unicode string, UT16 encoded input, into a byte[]
                //byte[] unicodeBytes = unicode.GetBytes(newString);


                ////Perform the conversion from UT16 to ASCII
                //byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);
                //newString = Encoding.ASCII.GetString(asciiBytes);
            }


            //If the source encoding or destination encoding parameters are null from Encoding.Convert(), then inform the user
            catch (ArgumentNullException nullException)
            {
                throw new NotImplementedException();
            }

            return newString;
        }//...GetClientInput




        public static bool ValidateInputString(ref string clientStringToValidate)
        {
            bool isStringValid = true;


            //Ensure the clients string isnt above the 1024 char length limit
            if (clientStringToValidate.Length > 1023) //Leave space for the new line char
            {
                isStringValid = false;
                clientStringToValidate = string.Empty;
            }


            if (clientStringToValidate.Length < 1) //Min input length is 1 char
            {
                isStringValid = false;
                clientStringToValidate = string.Empty;
            }


            //The clients input string has been converted to ASCII from unicode, so check for any non-english chars
            //  ASCII values below 32, and above 127 are considered invalid
            int invalidCharCount = 0;
            foreach (char c in clientStringToValidate)
            {
                if ((int)c > 127)
                {
                    invalidCharCount++;
                }

                if ((int)c < 32)
                {
                    invalidCharCount++;
                }
            }

            if (invalidCharCount > 0)
            {
                isStringValid = false;
            }

            return isStringValid;
        }//...ValidateInputString

    }//...class Utility
}//...namespace
