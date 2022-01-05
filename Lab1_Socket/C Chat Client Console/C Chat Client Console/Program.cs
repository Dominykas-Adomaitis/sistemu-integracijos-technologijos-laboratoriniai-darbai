using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace C_Chat_Client_Console
{
    class Program
    {
        public static System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();  // client socket
        public static NetworkStream serverStream = default(NetworkStream); // server stream
        public static string readData = null;
        static void Main(string[] args)
        {
            readData = "Conected to Chat Server ...";
            // writes to console readData string
            msg();
            Console.Write("Iveskite savo varda: ");

            clientSocket.Connect("127.0.0.1", 8888); // connect to server running on 127.0.0.1 at port no. 8888
            serverStream = clientSocket.GetStream(); // get stream

            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(Console.ReadLine() + "$"); // reads client name from console
            //Console.WriteLine(Encoding.ASCII.GetString(outStream));
            serverStream.Write(outStream, 0, outStream.Length); // write to stream

            serverStream.Flush();

            Thread ctThread = new Thread(getMessage);   // start thread for client
            ctThread.Start();

            while (true)
            {
                outStream = System.Text.Encoding.ASCII.GetBytes(Console.ReadLine() + "$");  // reads client message from console
                serverStream.Write(outStream, 0, outStream.Length); // write to stream
                //Console.WriteLine(Encoding.ASCII.GetString(outStream));
                serverStream.Flush();
            }
                
        }

        public static void getMessage()
        {
            while (true)
            {
                // access stream
                serverStream = clientSocket.GetStream(); 
                // create byte array to receive data  
                byte[] inStream = new byte[100];
                // read data from stream
                serverStream.Read(inStream, 0, inStream.Length);  
                //Console.WriteLine(inStream.Length);
                // converts byte array to string
                string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                readData = "" + returndata;
                // print out client message
                msg();
            }
        }

        public static void msg()
        {
            Console.WriteLine(" >> " + readData);
        }
    }
}
