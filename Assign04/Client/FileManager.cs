/* 
*  FILE          : FileManager.cs
*  PROJECT       : PROG 2120 - Assignment 4
*  PROGRAMMER    : Bence Karner & Randy Lefebvre
*  DESCRIPTION   : This file contains the DEBUG
*/


using System;
using System.IO;
using System.Text;


namespace FileManager
{

    /* 
    *   NAME    : 
    *   PURPOSE :
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
        public static void CreateFile(string filePath)
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
                //DEBUG ADD ERROR LOG
                throw new NotImplementedException();
            }

            catch (DirectoryNotFoundException missingDirectory)
            {
                //DEBUG ADD ERROR LOG
                throw new NotImplementedException();
            }

            //Generic catch block for all remaining exceptions
            catch (Exception errorMessage)
            {
                //DEBUG ADD ERROR LOG
                throw new NotImplementedException();
            }
        }//... CreateFile



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
                //DEBUG ADD ERROR LOG
                throw new NotImplementedException();
            }
        }//... AppendToFile


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
                //DEBUG ADD ERROR LOG
                throw new NotImplementedException();
            }
        }//...WriteToFile

    }//...class
}//...namespace
