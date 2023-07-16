using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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

        protected override void OnReceivedRequest(HttpRequest request)
        {
            // 输出请求Method与对应Url
            Console.WriteLine(request.Method + " > " + request.Url);

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