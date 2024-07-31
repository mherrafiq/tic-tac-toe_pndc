using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Program
    {
        static private Socket server_socket;
        static private int no_of_clients=0;
        static private string[] client_ids = new string[100];
        static Program p =new Program();
        static string[] arr= new string[10];
        int max_turns = 0;
        
        private static Dictionary<string, Socket> connectedClients = new Dictionary<string, Socket>();

        static void Main(string[] args)
        {
            server_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipe = new IPEndPoint(IPAddress.Any, 1499);
            server_socket.Bind(ipe);
            server_socket.Listen(2);

            for(int i=0; i<arr.Length; i++)
            {
                arr[i] = "";
            }
            
            Console.WriteLine("Server listening...");
            Socket client_socket = default(Socket);
            while (true)
            {
                client_socket = server_socket.Accept();

                string clientId = Guid.NewGuid().ToString(); // Generate a unique identifier for the client
                connectedClients.Add(clientId, client_socket); // Add the client to the connected clients collection

                client_ids[no_of_clients] = clientId;
                //Console.WriteLine(client_ids[no_of_clients]);
                
                Console.WriteLine(no_of_clients+1 + " clients connected! "+ clientId);
                no_of_clients++;
                int playerNumber = no_of_clients % 2;

                if (playerNumber == 0)
                {
                    playerNumber = 2;
                }
                string data = "Connected... You are player "+playerNumber.ToString();
                Console.WriteLine(data);
                byte[] msg = Encoding.ASCII.GetBytes(data);
                Console.WriteLine(msg.Length);
                //send message to particular client
                client_socket.SendTo(msg, client_socket.RemoteEndPoint);
                Thread thread = new Thread(new ThreadStart(() => p.User(clientId)));
                thread.Start();
            }

        }

        int CheckWin()
        {

            //horizontal Wins
            if((arr[1]==arr[2] && arr[2] == arr[3]) && (arr[1]!="" && arr[2]!="" && arr[3]!=""))
            {
                return win_type(arr[2]);
            }
            
            if((arr[4] == arr[5] && arr[5] == arr[6]) && (arr[4] != "" && arr[5] != "" && arr[6] != ""))
            {
                return win_type(arr[5]);
            }
            
            if((arr[7] == arr[8] && arr[8] == arr[9]) && (arr[7] != "" && arr[8] != "" && arr[9] != ""))
            {
                return win_type(arr[8]);
            }

            //vertical win
            if ((arr[1] == arr[4] && arr[4] == arr[7]) && (arr[1] != "" && arr[4] != "" && arr[7] != ""))
            {
                return win_type(arr[4]);
            }

            if ((arr[2] == arr[5] && arr[5] == arr[8]) && (arr[2] != "" && arr[5] != "" && arr[8] != ""))
            {
                return win_type(arr[5]);
            }

            if ((arr[3] == arr[6] && arr[6] == arr[9]) && (arr[3] != "" && arr[6] != "" && arr[9] != ""))
            {
                return win_type(arr[6]);
            }

            //diagonal win
            if ((arr[1] == arr[5] && arr[5] == arr[9]) && (arr[1] != "" && arr[5] != "" && arr[9] != ""))
            {
                return win_type(arr[5]);
            }

            if ((arr[3] == arr[5] && arr[5] == arr[7]) && (arr[3] != "" && arr[5] != "" && arr[7] != ""))
            {
                return win_type(arr[5]);
            }

            return 0; //no one is winning currently
        }

        void clear()
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = "";
            }
            max_turns = 0;
        }

        int win_type(string type)
        {
            if (type==("X"))
            {
                return 1; //clien 1 wins
            }
            else
            {
                return 2; //clien 2 wins
            }
        }

        void User(string clientId)
        {
            while (true)
            {

                if (client_ids[0].Contains(clientId)) //1st client
                {
                    byte[] msg = new byte[1024];
                    Socket clientSocket = connectedClients[client_ids[0]];

                    clientSocket.Receive(msg);
                    string received_text = Encoding.ASCII.GetString(msg);

                    string[] break_text = received_text.Split(" ");

                    //append he mark in the array
                    int position = Convert.ToInt32(break_text[1]);
                    arr[position] = break_text[0];
                    Console.WriteLine(break_text[0]+ " and "+break_text[1]+" from client 1");

                    clientSocket = connectedClients[client_ids[1]];
                    clientSocket.SendTo(Encoding.ASCII.GetBytes(received_text), clientSocket.RemoteEndPoint);

                    int check = CheckWin();
                    if (check != 0)
                    {
                        //means there is a win
                        //send to all

                        if (check == 1) //client 1 wins
                        {
                            clientSocket = connectedClients[client_ids[0]];

                            string text = "You win.";
                            Console.WriteLine(text);
                            clientSocket.SendTo(Encoding.ASCII.GetBytes(text), clientSocket.RemoteEndPoint);

                            clientSocket = connectedClients[client_ids[1]];

                            text = "You loose.";
                            clientSocket.SendTo(Encoding.ASCII.GetBytes(text), clientSocket.RemoteEndPoint);
                            clear();
                        }

                        else //client 2 wins
                        {
                            clientSocket = connectedClients[client_ids[0]];

                            string text = "You loose.";
                            Console.WriteLine(text);
                            clientSocket.SendTo(Encoding.ASCII.GetBytes(text), clientSocket.RemoteEndPoint);

                            clientSocket = connectedClients[client_ids[1]];

                            text = "You win.";
                            clientSocket.SendTo(Encoding.ASCII.GetBytes(text), clientSocket.RemoteEndPoint);
                            clear();
                        }
                    }
                    else
                    {
                        max_turns++;
                        if (max_turns == 9)
                        {
                            clientSocket = connectedClients[client_ids[0]];

                            string text = "Game tied.";
                            Console.WriteLine(text);
                            clientSocket.SendTo(Encoding.ASCII.GetBytes(text), clientSocket.RemoteEndPoint);

                            clientSocket = connectedClients[client_ids[1]];
                            clientSocket.SendTo(Encoding.ASCII.GetBytes(text), clientSocket.RemoteEndPoint);
                            clear();
                        }
                    }

                }

                if (client_ids[1].Contains(clientId)) //2nd client
                {
                    byte[] msg = new byte[1024];
                    Socket clientSocket = connectedClients[client_ids[1]];
                    clientSocket.Receive(msg);
                    string received_text = Encoding.ASCII.GetString(msg);

                    string[] break_text = received_text.Split(" ");

                    //append he mark in the array
                    int position = Convert.ToInt32(break_text[1]);
                    arr[position] = break_text[0];
                    Console.WriteLine(break_text[0] + " and " + break_text[1]+" from client 2");

                    clientSocket = connectedClients[client_ids[0]];
                    
                    //clientSocket.Send(buffer);
                    clientSocket.SendTo(Encoding.ASCII.GetBytes(received_text), clientSocket.RemoteEndPoint);

                    int check = CheckWin();
                    if (check != 0)
                    {
                        //means there is a win
                        //send to all

                        if (check == 1)
                        {
                            clientSocket = connectedClients[client_ids[0]];

                            string text = "You win.";
                            Console.WriteLine(text);
                            clientSocket.SendTo(Encoding.ASCII.GetBytes(text), clientSocket.RemoteEndPoint);

                            clientSocket = connectedClients[client_ids[1]];

                            text = "You loose.";
                            clientSocket.SendTo(Encoding.ASCII.GetBytes(text), clientSocket.RemoteEndPoint);
                            clear();
                        }

                        else
                        {
                            clientSocket = connectedClients[client_ids[0]];

                            string text = "You loose.";
                            Console.WriteLine(text);
                            clientSocket.SendTo(Encoding.ASCII.GetBytes(text), clientSocket.RemoteEndPoint);

                            clientSocket = connectedClients[client_ids[1]];

                            text = "You win.";
                            clientSocket.SendTo(Encoding.ASCII.GetBytes(text), clientSocket.RemoteEndPoint);
                            clear();
                        }
                    }
                    else
                    {
                        max_turns++; 
                        if (max_turns == 9)
                        {
                            clientSocket = connectedClients[client_ids[0]];

                            string text = "Game tied.";
                            Console.WriteLine(text);
                            clientSocket.SendTo(Encoding.ASCII.GetBytes(text), clientSocket.RemoteEndPoint);

                            clientSocket = connectedClients[client_ids[1]];
                            clientSocket.SendTo(Encoding.ASCII.GetBytes(text), clientSocket.RemoteEndPoint);
                            clear();
                        }
                    }
                }

            }
        }
    }
}
