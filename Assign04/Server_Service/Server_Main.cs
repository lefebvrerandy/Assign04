/* 
*  FILE          : Server.cs
*  PROJECT       : PROG 2120 - Assignment 5
*  PROGRAMMER    : Randy Lefebvre & Bence Karner
*  DESCRIPTION   : This file contains the starting point of the Server applications. The program 
*                  creates a file, and logs the start of the application before executing its networking
*                  functionality. The purpose of the server is to open unidirectional pipes for all incoming,
*                  and outgoing connections from the clients. The applications receives messages from the clients
*                  and redirects them to everyone connected to the pipes. 
*/



using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;

namespace Server_Service
{
    public class Server
    {
        /*
         * Class wide variables
         */
        public static bool Run { set; get; }
        public static bool toDisconnect;
        public static bool Pause;
        static public NamedPipeServerStream newServerInPipe;
        static public NamedPipeServerStream newServerOutPipe;
        static NamedPipeServerStream pipe_in;
        static NamedPipeServerStream pipe_out;
        public Server()
        {
            Run = true;
        }

        public static void RunServer()
        {
            int messageCounter = 0;             // Keep track of how many messages there is
            Dictionary<int, string> messageList = new Dictionary<int, string>();    // A repository of messages
            var PairPipesIn = new Dictionary<NamedPipeServerStream, NamedPipeServerStream>();   // Key value for the two pipes. Key being In, Value being Out pipes
            var PairPipesOut = new Dictionary<NamedPipeServerStream, NamedPipeServerStream>();  // Key value for the two pipes. Key being Out, Value being Out In



            FileIO fileManager = new FileIO();
            string incomingPipeName = fileManager.ReadXMLDocument("pipeName-incoming");      //string indicator of the element to search in the XML doc
            string outgoingPipename = fileManager.ReadXMLDocument("pipeName-outgoing");      //string indicator of the element to search in the XML doc
            ServerPipes openPipes = new ServerPipes();      // Create a new instance of ServerPipes
            bool PipeInIsConnected = false;                 // Flag for pipeIn being connected
            bool PipeOutIsConnected = false;                // Flag for pipeOut being connected
            Thread RecieveFromClients = null;               // Empty thread - Used to receive messages from clients
            Thread SendToClients = null;                    // Empty thread - Used to sends messages from clients

            Logger.LogApplicationEvents("Server Started");
            do
            {
                // Open the pipes for the next client. Goes into an async loop
                // until a client is connected, then loops again
                openPipes.OpenInPipe("serverIn");      
                openPipes.OpenOutPipe("serverOut");

                // If a new client connects start a new thread that looks for new messages sent from that client
                if (newServerInPipe != null)
                {
                    pipe_in = newServerInPipe;
                    newServerInPipe = null;
                    RecieveFromClients = new Thread(() => RecieveFromAllClients(pipe_in, ref messageList, ref messageCounter, ref PairPipesIn, ref Pause));
                    //RecieveFromClients.Name = "RecieveMessageThread";
                    RecieveFromClients.Start();
                    PipeInIsConnected = true;
                }


                // If a new client connects start a new thread that looks to send the new messages to the client
                if (newServerOutPipe != null)
                {
                    pipe_out = newServerOutPipe;
                    newServerOutPipe = null;
                    SendToClients = new Thread(() => SendToAllClients(pipe_out, ref messageList, ref messageCounter, ref PairPipesOut, ref toDisconnect,ref Pause));
                    //SendToClients.Name = "SendMessageThread";
                    SendToClients.Start();
                    PipeOutIsConnected = true;
                }


                //Lets store both pipes together in a list as a pair. We use this to keep track of adjacent pipes.
                // That way when one closes, we can close the other also.
                if ((PipeInIsConnected) && (PipeOutIsConnected))
                {
                    PairPipesIn.Add(pipe_in, pipe_out);     // Key = pipe in, value = pipe out
                    PairPipesOut.Add(pipe_out, pipe_in);     // Key = pipe out, value = pipe in

                    //Increment the client counter so the server knows when to return
                    openPipes.ClientCounter++;
                    Thread.Sleep(100);
                    pipe_in = null;
                    PipeInIsConnected = false;
                    PipeOutIsConnected = false;
                    pipe_out = null;
                }


                Thread.Sleep(1);

                // If the service is stopped. Join all threads
                if (Run == false)
                {
                    Thread.Sleep(500);
                    if (RecieveFromClients != null)
                        RecieveFromClients.Join();
                    if (SendToClients != null)
                        SendToClients.Join();
                }
            } while(Run);
        }


