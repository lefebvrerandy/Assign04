using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
