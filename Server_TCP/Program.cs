using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace Server_TCP
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding= Encoding.Unicode;
            const string IP_Server = "127.0.0.1";
            const int PORT = 8080;
            Socket socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            IPAddress iPAddress = IPAddress.Parse(IP_Server);
            IPEndPoint iPEndPoint= new IPEndPoint(IPAddress.Any, PORT);
            socket.Bind(iPEndPoint);
            socket.Listen(15);
            try
            {
                while (true)
                {
                    int l = 0;
                    ArraySegment<byte>buffer = new byte[1024];
                    Task<Socket> s = socket.AcceptAsync();
                    Socket ns = await s;
                    ArraySegment<byte> mess = Encoding.Unicode.GetBytes($"{DateTime.Now}: {ns.RemoteEndPoint.ToString()}: Привіт клієнт!");
                    Task<int> task =ns.SendAsync(mess,SocketFlags.None);
                    int tmp = await task;
                    Task<int> task2 = ns.ReceiveAsync(buffer,SocketFlags.None);
                    l= await task2;
                    Console.WriteLine(Encoding.Unicode.GetString(buffer.Array, 0, l));
                    Console.ReadKey();
                    ns.Shutdown(SocketShutdown.Both);
                    ns.Close();
                }
                
            }catch(SocketException ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }
        }
    }
}