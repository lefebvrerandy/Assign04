/* 
*  FILE          : ServerPipes.cs
*  PROJECT       : PROG 2120 - Assignment 4
*  PROGRAMMER    : Bence Karner & Randy Lefebvre
*  DESCRIPTION   : This file contains the the networking methods required to open named pipes for 
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
        bool pipeWaitingIn = false;
        bool pipeWaitingOut = false;
        public int ClientCounter { get; set; }  //Counts the number of clients currently connected to the server
        private NamedPipeServerStream ServerOutPipe = null;
        private NamedPipeServerStream ServerInPipe = null;


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
                //ServerInPipe = new NamedPipeServerStream(pipeName, PipeDirection.In, 100);
                //ServerInPipe.WaitForConnection();
                if (!pipeWaitingIn)
                {
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

        private void OnConnectedIn(IAsyncResult result)
        {
            ServerInPipe.EndWaitForConnection(result);
            var server = result.AsyncState as NamedPipeServerStream;
            Server.newServerInPipe = server;
            pipeWaitingIn = false;
        }

        private void OnConnectedOut(IAsyncResult result)
        {
            ServerOutPipe.EndWaitForConnection(result);
            Server.newServerOutPipe = result.AsyncState as NamedPipeServerStream;
            pipeWaitingOut = false;
        }
    }


 
  
}
