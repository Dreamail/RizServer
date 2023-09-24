using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using NetCoreServer;

namespace RizServerConsole
{
    class HttpsCacheSession : HttpsSession
    {
        public HttpsCacheSession(NetCoreServer.HttpsServer server) : base(server) { }

        public void CustomSendStatus200AndNoHeader(HttpResponse Response , string body)
        {
            Response.Clear();
            Response.SetBegin(200);//200响应代码必须放置在Clear之后的一行中，否则会卡死
            Response.SetBody(body);
            SendResponseAsync(Response);
        }

        public void CustomSendStatus200AndNoHeaderByteArray(HttpResponse Response, byte[] body)
        {
            Response.Clear();
            Response.SetBegin(200);//200响应代码必须放置在Clear之后的一行中，否则会卡死
            Response.SetBody(body);
            SendResponseAsync(Response);
        }

        public void CustomSendStatus200WithSetTokenHeader(HttpResponse Response, string body, string token)
        {
            Response.Clear();
            Response.SetBegin(200);//200响应代码必须放置在Clear之后的一行中，否则会卡死
            Response.SetHeader("set_token", token);
            Response.SetBody(body);
            SendResponseAsync(Response);
        }

        public void CustomSendStatus200WithSignHeader(string verify, HttpResponse Response, string body, string sign)
        {
            Response.Clear();
            Response.SetBegin(200);//200响应代码必须放置在Clear之后的一行中，否则会卡死
            Response.SetHeader("sign", sign);
            Response.SetHeader("verify", verify);
            Response.SetBody(body);
            SendResponseAsync(Response);
        }

        public (string,string) GetHeadersInRequest(HttpRequest request)
        {
            bool found_token = false;
            bool found_verify = false;
            string token = "";
            string verify = "";
            foreach (int i in Enumerable.Range(0, (int)request.Headers))
            {
                if (found_token && found_verify)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("<Func>GetHeadersInRequest: header(i).item1=" + request.Header(i).Item1 + " header(i).item2=" + request.Header(i).Item2);
                    if (request.Header(i).Item1 == "token")
                    {
                        token = request.Header(i).Item2;
                        found_token = true;
                    }
                    if (request.Header(i).Item1 == "verify")
                    {
                        verify = request.Header(i).Item2;
                        found_verify = true;
                    }
                }
            }
            Console.WriteLine("<Func>GetHeadersInRequest: token=" + token + " verify=" + verify);
            return (token, verify);
        }

