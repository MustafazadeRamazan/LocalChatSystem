using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace server
{
    class Program
    {
        static List<Socket> sockets = new();
        static void Main(string[] args)
        {
            using var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            var ipAddress = IPAddress.Parse("10.2.25.33");
            var port = 27001;

            var endPoint = new IPEndPoint(ipAddress, port);
            listener.Bind(endPoint);

            var backLog = 10;

            listener.Listen(backLog);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Listener: {listener.LocalEndPoint}");

            while (true)
            {
                var client = listener.Accept();
                sockets.Add(client);
                Console.WriteLine($"Client: {client.RemoteEndPoint}");


                Task.Run(() =>
                {
                    var consoleColor = (ConsoleColor)((client.RemoteEndPoint as IPEndPoint).Port % 15);
                    var bytes = new byte[1024];
                    var msg = "";
                    int len = 0;

                    while (true)
                    {
                        bytes = new byte[1024];
                        len = 0;
                        msg = "";

                        len = client.Receive(bytes);
                        msg = Encoding.Default.GetString(bytes, 0, len);
                        Console.ForegroundColor = consoleColor;

                        msg = $"{client.RemoteEndPoint}: {msg}";
                        Console.WriteLine(msg);

                        foreach (var item in sockets)
                        {
                            if (client != item) item.Send(bytes);
                        }

                    }
                });

            }

        }
    }
}