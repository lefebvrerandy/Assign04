﻿/* 
*  FILE          : Server.cs
*  PROJECT       : PROG 2120 - Assignment 4
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
namespace Server
{
    public class Server_Main
    {
        public static void Main()
        {

            //Set the console windows attributes, and print the UI
            //Utility UIManager = new Utility();
            //UIManager.SetConsoleAttributes();
            FileIO fileManager = new FileIO();
            //fileManager.PrintConsoleUI();


            //Grab the filepath for the event logger, and log the server start time
            string filepath = fileManager.ReadXMLDocument("logFilePath");
            fileManager.CreateFile(filepath);
            Logger.LogApplicationEvents(filepath, "SERVER START");

            ServerAcceptLoopThread();

            //Wait with the main thread until all child threads have returned
            while (true)
            {
                Thread.Sleep(1000);
            }


        }



        /*  
        *  METHOD        : ServerAcceptLoopThread
        *  DESCRIPTION   : This method is used to open the incoming and outgoing pipes to the clients
        *                   before treading off to open another connection
        *  PARAMETERS    : void : The method takes no arguments
        *  RETURNS       : void : The methods has no return
        */
        public static void ServerAcceptLoopThread()
        {

            //DEBUG
            int messageCounter = 0;
            Dictionary<int, string> messageList = new Dictionary<int, string>();
            var PairPipes = new Dictionary<NamedPipeServerStream, NamedPipeServerStream>();


            FileIO fileManager = new FileIO();
            string incomingPipeName = fileManager.ReadXMLDocument("pipeName-incoming");      //string indicator of the element to search in the XML doc
            string outgoingPipename = fileManager.ReadXMLDocument("pipeName-outgoing");      //string indicator of the element to search in the XML doc
            ServerPipes openPipes = new ServerPipes();


            /*  Use the var  initalClientConnection to allow the thread to enter the main connection loop 
             *  ONLY one the first cycle before a client has connected. For all other subsequent iterations 
             *  of the loop keep cycling until all clients have shutdown, or logged off */
            int initalClientConnection = 1;
            while ((initalClientConnection == 1) || (openPipes.ClientCounter > 0))
            {


                ////Console.WriteLine("Waiting for connections...");
                NamedPipeServerStream pipe_in = openPipes.OpenInPipe("serverIn");
                NamedPipeServerStream pipe_out = openPipes.OpenOutPipe("serverOut");


                // Start a new thread, Send the pipe_in pipe to the new thread
                Thread RecieveFromClients = new Thread(() => RecieveFromAllClients(pipe_in, ref messageList, ref messageCounter, ref PairPipes));
                RecieveFromClients.Name = "RecieveMessageThread";
                RecieveFromClients.Start();


                //Start a new thread, Send the pipe_out pipe to the new thread
                Thread SendToClients = new Thread(() =>  SendToAllClients(pipe_out, ref messageList, ref messageCounter));
                SendToClients.Name = "SendMessageThread";
                SendToClients.Start();


                //Lets store both pipes together in a list as a pair. That way when one closes, we can close the other also.
                PairPipes.Add(pipe_in, pipe_out);
                

                //Increment the client counter so the server knows when to return
                openPipes.ClientCounter++;
                initalClientConnection = 0;
                ////Console.WriteLine("Client {0} connected...", openPipes.ClientCounter);
                Logger.LogApplicationEvents("Not Necessary", "Client Connected");
            }
        }



        /*  
        *  METHOD        : RecieveFromAllClients
        *  DESCRIPTION   : This method is used to receive messages from the clients,
        *   and save them to the data repository where they can be accessed by the sending thread
        *  PARAMETERS    : object data : Generic object type argument which us
        *  cast to a pipe, and used to receive incoming messages
        *  RETURNS       : void : The methods has no return
        */
        private static void RecieveFromAllClients(object data, ref Dictionary<int, string> messageList, ref int messageCounter, ref Dictionary<NamedPipeServerStream, NamedPipeServerStream> PairPipes)
        {

            //Cast the object as a pipe
            NamedPipeServerStream Client_IN = null;
            Client_IN = (NamedPipeServerStream)data;


            //Keep cycling an receiving new messages until the clients indicate they are shutting down
            bool clientDisconnectCommand = false;
            while (clientDisconnectCommand == false)
            {
                //Open an stream to the pipe taking incoming messages, and write the message to the string         
                StreamReader readFromPipe = new StreamReader(Client_IN);
                string incomingMessage = readFromPipe.ReadLine();

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
            if (PairPipes.TryGetValue(Client_IN, out NamedPipeServerStream Client_OUT))
            {
                Client_OUT.Close();
            }
            Client_IN.Close();

//// ///////////////////////////////////
//            // DEBUG
//            foreach (KeyValuePair<NamedPipeServerStream, NamedPipeServerStream> kv in PairPipes)
//                Console.WriteLine(kv);
//// ///////////////////////////////////

        }//RecieveFromAllClients


        /*  
        *  METHOD        : SendToAllClients
        *  DESCRIPTION   : This method is used to DEBUG
        *  PARAMETERS    : void : The method takes no arguments
        *  RETURNS       : void : The methods has no return
        */
        private static void SendToAllClients(object data, ref Dictionary<int, string> messageList, ref int messageCounter)
        {

            //Cast the object as a 
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
                        Logger.LogApplicationEvents(filepath, errorMessage.ToString());
                    }
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

