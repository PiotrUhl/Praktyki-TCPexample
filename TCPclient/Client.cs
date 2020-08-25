using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;

namespace TCPclient {
    class Client {
        public static void Main() {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            UInt16 port = 50000;
            TcpClient tcpClient = null;
            try {
                while (true)
                {
                    Console.Write("Enter data or \'exit\': ");
                    String outMessage = Console.ReadLine();
                    if (outMessage.Equals("exit"))
                        break;

                    Console.WriteLine("Connecting...");
                    tcpClient = new TcpClient();
                    tcpClient.Connect(ip, port);
                    Console.WriteLine("Connected");

                    Stream stream = tcpClient.GetStream(); //get stream from connection

                    byte[] outData = System.Text.Encoding.ASCII.GetBytes(outMessage); //convert string to raw data
                    stream.Write(outData, 0, outData.Length); //write data to stream
                    Console.WriteLine("Sent message: \"" + outMessage + '\"');

                    byte[] inData = new byte[256];
                    int bytes = stream.Read(inData, 0, 256); //read incomming data from stream
                    String inMessage = System.Text.Encoding.ASCII.GetString(inData, 0, bytes);
                    Console.WriteLine("Recieved message: \"" + inMessage + '\"');

                    stream.Close();
                    tcpClient.Close();
                    tcpClient = null;
                }
            }
            catch (Exception e) {
                Console.WriteLine("Error!: " + e.StackTrace);
            }
            finally {
                if (tcpClient != null && tcpClient.Connected)
                    tcpClient.Close();
            }
        }
    }
}
