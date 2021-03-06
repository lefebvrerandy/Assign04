﻿/* 
*  FILE          : EventLogger.cs
*  PROJECT       : PROG 2120 - Assignment 4
*  PROGRAMMER    : Bence Karner & Randy Lefebvre
*  DESCRIPTION   : This file contains the Logger class, and is used for recording data related to 
*                  errors, or application events to a log file. 
*                  
*  NOTE: As the author (Bence Karner) of the class, and methods, I have chosen to share the source code with my team
*        for the INFO-2180 project managment project. Just as well, this exact class and method was used
*        in a prior PROG-2110 assignment.
*/


using System;
using System.IO;
namespace Client
{

    /* 
    *   NAME    : Logger
    *   PURPOSE : The method was designed to operate independent of the application its embedded in. 
    *             The class provides a method for recording errors or events to a specific log file
    *              and only requires a filepath, and error string to execute its full functionality. 
    */
    class Logger
    {

        /*  
        *  METHOD        : LogApplicationEvents
        *  DESCRIPTION   : The method is used to record a string originating from an event,
        *                  to a file located at the filepath parameter
        *  PARAMETERS    : The parameters are as follows,
        *  string filepath      : The filepath where the events are recorded
        *  string messageToLog  : The specific message to record in the file
        *  RETURNS       : void : The method has no return value
        */
        public static void LogApplicationEvents(string filepath, string messageToLog)
        {

            var currentTime = DateTime.Now;
            string timeStamp = currentTime.ToString("yyyy: MM: dd: hh: mm: ss -> ");


            string eventString = null;
            eventString += timeStamp + messageToLog;
            try
            {
                File.AppendAllText(filepath, eventString + Environment.NewLine);
            }
            catch(Exception e)
            {
                UIController.PrintErrorToMessageBox("File path not valid",e.ToString());
            }


        }// LogApplicationEvents

    }//class
}//namepspace
