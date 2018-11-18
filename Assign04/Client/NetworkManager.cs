/* 
*  FILE          : NetworkManager.cs
*  PROJECT       : PROG 2120 - Assignment 4
*  PROGRAMMER    : Bence Karner & Randy Lefebvre
*  DESCRIPTION   : This file contains the DEBUG
*/



using System;
using System.Text;
using System.Net;
using System.IO;
using System.IO.Pipes;
using System.Xml;
namespace Client
{

    /* 
    *   NAME    : ClientStreamPipe
    *   PURPOSE :
    */
    public class ClientStreamPipe
    {


        /*  
        *  METHOD        : 
        *  DESCRIPTION   : 
        *  PARAMETERS    : 
        *  RETURNS       : NamedPipeClientStream : A reference to the connected pipe that was just opened
        */
        public NamedPipeClientStream OpenOutgoingPipe(string pipeName)
        {

            NamedPipeClientStream establishedClientPipe = null;
            try
            {
                establishedClientPipe = new NamedPipeClientStream(".", pipeName, PipeDirection.Out);
                establishedClientPipe.Connect();
            }

            //Inform the user that they can't connect to the server using the pipe
            catch (Exception errorMessage)
            {

                //Grab the filepath for the logger
                FileIO fileManager = new FileIO();
                string filepath = fileManager.ReadXMLDocument("logFilePath");   //Indicator of the element to search in the XML doc


                //Decide which exception path is taken based on the error
                if ((errorMessage is ArgumentNullException) || (errorMessage is ArgumentException))
                {
                    UIController.PrintErrorToMessageBox("PipeError: ", "Unable to open " + pipeName + " pipe; check the pipe name, and ensure it's valid");
                    Logger.LogApplicationEvents(filepath, errorMessage.ToString());
                }

                else
                {
                    UIController.PrintErrorToMessageBox("GenericError: ", errorMessage.ToString());
                    Logger.LogApplicationEvents(filepath, errorMessage.ToString());
                }
            }
            
            return establishedClientPipe;
        }



        /*  
        *  METHOD        : 
        *  DESCRIPTION   : 
        *  PARAMETERS    : 
        *  RETURNS       : NamedPipeClientStream :
        */
        public NamedPipeClientStream OpenIncomingPipe(string pipeName)
        {
            NamedPipeClientStream establishedClientPipe = null;
            try
            {
                establishedClientPipe = new NamedPipeClientStream(".", pipeName, PipeDirection.In);
                establishedClientPipe.Connect();
            }

            //Inform the user that they can't connect to the server using the pipe
            catch (Exception errorMessage)
            {

                //Grab the filepath for the logger
                FileIO fileManager = new FileIO();
                string filepath = fileManager.ReadXMLDocument("logFilePath");   //Indicator of the element to search in the XML doc


                //Decide which exception path is taken based on the error
                if ((errorMessage is ArgumentNullException) || (errorMessage is ArgumentException))
                {
                    UIController.PrintErrorToMessageBox("PipeError: ", "Unable to open " + pipeName + " pipe; check the pipe name, and ensure it's valid");
                    Logger.LogApplicationEvents(filepath, errorMessage.ToString());
                }

                else
                {
                    UIController.PrintErrorToMessageBox("GenericError: ", errorMessage.ToString());
                    Logger.LogApplicationEvents(filepath, errorMessage.ToString());
                }
            }

            return establishedClientPipe;
        }


        /*  
        *  METHOD        : CheckForExitCondition
        *  DESCRIPTION   : 
        *  PARAMETERS    : 
        *  RETURNS       : bool : 
        */
        public bool CheckForExitCondition(string incomingMessage)
        {
            bool isExitConfirmed = false;



            return isExitConfirmed;
        }

    }//class
}//namespace

