using UnityEngine;
using NetworkFrame;
using Frame;
using Google.Protobuf;

public class GameLaunch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            SendTestMessage();
        }
    }

    void SendTestMessage() 
    {
        RequestTestReq requestTestReq = new RequestTestReq();
        requestTestReq.Content = Time.realtimeSinceStartup.ToString();
        NetworkServer.SendMessage(NetworkMessageID.RequestTest, requestTestReq);
    }
}
