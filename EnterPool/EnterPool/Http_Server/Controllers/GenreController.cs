using EnterPool.Http_Server.Attributes;
using EnterPool.Http_Server.models;

namespace EnterPool.Http_Server.Controllers;

[HttpController("genres")]
public class GenreController
{
    private static ORM.ORM _orm =
        new(@"Data Source=DESKTOP-M25AG3C\SQLEXPRESS;Initial Catalog=WebAppSem;Integrated Security=True");

    [HttpGET("")]
    public byte[] GetGenres()
    {
        var path = "./site/html/genres/index.html";
        var genres = _orm.Select<Genre>();
        return View.GetView(path, genres);
    }
}