using Frame;
using Google.Protobuf;
using System.Net;
using System.Text;

namespace localServer
{
    internal class LaunchLocalServer
    {
        static void Main(string[] args)
        {
            // 创建 HttpListener 实例
            var listener = new HttpListener();

            // 添加要监听的 URI
            listener.Prefixes.Add("http://localhost:7800/");

            // 开始监听请求
            listener.Start();

            Console.WriteLine("Listening for requests...");

            // 处理请求
            while (true)
            {
                NetworkServer.ReceiveMessage(listener);
            }
        }
    }
}