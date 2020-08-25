using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace TCPserver {
    class Server {
        public static void Main() {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            UInt16 port = 50000;
            TcpListener listener = null;
            Socket socket = null;
            try {
                listener = new TcpListener(ip, port);
                listener.Start();
                Console.WriteLine("Listening at " + listener.LocalEndpoint);
                while (true) {
                    socket = listener.AcceptSocket(); //accept connection
                    Console.WriteLine("Connection accepted from " + socket.RemoteEndPoint);

                    byte[] data = new byte[256];
                    int bytes = socket.Receive(data); //get data
                    String message = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    Console.WriteLine("Recieved message: \"" + message + '\"');
                    String newMessage = String.Empty;
                    foreach (var k in message) { //encode data with Caesar cipher
						ushort c = Convert.ToUInt16(k);
                        c += 5;
                        if (c > 126)
                            c -= 95;
                        newMessage += Convert.ToChar(c);
					}
                    socket.Send(System.Text.Encoding.ASCII.GetBytes(newMessage)); //send encoded data
                    Console.WriteLine("Sent message:     \"" + newMessage + "\"\n");

                    socket.Close(); //close connection
                    socket = null;
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
                if (socket != null)
                    socket.Close();
                if (listener != null)
                    listener.Stop();
            }
        }
    }
}
