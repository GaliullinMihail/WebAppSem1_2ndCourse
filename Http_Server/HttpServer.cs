using System.Text;
using System.Net;
using System.Text.Json;

namespace _01_10_2022_server_http_steam
{
    public class HttpServer : IDisposable
    {
        private readonly HttpListener _listener;
        private ServerSettings? _serverSetting;

        public HttpServer()
        {
            _listener = new HttpListener();
        }

        public void Start()
        {
            _listener.Start();
            if (File.Exists("./settings.json"))
            {
                _serverSetting = JsonSerializer.Deserialize<ServerSettings>(File.ReadAllBytes("./settings.json"));
                
            }
            else
            {
                _serverSetting = new ServerSettings
                {
                    Port = 7777,
                    Path = "@\"./site/\""
                };
            }
            _listener.Prefixes.Clear();
            if (_serverSetting != null) _listener.Prefixes.Add($"http://localhost:{_serverSetting.Port}/");
            Listening();
        }

        public void Stop()
        {
            _listener.Stop();
        }

        private void Listening()
        {
            _listener.BeginGetContext(ListenerCallBack, _listener);
        }

        private void ListenerCallBack(IAsyncResult result)
        {
            if(_listener.IsListening)
            {
                var httpContext = _listener.EndGetContext(result); 
                var response = httpContext.Response;
                byte[] buffer;

                if(Directory.Exists(_serverSetting?.Path))
                {
                    buffer = GetFIle(httpContext.Request.RawUrl?.Replace("%20", " "));

                    if (buffer == null)
                    {
                        response.Headers.Set("Content-Type", "text/plain");
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        string err = "404 - not found";
                        buffer = Encoding.UTF8.GetBytes(err);
                    }
                }
                else
                {
                    var err = $"Directory ' {_serverSetting?.Path}' not found";

                    buffer = Encoding.UTF8.GetBytes(err);
                }

                Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
                Listening();
            }
        }

        private byte[] GetFIle(string? rawUrl)
        {
            var buffer = new byte[] { };
            var filePath = _serverSetting?.Path + rawUrl;

            if(Directory.Exists(filePath))
            {
                filePath = filePath + "/index.html";
                if(File.Exists(filePath))
                {
                    buffer = File.ReadAllBytes(filePath);
                }
            }
            else if(File.Exists(filePath))
            {
                buffer = File.ReadAllBytes(filePath);
            }
            return buffer;
        }

        public void Dispose()
        {
            _listener.Stop();
        }
    }
}
