using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        Socket client_socket;
        int clicked_btn_position = 0;
        byte[] msg = new byte[1024];
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(textBox1.Text), 1499);
            client_socket.BeginConnect(ipe, new AsyncCallback(Connected), client_socket);
        }

        void Connected(IAsyncResult iar)
        {
            Socket client = (Socket)iar.AsyncState;
            try
            {
                client.EndConnect(iar);

                this.BeginInvoke((MethodInvoker)delegate ()
                {
                    label3.Text = "Connected to: " + client.RemoteEndPoint.ToString();
                });

                
            }
            catch (SocketException)
            {
                this.BeginInvoke((MethodInvoker)delegate ()
                {
                    label3.Text = "Error Connecting!";
                });
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "")
            {
                button2.Text = "X";
                clicked_btn_position = 3;

                msg = Encoding.ASCII.GetBytes(clicked_btn_position.ToString());
                client_socket.Send(msg, SocketFlags.None);
                //client_socket.BeginReceive(msg, 0, msg.Length, SocketFlags.None, new AsyncCallback(ReceiveData), client_socket);
            }
        }

        
    }
}
