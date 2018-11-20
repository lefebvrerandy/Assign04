/* 
*  FILE          : User.cs
*  PROJECT       : PROG 2120 - Assignment 4
*  PROGRAMMER    : Bence Karner & Randy Lefebvre
*  DESCRIPTION   : This file contains the User class, which has methods for getting, and setting the values 
*                  associated with the user (both literally for the user behind the keyboard, and figuratively
*                  for the user class)
*                  
*/



using System;
namespace Client
{

    /* 
    *   NAME    : User
    *   PURPOSE : The class was designed to to mimic the properties a user of the chat system might have. Users would have
    *   a message they intend to send, and a userName thats picked during the inital stages of the program. The class was
    *   not meant to be a perfect representation of a human, but more of an an abstract interpretation of the 3 key elements 
    *   required to identify a user
    */
    public static class User
    {
        public static string ClientID {get; set;}       //Contains the users ID/userName as entered from the login window 
        public static string Command {get; set;}        //Contains the users command identifier
        public static string Message {get; set;}        //Contains the users message
    }//class
}//namepsace
