using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Diagnostics;
using System.Windows.Threading;
using System.Threading.Tasks;

namespace Client
{
    #region Delegates

    public delegate void NewConnectionEventHandler(string output);

    #endregion

    class Network
    {
        #region Enums

        public enum eNetworkCommands
        {
            Error = 0,
            LoginCommand = 1,
            RegisterCommand = 2,
            NewGroupCommand = 3,
            GetUsersCommand = 4,
            UserGroupsCommand = 5,
            ErrorGroup = 6,
            GroupIsOK = 7,
            Messages = 8,
            ChatMessage = 9
        }

        #endregion

        #region Fields

        private TcpClient client;
        private string SERVER_ADDRESS = "127.0.0.1";
        private int SERVER_PORT = 8870;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new instance of this class.
        /// </summary>
        public Network()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(SERVER_ADDRESS), SERVER_PORT); // endpoint where server is listening
            this.client = new TcpClient(); 
            this.client.Connect(ep);
            this.ShowOutPut(this.client.Client.AddressFamily.ToString());

            Task.Factory.StartNew(Run);
        }
       
        #endregion

        #region Events

        public event NewConnectionEventHandler NewConnection;

        #endregion

        #region Public Methods

        /// <summary>
        /// Send message
        /// </summary>
        /// <param name="mess">package bytes</param>
        public void SendMessages(byte[] mess)
        {
            //tacking data from window and sending to client
            try { this.client.Client.Send(mess); }
            catch { }
        }
        #endregion

        #region Private Methods

        //recieving data from server
        private void Run()
        {
            while (true)
            {
                byte[] buffer = new byte[client.ReceiveBufferSize];

                string dataReceived = "";
                try
                {
                    int bytesRead = this.client.Client.Receive(buffer);
                    dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                }
                catch { }

                this.ShowOutPut(dataReceived);
            }
        }

        //calling NewConnection method (from MainWindow) by an event
        private void ShowOutPut(string output)
        {
            var handler = this.NewConnection;
            if (handler != null)
                handler(output);
        }

        #endregion
    }
}