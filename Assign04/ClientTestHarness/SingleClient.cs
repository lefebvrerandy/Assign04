using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Security.Principal;
using System.Diagnostics;
using System.Threading;

public class PipeClient
{
    private static int numClients = 100;

    public static void Main(string[] Args)
    {
        // Create 2 threads. 
        // Thread 1 = Loop 1
        // Thread 2 = Loop 2

        // loop 1
        // Get user input
        // Connect to server (open pipe)
        // Send user input to server
        // Recieve Feedback from server
        // disconnect from server
        // return loop 1

        // loop 2
        // (sleep X amount)
        // connect to server and check for new messages
        // disconnect from server
        // return loop 2



     NamedPipeClientStream pipeClient =
        new NamedPipeClientStream(".", "testpipe",
        PipeDirection.InOut, PipeOptions.None,
        TokenImpersonationLevel.Impersonation);

        Console.WriteLine("Connecting to server...\n");
        pipeClient.Connect();

        StreamString ss = new StreamString(pipeClient);
        // Validate the server's signature string
        if (ss.ReadString() == "I am the one true server!")
        {
            // The client security token is sent with the first write.
            // Send the name of the file whose contents are returned
            // by the server.
            ss.WriteString("c:\\textfile.txt");

            // Print the file to the screen.
            Console.Write(ss.ReadString());
        }
        else
        {
            Console.WriteLine("Server could not be verified.");
        }
        pipeClient.Close();
                // Give the client process some time to display results before exiting.
        Thread.Sleep(4000);



        Console.ReadKey();
    }
}


// Defines the data protocol for reading and writing strings on our stream
public class StreamString
{
    private Stream ioStream;
    private UnicodeEncoding streamEncoding;

    public StreamString(Stream ioStream)
    {
        this.ioStream = ioStream;
        streamEncoding = new UnicodeEncoding();
    }

    public string ReadString()
    {
        int len;
        len = ioStream.ReadByte() * 256;
        len += ioStream.ReadByte();
        byte[] inBuffer = new byte[len];
        ioStream.Read(inBuffer, 0, len);

        return streamEncoding.GetString(inBuffer);
    }

    public int WriteString(string outString)
    {
        byte[] outBuffer = streamEncoding.GetBytes(outString);
        int len = outBuffer.Length;
        if (len > UInt16.MaxValue)
        {
            len = (int)UInt16.MaxValue;
        }
        ioStream.WriteByte((byte)(len / 256));
        ioStream.WriteByte((byte)(len & 255));
        ioStream.Write(outBuffer, 0, len);
        ioStream.Flush();

        return outBuffer.Length + 2;
    }
}