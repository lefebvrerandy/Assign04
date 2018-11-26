/* 
*  FILE          : ServerPipes.cs
*  PROJECT       : PROG 2120 - Assignment 5
*  PROGRAMMER    : Bence Karner & Randy Lefebvre
*  DESCRIPTION   : This file contains the Service. Starts a new service and runs it. 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Server_Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Server_Service()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
