using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            #region RamoServer
            var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ip = IPAddress.Parse("10.2.25.33");
            var port = 27001;
            var ep = new IPEndPoint(ip, port);

            try
            {
                client.Connect(ep);
                if (client.Connected)
                {
                    Console.WriteLine("Connected to server");

                    Task.Run(() =>
                    {
                        var consoleColor = (ConsoleColor)((client.RemoteEndPoint as IPEndPoint).Port % 15);
                        var bytes = new byte[1024];
                        var len = 0;
                        var msg = "";
                        while (true)
                        {
                            len = client.Receive(bytes);
                            msg = Encoding.Default.GetString(bytes, 0, len);
                            Thread.Sleep(3000);
                            Console.ForegroundColor = consoleColor;
                            Console.WriteLine("\n"+msg);
                            Console.Beep(1000, 100);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        }

                    });

                    Console.Write("Enter your name: ");
                    string name = Console.ReadLine();
                    Random r = new Random();
                    int rInt = r.Next(0, 100);
                    if (name == "" || name.EndsWith(" ") || name.StartsWith(" "))
                    {
                        name = $"Anonim {rInt}";
                    }
                    Console.Clear();
                    bool qosuldu = true;

                    if (qosuldu == true)
                    {
                        var msgconnected = $"\n-------------------------------------------{name} Connected-------------------------------------------";
                        Console.Beep(500, 100);
                        var bytes = Encoding.Default.GetBytes(msgconnected);
                        client.Send(bytes);
                        qosuldu = false;
                    }

                    while (true)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        var msg = $"- {name}: ";
                        var msg2 = Console.ReadLine();

                        if (msg2 != "" && !msg2.EndsWith(" ") && !msg2.StartsWith(" "))
                        {
                            Console.Beep(500, 100);
                            var bytes = Encoding.Default.GetBytes(msg + msg2);
                            client.Send(bytes);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Cannot connect.");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
        }
    }
}