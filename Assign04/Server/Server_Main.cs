/* 
*  FILE          : Server.cs
*  PROJECT       : PROG 2120 - Assignment 4
*  PROGRAMMER    : Randy Lefebvre & Bence Karner
*  DESCRIPTION   : This file contains the the starting point of the Server applications. The program 
*                  creates a file, and logs the start of the application before executing its networking
*                  functionality. The purpose of the server is to open unidirectional pipes for all incoming,
*                  and outgoing connections from the clients. The applications recieves messages from the clients
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
            Utility UIManager = new Utility();
            UIManager.SetConsoleAttributes();
            FileIO fileManager = new FileIO();
            fileManager.PrintConsoleUI();


            //Grab the filepath for the event logger, and log the server start time
            string filepath = fileManager.ReadXMLDocument("logFilePath");
            fileManager.CreateFile(filepath);
            Logger.LogApplicationEvents(filepath, "SERVER START");


            //Default the value of the message counter before a message is added
            DataRepository.MessageCounter = 0;


            //Thead the server, and go into a wait loop
            //Thread ServerPipeLoop = new Thread(ServerAcceptLoopThread);
            //ServerPipeLoop.Name = "ServerPipeLoopThread";
            //ServerPipeLoop.Start();

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
        *                   before theading off to open another connection
        *  PARAMETERS    : void : The method takes no arguments
        *  RETURNS       : void : The methods has no return
        */
        private static void ServerAcceptLoopThread()
        {

            //DEBUG
            int messageCounter = 0;
            Dictionary<int, string> messageList = new Dictionary<int, string>();


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


                Console.WriteLine("Waiting for connections...");
                NamedPipeServerStream pipe_in = openPipes.OpenInPipe("serverIn");
                NamedPipeServerStream pipe_out = openPipes.OpenOutPipe("serverOut");


                // Start a new thread, Send the pipe_in pipe to the new thread
                Thread RecieveFromClients = new Thread(() => RecieveFromAllClients(pipe_in, ref messageList, ref messageCounter));
                RecieveFromClients.Name = "RecieveMessageThread";
                RecieveFromClients.Start();


                //Start a new thread, Send the pipe_out pipe to the new thread
                Thread SendToClients = new Thread(() =>  SendToAllClients(pipe_out, ref messageList, ref messageCounter));
                SendToClients.Name = "SendMessageThread";
                SendToClients.Start();


                //Increment the client counter so the server knows when to return
                openPipes.ClientCounter++;
                initalClientConnection = 0;
                Console.WriteLine("Client {0} connected...", openPipes.ClientCounter);
            }
        }



        /*  
        *  METHOD        : RecieveFromAllClients
        *  DESCRIPTION   : This method is used to recieve messages from the clients,
        *   and save them to the data repository where they can be accessed by the sending thread
        *  PARAMETERS    : object data : Generic object type argument which us
        *  cast to a pipe, and used to recieve incoming messages
        *  RETURNS       : void : The methods has no return
        */
        private static void RecieveFromAllClients(object data, ref Dictionary<int, string> messageList, ref int messageCounter)
        {

            //Cast the object as a pipe
            NamedPipeServerStream Client_IN = null;
            Client_IN = (NamedPipeServerStream)data;


            //Keep cycling an recieving new messages until the clients indicate they are shutting down
            bool clientDisconnectCommand = false;
            while (clientDisconnectCommand == false)
            {
                //Open an stream to the pipe taking incoming messages, and write the message to the string         
                StreamReader readFromPipe = new StreamReader(Client_IN);
                string incomingMessage = readFromPipe.ReadLine();
                messageCounter++;
                messageList.Add(messageCounter, incomingMessage);
            }
            Thread.Sleep(100);
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



            //Save the current message count in the ditctionary
            int currentMessageCount = messageCounter;



            //Keep cycling looking for new messages in the dictionary
            //For every message thats added the messageRepository, the thread managing incoming messages will increment the counter

           bool clientDisconnectCommand = false;
           while (clientDisconnectCommand == false)
           {
                if (messageCounter >= currentMessageCount)
                {
                    //A new message has been added to the reppsotory since the last check
                    //Grab the message associate with the current counter, and send it out to the client
                    string outgoingClientMessage = string.Empty;
                    messageList.TryGetValue(currentMessageCount, out outgoingClientMessage);
                    outputStream.WriteLine(outgoingClientMessage);
                    outputStream.Flush();


                    //Increment the message counter and cycle back to check again for a new message
                    currentMessageCount++;
                }
           } 
            Thread.Sleep(100);
        }//SendToAllClients
    }//class
}//namepsace

