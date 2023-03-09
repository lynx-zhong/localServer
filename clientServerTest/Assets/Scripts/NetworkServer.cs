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
            Debug.LogError("������Ϣ");

            HeadMessage headMessage = new HeadMessage();
            headMessage.MessageContent = message.ToByteString();
            headMessage.MessageID = (int)messageID;

            // ����
            byte[] bytes = headMessage.ToByteArray();
            ByteArrayContent httpContent = new ByteArrayContent(bytes);

            // ���� HttpClient ʵ��
            var client = new HttpClient();

            // ���� POST ����
            var response = await client.PostAsync("http://localhost:7800/", httpContent);

            // ����Ƿ�ɹ�
            if (response.IsSuccessStatusCode)
            {
                // ��ȡ��Ӧ����
                byte[] receiveBytesContent = await response.Content.ReadAsByteArrayAsync();
                HeadMessage receiveMessage = (HeadMessage)HeadMessage.Descriptor.Parser.ParseFrom(receiveBytesContent);

                RequestTestReq requestTestReq =(RequestTestReq)RequestTestReq.Descriptor.Parser.ParseFrom(receiveMessage.MessageContent);

                Debug.Log($"�յ���������Ϣ��messageContent = {requestTestReq.Content}");
            }
            else
            {
                Debug.Log(string.Format("Failed with status code: {0}", response.StatusCode));
            }
        }
    }
}