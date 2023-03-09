using UnityEngine;
using NetworkFrame;
using Frame;
using Google.Protobuf;

public class GameLaunch : MonoBehaviour
{
    void Start()
    {
        NetworkServer.BindNetworkMessage(NetworkMessageID.RequestTest, BindTestCallBack);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            NetworkServer.ConnectServer();
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            RequestTestReq requestTestReq = new RequestTestReq();
            requestTestReq.Content = "你好服务器，第一条消息测试";

            NetworkServer.SendMessage(NetworkMessageID.RequestTest, requestTestReq);
        }
    }

    void BindTestCallBack(byte[] bytes) 
    {
        RequestTestRsp requestTestRsp = (RequestTestRsp)RequestTestRsp.Parser.ParseFrom(bytes);
    }
}
