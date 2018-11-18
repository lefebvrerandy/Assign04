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
    *   PURPOSE :
    */
    public static class User
    {
        public static string ClientID {get; set;}       //
        public static string Command {get; set;}        //
        public static string Message {get; set;}        //
    }//class
}//namepsace
