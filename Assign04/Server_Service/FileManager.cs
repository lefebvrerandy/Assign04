/* 
*  FILE          : FileManager.cs
*  PROJECT       : PROG 2120 - Assignment 4
*  PROGRAMMER    : Bence Karner & Randy Lefebvre
*  DESCRIPTION   : This file contains the FileIO class, and is used for opening, writing, creating
*                   and appending to external files. 
*                  
*  NOTE: As the author (Bence Karner) of the class, and method, I have chosen to share the source code with my team
*        for the INFO-2180 project managment project. Just as well, this exact class and method was used
*        in a prior PROG-2110 assignment. 
*/



using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
namespace Server_Service
{

    /* 
    *   NAME    : FileIO
    *   PURPOSE : The purpose of this class is to allow the application to interact with files and information external to the program.
    *             The class functions much like a generic file IO module, in that it allows the importation and exporation of data 
    *             to sources outsie the program. Methods are included for creating, reading, writing, and appending to files. 
    */
    class FileIO
    {

        /*  
        *  METHOD        : CreateFile
        *  DESCRIPTION   : This method is used to create text file with the name supplied by the parameters; if one already exists, 
        *                  then the method does nothing and returns
        *  PARAMETERS    : string filePath : The file path of the file to be created
        *  RETURNS       : void : The method has no return
        */
        public void CreateFile(string filePath)
        {
            try
            {
                //If the file doen't already exist, create it
                #pragma warning disable CS0642 //Remove warning regarding a possible mistaken empty statement
                if (!File.Exists(filePath)) ;
                {
                    FileStream masterFileStream = null;
                    masterFileStream = File.Create(filePath);
                    masterFileStream.Close();
                }

                #pragma warning restore CS0642 //Restore warning regarding possible mistaken empty statement
            }

            //In regards to the warning above, catch any exceptions thrown due to the masterFilePath variable being empty, or incorrect
            catch (ArgumentNullException nullException)
            {
                Logger.LogApplicationEvents(nullException.ToString());
            }
            

            catch (DirectoryNotFoundException missingDirectory)
            {
                Logger.LogApplicationEvents(missingDirectory.ToString());
            }

            //Generic catch block for all remaining exceptions
            catch (Exception errorMessage)
            {
                Logger.LogApplicationEvents(errorMessage.ToString());
            }
        }// CreateFile



        /*  
        *  METHOD        : AppendToFile
        *  DESCRIPTION   : This method is used to append strings from a string array to the file
        *  PARAMETERS    : Parameters are as follows,
        *  string filepath: The file path where the strings will be appended
        *  string[] newData : String array containing the new data to append
        *  RETURNS       : void : The method has no return
        */
        public static void AppendToFile(string filepath, string[] newData)
        {
            int lineNumber = 0;

            try
            {
                //Append each line in the string array to the file
                foreach (var line in newData)
                {
                    File.AppendAllText(filepath, newData[lineNumber] + Environment.NewLine);
                    lineNumber++;
                }
            }


            //Generic catch block for all remaining exceptions
            catch (Exception errorMessage)
            {
                Logger.LogApplicationEvents(errorMessage.ToString());
            }
        }// AppendToFile


        /*  
        *  METHOD        : WriteToFile
        *  DESCRIPTION   : This method is used to write each string in the array, to a file specified by the filepath parameter
        *  PARAMETERS    : The parameters are as follows,
        *  string filepath : String for the filepath where the data will be written
        *  string[] fileContents : String array containing the contents of the application to be written to a file
        *  RETURNS       : void : The method has no return
        *  
        */
        public void WriteToFile(string filepath, string[] fileContents)
        {

            try
            {
                CreateFile(filepath);


                //Flush the filestream and clear the contents of the file
                FileStream fileStream = File.Open(filepath, FileMode.Open);
                fileStream.SetLength(0);
                fileStream.Close();


                //Write each line from the string array, into the target file; the file is 
                //  closed automatically once all write operations are complete
                foreach (var line in fileContents)
                {
                    File.WriteAllLines(filepath, fileContents, Encoding.ASCII);
                }
            }


            //Generic catch block for all exceptions
            catch (Exception errorMessage)
            {
                Logger.LogApplicationEvents(errorMessage.ToString());
            }
        }//WriteToFile




        /*  
        *  METHOD        : ReadXMLDocument
        *  DESCRIPTION   : This method is used to find a constant string in the XML file, that corrosponds with the search string
        *  PARAMETERS    : string elementToLocate: The string used to identify the target resource in the XML file
        *  RETURNS       : string :Returns a string retrieved from the XML file
        */
        public string ReadXMLDocument(string elementToLocate)
        {
            string  stringFromDocument = string.Empty;
            try
            {
                string pathDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                //Load the xml file
                XDocument constantsDocument = XDocument.Load(pathDirectory + "./constants.xml");
                //XDocument constantsDocument = XDocument.Load("./constants.xml");


                //Look for the pipe name element
                if (elementToLocate == "pipeName-incoming")
                {
                    stringFromDocument = constantsDocument.XPathSelectElement("/root/constants/networking/incomingPipe").Value;     //Returns serverIn
                }


                
                else if (elementToLocate == "pipeName-outgoing")
                {
                    stringFromDocument = constantsDocument.XPathSelectElement("/root/constants/networking/outgoingPipe").Value;     //Returns serverOut
                }


                //Look for the pipe used to signal the client's status
                else if (elementToLocate == "pipeName-clientStatus")
                {
                    stringFromDocument = constantsDocument.XPathSelectElement("/root/constants/networking/statusPipe").Value;
                }


                //Look for the log file path
                else if (elementToLocate == "logFilePath")
                {

                    stringFromDocument = constantsDocument.XPathSelectElement("/root/constants/filePaths/logFile").Value;
                }


                //Look for the filepath of the ASCII art used for the servers title/UI
                else if(elementToLocate == "serverUI")
                {
                    stringFromDocument = constantsDocument.XPathSelectElement("/root/constants/filePaths/serverUI").Value;
                }


                //Incorrect element name was given
                else
                {
                    throw new Exception();  //Exception was added on purpose so that it triggers a log of the error
                }
            }


            //XML file could not be located, or the element name was incorrect
            catch (Exception errorMessage)
            {
                Logger.LogApplicationEvents(errorMessage.ToString());
            }

            return stringFromDocument;
        }//ReadXMLDocument
    }//class
}//namespace
