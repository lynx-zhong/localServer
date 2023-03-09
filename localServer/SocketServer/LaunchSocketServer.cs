using System.Net.Sockets;
using System;
using System.Net;
using System.Text;
using Google.Protobuf;
using Frame;

namespace localServer
{
    internal class LaunchLocalServer
    {
        // 新建socket 定义传输的内容 与 协议方式
        static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        static void Main(string[] args)
        {
            Launch();
        }

        static void Launch() 
        {
            // 新建一个全局socket


            // 绑定一个IP 和 端口，cmd ipconfig 查看本机ip
            IPAddress iP = IPAddress.Parse("192.168.3.12");
            EndPoint ep = new IPEndPoint(iP, 9131);
            socket.Bind(ep);

            // 设置最大的允许连接数
            socket.Listen(100);
            Console.WriteLine("服务器启动完成, 等待客户端连接");

            // 阻塞进程 等待客户端连接
            Socket clientSocket = socket.Accept();
            Console.WriteLine("客户端连接成功");

            // 发消息给客户端
            RequestTestRsp requestTestRsp = new RequestTestRsp();
            requestTestRsp.Content = "Hello ";

            HeadMessage headMessage = new HeadMessage();
            headMessage.MessageID = (int)NetworkMessageID.Zero;
            headMessage.MessageContent = requestTestRsp.ToByteString();
            clientSocket.Send(headMessage.ToByteArray());
            Console.WriteLine("发消息给客户端");

            // 收到客户端消息才会往下走
            byte[] clientMessageBytes = new byte[1024];
            int messageLength = clientSocket.Receive(clientMessageBytes);       // 读取客户端消息
            ReceviceMessage(clientMessageBytes);
        }

        static void ReceviceMessage(byte[] clientMessageBytes) 
        {
            HeadMessage headMessage = (HeadMessage)HeadMessage.Parser.ParseFrom(clientMessageBytes);
            Console.WriteLine($"收到客户端的消息, messageID：   {headMessage.MessageID}");

            if (headMessage.MessageID == 1)
            {
                SendTestMessage(headMessage.MessageContent);
            }
        }

        static void SendTestMessage(ByteString byteStr) 
        {
            byte[] clientMessageBytes = byteStr.ToByteArray();
            RequestTestReq requestTestReq = (RequestTestReq)RequestTestReq.Descriptor.Parser.ParseFrom(clientMessageBytes);

            RequestTestRsp requestTestRsp = new RequestTestRsp();
            requestTestRsp.Content = $"我收到了消息，直接恢复：   {requestTestReq.Content}";

            HeadMessage headMessage = new HeadMessage();
            headMessage.MessageID = (int)NetworkMessageID.RequestTest;
            headMessage.MessageContent = requestTestRsp.ToByteString();
            socket.Send(headMessage.ToByteArray());
        }
    }
}