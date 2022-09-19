using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatList;

internal static class Program
{
    private static void Main(string[] args)
    {
        var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        var ip = IPAddress.Parse("10.2.25.33");
        var port = 27001;

        var ep = new IPEndPoint(ip, port);

        Console.ForegroundColor = ConsoleColor.Blue;

        try
        {
            client.Connect(ep);

            if (client.Connected)
            {
                Console.WriteLine("Connected to server");

                byte[] bytes;
                string msg;
                ushort length;

                while (true)
                {
                    bytes = new byte[client.ReceiveBufferSize];
                    msg = string.Empty;
                    length = default;

                    length = (ushort)client.Receive(bytes);
                    msg = Encoding.Default.GetString(bytes, default, length);

                    Console.WriteLine(msg);
                }
            }
            else Console.WriteLine("Cannot connect.");

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}