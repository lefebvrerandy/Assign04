/* 
*  FILE          : User.cs
*  PROJECT       : PROG 2120 - Assignment 4
*  PROGRAMMER    : Bence Karner & Randy Lefebvre
*  DESCRIPTION   : This file contains the User class, which has methods for getting, and setting the values 
*                  associated with the user (both literally for the user behind the keyboard, and figuratively
*                  for the user class), and for validating userName field. 
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


        /*  
        *  METHOD        : IsUserNameValid
        *  DESCRIPTION   : Used to run the userName string through a series of validation checks
        *                  to ensure it's valid; checks for,
        *                  1) String length
        *                  2) Char usage
        *  PARAMETERS    : string newUserName: The string containing the userName
        *  RETURNS       : bool : Retruns true if the string is valid
        */
        public static bool IsUserNameValid(string newUserName)
        {
            bool nameValidity = false;

            //instandiate helper method to check username validity

            return nameValidity;

        }//IsUserNameValid
    }//class
}//namepsace
