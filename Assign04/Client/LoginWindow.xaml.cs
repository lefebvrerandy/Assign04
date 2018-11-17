



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UtilityMethods;
namespace Client
{

    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {


        /*  
        *  METHOD        : 
        *  DESCRIPTION   : 
        *  PARAMETERS    : 
        *  RETURNS       : 
        */
        public LoginWindow()
        {
            InitializeComponent();
            loginWindowErrorOutput.Text = "UserName rules are as follows: \n -Must be within 1 - 16 characters in length \n - Must only contain characters withing the range of ASCII 32 - 126";
        }



        /*  
        *  METHOD        : 
        *  DESCRIPTION   : 
        *  PARAMETERS    : 
        *  RETURNS       : 
        */
        private void LoginOKButtonClick(object sender, RoutedEventArgs e)
        {
            bool userNameValidity = false;
            Utility ValidationMethods = new Utility();

            //Get the users name from the login textbox, and ensure it meets the standards set forth in the login window
            string newUserName = loginWindowUserName.Text;


            //Ensure the userName is above 0 chars, and below the max of 17
            if ((userNameValidity = ValidationMethods.CheckUserNameLength(newUserName)) == true)
            {

                //Check if the userName is using any invalid chars (i.e. 31 < char ASCII value < 127 )
                if ((userNameValidity = ValidationMethods.CheckCharactersInString(newUserName)) == true)
                {

                    //All tests passed; save the userName, and close the window
                    User.ClientID = newUserName;
                    this.Close();
                }


                //The username contained invalid chars
                else
                {
                    loginWindowErrorOutput.Text = "Error: Your userName was invalid; Please only use letters A-Z, a-z, and special characters between ASCII 32, and 126. \n" +
                        @"For more information on ASCII code of valid characters, please see: http://www.asciitable.com";
                }
            }


            //The username was not the appropriate length
            else
            {
                loginWindowErrorOutput.Text = "Error: Your userName was invalid; Please limit your input between 1 - 16 characters";
            }


        }//...LoginOKButtonClick
    }//...class
}//...namespace
