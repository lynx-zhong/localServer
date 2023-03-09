using System;
using System.IO;
using Google.Protobuf;
using Frame;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using System.Threading.Tasks;

namespace NetworkFrame
{
    public static class NetworkServer
    {
        public static async Task SendMessage(NetworkMessageID messageID, IMessage message)
        {
            Debug.LogError("发送消息");

            HeadMessage headMessage = new HeadMessage();
            headMessage.MessageContent = message.ToByteString();
            headMessage.MessageID = (int)messageID;

            // 数据
            byte[] bytes = headMessage.ToByteArray();
            ByteArrayContent httpContent = new ByteArrayContent(bytes);

            // 创建 HttpClient 实例
            var client = new HttpClient();

            // 发送 POST 请求
            var response = await client.PostAsync("http://localhost:7800/", httpContent);

            // 检查是否成功
            if (response.IsSuccessStatusCode)
            {
                // 获取响应内容
                byte[] receiveBytesContent = await response.Content.ReadAsByteArrayAsync();
                HeadMessage receiveMessage = (HeadMessage)HeadMessage.Descriptor.Parser.ParseFrom(receiveBytesContent);

                RequestTestReq requestTestReq =(RequestTestReq)RequestTestReq.Descriptor.Parser.ParseFrom(receiveMessage.MessageContent);

                Debug.Log($"收到服务器消息，messageContent = {requestTestReq.Content}");
            }
            else
            {
                Debug.Log(string.Format("Failed with status code: {0}", response.StatusCode));
            }
        }
    }
}