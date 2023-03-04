namespace HTMLEngineLibrary;

public static class PropertyReplacer
{
    public static string ReplacePropertyTags(string template, object model)
    {
        var allTags = template.Split(new[] { '{', '}' });  

        if (allTags.Length <= 1)
            return template;

        for (int i = 1; i < allTags.Length; i += 2)
        {
            template = ReplacePropertyTag(template, model, allTags[i]);
        }

        return RemoveTags(template);
    }
    
    private static string ReplacePropertyTag(string template, object model, string tag)
    {
        var modifiedTag = tag.Substring(tag.IndexOf('.') + 1);
        object propertyValue = GetPropertyValue(model, modifiedTag);
        
        return template.Replace(tag, propertyValue.ToString());
    }
    
    private static object GetPropertyValue(object obj, string propertyName)
    {
        foreach (var prop in propertyName.Split('.').Select(s => obj.GetType().GetProperty(s)))
            obj = prop.GetValue(obj, null);

        return obj;
    }
    
    private static string RemoveTags(string template)
    {
        return template
            .Replace('{', ' ')
            .Replace('}', ' ');
    }
}

