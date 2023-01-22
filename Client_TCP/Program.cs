using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client_TCP
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
            const string IP_Server = "127.0.0.1";
            const int PORT = 8080;
            IPAddress iPAddress = IPAddress.Parse(IP_Server);
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, PORT);
            Socket socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(iPEndPoint);
                if(socket.Connected)
                {
                    string SendToServer = $"{DateTime.Now}, Привіт сервер 1!";
                    ArraySegment<byte> mess = Encoding.Unicode.GetBytes(SendToServer);
                    Task <int> task =socket.SendAsync(mess,SocketFlags.None);
                    ArraySegment<byte> buffer =new  byte[1024];
                    int l=0;
                    do
                    {
                      Task<int> task1= socket.ReceiveAsync(buffer,SocketFlags.None);
                        l=await task1;
                      Console.WriteLine(System.Text.Encoding.Unicode.GetString(buffer.Array, 0,l));
                    } while (l > 1);
                    int tmp = await task;
                }
            }catch(Exception ex) { 
                Console.WriteLine(ex.ToString());
            }
            finally{

                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }
    }
}