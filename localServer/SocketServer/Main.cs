using System.Collections.Generic;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using Google.Protobuf;
using System.Net;
using System;
using Frame;

namespace SocketServer
{
    public partial class Main : Form
    {
        Socket serverSocket;
        public delegate void NetworkReceiveCallBack(byte[] buffer, string titleContent=null);
        Dictionary<NetworkMessageID, NetworkReceiveCallBack> receiveCallBack;

        public Main()
        {
            InitializeComponent();
            GetLocalPC_IP();
            BindNetworkEvent();
        }

        public void BindNetworkEvent() 
        {
            receiveCallBack = new Dictionary<NetworkMessageID, NetworkReceiveCallBack>();
            receiveCallBack.Add(NetworkMessageID.Zero,NetworkZeonCallBack);
            receiveCallBack.Add(NetworkMessageID.RequestTest,NetworkRequestCallBack);
        }

        private void launchServer_Click(object sender, EventArgs e)
        {
            if (serverSocket != null)
                return;

            // 新建一个socket
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) ;

            // 绑定一个ip 端口
            IPAddress iPAddress = IPAddress.Parse(iPText.Text);
            int port = int.Parse(portText.Text);
            EndPoint endPoint = new IPEndPoint(iPAddress, port);
            serverSocket.Bind(endPoint);

            // 监听客户端socket的最大数量
            serverSocket.Listen(100);

            // 开新线程开始监听客户端的请求 .Accept() 会阻碍线程，所以开个新线程
            Thread threadService = new Thread(AcceptClientConnect);
            threadService.IsBackground = true;
            threadService.Start(serverSocket);

            ShowLog("服务器已启动......");
        }

        Socket clientSocket;
        void AcceptClientConnect(Object obj) 
        {
            Socket _serverSocket = obj as Socket;
            while (true)
            {
                // 接受客户端的链接
                clientSocket = _serverSocket.Accept();

                // 给客户端回复 ！！ 这步很重要 并且顺序不能错
                HeadMessage headMessage = new HeadMessage();
                headMessage.MessageID = (int)NetworkMessageID.Zero;
                DispenseReceiveMessage(headMessage);

                //给通信创建新的线程去执行
                Thread th = new Thread(Receive);
                th.IsBackground = true;
                th.Start(clientSocket);
            }
        }

        private void Receive(object obj)
        {
            Socket _clientSocket = obj as Socket;
            while (true)
            {
                try
                {
                    //客户端连接成功后，服务器应该接收客户端发来的消息
                    byte[] buffer = new byte[1024 * 1024 * 5];
                    //实际接收到的有效字节数
                    int lenth = _clientSocket.Receive(buffer);
                    if (lenth == 0) 
                        break;

                    HeadMessage receiveMessage = HeadMessage.Parser.ParseFrom(buffer,0, lenth);
                    DispenseReceiveMessage(receiveMessage);
                }
                catch { }
            }
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
                receiveCallBack[networkMessageID](headMessage.MessageContent.ToByteArray(),headMessage.Title);
            }
        }

        public void NetworkZeonCallBack(byte[] buffer,string content) 
        {
            ShowLog("收到客户端连接请求...." + content);

            HeadMessage headMessage = new HeadMessage();
            headMessage.MessageID = (int)NetworkMessageID.Zero;
            headMessage.Title = "收到客户端连接，连接成功";
            byte[] vs = headMessage.ToByteArray();
            clientSocket.Send(vs);
        }

        private void NetworkRequestCallBack(byte[] buffer, string content)
        {
            RequestTestReq requestTestReq = RequestTestReq.Parser.ParseFrom(buffer);
            ShowLog("收到协议测试数据：  " + requestTestReq.Content);

            RequestTestRsp requestTestRsp = new RequestTestRsp();
            requestTestRsp.Content = "服务器回复： " + requestTestReq.Content;

            HeadMessage headMessage = new HeadMessage();
            headMessage.MessageContent = requestTestRsp.ToByteString();
            headMessage.MessageID = (int)NetworkMessageID.RequestTest;

            clientSocket.Send(headMessage.ToByteArray());
        }

        void ShowLog(string message) 
        {
            Action action = () =>
            {
                content.Text = string.Format($"{content.Text}\r\n{message}");
            };

            Invoke(action);
        }

        /// <summary>
        /// 获取本机ip
        /// </summary>
        private void GetLocalPC_IP()
        {
            string hostName = Dns.GetHostName();//本机名称
            IPAddress[] ipList = Dns.GetHostAddresses(hostName);//本机ip（包括ipv4和ipv6）
            foreach (IPAddress ip in ipList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    iPText.Text = ip.ToString().Trim();
                }
            }
        }
    }
}
