/* 
*  FILE          : MainWindow.xaml.cs
*  PROJECT       : PROG 2120 - Assignment 4
*  PROGRAMMER    : Bence Karner & Randy Lefebvre
*  DESCRIPTION   : This file houses the main entry point for the application, and contains all the logic 
*                  requried to execute each module.. The program was created as per assigment requirments, 
*                  and is intended to function as part of a client-server chat system. The chat client 
*				   uses a duplex named pipe to communicate with the server. The program, allows the user to
*				   send a message using the text box input area, and display received messages from the server
*/



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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
