using System.Net;
using EnterPool.Http_Server.ResponseLogic;

namespace EnterPool.Http_Server.ServerLogic;

public class HttpServer : IDisposable
{
    private readonly HttpListener _listener;
    private ServerSettings _serverSetting;
    public ServerStatus Status { get; private set; }
    

    public HttpServer()
    {
        _listener = new HttpListener();
        Status = ServerStatus.Stop;
        _serverSetting = new ServerSettings();
    }

    public void Start()
    {
        if (Status == ServerStatus.Start) return;
        _listener.Start();
        Status = ServerStatus.Start; 
        
        _serverSetting = ServerFileHandler.ReadJsonSettings("./settings.json"); 
        
        _listener.Prefixes.Clear();
        _listener.Prefixes.Add($"http://localhost:{_serverSetting.Port}/");
  
        Listening();    
    }

    private void Stop()
    {
        if (Status == ServerStatus.Stop)
        {
            return;
        }
        
        _listener.Stop();

        Status = ServerStatus.Stop;
    }

    private void Listening()
    {
        _listener.BeginGetContext(new AsyncCallback(ListenerCallBack), _listener);
    }

    private void ListenerCallBack(IAsyncResult result)
    {
        try
        {
            if (!_listener.IsListening) return;
            var httpContext = _listener.EndGetContext(result);
            var response = ServerResponseProvider.GetResponse(_serverSetting.Path, httpContext);
            response.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e}");
        }
        Listening();
    }

    public void Dispose()
    {
        Stop();
    }
}