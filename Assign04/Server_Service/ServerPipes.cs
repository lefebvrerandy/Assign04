/* 
*  FILE          : ServerPipes.cs
*  PROJECT       : PROG 2120 - Assignment 5
*  PROGRAMMER    : Bence Karner & Randy Lefebvre
*  DESCRIPTION   : This file contains the networking methods required to open named pipes for 
*                  the chat system to function. 
*/


using System;
using System.Text;
using System.Net;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

namespace Server_Service
{


    /* 
    *   NAME    : ClientStreamPipe
    *   PURPOSE : This class was designed to act as a network manager for connected pipes. Two methods are included for opening an incoming 
    *             and outgoing pipe that will be used by the clients to send/recieve messages
    */
    public class ServerPipes
    {
        bool pipeWaitingIn = false;     // Flag to keep multiple unused pipes from opening. Only open a new when the last was connected
        bool pipeWaitingOut = false;    // Flag to keep multiple unused pipes from opening. Only open a new when the last was connected
        public int ClientCounter { get; set; }  //Counts the number of clients currently connected to the server
        private NamedPipeServerStream ServerOutPipe = null;  // Keep track of the new pipe
        private NamedPipeServerStream ServerInPipe = null;   // Keep track of the new pipe


        //  METHOD        : OpenOutPipe()
        //  DESCRIPTION   : This method is used to open an outwards facing pipe used to pushes messages out the door
        // and onto the clients
        //  PARAMETERS    : string pipeName : The name of the specific pipe to open for clients to get messages from the server
        //  RETURNS       : NamedPipeServerStream ServerOutPipe: Reference to the pipe that was just opened and connected
        //
        public void OpenOutPipe(string pipeName)
        {


            try
            {
                //ServerOutPipe = new NamedPipeServerStream(pipeName, PipeDirection.Out, 100);
                //ServerOutPipe.WaitForConnection();
                if (!pipeWaitingOut)
                {
                    ServerOutPipe = new NamedPipeServerStream(pipeName, PipeDirection.Out, 100, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
                    var asyncResult = ServerInPipe.BeginWaitForConnection(OnConnectedOut, ServerOutPipe);
                    pipeWaitingOut = true;
                }

            }

            //This is the error handling exception. This will Post information to the event logger
            catch (Exception errorMessage)
            {
                Logger.LogApplicationEvents(errorMessage.ToString());
            }
        }



        //  METHOD        : OpenInPipe()
        //  DESCRIPTION   : This method is used to open a named pipe for receiving messages from clients
        //  PARAMETERS    : string pipeName : The specific name of the pipe that will be opened for the clients to send messages
        //  RETURNS       : NamedPipeServerStream ServerInPipe : A reference to the opened and connected pipe
        //
        public void OpenInPipe(string pipeName)
        {
            try
            {
                // If a new pipe hasnt been opened, start a new pipe
                // If a new pipe has been opened but the client hasnt connected, dont open a new one
                if (!pipeWaitingIn)
                {
                    // Open an Async pipe
                    ServerInPipe = new NamedPipeServerStream(pipeName, PipeDirection.In, 100, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
                    var asyncResult = ServerInPipe.BeginWaitForConnection(OnConnectedIn, ServerInPipe);
                    pipeWaitingIn = true;
                }

                
            }

            //This is the error handling exception. This will Post information to the event logger
            catch (Exception errorMessage)
            {
                Logger.LogApplicationEvents(errorMessage.ToString());
            }
        }


        //  METHOD        : OnConnectedIn()
        //  DESCRIPTION   : This is a Async callback method. It will get triggered once the pipe has been connected to a client.
        //                  We will store the new pipe into the Servers variable "newServerInPipe" so that way main knows about it.
        //  PARAMETERS    : IAsyncResult result : The new pipes address
        //  RETURNS       : Void
        //
        private void OnConnectedIn(IAsyncResult result)
        {
            ServerInPipe.EndWaitForConnection(result);
            Server.newServerInPipe = result.AsyncState as NamedPipeServerStream;
            pipeWaitingIn = false;
        }


        //  METHOD        : OnConnectedIn()
        //  DESCRIPTION   : This is a Async callback method. It will get triggered once the pipe has been connected to a client.
        //                  We will store the new pipe into the Servers variable "newServerOutPipe" so that way main knows about it.
        //  PARAMETERS    : IAsyncResult result : The new pipes address
        //  RETURNS       : Void
        //
        private void OnConnectedOut(IAsyncResult result)
        {
            ServerOutPipe.EndWaitForConnection(result);
            Server.newServerOutPipe = result.AsyncState as NamedPipeServerStream;
            pipeWaitingOut = false;
        }
    }


 
  
}
