using System.Text;
using EnterPool.Http_Server.models;
using EnterPool.Http_Server.ResponseLogic;

namespace EnterPool.Http_Server.Controllers;

public static class View
{
    private static ORM.ORM _orm =
        new(@"Data Source=DESKTOP-M25AG3C\SQLEXPRESS;Initial Catalog=WebAppSem;Integrated Security=True");

    public static byte[] GetView(string path)
    {
        var buffer = Array.Empty<byte>();
        if (File.Exists(path))
        {
            ServerResponseProvider.TryGetFile(path, out buffer);
        }

        return buffer;
    }

    public static byte[] GetView(string path, Account account)
    {
        var buffer = Array.Empty<byte>();
        if (File.Exists(path))
        {
            var template = Scriban.Template.Parse(File.ReadAllText(path));
            var result = template.Render(new { account = account });
            buffer = Encoding.ASCII.GetBytes(result);
        }
        
        return buffer;
    }

    public static byte[] GetView(string path, List<Genre> genres)
    {
        var buffer = Array.Empty<byte>();
        if (File.Exists(path))
        {
            var template = Scriban.Template.Parse(File.ReadAllText(path));
            var result = template.Render(new { genres = genres });
            buffer = Encoding.ASCII.GetBytes(result);
        }
        
        return buffer;
    }

    public static byte[] GetView(string path, List<FavoriteSite> sites)
    {
        var buffer = Array.Empty<byte>();
        if (File.Exists(path))
        {
            var template = Scriban.Template.Parse(File.ReadAllText(path));
            var result = template.Render(new { sites = sites });
            buffer = Encoding.ASCII.GetBytes(result);
        }
        
        return buffer;
    }
    
    public static byte[] GetView(string path, (List<Site>, Account?) sites)
    {

        var flag = false;
        var buffer = Array.Empty<byte>();
        
        var valueTuples = new List<SiteToAdd>();
        if (File.Exists(path))
        {
            if (sites.Item2 != null)
            {
                flag = true;
                var library = _orm.Select<FavoriteSite>()
                    .Where(site => site.Id == sites.Item2.Id).ToList();
                valueTuples = sites.Item1
                    .Select(site => new SiteToAdd(site, !library.Any(librarySite => librarySite.Site == site.Path))).ToList();
            }
            else
            {
                valueTuples = sites.Item1.Select(site => new SiteToAdd(site, false)).ToList();
            }

        }
        var template = Scriban.Template.Parse(File.ReadAllText(path));
        var result = template.Render(new { sites = valueTuples, flag = flag});
        buffer = Encoding.ASCII.GetBytes(result);
        
        return buffer;
    }
    
    public static byte[] GetView(string path, List<Post> posts, bool flag, Account account)
    {
        var buffer = Array.Empty<byte>();
        if (File.Exists(path))
        {
            var template = Scriban.Template.Parse(File.ReadAllText(path));
            string result;
            if (account != null)
            { 
                result = template.Render(new { posts = posts, flag = flag, account = account });
            }
            else
            {
                result = template.Render(new { posts = posts, flag = flag, account = new Account(-1, "-1", "-1")});
            }

            buffer = Encoding.ASCII.GetBytes(result);
        }
        
        return buffer;
    }
    
    private class SiteToAdd
    {
        public Site site;
        public bool flag;

        public SiteToAdd(Site site, bool flag)
        {
            this.site = site;
            this.flag = flag;
        }
    }
}