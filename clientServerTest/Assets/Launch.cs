using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Launch : MonoBehaviour
{
    public void OnConnectedToServer()
    {
        NetworkManager.Instance.BindNetworkEvent();
        NetworkManager.Instance.ConnectServer();
    }

    public void SendMessage() 
    {
        NetworkManager.Instance.SendMessage();
    }
}
