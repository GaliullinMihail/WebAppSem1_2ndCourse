namespace HTMLEngineLibrary;

public static class IfElseReplacer
{
    public static string ReplaceIfElseTags(string template, object model)
    { 
        //TODO [if condition] operator ; [else] operator ;
        //TODO[if condition] operator ;
        //TODO 000000[1111if Professor.Discipline.list.Length < 15]222222 - группа переполнена ;33333[44444else]555555 ;66666
        var tokens = template.Split(new[] { '[', ']', ';' });

        for (int i = 0; i < tokens.Length; i++)
        {
            
        }

        return null;
    }

    private static string ReplaceIfElseTag(string template, object model, string tag, IFStatement statement)
    {
        throw new NotImplementedException(); // TODO if condition : result 
    }
}






// microsoft анализатор стандартный его мб использовтаь