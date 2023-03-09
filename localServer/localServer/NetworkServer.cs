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