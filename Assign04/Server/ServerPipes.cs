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
namespace Server
{


    /* 
    *   NAME    : ClientStreamPipe
    *   PURPOSE : This class was designed to act as a network manager for connected pipes. Two methods are included for opening an incoming 
    *             and outgoing pipe that will be used by the clients to send/recieve messages
    */
    public class ServerPipes
    {

        public int ClientCounter { get; set; }  //Counts the number of clients currently connected to the server


        //  METHOD        : OpenOutPipe()
        //  DESCRIPTION   : This method is used to open an outwards facing pipe used to pushes messages out the door
        // and onto the clients
        //  PARAMETERS    : string pipeName : The name of the specific pipe to open for clients to get messages from the server
        //  RETURNS       : NamedPipeServerStream ServerOutPipe: Reference to the pipe that was just opened and connected
        //
        public NamedPipeServerStream OpenOutPipe(string pipeName)
        {

            NamedPipeServerStream ServerOutPipe = null;
            try
            {
                ServerOutPipe = new NamedPipeServerStream(pipeName, PipeDirection.Out, 100);
                ServerOutPipe.WaitForConnection();
            }

            //This is the error handling exception. This will Post information to the eventlogger
            catch (Exception errorMessage)
            {
                Console.WriteLine(errorMessage.ToString());
            }

            return ServerOutPipe;
        }



        //  METHOD        : OpenInPipe()
        //  DESCRIPTION   : This method is used to open a named pipe for recieving messages from clients
        //  PARAMETERS    : string pipeName : The specific name of the pipe that will be opened for the clients to send messages
        //  RETURNS       : NamedPipeServerStream ServerInPipe : A reference to the opened and connected pipe
        //
        public NamedPipeServerStream OpenInPipe(string pipeName)
        {
            NamedPipeServerStream ServerInPipe = null;
            try
            {
                ServerInPipe = new NamedPipeServerStream(pipeName, PipeDirection.In, 100);
                ServerInPipe.WaitForConnection();
            }

            // This is the error handling exception
            catch (Exception errorMessage)
            {
                Console.WriteLine(errorMessage.ToString());
            }

            return ServerInPipe;
        }
    }
}
