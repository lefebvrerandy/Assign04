using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Server_Service
{
    public partial class Server_Service : ServiceBase
    {
        Thread tServer = null;
        Server instanceServer = null;
        public Server_Service()
        {
            InitializeComponent();
            CanPauseAndContinue = true;
            instanceServer = new Server();
            ThreadStart ts = new ThreadStart(Server.RunServer);
            tServer = new Thread(ts);

        }

        protected override void OnStart(string[] args)
        {
            //Start the Thread
            Logger.LogApplicationEvents("Service Starting");
            Server.Run = true;
            ThreadStart ts = new ThreadStart(Server.RunServer);
            tServer = new Thread(ts);
            tServer.Start();
            Logger.LogApplicationEvents("Service Started");
        }

        protected override void OnStop()
        {  
            //Stop the Thread
            Logger.LogApplicationEvents("Service Stopping");
            Server.Run = false;
            Server.toDisconnect = true;
            // Wait for the server thread to finish
            Thread.Sleep(100);
            tServer.Join();
            Logger.LogApplicationEvents("Service Stopped");
        }

        protected override void OnPause()
        {
            //Pause the Thread
            Logger.LogApplicationEvents("Service Pausing");
            Server.Pause = true;
            Logger.LogApplicationEvents("Service Paused");
        }

        protected override void OnContinue()
        {
            //Resume the Thread
            Logger.LogApplicationEvents("Service Continuing");
            Server.Pause = false;
            Logger.LogApplicationEvents("Service Continued");
        }
    }
}
