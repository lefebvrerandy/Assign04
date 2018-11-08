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


namespace NetworkManager
{

    /* 
    *   NAME    : 
    *   PURPOSE :
    */
    public class ClientStreamPipe
    {

        /*  
        *  METHOD        : 
        *  DESCRIPTION   : 
        *  PARAMETERS    : 
        *  RETURNS       : 
        */
        public NamedPipeClientStream ConnectToPipe()
        {
            NamedPipeClientStream establishedClientPipe = new NamedPipeClientStream("DEBUG_INSERT_PIPE_NAME");
            establishedClientPipe.Connect();
            return establishedClientPipe;
        }


        /*  
        *  METHOD        : 
        *  DESCRIPTION   : 
        *  PARAMETERS    : 
        *  RETURNS       : 
        */
        public bool CheckForExitCondition(string incomingMessage)
        {
            bool isExitConfirmed = false;
            return isExitConfirmed;
        }

    }//...class
}//...namespace

