using System.Text.RegularExpressions;
using EnterPool.Http_Server.Attributes;
using EnterPool.Http_Server.models;

namespace EnterPool.Http_Server.Controllers;

[HttpController("sites")]
public class SitesController
{
    private static ORM.ORM _orm =
        new(@"Data Source=DESKTOP-M25AG3C\SQLEXPRESS;Initial Catalog=WebAppSem;Integrated Security=True");

    [HttpGET("")]
    public byte[] GetSites(string id)
    {
        var path = "./site/html/sites/index.html";
        var sites = _orm.Select<Site>();
        if (id != "")
        {
            var account = AccountController.GetById(id);
            return View.GetView(path, (sites, account));
        }

        return View.GetView(path, (sites, null));

    }

    [HttpPOST("addToFavorite")]
    public void AddToFavorite(string siteName, string id)
    {
        var site = _orm.Select<Site>().FirstOrDefault(site => site.Name == siteName);
        var account = AccountController.GetById(id);
        if (account == null || site == null)
            return;
        _orm.Insert<FavoriteSite>(account.Id.ToString(), site.Path, site.Genre);
    }

    [HttpPOST("addSite")]
    public void AddSite(string siteName, string genre, string description, string path)
    {
        var parsedPath = path.Replace("%3A", ":").Replace("%2F", "/");
        if (IsUrlValid(parsedPath))
        {
            _orm.Insert<Site>(siteName.Replace('+', ' '), genre, description.Replace('+', ' '), parsedPath);
        }
    }
    
    private static bool IsUrlValid(string url)
    {
        string pattern = @"^(http|https|ftp|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
        Regex reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        return reg.IsMatch(url);
    }

}