        /*  
        *  METHOD        : RecieveFromAllClients
        *  DESCRIPTION   : This method is used to receive messages from the clients,
        *   and save them to the data repository where they can be accessed by the sending thread
        *  PARAMETERS    : 
        *       object data : 
        *                       Generic object type argument which us
        *                       cast to a pipe, and used to receive incoming messages
        *       ref Dictionary<int, string> messageList: 
        *                       Message repository
        *       ref int messageCounter :
        *                       How many messages are currently sent from the client
        *       ref Dictionary<NamedPipeServerStream, NamedPipeServerStream> PairPipesIn:
        *                       Used to keep track of which two pipes are from the client
        *       ref bool Pause:
        *                       If the server should or shouldnt be paused
        *  RETURNS       : void : The methods has no return
        */
        private static void RecieveFromAllClients(object data, ref Dictionary<int, string> messageList, ref int messageCounter, ref Dictionary<NamedPipeServerStream, NamedPipeServerStream> PairPipesIn, ref bool Pause)
        {
            Thread.Sleep(100);

            // Store the NamedPipeServerStream "data" into "Client_IN"
            NamedPipeServerStream Client_IN = null;
            Client_IN = (NamedPipeServerStream)data;    


            //Keep cycling an receiving new messages until the clients indicate they are shutting down
            bool clientDisconnectCommand = false;
            while (clientDisconnectCommand == false)
            {
                string incomingMessage = string.Empty;
                //Open an stream to the pipe taking incoming messages, and write the message to the string         
                StreamReader readFromPipe = new StreamReader(Client_IN);
                do
                {
                    incomingMessage = readFromPipe.ReadLine();
                } while (Pause == true);

                // If a null comes in, that means the client disconnected.
                if (incomingMessage == null)
                    clientDisconnectCommand = true;
                if (clientDisconnectCommand == false)
                {
                    messageCounter++;
                    messageList.Add(messageCounter, incomingMessage);
                }

                Thread.Sleep(100);
            }

            // We should close the matching pipe for that client
            if (PairPipesIn.TryGetValue(Client_IN, out NamedPipeServerStream Client_OUT))
            {
                Client_OUT.Close();
                Client_IN.Close();
            }

        }//RecieveFromAllClients


        /*  
        *  METHOD        : SendToAllClients
        *  DESCRIPTION   : This method is used to DEBUG
        *  PARAMETERS    : 
        *       object data : 
        *                       Generic object type argument which us
        *                       cast to a pipe, and used to receive incoming messages
        *       ref Dictionary<int, string> messageList: 
        *                       Message repository
        *       ref int messageCounter :
        *                       How many messages are currently sent from the client
        *       ref Dictionary<NamedPipeServerStream, NamedPipeServerStream> PairPipesIn:
        *                       Used to keep track of which two pipes are from the client
        *       ref bool Pause:
        *                       If the server should or shouldnt be paused (Pause windows service)
        *       ref bool toDisconnect:
        *                       If the server should be disconnected (Stop windows service)
        *  RETURNS       : void : The methods has no return
        */
        private static void SendToAllClients(object data, ref Dictionary<int, string> messageList, ref int messageCounter, ref Dictionary<NamedPipeServerStream, NamedPipeServerStream> PairPipesOut, ref bool toDisconnect, ref bool Pause)
        {
            Thread.Sleep(100);

            // Store the NamedPipeServerStream "data" into "Client_OUT"
            NamedPipeServerStream Client_OUT = null;
            Client_OUT = (NamedPipeServerStream)data;

            
            //Open a new stream to the outgoing pipe
            StreamWriter outputStream = new StreamWriter(Client_OUT);

            
            //Save the current message count in the dictionary
            int currentMessageCount = messageCounter;

            
            //Keep cycling looking for new messages in the dictionary
            //For every message thats added the messageRepository, the thread managing incoming messages will increment the counter

           bool clientDisconnectCommand = false;
           while (clientDisconnectCommand == false)
           {
                // If the Server is paused, Lets go into a wait loop
                if (Pause == true)
                {
                    Thread.Sleep(100);
                }
                if (messageCounter >= currentMessageCount)
                {
                    //A new message has been added to the repository since the last check
                    //Grab the message associate with the current counter, and send it out to the client
                    string outgoingClientMessage = string.Empty;
                    messageList.TryGetValue(currentMessageCount, out outgoingClientMessage);
                    outputStream.WriteLine(outgoingClientMessage);
                    try
                    {
                        outputStream.Flush();
                        //Increment the message counter and cycle back to check again for a new message
                        currentMessageCount++;
                    }
                    catch (Exception errorMessage)
                    {
                        clientDisconnectCommand = true;


                        //Log the error before returning up the calling stack
                        FileIO fileManager = new FileIO();
                        string filepath = fileManager.ReadXMLDocument("logFilePath");
                        Logger.LogApplicationEvents(errorMessage.ToString());
                    }
                }
                if (toDisconnect == true)
                {
                    // We should close the matching pipe for that client
                    if (PairPipesOut.TryGetValue(Client_OUT, out NamedPipeServerStream Client_IN))
                    {
                        StreamReader SendDisconnect = new StreamReader(Client_IN);
                        outputStream.WriteLine("");
                        Client_IN.Close();
                        Client_OUT.Close();
                    }
                    clientDisconnectCommand = true;
                }
                Thread.Sleep(100);
                if (Client_OUT.IsConnected == false)
                {
                    clientDisconnectCommand = true;
                }
           }
        }//SendToAllClients
    }//class
}//namespace

