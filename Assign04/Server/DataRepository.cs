



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Server
{

    static class DataRepository
    {
        public static int MessageCounter { get; set; }
        public static Dictionary<int,string> messageList;


        //Returns a reference to the list
        //Only need to call once per outgoing message thread
        public static ref Dictionary<int, string> MessageRepository
        {
            get { return ref messageList; }
        }


        //Add a message to the list
        //Every new message flips the bool to true, indicating to the other threads that they can pull from the list again
        public static void AddMessageToRepository(string clientMessage)
        {
            MessageCounter++;
            messageList.Add(MessageCounter, clientMessage);
        }
    }
}