        protected override void OnReceivedRequest(HttpRequest request)
        {
            // 输出请求Method与对应Url
            Console.WriteLine(request.Method + " >> " + request.Url);

            // 对HTTP请求做出处理
            if (request.Method == "HEAD")
            {
                SendResponseAsync(Response.MakeHeadResponse());
            }
            else if (request.Method == "CONNECT")
            {
                SendResponseAsync(Response.MakeOkResponse());//不知道为什么游戏会发送一些CONNECT请求 虽然没什么意义 但也给做Ok处理吧
            }
            else if (request.Method == "GET")
            {
                if (request.Url == "/")
                {
                    SendResponseAsync(Response.MakeGetResponse("Looks like RizServerConsole is working fine, keep going!"));
                }
                else if (request.Url == "/check/status")
                {
                    CustomSendStatus200AndNoHeader(Response, "OK");
                }
                else if (request.Url == "/configs/game_config.json")
                {
                    Console.WriteLine("请求config");
                    CustomSendStatus200AndNoHeader(Response, "{\r\n\r\n    \"configs\": [\r\n\r\n        {\r\n\r\n            \"version\": \"1.0.8\",\r\n\r\n            \"resourceUrl\": \"http://rizlineassetstore.pigeongames.cn/versions/v1_0_8_0\"\r\n\r\n        },\r\n\r\n        {\r\n\r\n            \"version\": \"1.0.9\",\r\n\r\n            \"resourceUrl\": \"http://rizlineassetstore.pigeongames.cn/versions/v1_0_8_0\"\r\n\r\n        }\r\n\r\n    ],\r\n\r\n    \"minimalVersion\": \"1.0.8\",\r\n\r\n    \"underMaintenance\": false,\r\n\r\n    \"maintenanceNoticeZhHans\": \"\",\r\n\r\n    \"maintenanceNoticeZhHant\": \"\",\r\n\r\n    \"maintenanceNoticeEn\": \"\",\r\n\r\n    \"maintenanceNoticeJa\": \"\"\r\n\r\n}");
                }
                else if (request.Url.Contains("/versions/v"))
                {
                    try
                    {
                        var filename = "resources/HotUpdateResources/" + HttpUtility.UrlDecode((new Uri("https://1.com" + request.Url)).Segments.Last());
                        Console.WriteLine("资源文件请求：" + filename);
                        using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                        {
                            using (BinaryReader br = new BinaryReader(fs))
                            {
                                byte[] data = br.ReadBytes((int)fs.Length);
                                CustomSendStatus200AndNoHeaderByteArray(Response, data);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("在进行资源文件分发时报错，错误信息：" + ex + "\n绝大部分原因都是HotUpdateResources放置不正确或压根没放置造成的，要么你就别使用资源文件分发功能（在Fiddler Script中修改规则，也可以用文档中提供给不同用途的对应Fiddler Script），直接从官方服务器获取更新文件，要么就检查你的放置是否正确");
                    }
                }
                else
                {
                    SendResponseAsync(Response.MakeErrorResponse(404, "Error Code: 404"));
                }
            }
            else if ((request.Method == "POST"))
            {
                if (request.Url == "/check/testpost")
                {
                    Response.SetHeader("SetHeaderTest", "OK");
                    SendResponseAsync(Response.MakeGetResponse("this is a post! post body: " + request.Body));
                }
                else if (request.Url == "/account/check_email")
                {
                    CustomSendStatus200AndNoHeader(Response, RizServerCoreSharp.ReRhyth.CheckEmail.Check(request.Body));
                }
                else if (request.Url == "/account/send_email")
                {
                    CustomSendStatus200AndNoHeader(Response, RizServerCoreSharp.ReRhyth.SendEmail.Send(request.Body));
                }
                else if (request.Url == "/account/register")
                {
                    var CoreReturn = RizServerCoreSharp.ReRhyth.Register.Reg(request.Body);
                    CustomSendStatus200WithSetTokenHeader(Response, CoreReturn.ret, CoreReturn.header_set_token);
                }
                else if (request.Url == "/account/login")
                {
                    var CoreReturn = RizServerCoreSharp.ReRhyth.RhythAccountLogin.Login(request.Body);
                    CustomSendStatus200WithSetTokenHeader(Response, CoreReturn.ret, CoreReturn.header_set_token);
                }
                else if (request.Url == "/account/Insensitive_login")
                {
                    CustomSendStatus200AndNoHeader(Response,RizServerCoreSharp.ReRhyth.Insensitive_login.InsensitiveLogin(request.Body));
                }
                else if (request.Url == "/game/rn_login")
                {
                    bool req_sended = false;
                    var headers = GetHeadersInRequest(request);
                    if (headers.Item1 == "" || headers.Item2 == "")
                    {
                        CustomSendStatus200AndNoHeader(Response,"missing headers");
                        req_sended = true;
                    }

                    if (!req_sended)
                    {
                        var CoreReturn = RizServerCoreSharp.ReRizApi.RizLogin.Login(headers.Item1);
                        CustomSendStatus200WithSignHeader(headers.Item2, Response, CoreReturn.ResponseBody, CoreReturn.ResponseHeaderSign);
                    }
                    //CustomSendStatus200AndNoHeader(Response, "Header missing");
                }
                else if (request.Url == "/game/check_buy_count")
                {
                    bool req_sended = false;
                    var headers = GetHeadersInRequest(request);
                    if (headers.Item1 == "" || headers.Item2 == "")
                    {
                        CustomSendStatus200AndNoHeader(Response, "missing headers");
                        req_sended = true;
                    }

                    if (!req_sended)
                    {
                        var CoreReturn = RizServerCoreSharp.ReRizApi.check_buy_count.Check();
                        CustomSendStatus200WithSignHeader(headers.Item2,Response, CoreReturn.ResponseBody, CoreReturn.ResponseHeaderSign);
                    }
                }
                else if (request.Url == "/game/after_play")
                {
                    bool req_sended = false;
                    var headers = GetHeadersInRequest(request);
                    if (headers.Item1 == "" || headers.Item2 == "")
                    {
                        CustomSendStatus200AndNoHeader(Response, "missing headers");
                        req_sended = true;
                    }

                    if (!req_sended)
                    {
                        var CoreReturn = RizServerCoreSharp.ReRizApi.AfterPlay.AfterPlayMain(headers.Item1,request.Body);
                        CustomSendStatus200WithSignHeader(headers.Item2, Response, CoreReturn.ResponseBody, CoreReturn.ResponseHeaderSign);
                    }
                }
                else
                {
                    SendResponseAsync(Response.MakeErrorResponse(404, "Error Code: 404"));
                }
            }
            else
            {
                SendResponseAsync(Response.MakeErrorResponse("Unsupported or blocked HTTP method: " + request.Method));
            }
        }

        protected override void OnReceivedRequestError(HttpRequest request, string error)
        {
            Console.WriteLine($"Request error: {error}");
        }

        protected override void OnError(SocketError error)
        {
            if (error.ToString().Contains("NotConnected"))
            {
                Console.WriteLine($"HTTPS session not connected, Please check whether the client has trusted the HTTPS certificate of the corresponding server!");
            }
            else
            {
                Console.WriteLine($"HTTPS session caught an error: {error}");
            }
        }
    }

    class HttpsCacheServer : NetCoreServer.HttpsServer
    {
        public HttpsCacheServer(SslContext context, IPAddress address, int port) : base(context, address, port) { }

        protected override SslSession CreateSession() { return new HttpsCacheSession(this); }

        protected override void OnError(SocketError error)
        {
            if (error.ToString().Contains("NotConnected"))
            {
                Console.WriteLine($"HTTPS session not connected, Please check whether the client has trusted the HTTPS certificate of the corresponding server!");
            }
            else
            {
                Console.WriteLine($"HTTPS session caught an error: {error}");
            }
        }
    }

    class ServerMain
    {
        static void Main(string[] args)
        {
            RizServerCoreSharp.Classes.DBMain.Init();
            RizServerCoreSharp.Classes.DBMain.InitTargetJsonFile("RizServerCoreSharp");

            // HTTPS server port
            int port = 8443;
            if (args.Length > 0)
                port = int.Parse(args[0]);
            // HTTPS server content path
            //string www = "../../../../../www/api";

            Console.WriteLine($"RizServer HTTPS server port: {port}");
            Console.WriteLine($"Now you can try to send a request to RizServer: https://localhost:{port}/");

            Console.WriteLine();

            // Create and prepare a new SSL server context
            var context = new SslContext(SslProtocols.Tls12, new X509Certificate2("certs/cert.pfx", File.ReadAllText("certs/certpwd.txt")));

            // Create a new HTTP server
            var server = new HttpsCacheServer(context, IPAddress.Any, port);
            //server.AddStaticContent(www, "/api");

            // Start the server
            Console.Write("Server starting...");
            server.Start();
            Console.WriteLine("Done!");

            Console.WriteLine("Press Enter to stop the server or '!' to restart the server...");

            // Perform text input
            for (; ; )
            {
                string line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                    break;

                // Restart the server
                if (line == "!")
                {
                    Console.Write("Server restarting...");
                    server.Restart();
                    Console.WriteLine("Done!");
                }
            }

            // Stop the server
            Console.Write("Server stopping...");
            server.Stop();
            Console.WriteLine("Done!");
        }
    }
}