/* 
*  FILE          : UtilityMethods.cs
*  PROJECT       : PROG 2120 - Assignment 4
*  PROGRAMMER    : Bence Karner & Randy Lefebvre
*  DESCRIPTION   : This file acts as a repository for helper methods that don't logically fit into 
*                  any of the other modules. The file contains methods used primarily for user input 
*                  validation, and message formating
*/


using System;
using System.Text;
namespace Client
{


    /* 
    *   NAME    : Utility
    *   PURPOSE : Utility class designed to house helper methods that do not logically fit into the other classes. Methods are included for processing incoming,
    *             and outgoing messages, by performing validation checks, and ensuring a common encoding standard is enforced. Addditionally, methods are included
    *             for enforcing rules related to userName and message character usage.
    *   
    */
    public class Utility
    {


        /*  
        *  METHOD        : 
        *  DESCRIPTION   : 
        *  PARAMETERS    : 
        *  RETURNS       : 
        */
        public static string[] StringSplitter (string stringToSplit)
        {

            //Split the chat logs by the new line char
            string[] returnStringArray = new string[] { };
            returnStringArray = stringToSplit.Split('\n');
            return returnStringArray;
        }

        /*  
        *  METHOD        : 
        *  DESCRIPTION   : 
        *  PARAMETERS    : 
        *  RETURNS       : 
        */
        public static string BuildOutboundString(string userName, string command, string outboundMessage)
        {
            string completeOutboundMessage = null;
            completeOutboundMessage += userName + ',' + command + ',' + outboundMessage;
            return completeOutboundMessage;

        }//BuildClientOutboundString


        /*  
        *  METHOD        : 
        *  DESCRIPTION   : 
        *  PARAMETERS    : 
        *  RETURNS       : 
        */
        public static string BuildDisplayString(string messageFromServer)
        {
            string completeOutboundMessage = null;

            //
            string timeStamp = DateTime.Now.ToString("dd: hh: mm > ");
            completeOutboundMessage += timeStamp + messageFromServer;
            return completeOutboundMessage;

        }//BuildClientOutboundString


        /*  
        *  METHOD        : 
        *  DESCRIPTION   : 
        *  PARAMETERS    : 
        *  RETURNS       : 
        */
        public bool ValidateOutboundString(string newStringToValidate)
        {
            bool isStringValid = false;


            //Ensure the string has an acceptable length, and uses valid chars
            if (CheckOutboundStringLength(newStringToValidate) == true)
            {
                if (CheckCharactersInString(newStringToValidate) == true)
                {
                    isStringValid = true;
                }
            }

            return isStringValid;
        }//ValidateInputString



        /*  
        *  METHOD        : 
        *  DESCRIPTION   : 
        *  PARAMETERS    : 
        *  RETURNS       : 
        */
        public bool CheckOutboundStringLength(string stringToCheck)
        {
            bool isStringValid = false;


            //Max length of the users message is 2000 characters; reject the message if the string is too long or short
            if ((stringToCheck.Length < 2001) && (stringToCheck.Length > 0))
            {
                isStringValid = true;
            }

            return isStringValid;
        }//CheckOutboundStringLength



        /*  
        *  METHOD        : 
        *  DESCRIPTION   : 
        *  PARAMETERS    : 
        *  RETURNS       : 
        */
        public bool CheckUserNameLength(string stringToCheck)
        {
            bool isStringValid = false;


            //Reject the users name if it's greater than the max of 16 chars or less than 1
            if ((stringToCheck.Length < 17) && (stringToCheck.Length > 0))
            {
                isStringValid = true;
            }

            return isStringValid;
        }//CheckUserNameLength



        /*  
        *  METHOD        : 
        *  DESCRIPTION   : 
        *  PARAMETERS    : 
        *  RETURNS       : 
        */
        public bool CheckCharactersInString (string stringToCheck)
        {
            bool isStringValid = false;


            //Check each character in the string and count the number of errors, if count > 0, then the user must renter the string
            //  with valid characters
            int invalidCharCount = 0;
            foreach (char c in stringToCheck)
            {
                if ((int)c > 126)
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
        }//CheckCharactersInString




        /*  
        *  METHOD        : 
        *  DESCRIPTION   : 
        *  PARAMETERS    : 
        *  RETURNS       : 
        */
        public string ASCIIEncodeMessage(string outboundString)
        {
            //Get input from the user through the console window
            string stringToEncode = string.Empty;
            stringToEncode = outboundString;

            try
            {
                //Create two different encodings.
                Encoding unicode = Encoding.Unicode;
                Encoding ascii = Encoding.ASCII;


                //Convert the unicode string, UT16 encoded input, into a byte[]
                byte[] unicodeBytes = unicode.GetBytes(stringToEncode);


                //Perform the conversion from UT16 to ASCII
                byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);
                stringToEncode = Encoding.ASCII.GetString(asciiBytes);
            }


            //If the source encoding or destination encoding parameters are null from Encoding.Convert(), then inform the user
            catch (ArgumentNullException nullException)
            {
                throw new NotImplementedException();
            }

            return stringToEncode;

        }//GetClientInput
    }//class
}//namespace
