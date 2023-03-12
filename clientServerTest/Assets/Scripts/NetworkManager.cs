using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Google.Protobuf;
using System.Text;
using UnityEngine;
using System.Net;
using System;
using Frame;

public class NetworkManager 
{
    #region 单例
    private NetworkManager(){}
    private static NetworkManager instance;
    public static NetworkManager Instance 
    {
        get 
        {
            if (instance == null)
                instance = new NetworkManager();;

            return instance;
        }
    }
    #endregion

    public string serverIPString
    { 
        get {
            return GetLocalPC_IP();
        } 
    }
    public int serverPort = 8700;

    Socket socket;
    public delegate void NetworkReceiveCallBack(byte[] buffer, string titleContent = null);
    Dictionary<NetworkMessageID, NetworkReceiveCallBack> receiveCallBack;

    public void BindNetworkEvent()
    {
        receiveCallBack = new Dictionary<NetworkMessageID, NetworkReceiveCallBack>();
        receiveCallBack.Add(NetworkMessageID.Zero, NetworkZeonCallBack);
        receiveCallBack.Add(NetworkMessageID.RequestTest, NetworkRequestCallBack);
    }

    public void ConnectServer() 
    {
        // 创建
        if (socket == null)
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        if (socket.Connected)
            return;

        // 连接服务器
        Thread thread = new Thread(new ThreadStart(Connect));
        thread.IsBackground = true;
        thread.Start();
    }

    void Connect() 
    {
        // 绑定 server ip port
        IPAddress iPAddress = IPAddress.Parse(serverIPString);
        EndPoint ep = new IPEndPoint(iPAddress, serverPort);

        // 连接服务器
        Debug.Log("开始连接服务器.....");
        socket.Connect(ep);

        // 连接成功 接受数据
        Debug.Log("连接成功");
        Thread thread = new Thread(new ThreadStart(Receive));
        thread.IsBackground = true;
        thread.Start();
    }

    void Receive() 
    {
        while (true)
        {
            try
            {
                byte[] buffer = new byte[1024 * 1024];
                int lenth = socket.Receive(buffer);
                if (lenth == 0)
                    break;

                HeadMessage receiveMessage = HeadMessage.Parser.ParseFrom(buffer,0, lenth);
                DispenseReceiveMessage(receiveMessage);
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }

    public void SendMessage()
    {
        if (socket == null)
            return;

        RequestTestReq requestTestReq = new RequestTestReq();
        requestTestReq.Content = string.Format($"你好服务器，现在游戏时间长： {Time.realtimeSinceStartup}");

        HeadMessage headMessage = new HeadMessage();
        headMessage.MessageID = (int)NetworkMessageID.RequestTest;
        headMessage.MessageContent = requestTestReq.ToByteString();

        socket.Send(headMessage.ToByteArray());
    }

    /// <summary>
    /// 分发接收到的协议数据
    /// </summary>
    /// <param name="headMessage"></param>
    void DispenseReceiveMessage(HeadMessage headMessage)
    {
        NetworkMessageID networkMessageID = (NetworkMessageID)headMessage.MessageID;
        if (receiveCallBack.ContainsKey(networkMessageID))
        {
            receiveCallBack[networkMessageID](headMessage.MessageContent.ToByteArray(), headMessage.Title);
        }
    }

    public void NetworkZeonCallBack(byte[] buffer, string content)
    {
        Debug.Log(content);
    }

    private void NetworkRequestCallBack(byte[] buffer, string content)
    {
        RequestTestRsp requestTestRsp = RequestTestRsp.Parser.ParseFrom(buffer);

        Debug.Log(requestTestRsp.Content);
    }

    /// <summary>
    /// 获取本机ip
    /// </summary>
    string GetLocalPC_IP()
    {
        string hostName = Dns.GetHostName();//本机名称
        IPAddress[] ipList = Dns.GetHostAddresses(hostName);//本机ip（包括ipv4和ipv6）
        foreach (IPAddress ip in ipList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString().Trim();
            }
        }

        return null;
    }
}
