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
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginOKButtonClick(object sender, RoutedEventArgs e)
        {
            bool userNameValidity = false;
            //userNameValidity = User.IsUserNameValid();


            //if (userNameValidity == true)
            //{
            //    User.ClientID = newUserName;
            //}
            this.Close();
        }
    }
}
