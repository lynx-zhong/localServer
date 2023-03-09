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
        #region ����
        static string serverIp = "192.168.3.12";
        static int serverPort = 9131;
        static Socket tcpSocket;
        #endregion

        public delegate void NetworkCallBack(byte[] vs);
        static Dictionary<NetworkMessageID, NetworkCallBack> networkCallBackDic = new Dictionary<NetworkMessageID, NetworkCallBack>();


        public static void ConnectServer() 
        {
            Debug.Log("��������");

            // �½�һ��socket
            tcpSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);

            // ��ip �˿�
            IPAddress iPAddress = IPAddress.Parse(serverIp);
            EndPoint endPoint = new IPEndPoint(iPAddress,serverPort);

            // ���ӷ�����
            tcpSocket.Connect(endPoint);

            // ���ܷ���������Ϣ
            byte[] buffer = new byte[1024];
            int messageLength = tcpSocket.Receive(buffer);

            // ����
            ReceiveMessage(buffer);
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public static void SendMessage(NetworkMessageID messageID,IMessage message) 
        {
            HeadMessage headMessage = new HeadMessage();
            headMessage.MessageID = (int)messageID;
            headMessage.MessageContent = message.ToByteString();

            tcpSocket.Send(headMessage.ToByteArray());

            Debug.Log("������Ϣ  ");
        }

        /// <summary>
        /// ������Ϣ ��Ϣ�ַ�
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
        /// ��Ϣ��
        /// </summary>
        public static void BindNetworkMessage(NetworkMessageID messageID, NetworkCallBack callBack) 
        {
            networkCallBackDic.Add(messageID, callBack);
        }
    }
}