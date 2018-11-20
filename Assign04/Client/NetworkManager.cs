/* 
*  FILE          : NetworkManager.cs
*  PROJECT       : PROG 2120 - Assignment 4
*  PROGRAMMER    : Bence Karner & Randy Lefebvre
*  DESCRIPTION   : This file contains the the networking methods required to connect to the named pipes opened by the server. 
*                  The class contains two methods, one for connecting to incoming pipe, and another for connecting to an out facing pipe. 
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
    *   PURPOSE : This class was designed to act as a network manager for connected pipes. Two methods are included for connecting to an open incoming 
    *             and outgoing pipe hosted by the server. 
    */
    public class ClientStreamPipe
    {


        /*  
        *  METHOD        : OpenOutgoingPipe
        *  DESCRIPTION   : Used to connect to a pipe with data flowing OUTwards from the client
        *  PARAMETERS    : string pipeName : String containing the pipe name used during connection attempts
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
        *  METHOD        : OpenIncomingPipe
        *  DESCRIPTION   : Used to connect to a pipe with data flowing INwards to the client
        *  PARAMETERS    : string pipeName : String containing the pipe name used during connection attempts
        *  RETURNS       : NamedPipeClientStream : A reference to the connected pipe that was just opened
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
    }//class
}//namespace

