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
        *  METHOD        : StringSplitter
        *  DESCRIPTION   : This method is used to split the alrger string delimited by the new line char, and return the sub strings
        *   as part of an array
        *  PARAMETERS    : string stringToSplit : string that will be split by the delimiter ('\n')
        *  RETURNS       : string [] : String array containing the individual strings from the larger parent string
        */
        public static string[] StringSplitter (string stringToSplit)
        {

            //Split the chat logs by the new line char
            string[] returnStringArray = new string[] { };
            returnStringArray = stringToSplit.Split('\n');
            return returnStringArray;
        }//StringSplitter


        /*  
        *  METHOD        : BuildOutboundString
        *  DESCRIPTION   : 
        *  PARAMETERS    : Parameters are as follows
        *  string userName: String containing the users userName
        *  string command: String containing the client command identifier
        *  string outboundMessage: String containing the actual message typed into the client message window
        *  RETURNS       : string : Return string with each of the argument strings stiched together
        */
        public string BuildOutboundString(string userName, string command, string outboundMessage)
        {
            string completeOutboundMessage = null;
            completeOutboundMessage += userName + ',' + command + ',' + outboundMessage;
            return completeOutboundMessage;

        }//BuildClientOutboundString


        /*  
        *  METHOD        : BuildDisplayString
        *  DESCRIPTION   : Used to prepend a time stamp to the incoming string
        *  PARAMETERS    : string messageFromServer : String that will ahve the DateTime prepended to it
        *  RETURNS       : string : Returns the completed string with the time stamp appended
        *   to the begining of the argument string
        */
        public string BuildDisplayString(string messageFromServer)
        {
            string completeOutboundMessage = null;

            //
            string timeStamp = DateTime.Now.ToString("dd: hh: mm > ");
            string[] splitString = messageFromServer.Split(',');
            string userName = splitString[0];
            string userMessage = splitString[2];
            if (userName == User.ClientID)
                completeOutboundMessage += timeStamp + userName + " > " + userMessage;
            else
                completeOutboundMessage += timeStamp + userName + " < " + userMessage;

            return completeOutboundMessage;

        }//BuildClientOutboundString


        /*  
        *  METHOD        : CheckUserNameLength
        *  DESCRIPTION   : This method is used to check the length of the clients userName. If its
        *   above 0 and below 17 chars in length, then its valid
        *  PARAMETERS    : string stringToCheck : 
        *  RETURNS       : bool : Returns true if the username is less than 17 chars and above 0
        */
        public bool CheckUserNameLength(string stringToCheck)
        {
            bool isStringValid = false;


            //Reject the users name if it's greater than the max of 16 chars or less than 1
            if ((stringToCheck.Length > 0) && (stringToCheck.Length < 17))
            {
                isStringValid = true;
            }

            return isStringValid;
        }//CheckUserNameLength


        /*  
        *  METHOD        : CheckCharactersInString
        *  DESCRIPTION   : Used to check the characters in the parameter string. if chars outside the range of 
        *   32 - 126 are detected, then the entire string is considerd invalid
        *  PARAMETERS    : string stringToCheck: The string that will be checked
        *  RETURNS       : bool : Returns true if the string contains only valid characters
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


            //Inform the user if the source encoding or destination encoding parameters are null from Encoding.Convert()
            catch (ArgumentNullException nullException)
            {

                //Print the error to a message box
                UIController.PrintErrorToMessageBox("nullException: ", "The source encoding or destination encoding parameters were null" + Environment.NewLine + 
                    "Your message may not display properly in the chat window");


                //Log the error
                FileIO fileManager = new FileIO();
                string filepath = fileManager.ReadXMLDocument("logFilePath");   //Indicator of the element to search in the XML doc
                Logger.LogApplicationEvents(filepath, nullException.ToString());
            }

            return encodedString;

        }//GetClientInput


        /*  
        *  METHOD        : AutomateGenerateString
        *  DESCRIPTION   : This method is used to randomly select a string from the list below
        *  PARAMETERS    : void : The method takes no arguments
        *  RETURNS       : string : The randomly selected string
        */
        public string AutomateGenerateString()
        {
            string message = string.Empty;
            string GeneratedString = string.Empty;
            string[] randomString = new string[] 
            {
                "anyone asked the camel if he’s happy about it?",
                "If Purple People Eaters are real… where do they find purple people to eat?",
                "Where do random thoughts come from?",
                "If you like tuna and tomato sauce- try combining the two. It’s really not as bad as it sounds.",
                "Sometimes, all you need to do is completely make an ass of yourself and laugh it off to realise that life isn’t so bad after all.",
                "It was getting dark, and we weren’t there yet.",
                "He ran out of money, so he had to stop playing poker.",
                "This is a Japanese doll.",
                "This is the last random sentence I will be writing and I am going to stop mid-sent",
                "My Mum tries to be cool by saying that she likes all the same things that I do."
            };
            Random rnd = new Random();

            //int randomIndex = rnd.Next(randomString.Length);
            int randomIndex = rnd.Next(0, 9);

            //GeneratedString = BuildOutboundString(user_name, command, randomString[randomIndex]);
            GeneratedString = randomString[randomIndex] + '\n';     // Set up the generated string and add the newline character at the end

            return GeneratedString;

        }//AutomateGenerateString


        /*  
        *  METHOD        : AutomateGenerateSleep
        *  DESCRIPTION   : This methos is used to generate a random sleep time
        *  PARAMETERS    : void : The method takes no arguments
        *  RETURNS       : int : Random sleep time
        */
        public int AutomateGenerateSleep()
        {
            Random rnd = new Random();
            int randomSleep = rnd.Next(100, 1000);

            return randomSleep;
        }//AutomateGenerateString
    }//class
}//namespace
