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
        string player_type = null;
        string opp_player_type = null;

        public void button_false()
        {
            //button1.Invoke((MethodInvoker)(delegate () {
            //    button1.Enabled = false;
            //}));

            button2.Invoke((MethodInvoker)(delegate () {
                button2.Enabled = false;
            }));

            button3.Invoke((MethodInvoker)(delegate () {
                button3.Enabled = false;
            }));

            button4.Invoke((MethodInvoker)(delegate () {
                button4.Enabled = false;
            }));

            button7.Invoke((MethodInvoker)(delegate () {
                button7.Enabled = false;
            }));

            button6.Invoke((MethodInvoker)(delegate () {
                button6.Enabled = false;
            }));

            button8.Invoke((MethodInvoker)(delegate () {
                button8.Enabled = false;
            }));

            button9.Invoke((MethodInvoker)(delegate () {
                button9.Enabled = false;
            }));

            button10.Invoke((MethodInvoker)(delegate () {
                button10.Enabled = false;
            }));

            button5.Invoke((MethodInvoker)(delegate () {
                button5.Enabled = false;
            }));
        }

        public void button_clear()
        {
            //button1.Invoke((MethodInvoker)(delegate () {
            //    button1.Text = "";
            //}));
            button1.Enabled = false;
            button2.Invoke((MethodInvoker)(delegate () {
                button2.Text = "";
            }));

            button3.Invoke((MethodInvoker)(delegate () {
                button3.Text = "";
            }));

            button4.Invoke((MethodInvoker)(delegate () {
                button4.Text = "";
            }));

            button7.Invoke((MethodInvoker)(delegate () {
                button7.Text = "";
            }));

            button6.Invoke((MethodInvoker)(delegate () {
                button6.Text = "";
            }));

            button8.Invoke((MethodInvoker)(delegate () {
                button8.Text = "";
            }));

            button9.Invoke((MethodInvoker)(delegate () {
                button9.Text = "";
            }));

            button10.Invoke((MethodInvoker)(delegate () {
                button10.Text = "";
            }));

            button5.Invoke((MethodInvoker)(delegate () {
                button5.Text = "";
            }));
        }

        public void button_true()
        {
            //button1.Invoke((MethodInvoker)(delegate () {
            //    button1.Enabled = true;
            //}));

            button2.Invoke((MethodInvoker)(delegate () {
                button2.Enabled = true;
            }));

            button3.Invoke((MethodInvoker)(delegate () {
                button3.Enabled = true;
            }));

            button4.Invoke((MethodInvoker)(delegate () {
                button4.Enabled = true;
            }));

            button7.Invoke((MethodInvoker)(delegate () {
                button7.Enabled = true;
            }));

            button6.Invoke((MethodInvoker)(delegate () {
                button6.Enabled = true;
            }));

            button8.Invoke((MethodInvoker)(delegate () {
                button8.Enabled = true;
            }));

            button9.Invoke((MethodInvoker)(delegate () {
                button9.Enabled = true;
            }));

            button10.Invoke((MethodInvoker)(delegate () {
                button10.Enabled = true;
            }));

            button5.Invoke((MethodInvoker)(delegate () {
                button5.Enabled = true;
            }));
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(textBox1.Text), 1499);
            //client_socket.BeginConnect(ipe, new AsyncCallback(Connected), client_socket);
            RequestConnection(client_socket, ipe);
            button1.Enabled = false;
        }

        void RequestConnection(Socket server, IPEndPoint ipe)
        {
            //Socket client = (Socket)iar.AsyncState;
            try
            {
                server.Connect(ipe);

                if (server.Connected)
                {
                    server.Receive(msg);
                    //label3.Text = Encoding.ASCII.GetString(msg);
                    this.BeginInvoke((MethodInvoker)delegate ()
                    {
                        string text= Encoding.ASCII.GetString(msg);
                        label3.Text = text;
                        Thread thread = new Thread(new ThreadStart(() => Start_Receive(server)));
                        thread.Start();
                        if (text.Contains("Connected... You are player 1"))
                        {
                            //the player is X
                            player_type = "X";
                            opp_player_type = "O";
                            button_true();
                        }

                        if (text.Contains("Connected... You are player 2"))
                        {
                            //the player is O
                            player_type = "O";
                            opp_player_type = "X";
                            button_false();
                        }
                    });

                }
            }
            catch (SocketException)
            {
                this.BeginInvoke((MethodInvoker)delegate ()
                {
                    label3.Text = "Error Connecting!";
                });
            }
        }

        void Start_Receive(Socket server)
        {
            //client.BeginReceive(msg, 0, msg.Length, SocketFlags.None, new AsyncCallback(recieve), client_socket);

            while (server.Connected)// while client is connected to the server
            {

                server.Receive(msg);
                string text = Encoding.ASCII.GetString(msg);
                Console.WriteLine(text);

                if(text.Contains("You win."))
                {
                    this.BeginInvoke((MethodInvoker)(delegate()
                    {
                        MessageBox.Show("You win.", "INFO", MessageBoxButtons.OK);
                    }));
                    button_true();
                    button_clear();
                }

                if(text.Contains("You loose."))
                {
                    this.BeginInvoke((MethodInvoker)(delegate ()
                    {
                        MessageBox.Show("You loose.", "INFO", MessageBoxButtons.OK);
                    }));
                    button_false();
                    button_clear();
                }
                if (text.Contains("Game tied."))
                {
                    this.BeginInvoke((MethodInvoker)(delegate ()
                    {
                        MessageBox.Show("Game tied.", "INFO", MessageBoxButtons.OK);
                    }));
                    button_true();
                    button_clear();
                }

                if (text.Contains("3"))
                {
                    button3.Invoke((MethodInvoker)(delegate() { 
                        if(button3.Text=="") button3.Text = opp_player_type; //reduce overriding of previous marked
                    }));
                    button_true();
                }

                if (text.Contains("1"))
                {
                    button10.Invoke((MethodInvoker)(delegate () {
                        if (button10.Text == "") button10.Text = opp_player_type;
                    }));
                    button_true();
                }

                if (text.Contains("2"))
                {
                    button2.Invoke((MethodInvoker)(delegate () {
                        if (button2.Text == "") button2.Text = opp_player_type;
                    }));
                    button_true();
                }

                if (text.Contains("4"))
                {
                    button4.Invoke((MethodInvoker)(delegate () {
                        if (button4.Text == "") button4.Text = opp_player_type;
                    }));
                    button_true();
                }

                if (text.Contains("5"))
                {
                    button5.Invoke((MethodInvoker)(delegate () {
                        if (button5.Text == "") button5.Text = opp_player_type;
                    }));
                    button_true();
                }

                if (text.Contains("6"))
                {
                    button6.Invoke((MethodInvoker)(delegate () {
                        if (button6.Text == "") button6.Text = opp_player_type;
                    }));
                    button_true();
                }

                if (text.Contains("7"))
                {
                    button7.Invoke((MethodInvoker)(delegate () {
                        if (button7.Text == "") button7.Text = opp_player_type;
                    }));
                    button_true();
                }

                if (text.Contains("8"))
                {
                    button8.Invoke((MethodInvoker)(delegate () {
                        if (button8.Text == "") button8.Text = opp_player_type;
                    }));
                    button_true();
                }

                if (text.Contains("9"))
                {
                    button9.Invoke((MethodInvoker)(delegate () {
                        if (button9.Text == "") button9.Text = opp_player_type;
                    }));
                    button_true();
                }

            }

            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "")
            {
                button2.Text = player_type;
                clicked_btn_position = 2;

                byte[] my_message = Encoding.ASCII.GetBytes(player_type + " " + clicked_btn_position.ToString());
                client_socket.Send(my_message);

                button_false();

            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (button4.Text == "")
            {
                button4.Text = player_type;
                clicked_btn_position = 4;

                byte[] my_message = Encoding.ASCII.GetBytes(player_type + " " + clicked_btn_position.ToString());
                client_socket.Send(my_message);

                button_false();

            }

        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "")
            {
                button3.Text = player_type;
                clicked_btn_position = 3;

                byte[] my_message = Encoding.ASCII.GetBytes(player_type + " " + clicked_btn_position.ToString());
                client_socket.Send(my_message);

                button_false();
                //client_socket.BeginReceive(msg, 0, msg.Length, new AsyncCallback(ReceiveData), client_socket);
            }

           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (button5.Text == "")
            {
                button5.Text = player_type;
                clicked_btn_position = 5;

                byte[] my_message = Encoding.ASCII.GetBytes(player_type + " " + clicked_btn_position.ToString());
                client_socket.Send(my_message);

                button_false();
                //client_socket.BeginReceive(msg, 0, msg.Length, new AsyncCallback(ReceiveData), client_socket);
            }

            

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (button6.Text == "")
            {
                button6.Text = player_type;
                clicked_btn_position = 6;

                byte[] my_message = Encoding.ASCII.GetBytes(player_type + " " + clicked_btn_position.ToString());
                client_socket.Send(my_message);

                button_false();
                //client_socket.BeginReceive(msg, 0, msg.Length, new AsyncCallback(ReceiveData), client_socket);
            }

           
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (button7.Text == "")
            {
                button7.Text = player_type;
                clicked_btn_position = 7;

                byte[] my_message = Encoding.ASCII.GetBytes(player_type + " " + clicked_btn_position.ToString());
                client_socket.Send(my_message);

                button_false();
                //client_socket.BeginReceive(msg, 0, msg.Length, new AsyncCallback(ReceiveData), client_socket);
            }

            
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (button8.Text == "")
            {
                button8.Text = player_type;
                clicked_btn_position = 8;

                byte[] my_message = Encoding.ASCII.GetBytes(player_type + " " + clicked_btn_position.ToString());
                client_socket.Send(my_message);

                button_false();
                //client_socket.BeginReceive(msg, 0, msg.Length, new AsyncCallback(ReceiveData), client_socket);
            }

            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (button9.Text == "")
            {
                button9.Text = player_type;
                clicked_btn_position = 9;

                byte[] my_message = Encoding.ASCII.GetBytes(player_type + " " + clicked_btn_position.ToString());
                client_socket.Send(my_message);

                button_false();
                //client_socket.BeginReceive(msg, 0, msg.Length, new AsyncCallback(ReceiveData), client_socket);
            }

           
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (button10.Text == "")
            {
                button10.Text = player_type;
                clicked_btn_position = 1;
                
                //purpose of byte [] message: it will give previous buon pessed by opponent client
                byte[] my_message = Encoding.ASCII.GetBytes(player_type + " " + clicked_btn_position.ToString());
                client_socket.Send(my_message);

                button_false();
                //client_socket.BeginReceive(msg, 0, msg.Length, new AsyncCallback(ReceiveData), client_socket);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
