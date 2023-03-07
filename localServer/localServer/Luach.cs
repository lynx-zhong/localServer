using System.Diagnostics;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace localServer
{
    internal class Luach
    {
        static void Main(string[] args)
        {
            // 创建 HttpListener 实例
            var listener = new HttpListener();

            // 添加要监听的 URI
            listener.Prefixes.Add("http://localhost:7800/");

            // 开始监听请求
            listener.Start();

            Console.WriteLine("Listening for requests...");

            // 处理请求
            while (true)
            {
                HttpListenerContext context = listener.GetContext(); // 获取请求上下文
                HttpListenerRequest request = context.Request; // 获取请求对象

                // 从客户端请求中读取数据
                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    string requestBody = reader.ReadToEnd();
                    //JsonConvert.DeserializeObject<string>(requestBody);
                    Console.WriteLine("Received request: {0}", requestBody);
                }

                // 发送响应内容
                var response = context.Response; // 获取响应对象
                var responseBytes = Encoding.UTF8.GetBytes("Hello World!");
                response.ContentLength64 = responseBytes.Length;
                response.OutputStream.Write(responseBytes, 0, responseBytes.Length);

                // 关闭响应
                response.OutputStream.Close();
            }
        }

        //static async Task Main()
        //{
        //    Debug.LogError("发送消息");

        //    // 创建 HttpClient 实例
        //    var client = new HttpClient();

        //    string time = Time.realtimeSinceStartup.ToString();

        //    // 准备要发送的数据
        //    var content = new StringContent(time, Encoding.UTF8, "application/json");

        //    // 发送 POST 请求
        //    var response = await client.PostAsync("http://localhost:7800/", content);

        //    // 检查是否成功
        //    if (response.IsSuccessStatusCode)
        //    {
        //        // 获取响应内容
        //        var responseJson = await response.Content.ReadAsStringAsync();

        //        Debug.Log(string.Format("Response: {0}", responseJson));
        //    }
        //    else
        //    {
        //        Debug.Log(string.Format("Failed with status code: {0}", response.StatusCode));
        //    }
        //}
    }
}