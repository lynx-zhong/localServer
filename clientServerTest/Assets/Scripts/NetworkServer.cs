using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using Google.Protobuf;
using Frame;
using System.Collections.Generic;

namespace NetworkFrame
{
    public class NetworkServer
    {
        #region 常量
        static string serverIp = "192.168.3.12";
        static int serverPort = 9131;
        static Socket tcpSocket;
        #endregion

        public delegate void NetworkCallBack(byte[] vs);
        static Dictionary<NetworkMessageID, NetworkCallBack> networkCallBackDic = new Dictionary<NetworkMessageID, NetworkCallBack>();


        public static void ConnectServer() 
        {
            Debug.Log("请求连接");

            // 新建一个socket
            tcpSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);

            // 绑定ip 端口
            IPAddress iPAddress = IPAddress.Parse(serverIp);
            EndPoint endPoint = new IPEndPoint(iPAddress,serverPort);

            // 连接服务器
            tcpSocket.Connect(endPoint);

            // 接受服务器的消息
            byte[] buffer = new byte[1024];
            int messageLength = tcpSocket.Receive(buffer);

            // 接受
            ReceiveMessage(buffer);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        public static void SendMessage(NetworkMessageID messageID,IMessage message) 
        {
            HeadMessage headMessage = new HeadMessage();
            headMessage.MessageID = (int)messageID;
            headMessage.MessageContent = message.ToByteString();

            tcpSocket.Send(headMessage.ToByteArray());

            Debug.Log("发送消息  ");
        }

        /// <summary>
        /// 接受消息 消息分发
        /// </summary>
        static void ReceiveMessage(byte[] buffer) 
        {
            HeadMessage headMessage = (HeadMessage)HeadMessage.Descriptor.Parser.ParseFrom(buffer);
            NetworkMessageID messageID = (NetworkMessageID)headMessage.MessageID;
            if (networkCallBackDic.ContainsKey(messageID))
            {
                networkCallBackDic[messageID](headMessage.MessageContent.ToByteArray());
            }
        }

        /// <summary>
        /// 消息绑定
        /// </summary>
        public static void BindNetworkMessage(NetworkMessageID messageID, NetworkCallBack callBack) 
        {
            networkCallBackDic.Add(messageID, callBack);
        }
    }
}