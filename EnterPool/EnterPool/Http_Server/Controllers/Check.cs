using EnterPool.Http_Server.Attributes;
using EnterPool.Http_Server.ORM;

namespace EnterPool.Http_Server.Controllers;

[HttpController("check")]
public class Check
{
    private static AccountRepository _repository = 
        new (@"Data Source=DESKTOP-M25AG3C\SQLEXPRESS;Initial Catalog=WebAppSem;Integrated Security=True");
    [HttpPOST("")]
    public bool CheckLogin(string login)
    {
        return _repository.GetAll().Any(acc => acc.Login == login);
    }
}