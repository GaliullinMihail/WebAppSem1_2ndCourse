using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using EnterPool.Http_Server.Attributes;
using EnterPool.Http_Server.models;
using EnterPool.Http_Server.ORM;
using EnterPool.Http_Server.Sessions;
using Konscious.Security.Cryptography;

namespace EnterPool.Http_Server.Controllers;

public class AccountController
{
    private static AccountRepository _repository = 
    new (@"Data Source=DESKTOP-M25AG3C\SQLEXPRESS;Initial Catalog=WebAppSem;Integrated Security=True");

    private static ORM.ORM _orm =
        new(@"Data Source=DESKTOP-M25AG3C\SQLEXPRESS;Initial Catalog=WebAppSem;Integrated Security=True");
    private static byte[] salt = new byte[16];
    
    public AccountController()
    {
        
    }
    [HttpController("authorisation")]
    public class AuthorisationController
    {
        [HttpGET("")]
        public byte[] LoginSite(string id)
        {
            var path = "./site/html/authorisation/index.html";
            return View.GetView(path);
        }
        //rememberMe = "on" or "off"
        [HttpPOST("")]
        public (bool, Account?, bool) Login(string login, string password, string rememberMe = "")
        {
            if (!Validate(login) || !Validate(password))
                return (false, null, false);
            var hashPass = HashPassword(password, salt);
            var guid = new Guid(hashPass);
            
            var acc = _repository.GetAll().FirstOrDefault(acc => acc.Login == login && acc.Password == guid.ToString());
            if (acc == null)
                return (false, null, false);
            Guid result;
            
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(acc.Login + acc.Password));
                result = new Guid(hash);
            }
            if(acc is not null && !_orm.Select<Cookie>().Any(cookie => cookie.Guid == result.ToString()))
                _orm.Insert(new Cookie(result.ToString(), acc.Id));
            return (acc is not null, acc, String.Equals(rememberMe,"on"));
        }
    }

    [HttpController("registration")]
    public class Registration
    {
        [HttpGET("")]
        public byte[] RegistrationSite()
        {
            var path = "./site/html/registration/index.html";

            return View.GetView(path);
        }

        [HttpPOST("")]
        public (bool, Account?) Registrate(string login, string password, string againPassword)
        {
            if (!Validate(login) || !Validate(password) || password != againPassword)
                return (false, null);
            
            var hashPass = HashPassword(password, salt);
            var guid = new Guid(hashPass);
            
            //TODO : check password and againPassword JS
            if (password == againPassword && !_repository.GetAll().Any(acc => acc.Login == login))
            {
                _repository.Insert(login, guid.ToString()); 
                return (true, _repository.GetAll().First(acc => acc.Login == login && acc.Password == guid.ToString())); 
            }
            
            return (false , null);
        }
    }

    [HttpController("profile")]
    public class Profile
    {
        [HttpGET("")]
        public byte[] GetProfile(string id)
        {

            var profile = GetById(id);
            
            if (profile == null)
            {
                return null;
            }

            var path = "./site/html/profile/index.html";
            return View.GetView(path, profile);

        }

        [HttpPOST("")]
        public void ExitProfile()
        {
            
        }
    }

    public static Account? GetById(string id)
    {
        var guid = new Guid(id);
        var session = SessionManager.GetSessionInfo(guid);
        int accId;
        if (session is not null)
        {
            accId = session.AccountId;
        }
        else
        {
            var cookie = _orm.Select<Cookie>().FirstOrDefault(cookie => cookie.Guid == id);
            if (cookie == null)
                return null;
            accId = cookie.IdAcc;
        }
        return _repository.GetById(accId);
    }
    
    private static byte[] CreateSalt()
    {
        var buffer = new byte[16];
        var rng = new RNGCryptoServiceProvider();
        rng.GetBytes(buffer);
        return buffer;
    }
    
    private static byte[] HashPassword(string password, byte[] salt)
    {
        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));

        argon2.Salt = salt;
        argon2.DegreeOfParallelism = 8; // four cores
        argon2.Iterations = 4;
        argon2.MemorySize = 1024 * 1024; // 1 GB

        return argon2.GetBytes(16);
    }
    
    private bool VerifyHash(string password, byte[] salt, byte[] hash)
    {
        var newHash = HashPassword(password, salt);
        return hash.SequenceEqual(newHash);
    }

    private static bool Validate(string text)
    {
        var regex = new Regex("^[a-zA-Z0-9]+$");
        if (text.Length > 30 || text.Length <= 5 || !regex.IsMatch(text))
            return false;
        return true;
    }
}