using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Linq;

namespace ConsoleApplication1
{
    class Program
    {
        public static Hashtable clientsList = new Hashtable();

        static void Main(string[] args)
        {
            TcpListener serverSocket = new TcpListener(8888); // listens on port number 8888
            TcpClient clientSocket = default(TcpClient);
            string dataFromClient = null;
            serverSocket.Start();  // start listener
            Console.WriteLine("Chat Server Started ....");
            while ((true))
            {
                // wait for client to make request
                clientSocket = serverSocket.AcceptTcpClient();
                // create byte array to receive data 
                byte[] bytesFrom = new byte[clientSocket.ReceiveBufferSize];
                dataFromClient = null;

                // access stream to get data from client
                NetworkStream networkStream = clientSocket.GetStream();
                // reads client name into byte array
                networkStream.Read(bytesFrom, 0, bytesFrom.Length);
                // converts byte array to string
                dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                // takes out needed string part
                dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

                // adds client to client list
                clientsList.Add(dataFromClient, clientSocket);
                // writes in client console that client has joined the chat
                broadcast(dataFromClient + " Joined ", dataFromClient, false);
                // writes in server console that client has joined the chat
                Console.WriteLine(dataFromClient + " Joined chat room ");
                // handleclient
                handleClient client = new handleClient();
                client.startClient(clientSocket, dataFromClient, clientsList);

            }

            clientsList.Remove(dataFromClient);
            // closes client socket
            clientSocket.Close();
            //serverSocket.Stop();
            Console.WriteLine("exit");
            //Console.ReadLine();
        }

        public static void broadcast(string msg, string uName, bool flag)
        {
            foreach (DictionaryEntry Item in clientsList)
            {
                TcpClient broadcastSocket;
                broadcastSocket = (TcpClient)Item.Value;
                NetworkStream broadcastStream = broadcastSocket.GetStream();
                Byte[] broadcastBytes = null;
                // creates broadcasting msg with user name says:
                if (!String.Equals(Item.Key, uName))
                {
                    if (flag == true)
                    {
                        broadcastBytes = Encoding.ASCII.GetBytes(uName + " says : " + msg);
                    }
                    // creates broadcasting msg with no additional info
                    else
                    {
                        broadcastBytes = Encoding.ASCII.GetBytes(msg);
                    }
                    // writes to client console created message
                    broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                    //Console.WriteLine(broadcastBytes.Length);
                    broadcastStream.Flush();
                }
            }
        }  //end broadcast function
    }//end Main class


    public class handleClient
    {
        TcpClient clientSocket;
        string clNo;
        Hashtable clientsList;

        public void startClient(TcpClient inClientSocket, string clineNo, Hashtable cList)
        {
            this.clientSocket = inClientSocket; // client socket 
            this.clNo = clineNo; // client name
            this.clientsList = cList; // client list
            Thread ctThread = new Thread(doChat); // starts client chat thread
            ctThread.Start();
        }

        private void doChat()
        {
            // create byte array to receive data 
            byte[] bytesFrom = new byte[clientSocket.ReceiveBufferSize];
            string dataFromClient = null;
            while ((true))
            {
                try
                {
                    // access stream to get data from client
                    NetworkStream networkStream = clientSocket.GetStream();
                    // reads client message into byte array
                    networkStream.Read(bytesFrom, 0, bytesFrom.Length);
                    // converts client message byte array to string
                    dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                    // in server console writes client message
                    Console.WriteLine("From client - " + clNo + " : " + dataFromClient);
                    // in clients console writes client message
                    Program.broadcast(dataFromClient, clNo, true); //Isiuncia zinute i kliento konsoles rasymo metoda
                }
                catch (Exception ex)
                {
                    clientsList.Remove(clNo);
                    Console.WriteLine(clNo + " atsijunge");
                    Program.broadcast(clNo + " atsijunge", dataFromClient, false);
                    //Console.WriteLine(ex.ToString());
                    break;
                }
            }
            clientsList.Remove(clNo); //Pasalina klienta is saraso
            clientSocket.Client.Shutdown(SocketShutdown.Both);
        }//end doChat
    } //end class handleClinet
}//end namespace