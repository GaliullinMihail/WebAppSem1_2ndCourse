using System.Net;
using EnterPool.Http_Server.Controllers;
using EnterPool.Http_Server.models;
using EnterPool.Http_Server.Sessions;

namespace EnterPool.Http_Server.ResponseLogic;

public static class Authentification
{
    public static bool IsAuthorised(HttpListenerRequest request)
    {
        var sessionCookie = request.Cookies["SessionId"];
        var cookie = request.Cookies["Cookie"];
        Account account = null;
        if (sessionCookie is not null && sessionCookie.Value != "" &&
            SessionManager.CheckSession(new Guid(sessionCookie.Value)))
        {
            var cookieAuthInfo = sessionCookie.Value;
            account = AccountController.GetById(cookieAuthInfo);
            if (account != null)
                return true;
        }

        if (cookie is not null && cookie.Value != "")
        {
            account = AccountController.GetById(cookie.Value);
        }

        return account != null;
    }
}