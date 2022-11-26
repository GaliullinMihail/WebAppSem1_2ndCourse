using EnterPool.Http_Server.Attributes;
using EnterPool.Http_Server.models;

namespace EnterPool.Http_Server.Controllers;

[HttpController("favorite")]
public class FavoriteSiteController
{
    private static ORM.ORM _orm =
        new(@"Data Source=DESKTOP-M25AG3C\SQLEXPRESS;Initial Catalog=WebAppSem;Integrated Security=True");

    [HttpGET("")]
    public byte[] GetLibrary(string id)
    {
        var path = "./site/html/favorite/index.html";
        var profile = AccountController.GetById(id);
        if (profile == null)
            return null; // незашел

        List<FavoriteSite> library;
        library = _orm.Select<FavoriteSite>()
            .Where(site => site.Id == profile.Id)
            .ToList();
        return View.GetView(path, library);
    }

    [HttpPOST("")]
    public void DeleteFromFavorite(string siteName, string id)
    {
        var account = AccountController.GetById(id);
        var orm = new ORM.ORM(@"Data Source=DESKTOP-M25AG3C\SQLEXPRESS;Initial Catalog=WebAppSem;Integrated Security=True");
        var path = siteName.Replace("%2F", "/").Replace("%3A", ":");
        orm.Delete<FavoriteSite>(path, account.Id);   
    }
}