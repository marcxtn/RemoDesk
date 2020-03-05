using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace remoDesk
{
    public partial class stream : Form
    {

        //create the required variables
        private readonly int port;
        private TcpClient client;
        private TcpListener server;
        private NetworkStream mainStream;

        private readonly Thread Listening;
        private readonly Thread GetImage;

        //load the window with the given var (The port)
        public stream(int Port)
        {
            //save the port in a new var
            port = Port;
            //create a new Tcp-Client/Listener and Image Recevier
            client = new TcpClient();
            Listening = new Thread(StartListening);
            GetImage = new Thread(ReceiveImage);
            InitializeComponent();
        }

        private void StartListening()
        {

            //while the client is not connected
            while (client.Connected == false)
            {

                //start the server
                server.Start();
                client = server.AcceptTcpClient();

            }

            //start getting images / the stream
            GetImage.Start();

        }

        private void StopListening()
        {

            //stop the server
            server.Stop();
            //set "client" to null
            client = null;
            //stop listening and getting Images
            if (Listening.IsAlive) Listening.Abort();
            if (GetImage.IsAlive) GetImage.Abort();

        }

        private void ReceiveImage()
        {

            //create a new Binary Formatter
            BinaryFormatter binFormatter = new BinaryFormatter();
            //while the client is connected
            while (client.Connected)
            {

                try
                {
                    //try to get the Stream
                    mainStream = client.GetStream();
                    pictureBox1.Image = (Image)binFormatter.Deserialize(mainStream);
                }
                catch
                {

                    //if it catch give an error message
                    MessageBox.Show("The client has left convergence!");

                }

            }

        }

        protected override void OnLoad(EventArgs e)
        {

            //define the IP address and the port and start listening
            base.OnLoad(e);
            server = new TcpListener(IPAddress.Any, port);
            Listening.Start();

        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {

            //stop Listening
            base.OnFormClosed(e);
            StopListening();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
