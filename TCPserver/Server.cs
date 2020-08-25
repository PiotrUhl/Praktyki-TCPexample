using System;
using System.Net;
using System.Net.Sockets;

namespace TCPserver {
    class Server {
        private static String encodeString(String str) {
            String ret = String.Empty;
            foreach (var k in str) { //encode data with Caesar cipher
                ushort c = Convert.ToUInt16(k);
                c += 5;
                if (c > 126)
                    c -= 95;
                ret += Convert.ToChar(c);
            }
            return ret;
        }
        public static void Main() {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            UInt16 port = 50000;
            TcpListener listener = null;
            TcpClient client = null;
            try {
                listener = new TcpListener(ip, port);
                listener.Start();
                Console.WriteLine("Listening at " + listener.LocalEndpoint);
                while (true) {
                    client = listener.AcceptTcpClient(); //accept connection
                    Console.WriteLine("Connection accepted from " + client.Client.RemoteEndPoint);

                    NetworkStream stream = client.GetStream(); //gets stream from connection

                    byte[] data = new byte[256];
                    int bytes = stream.Read(data, 0, 256); //get data
                    String message = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    Console.WriteLine("Recieved message: \"" + message + '\"');
                    String newMessage = encodeString(message);
                    stream.Write(System.Text.Encoding.ASCII.GetBytes(newMessage)); //send encoded data
                    Console.WriteLine("Sent message:     \"" + newMessage + "\"\n");

                    client.Close(); //close connection
                    client = null;
                }
            }
            catch (SocketException e) {
                Console.WriteLine("Error: " + e.Message);
            }
            catch (ObjectDisposedException e) {
                Console.WriteLine("Error: " + e.Message);
            }
            catch (InvalidOperationException e) {
                Console.WriteLine("Error: " + e.Message);
            }
            catch (Exception e) {
                Console.WriteLine("Error!: " + e.StackTrace);
            }
            finally {
                if (client != null)
                    client.Close();
                if (listener != null)
                    listener.Stop();
            }
        }
    }
}
