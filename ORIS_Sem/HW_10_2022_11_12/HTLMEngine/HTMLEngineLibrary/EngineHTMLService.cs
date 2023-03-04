using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace HTMLEngineLibrary;

public class EngineHTMLService : IEngineHTMLService
{
    public string GetHTML(string template, object model)
    {
        
        if (!CheckTemplate(template))
            throw new NotImplementedException(); 
        return ReplaceAllTags(template, model);
        
        
    }

    public string GetHTML(Stream pathTemplate, object model)
    {
        throw new NotImplementedException();
    }

    public string GetHTML(byte[] bytes, object model)
    {
        throw new NotImplementedException();
    }

    public Stream GetHTMLInStream(string template, object model)
    {
        throw new NotImplementedException();
    }

    public Stream GetHTMLInStream(Stream pathTemplate, object model)
    {
        throw new NotImplementedException();
    }

    public Stream GetHTMLInStream(byte[] bytes, object model)
    {
        throw new NotImplementedException();
    }

    public byte[] GetHTMLInByte(string template, object model)
    {
        throw new NotImplementedException();
    }

    public byte[] GetHTMLInByte(Stream pathTemplate, object model)
    {
        throw new NotImplementedException();
    }

    public byte[] GetHTMLInByte(byte[] bytes, object model)
    {
        throw new NotImplementedException();
    }

    public void GenerateAndSaveInDirectory(string templatePath, string outputPath, string outputNameFile, object model)
    {
        throw new NotImplementedException();
    }

    public void GenerateAndSaveInDirectory(Stream templatePath, string outputPath, string outputNameFile, object model)
    {
        throw new NotImplementedException();
    }

    public void GenerateAndSaveInDirectory(byte[] templatePath, string outputPath, string outputNameFile, object model)
    {
        throw new NotImplementedException();
    }

    public Task GenerateAndSaveInDirectoryTask(string templatePath, string outputPath, string outputNameFile, object model)
    {
        throw new NotImplementedException();
    }

    public Task GenerateAndSaveInDirectoryTask(Stream templatePath, string outputPath, string outputNameFile, object model)
    {
        throw new NotImplementedException();
    }

    public Task GenerateAndSaveInDirectoryTask(byte[] templatePath, string outputPath, string outputNameFile, object model)
    {
        throw new NotImplementedException();
    }

    private string ReplaceAllTags(string template, object model)
    {
        template = PropertyReplacer.ReplacePropertyTags(template, model);
     //   template = ReplaceIfElseTags(template, model);
       // template = ReplaceForeachTags(template, model);
       return template;
    }
    
    private bool CheckTemplate(string template)// TODO only 1 "{" and "}" have same number
    {
       // Regex regex = new Regex(@"*{{}}*");
       int indexer = 0;
       for (int i = 0; i < template.Length; i++)
       {
           if (template[i] == '{')
           {
               indexer++;
           }
           else if (template[i] == '}')
           {
               indexer--;
               if (indexer < 0) return false;
           }
       }

       return indexer == 0;
    }
    
}