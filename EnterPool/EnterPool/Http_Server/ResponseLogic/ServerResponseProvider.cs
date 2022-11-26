using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using EnterPool.Http_Server.Attributes;
using EnterPool.Http_Server.ServerLogic;
using EnterPool.Http_Server.Sessions;

namespace EnterPool.Http_Server.ResponseLogic;

public class ServerResponseProvider
{
    public static HttpListenerResponse GetResponse(string path, HttpListenerContext ctx)
    {
        var request = ctx.Request;
        var response = ctx.Response;
        var rawUrl = request.RawUrl ?? string.Empty;
        var buffer = Encoding.UTF8.GetBytes("Error 404. Not Found.");
        var urlReffer = ctx.Request.UrlReferrer is null ? "" : ctx.Request.UrlReferrer.AbsolutePath;
        if (!Directory.Exists(path))
            buffer = Encoding.UTF8.GetBytes($"Directory {path} not found.");
        else if (GetFileUpdateContext(path + "html" + urlReffer + rawUrl.Replace("%20", " "), ctx))
            return response;
        else if (TryHandleController(request, response))
            return response;
        ChangeResponse(response, "text/plain", (int)HttpStatusCode.NotFound, buffer);
        return response;
    }

    private static string GetContentType(string rawUrl)
    {
        var extension = rawUrl.Contains('.') ? rawUrl.Split('.')[1] : "html";
        return Mime.GetMimeType(extension);
    }

    public static bool GetFileUpdateContext(string filePath, HttpListenerContext ctx)
    {
        byte[] buffer;
        if (Directory.Exists(filePath) && File.Exists(filePath + "index.html"))
            buffer = File.ReadAllBytes(filePath + "index.html");
        else if (!File.Exists(filePath))
            return false;
        else buffer = File.ReadAllBytes(filePath);
        ChangeResponse(ctx.Response, GetContentType(ctx.Request.RawUrl ?? ""), (int)HttpStatusCode.OK, buffer);
        return true;
    }

    public static bool TryGetFile(string filePath, out byte[] buffer)
    {
        buffer = Array.Empty<byte>();
        if (Directory.Exists(filePath) && File.Exists(filePath + "index.html"))
            buffer = File.ReadAllBytes(filePath + "index.html");
        else if (!File.Exists(filePath))
            return false;
        else buffer = File.ReadAllBytes(filePath);
        return true;
    }

    private static bool TryHandleController(HttpListenerRequest request, HttpListenerResponse response)
    {
        if (request.Url!.Segments.Length < 2) return false;
        var isAuthorised = Authentification.IsAuthorised(request);

        using var sr = new StreamReader(request.InputStream, request.ContentEncoding);
        var bodyParam = sr.ReadToEnd();
    
        var controllerName = request.Url.Segments[1].Replace("/", "");
        var strParams = request.Url.Segments
            .Skip(3)
            .Select(s => s.Replace("/", ""))
            .Concat(bodyParam.Split('&').Select(p => p.Split('=').LastOrDefault()))
            .ToArray();

        var assembly = Assembly.GetExecutingAssembly();
        var controller = assembly.GetTypes()
            .Where(t => Attribute.IsDefined(t, typeof(HttpController)))
            .FirstOrDefault(t => string.Equals(
                (t.GetCustomAttribute(typeof(HttpController)) as HttpController)?.ControllerName,
                controllerName,
                StringComparison.CurrentCultureIgnoreCase));
        string redirect = RedirectLocation(controllerName);

        var method = controller?.GetMethods()
            .FirstOrDefault(t => t.GetCustomAttributes(true)
                .Any(attr => attr.GetType().Name == $"Http{request.HttpMethod}"
                             && Regex.IsMatch(request.RawUrl ?? "",
                                 attr.GetType()
                                     .GetField("MethodUri")?
                                     .GetValue(attr)?.ToString() ?? "")));
        if (method is null) return false;

        object? ret;
        
        if (IsRequiredAuthentification(controllerName) && !isAuthorised)
        {
            response.Redirect(Location.Authorisation);
            ChangeResponse(response, "text/html", (int)HttpStatusCode.Redirect, Array.Empty<byte>());
            return true;
        }

        if (IsRequiredRedirect(controllerName) && isAuthorised)
        {
            response.Redirect(Location.Profile);
            ChangeResponse(response, "text/html", (int)HttpStatusCode.Redirect, Array.Empty<byte>());
            return true;
        }
        

        strParams = AddAuthorisation(strParams, request);
        var queryParams = method.GetParameters()
            .Select((p, i) => Convert.ChangeType(strParams[i], p.ParameterType))
            .ToArray();
        

        ret = method.Invoke(Activator.CreateInstance(controller!), queryParams);

        if (method.Name == "CheckLogin")
        {
            var flag = (bool)ret;
            if (!flag)
            {
                ChangeResponse(response, "text/html", (int)HttpStatusCode.OK, Array.Empty<byte>());
                return true;
            }
            ChangeResponse(response, "text/html", (int)HttpStatusCode.BadRequest, Array.Empty<byte>());
            return true;

        }

        if (method.Name == "ExitProfile")
        {
            redirect = Location.Authorisation;
            ClearCookie(response);
        }

        if (method.Name == "Login") // ret == (bool, Account?, bool) 1-успешно, 2- акк, 3- нужно ли запоминать
        {
            var acc = ((bool, models.Account, bool))ret;
            if (acc.Item1)
            {
                AddCookieAndSession(acc, response);
            }
            else
            {
                redirect = Location.Authorisation;
            }
        }

        if (method.Name == "Registrate")
        {
            var acc = ((bool, models.Account))ret;
            if (!acc.Item1)
            {
                redirect = Location.Registration;
                ChangeResponse(response, "text/html", (int)HttpStatusCode.BadRequest, Array.Empty<byte>());
            }
            ChangeResponse(response, "text/html", (int)HttpStatusCode.OK, Array.Empty<byte>());
        }

        var buffer = ret;
        var statusCode = HttpStatusCode.OK;
        if (request.HttpMethod == "POST")
        {
            statusCode = HttpStatusCode.Redirect;
            response.Redirect(redirect);
            ChangeResponse(response, "text/html", (int)statusCode, Array.Empty<byte>());
            return true;
        }

        ChangeResponse(response, "text/html", (int)statusCode, (byte[])buffer);
        return true;
    }

