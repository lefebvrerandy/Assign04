/* 
*  FILE          : EventLogger.cs
*  PROJECT       : PROG 2120 - Assignment 4
*  PROGRAMMER    : Bence Karner & Randy Lefebvre
*  DESCRIPTION   : This file contains the DEBUG
*/


using System;
using System.IO;
using FileManager;

namespace EventLogger
{

    /* 
    *   NAME    : 
    *   PURPOSE :
    */
    class Logger
    {

        /*  
        *  METHOD        : 
        *  DESCRIPTION   : 
        *  PARAMETERS    : 
        *  RETURNS       : 
        */
        public static void LogApplicationEvents(string filepath, string messageToLog)
        {

            var currentTime = DateTime.Now;
            string timeStamp = currentTime.ToString("yyyy: MM: dd: hh: mm: ss -> ");


            string eventString = null;
            eventString += timeStamp + messageToLog;
            File.AppendAllText(filepath, eventString + Environment.NewLine);

        }//... LogApplicationEvents

    }//...class
}//...namepspace
