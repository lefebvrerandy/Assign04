/* 
*  FILE          : FileManager.cs
*  PROJECT       : PROG 2120 - Assignment 4
*  PROGRAMMER    : Bence Karner & Randy Lefebvre
*  DESCRIPTION   : This file contains the DEBUG
*/


using System;
using System.IO;


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
                throw new NotImplementedException();
            }

            catch (DirectoryNotFoundException missingDirectory)
            {
                throw new NotImplementedException();
            }

            //Generic catch block for all remaining exceptions
            catch (Exception errorMessage)
            {
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
                throw new NotImplementedException();
            }
        }//... AppendToFile

    }//...class
}//...namespace
