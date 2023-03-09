using System;
using System.IO;
using Google.Protobuf;
using Frame;
using System.Net;
using System.Text;

namespace localServer
{
    public class NetworkServer
    {
        static HttpListener listener = new HttpListener();

        // 消息接收定义
        delegate void NetworkMessageCallBack(byte[] bytes);
        static Dictionary<NetworkMessageID, NetworkMessageCallBack> messageCallBack = new Dictionary<NetworkMessageID, NetworkMessageCallBack>()
        {
            {NetworkMessageID.RequestTest, NetworkMessageTestReceive},
        };

        public static void ReceiveMessage(HttpListener _listener) 
        {
            Console.WriteLine("-------------->   接收客户端请求");

            listener = _listener;
            HttpListenerContext context = listener.GetContext(); // 获取请求上下文
            HttpListenerRequest request = context.Request; // 获取请求对象

            // 从客户端请求中读取数据
            byte[] buffer;
            using (Stream body = request.InputStream)
            {
                buffer = new byte[request.ContentLength64];
                int bytesRead = 0;
                while (bytesRead < request.ContentLength64)
                {
                    int bytesRemaining = (int)request.ContentLength64 - bytesRead;
                    int bytesToRead = Math.Min(bytesRemaining, buffer.Length);
                    int n = body.Read(buffer, bytesRead, bytesToRead);
                    if (n == 0) break;
                    bytesRead += n;
                }
                //将二进制数据转换为字符串，并输出到控制台
            }

            HeadMessage headMessage =(HeadMessage)HeadMessage.Descriptor.Parser.ParseFrom(buffer);
            if (messageCallBack.ContainsKey((NetworkMessageID)headMessage.MessageID))
            {
                NetworkMessageID networkMessageID = (NetworkMessageID)headMessage.MessageID;
                messageCallBack[networkMessageID](headMessage.MessageContent.ToByteArray());
            }
        }

        public static void SendMessage(NetworkMessageID messageID, IMessage message)
        {
            HeadMessage headMessage = new HeadMessage();
            headMessage.MessageContent = message.ToByteString();
            headMessage.MessageID = (int)messageID;

            HttpListenerContext context = listener.GetContext();


            byte[] bytesContent = headMessage.ToByteArray();
            var response = context.Response; // 获取响应对象
            response.ContentLength64 = bytesContent.Length;
            response.OutputStream.Write(bytesContent, 0, bytesContent.Length);

            // 关闭响应
            response.OutputStream.Close();
        }

        /// <summary>
        /// 测试函数 接收到客户端的消息
        /// </summary>
        static void NetworkMessageTestReceive(byte[] bytes)
        {
            Console.WriteLine("-------------->   回复客户端");

            RequestTestReq requestTestReq = (RequestTestReq)RequestTestReq.Descriptor.Parser.ParseFrom(bytes);

            RequestTestRsp requestTestRsp = new RequestTestRsp();
            requestTestRsp.Content = "回复客户端:   " + requestTestReq.Content;

            SendMessage(NetworkMessageID.RequestTest, requestTestRsp);
        }
    }
}