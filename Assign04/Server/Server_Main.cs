using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;



namespace Server
{
    public class Server_Main
    {
        private static int numThreads = 3;

        string pipename = "test";
        NamedPipeServerStream pipeServer = null;

        public static void Main()
        {
            int i;
            
            Thread ServerPipeLoop = new Thread(ServerAcceptLoopThread);
            //Thread Server;
            ServerPipeLoop.Start();

        }

        private static void ServerAcceptLoopThread(object data)
        {
            // Pipe name
            string pipename = "test";

            // This is an endless loop. This loop will 
            // Open two pipes per client, one being the IN pipe, other being the OUT pipe
            //  From there the method will spawn a new thread for each
            while (true)
            {
                NamedPipeServerStream pipe_in = OpenInPipe(pipename);
                NamedPipeServerStream pipe_out = OpenOutPipe(pipename);


                // Start a new thread, Send the pipe_in pipe to the new thread
                Thread RecieveFromClients = new Thread(RecieveFromAllClients);
                RecieveFromClients.Start(pipe_in);

                // Start a new thread, Send the pipe_out pipe to the new thread
                Thread SendToClients = new Thread(SendToAllClients);
                SendToClients.Start(pipe_out);
            }
        }
        private static void RecieveFromAllClients(object data)
        {
            // Cast the object as the proper datatype
            NamedPipeServerStream Client_IN = null;
            Client_IN = (NamedPipeServerStream)data;




        }

        private static void SendToAllClients(object data)
        {
            // Cast the object as the proper datatype
            NamedPipeServerStream Client_OUT = null;
            Client_OUT = (NamedPipeServerStream)data;

            // This is where we will recieve the information from the client, and send it to the
            //  OUT stream for each client.

        }


        private static void ServerThread(object data)
        {
            NamedPipeServerStream pipeServer =
                new NamedPipeServerStream("testpipe", PipeDirection.InOut, numThreads);

            int threadId = Thread.CurrentThread.ManagedThreadId;

            // Wait for a client to connect
            pipeServer.WaitForConnection();

            Console.WriteLine("Client connected on thread[{0}].", threadId);
            try
            {
                // Read the request from the client. Once the client has
                // written to the pipe its security token will be available.

                StreamString ss = new StreamString(pipeServer);

                // Verify our identity to the connected client using a
                // string that the client anticipates.

                ss.WriteString("I am the one true server!");
                string filename = ss.ReadString();

                // Read in the contents of the file while impersonating the client.
                ReadFileToStream fileReader = new ReadFileToStream(ss, filename);

                // Display the name of the user we are impersonating.
                Console.WriteLine("Reading file: {0} on thread[{1}] as user: {2}.",
                    filename, threadId, pipeServer.GetImpersonationUserName());
                pipeServer.RunAsClient(fileReader.Start);
            }
            // Catch the IOException that is raised if the pipe is broken
            // or disconnected.
            catch (IOException e)
            {
                Console.WriteLine("ERROR: {0}", e.Message);
            }
            pipeServer.Close();
        }
    }
}

