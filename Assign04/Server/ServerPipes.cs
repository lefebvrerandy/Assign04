using System;
using System.Text;
using System.Net;
using System.IO;
using System.IO.Pipes;

namespace Server
{
    public class ServerPipes
    {

        public int ClientCounter { get; set; }  //Counts the number of clients currently connected to the server


        //  METHOD        : OpenOutPipe()
        //  DESCRIPTION   : 
        //  PARAMETERS    : string pipeName
        //  RETURNS       : NamedPipeServerStream ServerOutPipe
        //
        public NamedPipeServerStream OpenOutPipe(string pipeName)
        {

            NamedPipeServerStream ServerOutPipe = null;
            try
            {
                ServerOutPipe = new NamedPipeServerStream(pipeName, PipeDirection.Out, NamedPipeServerStream.MaxAllowedServerInstances);
                //ServerOutPipe = new NamedPipeServerStream(pipeName, PipeDirection.Out);
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
        //  DESCRIPTION   : 
        //  PARAMETERS    : string pipeName
        //  RETURNS       : NamedPipeServerStream ServerInPipe
        //
        public NamedPipeServerStream OpenInPipe(string pipeName)
        {
            NamedPipeServerStream ServerInPipe = null;
            try
            {
                ServerInPipe = new NamedPipeServerStream(pipeName, PipeDirection.In, NamedPipeServerStream.MaxAllowedServerInstances);
                //ServerInPipe = new NamedPipeServerStream(pipeName, PipeDirection.In);
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
