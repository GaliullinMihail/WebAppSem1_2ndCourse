using EnterPool.Http_Server.Attributes;
using EnterPool.Http_Server.models;

namespace EnterPool.Http_Server.Controllers;

[HttpController("posts")]
public class PostController
{
    private static ORM.ORM _orm =
        new(@"Data Source=DESKTOP-M25AG3C\SQLEXPRESS;Initial Catalog=WebAppSem;Integrated Security=True");

    [HttpGET("")]
    public byte[] GetPosts(string id)
    {
        bool flag = false;
        Account account = null;
        if (id != "")
        {
            account = AccountController.GetById(id);
            flag = account != null;
        }
        var path = "./site/html/posts/index.html";
        var posts = _orm.Select<Post>();
        return View.GetView(path, posts, flag, account);
    }

    [HttpPOST("addPost")]
    public void AddPost(string text, string id)
    {
        var account = AccountController.GetById(id);
        _orm.Insert<Post>(text.Replace('+',' '), DateTime.Today.ToString(), account.Login);
    }

    [HttpPOST("editPost")]
    public void EditPost(string idPost, string text, string id)
    {
        _orm.Update<Post>(int.Parse(idPost), "text", text.Replace('+',' '));
        _orm.Update<Post>(int.Parse(idPost), "date", DateTime.Today.ToString());
    }
}