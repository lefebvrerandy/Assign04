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


namespace Client
{

    /* 
    *   NAME    : ClientStreamPipe
    *   PURPOSE :
    */
    public class ClientStreamPipe
    {

        /*  
        *  METHOD        : NamedPipeClientStream
        *  DESCRIPTION   : Used to connect to a new new named pipe, and return a reference to the connection
        *  PARAMETERS    : void : Takes no arguments
        *  RETURNS       : NamedPipeClientStream : A reference to the connected pipe that was just opened
        */
        public NamedPipeClientStream ConnectToPipe()
        {
            NamedPipeClientStream establishedClientPipe = null;
            try
            {
                establishedClientPipe = new NamedPipeClientStream(pipeName, );
                establishedClientPipe.Connect();
            }

            //Inform the user that they can't connect to the server using the pipe
            catch (Exception errorMessage)
            {
                if((errorMessage is ArgumentNullException) || (errorMessage is ArgumentException))
                {
                    UIController.PrintErrorToMessageBox("PipeError: ", "Unable to open " + pipeName.Tostring() + " pipe; check the pipe name, and ensure it's valid");
                    Logger.LogApplicationEvents("DEBUG FIEPATH", "");
                }

                else
                {
                    UIController.PrintErrorToMessageBox("GenericError: ", errorMessage.ToString());
                    Logger.LogApplicationEvents("DEBUG FIEPATH", errorMessage.ToString());
                }
            }
            
            return establishedClientPipe;

        }//...NamedPipeClientStream


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

    }//...class
}//...namespace