    private static void ClearCookie(HttpListenerResponse response)
    {
        response.Cookies.Add(new Cookie("SessionId", ""));
        response.Cookies.Add(new Cookie("Cookie", ""));
    }

    private static void AddCookieAndSession((bool, models.Account, bool) acc,
        HttpListenerResponse response)
    {
        response.Cookies.Add(new Cookie("SessionId",
            SessionManager.CreateSession(acc.Item2.Id, acc.Item2.Login, DateTime.Now).ToString()));

        if (!acc.Item3) return;
        Guid result;
        using (MD5 md5 = MD5.Create())
        {
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(acc.Item2.Login + acc.Item2.Password));
            result = new Guid(hash);
        }

        response.Cookies.Add(new Cookie("Cookie", result.ToString()));
    }

    private static string RedirectLocation(string controllerName) => controllerName switch
    {
        "authorisation" => Location.Profile,
        "registration" => Location.Authorisation,
        "sites" => Location.Sites,
        "favorite" => Location.Favorite,
        "posts" => Location.Posts,
        _ => Location.Main
    };

    private static void ChangeResponse(HttpListenerResponse response, string contentType, int statusCode, byte[] buffer)
    {
        response.Headers.Set("Content-Type", contentType);
        response.StatusCode = statusCode;
        response.OutputStream.Write(buffer, 0, buffer.Length);
    }

    private static string?[] AddAuthorisation(string?[] strParams, HttpListenerRequest request)
    {
        var sessionCookie = request.Cookies["SessionId"];
        var cookie = request.Cookies["Cookie"];
        if (sessionCookie is not null && sessionCookie.Value != "" &&
            SessionManager.CheckSession(new Guid(sessionCookie.Value)))
        {
            var cookieAuthInfo = sessionCookie.Value;
            if (strParams[0] == "")
            {
                return new[] { cookieAuthInfo };
            }

            var res = strParams.Concat(new string?[] { cookieAuthInfo }).ToArray();
            return res;
        } 
        
        if (cookie is not null && cookie.Value != "")
        {
            if (strParams[0] == "")
            {
                return new[] { cookie.Value };
            }
            var res = strParams.Concat(new string?[] { cookie.Value }).ToArray();
            return res;
        }
        
        return strParams;
    }

    private static bool IsRequiredAuthentification(string controllerName) => controllerName switch
    {
        "favorite" or "profile" => true,
        _ => false
    };

    private static bool IsRequiredRedirect(string controllerName) => controllerName switch
    {
        "authorisation" or "registration" => true,
        _ => false
    };
}